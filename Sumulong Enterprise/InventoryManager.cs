using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Sumulong_Enterprise
{
    public class InventoryManager
    {
        private readonly string connectionString = "Data Source=SumulongInventory.db;Version=3;";

        #region Inventory Loading & Reporting

        public DataTable LoadInventory()
        {
            DataTable table = new DataTable();
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT 
                        s.StockID,
                        p.PartName,
                        p.PartNumber,
                        p.Brand,
                        m.ModelName,
                        sup.SupplierName,
                        l.LocationName,
                        il.Quantity,
                        s.SRP,
                        s.WS_Price,
                        s.InternalCode
                    FROM INVENTORY s
                    JOIN PARTS p ON s.PartID = p.PartID
                    JOIN MOTORCYCLE_MODELS m ON s.ModelID = m.ModelID
                    JOIN SUPPLIERS sup ON s.SupplierID = sup.SupplierID
                    JOIN INVENTORY_LOCATIONS il ON il.StockID = s.StockID
                    JOIN LOCATIONS l ON il.LocationID = l.LocationID;
                ";

                using (var cmd = new SQLiteCommand(query, conn))
                using (var adapter = new SQLiteDataAdapter(cmd))
                {
                    adapter.Fill(table);
                }
            }

            return table;
        }

        public DataTable FilterInventory(string brand, string model, string part)
        {
            DataTable table = new DataTable();
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                var filters = new System.Collections.Generic.List<string>();
                var parameters = new System.Collections.Generic.List<SQLiteParameter>();

                if (!string.IsNullOrWhiteSpace(brand))
                {
                    filters.Add("p.Brand = @Brand");
                    parameters.Add(new SQLiteParameter("@Brand", brand));
                }

                if (!string.IsNullOrWhiteSpace(model))
                {
                    filters.Add("m.ModelName = @ModelName");
                    parameters.Add(new SQLiteParameter("@ModelName", model));
                }

                if (!string.IsNullOrWhiteSpace(part))
                {
                    filters.Add("p.PartName = @PartName");
                    parameters.Add(new SQLiteParameter("@PartName", part));
                }

                string query = @"
                    SELECT 
                        s.StockID,
                        p.PartName,
                        p.PartNumber,
                        p.Brand,
                        m.ModelName,
                        sup.SupplierName,
                        l.LocationName,
                        il.Quantity,
                        s.SRP,
                        s.WS_Price,
                        s.InternalCode
                    FROM INVENTORY s
                    JOIN PARTS p ON s.PartID = p.PartID
                    JOIN MOTORCYCLE_MODELS m ON s.ModelID = m.ModelID
                    JOIN SUPPLIERS sup ON s.SupplierID = sup.SupplierID
                    JOIN INVENTORY_LOCATIONS il ON il.StockID = s.StockID
                    JOIN LOCATIONS l ON il.LocationID = l.LocationID
                ";

                if (filters.Count > 0)
                    query += " WHERE " + string.Join(" AND ", filters);

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                    using (var adapter = new SQLiteDataAdapter(cmd))
                    {
                        adapter.Fill(table);
                    }
                }
            }

            return table;
        }

        /// <summary>
        /// Populates a ComboBox with values from a SQL query (e.g., list of Brands, Models, Suppliers).
        /// </summary>
        public void PopulateComboBox(System.Windows.Forms.ComboBox comboBox, string query)
        {
            comboBox.Items.Clear();
            comboBox.Items.Add(" "); // Default empty selection

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBox.Items.Add(reader[0].ToString());
                    }
                }
            }

            comboBox.SelectedIndex = 0;
        }


        #endregion

        #region Stock Movement Logic (A2)

        /// <summary>
        /// Adds stock to a location.
        /// </summary>
        public bool AddStock(long stockId, int locationId, int quantity, string uom, string transferCode = null)
        {
            if (quantity <= 0)
            {
                MessageBox.Show("Quantity must be greater than zero.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(uom))
            {
                MessageBox.Show("Unit type (UOM) is required.");
                return false;
            }

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Update INVENTORY_LOCATIONS
                        string updateLoc = @"
                            INSERT INTO INVENTORY_LOCATIONS (StockID, LocationID, Quantity)
                            VALUES (@StockID, @LocationID, @Quantity)
                            ON CONFLICT(StockID, LocationID) 
                            DO UPDATE SET Quantity = Quantity + @Quantity;
                        ";

                        using (var cmd = new SQLiteCommand(updateLoc, conn))
                        {
                            cmd.Parameters.AddWithValue("@StockID", stockId);
                            cmd.Parameters.AddWithValue("@LocationID", locationId);
                            cmd.Parameters.AddWithValue("@Quantity", quantity);
                            cmd.ExecuteNonQuery();
                        }

                        // Log movement
                        LogStockMovement(conn, stockId, null, locationId, quantity, uom, transferCode);

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error adding stock: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Deducts stock from a location.
        /// </summary>
        public bool DeductStock(long stockId, int locationId, int quantity, string uom, string transferCode = null)
        {
            if (quantity <= 0)
            {
                MessageBox.Show("Quantity must be greater than zero.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(uom))
            {
                MessageBox.Show("Unit type (UOM) is required.");
                return false;
            }

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Check current quantity
                        int currentQty = 0;
                        using (var cmdCheck = new SQLiteCommand(
                            "SELECT Quantity FROM INVENTORY_LOCATIONS WHERE StockID=@StockID AND LocationID=@LocationID", conn))
                        {
                            cmdCheck.Parameters.AddWithValue("@StockID", stockId);
                            cmdCheck.Parameters.AddWithValue("@LocationID", locationId);
                            var result = cmdCheck.ExecuteScalar();
                            if (result != null)
                                currentQty = Convert.ToInt32(result);
                        }

                        if (quantity > currentQty)
                        {
                            MessageBox.Show("Cannot deduct more than available stock.");
                            return false;
                        }

                        // Update INVENTORY_LOCATIONS
                        string updateLoc = @"
                            UPDATE INVENTORY_LOCATIONS
                            SET Quantity = Quantity - @Quantity
                            WHERE StockID=@StockID AND LocationID=@LocationID;
                        ";

                        using (var cmd = new SQLiteCommand(updateLoc, conn))
                        {
                            cmd.Parameters.AddWithValue("@StockID", stockId);
                            cmd.Parameters.AddWithValue("@LocationID", locationId);
                            cmd.Parameters.AddWithValue("@Quantity", quantity);
                            cmd.ExecuteNonQuery();
                        }

                        // Log movement
                        LogStockMovement(conn, stockId, locationId, null, quantity, uom, transferCode);

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error deducting stock: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Transfers stock between locations.
        /// </summary>
        public bool TransferStock(long stockId, int fromLocationId, int toLocationId, int quantity, string uom, string transferCode)
        {
            if (fromLocationId == toLocationId)
            {
                MessageBox.Show("Cannot transfer to the same location.");
                return false;
            }

            if (quantity <= 0)
            {
                MessageBox.Show("Quantity must be greater than zero.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(uom) || string.IsNullOrWhiteSpace(transferCode))
            {
                MessageBox.Show("Unit type and transfer code are required for transfers.");
                return false;
            }

            // --- Single transaction starts here ---
            using var conn = new SQLiteConnection(connectionString);
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                // Deduct from source
                string sqlDeduct = "UPDATE INVENTORY_LOCATIONS SET Quantity = Quantity - @Qty WHERE StockID=@StockID AND LocationID=@FromLoc;";
                using var cmdDeduct = new SQLiteCommand(sqlDeduct, conn, transaction);
                cmdDeduct.Parameters.AddWithValue("@Qty", quantity);
                cmdDeduct.Parameters.AddWithValue("@StockID", stockId);
                cmdDeduct.Parameters.AddWithValue("@FromLoc", fromLocationId);
                cmdDeduct.ExecuteNonQuery();

                // Add to destination
                string sqlAdd = @"
            INSERT INTO INVENTORY_LOCATIONS (StockID, LocationID, Quantity)
            VALUES (@StockID, @ToLoc, @Qty)
            ON CONFLICT(StockID, LocationID) DO UPDATE SET Quantity = Quantity + @Qty;";
                using var cmdAdd = new SQLiteCommand(sqlAdd, conn, transaction);
                cmdAdd.Parameters.AddWithValue("@StockID", stockId);
                cmdAdd.Parameters.AddWithValue("@ToLoc", toLocationId);
                cmdAdd.Parameters.AddWithValue("@Qty", quantity);
                cmdAdd.ExecuteNonQuery();

                // Log movement
                string sqlMove = @"
            INSERT INTO STOCK_MOVEMENTS
            (StockID, FromLocationID, ToLocationID, Quantity, UnitType, TransferCode, MovementDate)
            VALUES (@StockID, @FromLoc, @ToLoc, @Qty, @Unit, @Code, @Date);";
                using var cmdMove = new SQLiteCommand(sqlMove, conn, transaction);
                cmdMove.Parameters.AddWithValue("@StockID", stockId);
                cmdMove.Parameters.AddWithValue("@FromLoc", fromLocationId);
                cmdMove.Parameters.AddWithValue("@ToLoc", toLocationId);
                cmdMove.Parameters.AddWithValue("@Qty", quantity);
                cmdMove.Parameters.AddWithValue("@Unit", uom);
                cmdMove.Parameters.AddWithValue("@Code", transferCode);
                cmdMove.Parameters.AddWithValue("@Date", DateTime.Now);
                cmdMove.ExecuteNonQuery();

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show($"Transfer failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Logs stock movements to STOCK_MOVEMENTS table.
        /// </summary>
        private void LogStockMovement(SQLiteConnection conn, long stockId, int? fromLocationId, int? toLocationId,
                                      int quantity, string uom, string transferCode)
        {
            string insert = @"
                INSERT INTO STOCK_MOVEMENTS 
                (StockID, FromLocationID, ToLocationID, Quantity, UnitType, TransferCode, MovementDate)
                VALUES (@StockID, @FromLocationID, @ToLocationID, @Quantity, @UnitType, @TransferCode, @MovementDate);
            ";

            using (var cmd = new SQLiteCommand(insert, conn))
            {
                cmd.Parameters.AddWithValue("@StockID", stockId);
                cmd.Parameters.AddWithValue("@FromLocationID", (object)fromLocationId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ToLocationID", (object)toLocationId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@UnitType", uom);
                cmd.Parameters.AddWithValue("@TransferCode", string.IsNullOrWhiteSpace(transferCode) ? DBNull.Value : (object)transferCode);
                cmd.Parameters.AddWithValue("@MovementDate", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region LowStock & StockPerLocation (Reporting)

        public DataTable GetLowStock(int threshold)
        {
            DataTable dt = new DataTable();
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT i.StockID, p.PartName, p.PartNumber, p.Brand, m.ModelName, i.Quantity, s.SupplierName
                    FROM INVENTORY i
                    JOIN PARTS p ON i.PartID = p.PartID
                    JOIN MOTORCYCLE_MODELS m ON i.ModelID = m.ModelID
                    JOIN SUPPLIERS s ON i.SupplierID = s.SupplierID
                    WHERE i.Quantity <= @Threshold;
                ";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Threshold", threshold);
                    using (var adapter = new SQLiteDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable GetStockPerLocation()
        {
            DataTable dt = new DataTable();
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT 
                        i.StockID,
                        p.PartName,
                        p.PartNumber,
                        p.Brand,
                        m.ModelName,
                        l.LocationName,
                        il.Quantity AS LocationQty
                    FROM INVENTORY_LOCATIONS il
                    JOIN INVENTORY i ON il.StockID = i.StockID
                    JOIN PARTS p ON i.PartID = p.PartID
                    JOIN MOTORCYCLE_MODELS m ON i.ModelID = m.ModelID
                    JOIN LOCATIONS l ON il.LocationID = l.LocationID
                    ORDER BY l.LocationName, m.ModelName, p.PartName;
                ";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var adapter = new SQLiteDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        #endregion

        public void AddOrUpdateItem(string model, string brand, string partName, string partNumber,
        int quantity, decimal srp, decimal wsPrice, string supplier)
        {
            using var conn = new SQLiteConnection("Data Source=SumulongInventory.db;Version=3;");
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                // 1. Ensure model exists
                long modelId;
                using (var cmd = new SQLiteCommand("SELECT ModelID FROM MOTORCYCLE_MODELS WHERE ModelName=@Model", conn))
                {
                    cmd.Parameters.AddWithValue("@Model", model);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                        modelId = Convert.ToInt64(result);
                    else
                    {
                        using var insert = new SQLiteCommand("INSERT INTO MOTORCYCLE_MODELS (ModelName) VALUES (@Model); SELECT last_insert_rowid();", conn);
                        insert.Parameters.AddWithValue("@Model", model);
                        modelId = (long)insert.ExecuteScalar();
                    }
                }

                // 2. Ensure supplier exists
                long supplierId;
                using (var cmd = new SQLiteCommand("SELECT SupplierID FROM SUPPLIERS WHERE SupplierName=@Supplier", conn))
                {
                    cmd.Parameters.AddWithValue("@Supplier", supplier);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                        supplierId = Convert.ToInt64(result);
                    else
                    {
                        using var insert = new SQLiteCommand("INSERT INTO SUPPLIERS (SupplierName) VALUES (@Supplier); SELECT last_insert_rowid();", conn);
                        insert.Parameters.AddWithValue("@Supplier", supplier);
                        supplierId = (long)insert.ExecuteScalar();
                    }
                }

                // 3. Ensure part exists
                long partId;
                using (var cmd = new SQLiteCommand("SELECT PartID FROM PARTS WHERE PartName=@Part AND PartNumber=@PartNumber", conn))
                {
                    cmd.Parameters.AddWithValue("@Part", partName);
                    cmd.Parameters.AddWithValue("@PartNumber", partNumber);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                        partId = Convert.ToInt64(result);
                    else
                    {
                        using var insert = new SQLiteCommand(
                            "INSERT INTO PARTS (PartName, PartNumber, Brand) VALUES (@Part, @PartNumber, @Brand); SELECT last_insert_rowid();", conn);
                        insert.Parameters.AddWithValue("@Part", partName);
                        insert.Parameters.AddWithValue("@PartNumber", partNumber);
                        insert.Parameters.AddWithValue("@Brand", brand);
                        partId = (long)insert.ExecuteScalar();
                    }
                }

                // 4. Insert into INVENTORY
                using (var cmd = new SQLiteCommand(
                    "INSERT INTO INVENTORY (PartID, ModelID, SupplierID, Quantity, SRP, WS_Price, InternalCode) VALUES (@PartID,@ModelID,@SupplierID,@Qty,@SRP,@WS,@Code);", conn))
                {
                    cmd.Parameters.AddWithValue("@PartID", partId);
                    cmd.Parameters.AddWithValue("@ModelID", modelId);
                    cmd.Parameters.AddWithValue("@SupplierID", supplierId);
                    cmd.Parameters.AddWithValue("@Qty", quantity);
                    cmd.Parameters.AddWithValue("@SRP", srp);
                    cmd.Parameters.AddWithValue("@WS", wsPrice);
                    cmd.Parameters.AddWithValue("@Code", Guid.NewGuid().ToString());
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
