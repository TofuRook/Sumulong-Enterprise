using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Sumulong_Enterprise
{
    public class InventoryMovementManager
    {
        private string connectionString = "Data Source=SumulongInventory.db;Version=3;";

        /// <summary>
        /// Adds stock to a location.
        /// </summary>
        public bool AddStock(long stockId, long locationId, int quantity, string unitType, string user, string? transferCode = "")
        {
            if (quantity <= 0)
            {
                MessageBox.Show("Quantity must be greater than zero.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(unitType))
            {
                MessageBox.Show("Unit type is required.");
                return false;
            }

            using var conn = new SQLiteConnection(connectionString);
            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {
                // 1. Update INVENTORY_LOCATIONS
                string sqlLoc = @"
                    INSERT INTO INVENTORY_LOCATIONS (StockID, LocationID, Quantity, UnitType)
                    VALUES (@StockID,@LocationID,@Quantity,@UnitType)
                    ON CONFLICT(StockID, LocationID) DO UPDATE SET Quantity = Quantity + @Quantity;
                ";
                using var cmdLoc = new SQLiteCommand(sqlLoc, conn);
                cmdLoc.Parameters.AddWithValue("@StockID", stockId);
                cmdLoc.Parameters.AddWithValue("@LocationID", locationId);
                cmdLoc.Parameters.AddWithValue("@Quantity", quantity);
                cmdLoc.Parameters.AddWithValue("@UnitType", unitType);
                cmdLoc.ExecuteNonQuery();

                // 2. Update total INVENTORY
                string sqlInv = "UPDATE INVENTORY SET Quantity = Quantity + @Qty WHERE StockID=@StockID;";
                using var cmdInv = new SQLiteCommand(sqlInv, conn);
                cmdInv.Parameters.AddWithValue("@Qty", quantity);
                cmdInv.Parameters.AddWithValue("@StockID", stockId);
                cmdInv.ExecuteNonQuery();

                // 3. Log STOCK_MOVEMENTS
                string sqlMove = @"
                    INSERT INTO STOCK_MOVEMENTS
                    (StockID, FromLocationID, ToLocationID, Quantity, UnitType, TransferCode, DateTime, User, MovementType)
                    VALUES (@StockID,NULL,@ToLocation,@Qty,@Unit,@Code,@DateTime,@User,'ADD');
                ";
                using var cmdMove = new SQLiteCommand(sqlMove, conn);
                cmdMove.Parameters.AddWithValue("@StockID", stockId);
                cmdMove.Parameters.AddWithValue("@ToLocation", locationId);
                cmdMove.Parameters.AddWithValue("@Qty", quantity);
                cmdMove.Parameters.AddWithValue("@Unit", unitType);
                cmdMove.Parameters.AddWithValue("@Code", transferCode ?? "");
                cmdMove.Parameters.AddWithValue("@DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmdMove.Parameters.AddWithValue("@User", user);
                cmdMove.ExecuteNonQuery();

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show($"Add stock failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Deducts stock from a location.
        /// </summary>
        public bool DeductStock(long stockId, long locationId, int quantity, string unitType, string user, string? transferCode = "")
        {
            if (quantity <= 0)
            {
                MessageBox.Show("Quantity must be greater than zero.");
                return false;
            }

            using var conn = new SQLiteConnection(connectionString);
            conn.Open();

            using var transaction = conn.BeginTransaction();
            try
            {
                // 1. Check current stock at location
                long currentQty = Convert.ToInt64(new SQLiteCommand(
                    "SELECT Quantity FROM INVENTORY_LOCATIONS WHERE StockID=@StockID AND LocationID=@LocID;",
                    conn,
                    transaction
                )
                {
                    Parameters = {
                    new SQLiteParameter("@StockID", stockId),
                    new SQLiteParameter("@LocID", locationId)
                }
                }.ExecuteScalar() ?? 0);

                if (currentQty < quantity)
                {
                    MessageBox.Show("Not enough stock to deduct.");
                    return false;
                }

                // 2. Update INVENTORY_LOCATIONS
                string sqlLoc = "UPDATE INVENTORY_LOCATIONS SET Quantity = Quantity - @Qty WHERE StockID=@StockID AND LocationID=@LocID;";
                using var cmdLoc = new SQLiteCommand(sqlLoc, conn, transaction);
                cmdLoc.Parameters.AddWithValue("@Qty", quantity);
                cmdLoc.Parameters.AddWithValue("@StockID", stockId);
                cmdLoc.Parameters.AddWithValue("@LocID", locationId);
                cmdLoc.ExecuteNonQuery();

                // 3. Update total INVENTORY
                string sqlInv = "UPDATE INVENTORY SET Quantity = Quantity - @Qty WHERE StockID=@StockID;";
                using var cmdInv = new SQLiteCommand(sqlInv, conn, transaction);
                cmdInv.Parameters.AddWithValue("@Qty", quantity);
                cmdInv.Parameters.AddWithValue("@StockID", stockId);
                cmdInv.ExecuteNonQuery();

                // 4. Log STOCK_MOVEMENTS
                string sqlMove = @"
                    INSERT INTO STOCK_MOVEMENTS
                    (StockID, FromLocationID, ToLocationID, Quantity, UnitType, TransferCode, DateTime, User, MovementType)
                    VALUES (@StockID,@FromLocation,NULL,@Qty,@Unit,@Code,@DateTime,@User,'DEDUCT');
                ";
                using var cmdMove = new SQLiteCommand(sqlMove, conn, transaction);
                cmdMove.Parameters.AddWithValue("@StockID", stockId);
                cmdMove.Parameters.AddWithValue("@FromLocation", locationId);
                cmdMove.Parameters.AddWithValue("@Qty", quantity);
                cmdMove.Parameters.AddWithValue("@Unit", unitType);
                cmdMove.Parameters.AddWithValue("@Code", transferCode ?? "");
                cmdMove.Parameters.AddWithValue("@DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmdMove.Parameters.AddWithValue("@User", user);
                cmdMove.ExecuteNonQuery();

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show($"Deduct stock failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Transfers stock from one location to another.
        /// </summary>
        public bool TransferStock(long stockId, long fromLocationId, long toLocationId, int quantity, string unitType, string user, string transferCode)
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

            if (string.IsNullOrWhiteSpace(unitType))
            {
                MessageBox.Show("Unit type is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(transferCode))
            {
                MessageBox.Show("Transfer code is required.");
                return false;
            }

            using var conn = new SQLiteConnection(connectionString);
            conn.Open();

            using var transaction = conn.BeginTransaction();
            try
            {
                // Check available stock at source
                long currentQty = Convert.ToInt64(new SQLiteCommand(
                    "SELECT Quantity FROM INVENTORY_LOCATIONS WHERE StockID=@StockID AND LocationID=@LocID;",
                    conn, transaction
                )
                {
                    Parameters = {
                    new SQLiteParameter("@StockID", stockId),
                    new SQLiteParameter("@LocID", fromLocationId)
                }
                }.ExecuteScalar() ?? 0);

                if (currentQty < quantity)
                {
                    MessageBox.Show("Not enough stock to transfer.");
                    return false;
                }

                // Deduct from source
                string sqlDeduct = "UPDATE INVENTORY_LOCATIONS SET Quantity = Quantity - @Qty WHERE StockID=@StockID AND LocationID=@FromLoc;";
                using var cmdDeduct = new SQLiteCommand(sqlDeduct, conn, transaction);
                cmdDeduct.Parameters.AddWithValue("@Qty", quantity);
                cmdDeduct.Parameters.AddWithValue("@StockID", stockId);
                cmdDeduct.Parameters.AddWithValue("@FromLoc", fromLocationId);
                cmdDeduct.ExecuteNonQuery();

                // Add to destination
                string sqlAdd = @"
                    INSERT INTO INVENTORY_LOCATIONS (StockID, LocationID, Quantity, UnitType)
                    VALUES (@StockID,@ToLoc,@Qty,@Unit)
                    ON CONFLICT(StockID, LocationID) DO UPDATE SET Quantity = Quantity + @Qty;
                ";
                using var cmdAdd = new SQLiteCommand(sqlAdd, conn, transaction);
                cmdAdd.Parameters.AddWithValue("@StockID", stockId);
                cmdAdd.Parameters.AddWithValue("@ToLoc", toLocationId);
                cmdAdd.Parameters.AddWithValue("@Qty", quantity);
                cmdAdd.Parameters.AddWithValue("@Unit", unitType);
                cmdAdd.ExecuteNonQuery();

                // Log movement
                string sqlMove = @"
                    INSERT INTO STOCK_MOVEMENTS
                    (StockID, FromLocationID, ToLocationID, Quantity, UnitType, TransferCode, DateTime, User, MovementType)
                    VALUES (@StockID,@FromLoc,@ToLoc,@Qty,@Unit,@Code,@DateTime,@User,'TRANSFER');
                ";
                using var cmdMove = new SQLiteCommand(sqlMove, conn, transaction);
                cmdMove.Parameters.AddWithValue("@StockID", stockId);
                cmdMove.Parameters.AddWithValue("@FromLoc", fromLocationId);
                cmdMove.Parameters.AddWithValue("@ToLoc", toLocationId);
                cmdMove.Parameters.AddWithValue("@Qty", quantity);
                cmdMove.Parameters.AddWithValue("@Unit", unitType);
                cmdMove.Parameters.AddWithValue("@Code", transferCode);
                cmdMove.Parameters.AddWithValue("@DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmdMove.Parameters.AddWithValue("@User", user);
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
    }
}
