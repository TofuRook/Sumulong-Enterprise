using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sumulong_Enterprise
{
    public class InventoryManager
    {
        private string connectionString = "Data Source=SumulongInventory.db;Version=3;";

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
                        il.Quantity,        -- Quantity is retrieved from INVENTORY_LOCATIONS
                        s.SRP,
                        s.WS_Price,
                        s.InternalCode
                    FROM INVENTORY s
                    JOIN PARTS p ON s.PartID = p.PartID
                    JOIN MOTORCYCLE_MODELS m ON s.ModelID = m.ModelID
                    JOIN SUPPLIERS sup ON s.SupplierID = sup.SupplierID
                    JOIN INVENTORY_LOCATIONS il ON il.StockID = s.StockID -- Link to location quantity
                    JOIN LOCATIONS l ON il.LocationID = l.LocationID;      -- Link to location name
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
            List<string> filters = new List<string>();
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();

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
            {
                query += " WHERE " + string.Join(" AND ", filters);
            }

            DataTable table = new DataTable();
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                    using (var da = new SQLiteDataAdapter(cmd))
                    {
                        da.Fill(table);
                    }
                }
            }

            return table;
        }

        public void PopulateComboBox(ComboBox comboBox, string query)
        {
            comboBox.Items.Clear();
            comboBox.Items.Insert(0, " "); 

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        comboBox.Items.Add(reader[0].ToString());
                }
            }
        }

        public void ClearFilters(ComboBox comboBox)
        {
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }

        public void AddStock(long stockId, long locationId, int quantity, string unitType, string user)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be positive");

            using var conn = new SQLiteConnection(connectionString);
            conn.Open();
            using var transaction = conn.BeginTransaction();

            // Update INVENTORY_LOCATIONS
            string upsertSql = @"
                INSERT INTO INVENTORY_LOCATIONS (StockID, LocationID, Quantity, UnitType)
                VALUES (@StockID, @LocationID, @Quantity, @UnitType)
                ON CONFLICT(StockID, LocationID) DO UPDATE SET Quantity = Quantity + @Quantity;";
            using (var cmd = new SQLiteCommand(upsertSql, conn))
            {
                cmd.Parameters.AddWithValue("@StockID", stockId);
                cmd.Parameters.AddWithValue("@LocationID", locationId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@UnitType", unitType);
                cmd.ExecuteNonQuery();
            }

            // Log movement
            string logSql = @"
                INSERT INTO STOCK_MOVEMENTS 
                (StockID, FromLocationID, ToLocationID, Quantity, UnitType, DateTime, User, MovementType)
                VALUES (@StockID, NULL, @LocationID, @Quantity, @UnitType, @DateTime, @User, 'Add');";
            using (var cmd = new SQLiteCommand(logSql, conn))
            {
                cmd.Parameters.AddWithValue("@StockID", stockId);
                cmd.Parameters.AddWithValue("@LocationID", locationId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@UnitType", unitType);
                cmd.Parameters.AddWithValue("@DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@User", user);
                cmd.ExecuteNonQuery();
            }

            transaction.Commit();
        }

        public void DeductStock(long stockId, long locationId, int quantity, string unitType, string user)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be positive");

            using var conn = new SQLiteConnection(connectionString);
            conn.Open();
            using var transaction = conn.BeginTransaction();

            // Check current stock
            long currentQty = Convert.ToInt64(new SQLiteCommand(
                "SELECT Quantity FROM INVENTORY_LOCATIONS WHERE StockID=@StockID AND LocationID=@LocationID;",
                conn)
            { Parameters = { new SQLiteParameter("@StockID", stockId), new SQLiteParameter("@LocationID", locationId) } }
                .ExecuteScalar() ?? 0);

            if (currentQty < quantity)
                throw new InvalidOperationException("Not enough stock to deduct");

            // Update INVENTORY_LOCATIONS
            string updateSql = "UPDATE INVENTORY_LOCATIONS SET Quantity = Quantity - @Quantity WHERE StockID=@StockID AND LocationID=@LocationID;";
            using (var cmd = new SQLiteCommand(updateSql, conn))
            {
                cmd.Parameters.AddWithValue("@StockID", stockId);
                cmd.Parameters.AddWithValue("@LocationID", locationId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.ExecuteNonQuery();
            }

            // Log movement
            string logSql = @"
                INSERT INTO STOCK_MOVEMENTS 
                (StockID, FromLocationID, ToLocationID, Quantity, UnitType, DateTime, User, MovementType)
                VALUES (@StockID, @LocationID, NULL, @Quantity, @UnitType, @DateTime, @User, 'Deduct');";
            using (var cmd = new SQLiteCommand(logSql, conn))
            {
                cmd.Parameters.AddWithValue("@StockID", stockId);
                cmd.Parameters.AddWithValue("@LocationID", locationId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@UnitType", unitType);
                cmd.Parameters.AddWithValue("@DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@User", user);
                cmd.ExecuteNonQuery();
            }

            transaction.Commit();
        }

        public void TransferStock(long stockId, long fromLocationId, long toLocationId, int quantity, string unitType, string transferCode, string user)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be positive");
            if (string.IsNullOrWhiteSpace(transferCode)) throw new ArgumentException("Transfer code required");

            using var conn = new SQLiteConnection(connectionString);
            conn.Open();
            using var transaction = conn.BeginTransaction();

            // Check current stock at source
            long currentQty = Convert.ToInt64(new SQLiteCommand(
                "SELECT Quantity FROM INVENTORY_LOCATIONS WHERE StockID=@StockID AND LocationID=@FromLocationId;",
                conn)
            { Parameters = { new SQLiteParameter("@StockID", stockId), new SQLiteParameter("@FromLocationId", fromLocationId) } }
                .ExecuteScalar() ?? 0);

            if (currentQty < quantity)
                throw new InvalidOperationException("Not enough stock to transfer");

            // Deduct from source
            new SQLiteCommand(
                "UPDATE INVENTORY_LOCATIONS SET Quantity = Quantity - @Quantity WHERE StockID=@StockID AND LocationID=@FromLocationId;",
                conn)
            { Parameters = { new SQLiteParameter("@StockID", stockId), new SQLiteParameter("@FromLocationId", fromLocationId), new SQLiteParameter("@Quantity", quantity) } }
            .ExecuteNonQuery();

            // Add to destination
            string upsertSql = @"
                INSERT INTO INVENTORY_LOCATIONS (StockID, LocationID, Quantity, UnitType)
                VALUES (@StockID, @ToLocationId, @Quantity, @UnitType)
                ON CONFLICT(StockID, LocationID) DO UPDATE SET Quantity = Quantity + @Quantity;";
            new SQLiteCommand(upsertSql, conn)
            { Parameters = { new SQLiteParameter("@StockID", stockId), new SQLiteParameter("@ToLocationId", toLocationId), new SQLiteParameter("@Quantity", quantity), new SQLiteParameter("@UnitType", unitType) } }
            .ExecuteNonQuery();

            // Log movement
            string logSql = @"
                INSERT INTO STOCK_MOVEMENTS
                (StockID, FromLocationID, ToLocationID, Quantity, UnitType, TransferCode, DateTime, User, MovementType)
                VALUES (@StockID, @FromLocationId, @ToLocationId, @Quantity, @UnitType, @TransferCode, @DateTime, @User, 'Transfer');";
            new SQLiteCommand(logSql, conn)
            {
                Parameters =
                {
                    new SQLiteParameter("@StockID", stockId),
                    new SQLiteParameter("@FromLocationId", fromLocationId),
                    new SQLiteParameter("@ToLocationId", toLocationId),
                    new SQLiteParameter("@Quantity", quantity),
                    new SQLiteParameter("@UnitType", unitType),
                    new SQLiteParameter("@TransferCode", transferCode),
                    new SQLiteParameter("@DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    new SQLiteParameter("@User", user)
                }
            }.ExecuteNonQuery();

            transaction.Commit();
        }
    }
}