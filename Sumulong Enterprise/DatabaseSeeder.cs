using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sumulong_Enterprise
{
    public static class DatabaseSeeder
    {
        public static void Seed()
        {
            try
            {
                using (var conn = new SQLiteConnection("Data Source=SumulongInventory.db;Version=3;"))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand("PRAGMA journal_mode=WAL;", conn))
                        cmd.ExecuteNonQuery();

                    // --- Reference Data Seeding ---
                    string[] locations = { "Main Warehouse", "Secondary Warehouse" };
                    foreach (var loc in locations)
                    {
                        using var cmdLoc = new SQLiteCommand(
                            "INSERT OR IGNORE INTO LOCATIONS (LocationName) VALUES (@LocationName);", conn);
                        cmdLoc.Parameters.AddWithValue("@LocationName", loc);
                        cmdLoc.ExecuteNonQuery();
                    }

                    string[] suppliers = { "Default Supplier", "Yamaha Supplier" };
                    foreach (var sup in suppliers)
                    {
                        using var cmdSup = new SQLiteCommand(
                            "INSERT OR IGNORE INTO SUPPLIERS (SupplierName) VALUES (@SupplierName);", conn);
                        cmdSup.Parameters.AddWithValue("@SupplierName", sup);
                        cmdSup.ExecuteNonQuery();
                    }

                    string[] models = { "Model X", "Model Y", "Model Z" };
                    foreach (var model in models)
                    {
                        using var cmdModel = new SQLiteCommand(
                            "INSERT OR IGNORE INTO MOTORCYCLE_MODELS (ModelName) VALUES (@ModelName);", conn);
                        cmdModel.Parameters.AddWithValue("@ModelName", model);
                        cmdModel.ExecuteNonQuery();
                    }

                    var parts = new[]
                    {
                        new { Name = "Brake Pad", Number = "BP-100", Brand = "Yamaha" },
                        new { Name = "Oil Filter", Number = "OF-200", Brand = "Motul" },
                        new { Name = "Clutch Plate", Number = "CP-300", Brand = "Yamaha" },
                        new { Name = "Spark Plug", Number = "SP-400", Brand = "Honda" }
                    };

                    foreach (var part in parts)
                    {
                        using var cmdPart = new SQLiteCommand(
                            "INSERT OR IGNORE INTO PARTS (PartName, PartNumber, Brand) VALUES (@Name, @Number, @Brand);", conn);
                        cmdPart.Parameters.AddWithValue("@Name", part.Name);
                        cmdPart.Parameters.AddWithValue("@Number", part.Number);
                        cmdPart.Parameters.AddWithValue("@Brand", part.Brand);
                        cmdPart.ExecuteNonQuery();
                    }

                    // --- Inventory Creation & Location Assignment ---
                    Random rnd = new Random();

                    // Get a fixed location ID for seeding the initial stock
                    long mainWarehouseId = (long)new SQLiteCommand(
                                "SELECT LocationID FROM LOCATIONS WHERE LocationName='Main Warehouse';", conn).ExecuteScalar();

                    long defaultSupplierId = (long)new SQLiteCommand(
                        "SELECT SupplierID FROM SUPPLIERS WHERE SupplierName='Default Supplier';", conn).ExecuteScalar();


                    foreach (var model in models)
                    {
                        foreach (var part in parts)
                        {
                            // Get Model ID
                            long modelId;
                            using (var cmdModelId = new SQLiteCommand(
                                "SELECT ModelID FROM MOTORCYCLE_MODELS WHERE ModelName=@ModelName;", conn))
                            {
                                cmdModelId.Parameters.AddWithValue("@ModelName", model);
                                modelId = (long)cmdModelId.ExecuteScalar();
                            }

                            // Get Part ID
                            long partId;
                            using (var cmdPartId = new SQLiteCommand(
                                "SELECT PartID FROM PARTS WHERE PartName=@PartName AND PartNumber=@PartNumber AND Brand=@Brand;", conn))
                            {
                                cmdPartId.Parameters.AddWithValue("@PartName", part.Name);
                                cmdPartId.Parameters.AddWithValue("@PartNumber", part.Number);
                                cmdPartId.Parameters.AddWithValue("@Brand", part.Brand);
                                partId = (long)cmdPartId.ExecuteScalar();
                            }

                            // 1. Insert into INVENTORY (Base Stock Item)
                            // We use SELECT last_insert_rowid() to get the ID of the new row.
                            // The INSERT OR IGNORE behavior here is tricky: it will only insert the item once per (PartID, ModelID, SupplierID).
                            string insertInventorySql = @"
                                INSERT OR IGNORE INTO INVENTORY 
                                (PartID, ModelID, SupplierID, SRP, WS_Price, InternalCode)
                                VALUES (@PartID, @ModelID, @SupplierID, @SRP, @WS_Price, @InternalCode);
                                SELECT last_insert_rowid();";

                            using (var cmdInv = new SQLiteCommand(insertInventorySql, conn))
                            {
                                cmdInv.Parameters.AddWithValue("@PartID", partId);
                                cmdInv.Parameters.AddWithValue("@ModelID", modelId);
                                cmdInv.Parameters.AddWithValue("@SupplierID", defaultSupplierId);
                                cmdInv.Parameters.AddWithValue("@SRP", rnd.Next(50, 500));
                                cmdInv.Parameters.AddWithValue("@WS_Price", rnd.Next(40, 400));
                                cmdInv.Parameters.AddWithValue("@InternalCode", $"{part.Number}-{model.Replace(" ", "")}");

                                long stockId = (long)cmdInv.ExecuteScalar();

                                // If a new INVENTORY record was created (stockId > 0 means a new row was inserted)
                                // Note: In case of IGNORE, last_insert_rowid() might still return the last successful ID.
                                // A better check is required for true "INSERT OR IGNORE" logic if not using a transaction.
                                // For seeding, we'll assume it's new and attempt to find the StockID if it exists.

                                if (stockId == 0)
                                {
                                    // If INSERT IGNORE happened, find the existing StockID
                                    using var findCmd = new SQLiteCommand(@"
                                        SELECT StockID FROM INVENTORY 
                                        WHERE PartID=@PartID AND ModelID=@ModelID AND SupplierID=@SupplierID", conn);
                                    findCmd.Parameters.AddWithValue("@PartID", partId);
                                    findCmd.Parameters.AddWithValue("@ModelID", modelId);
                                    findCmd.Parameters.AddWithValue("@SupplierID", defaultSupplierId);
                                    stockId = (long)findCmd.ExecuteScalar();
                                }

                                // 2. Insert into INVENTORY_LOCATIONS (Assign Quantity to Location)
                                // Only do this if we successfully found/created a StockID
                                if (stockId > 0)
                                {
                                    using (var cmdInvLoc = new SQLiteCommand(
                                        @"INSERT OR IGNORE INTO INVENTORY_LOCATIONS 
                                        (StockID, LocationID, Quantity)
                                        VALUES (@StockID, @LocationID, @Quantity);", conn))
                                    {
                                        cmdInvLoc.Parameters.AddWithValue("@StockID", stockId);
                                        cmdInvLoc.Parameters.AddWithValue("@LocationID", mainWarehouseId);
                                        cmdInvLoc.Parameters.AddWithValue("@Quantity", rnd.Next(5, 50));
                                        cmdInvLoc.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }

                MessageBox.Show("Seeding successful!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Seeding failed: {ex.Message}");
            }
        }
    }
}