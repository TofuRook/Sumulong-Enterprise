using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Sumulong_Enterprise
{
    public static class DatabaseSeeder
    {
        public static void Seed()
        {
            try
            {
                using var conn = new SQLiteConnection("Data Source=SumulongInventory.db;Version=3;");
                conn.Open();
                using (var cmd = new SQLiteCommand("PRAGMA journal_mode=WAL;", conn))
                    cmd.ExecuteNonQuery();

                // --- Locations ---
                string[] locations = { "Main Warehouse", "Secondary Warehouse" };
                foreach (var loc in locations)
                {
                    using var cmdLoc = new SQLiteCommand(
                        "INSERT OR IGNORE INTO LOCATIONS (LocationName) VALUES (@LocationName);", conn);
                    cmdLoc.Parameters.AddWithValue("@LocationName", loc);
                    cmdLoc.ExecuteNonQuery();
                }

                // --- Suppliers ---
                string[] suppliers = { "Default Supplier", "Yamaha Supplier" };
                foreach (var sup in suppliers)
                {
                    using var cmdSup = new SQLiteCommand(
                        "INSERT OR IGNORE INTO SUPPLIERS (SupplierName) VALUES (@SupplierName);", conn);
                    cmdSup.Parameters.AddWithValue("@SupplierName", sup);
                    cmdSup.ExecuteNonQuery();
                }

                // --- Models ---
                string[] models = { "Model X", "Model Y", "Model Z" };
                foreach (var model in models)
                {
                    using var cmdModel = new SQLiteCommand(
                        "INSERT OR IGNORE INTO MOTORCYCLE_MODELS (ModelName) VALUES (@ModelName);", conn);
                    cmdModel.Parameters.AddWithValue("@ModelName", model);
                    cmdModel.ExecuteNonQuery();
                }

                // --- Parts ---
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
                        @"INSERT OR IGNORE INTO PARTS (PartName, PartNumber, Brand) 
                          VALUES (@Name, @Number, @Brand);", conn);
                    cmdPart.Parameters.AddWithValue("@Name", part.Name);
                    cmdPart.Parameters.AddWithValue("@Number", part.Number);
                    cmdPart.Parameters.AddWithValue("@Brand", part.Brand);
                    cmdPart.ExecuteNonQuery();
                }

                // --- Inventory Creation & Location Assignment ---
                Random rnd = new Random();

                long mainWarehouseId = (long)new SQLiteCommand(
                    "SELECT LocationID FROM LOCATIONS WHERE LocationName='Main Warehouse';", conn).ExecuteScalar();

                long defaultSupplierId = (long)new SQLiteCommand(
                    "SELECT SupplierID FROM SUPPLIERS WHERE SupplierName='Default Supplier';", conn).ExecuteScalar();

                foreach (var model in models)
                {
                    foreach (var part in parts)
                    {
                        long modelId = (long)new SQLiteCommand(
                            "SELECT ModelID FROM MOTORCYCLE_MODELS WHERE ModelName=@ModelName;", conn)
                        { Parameters = { new SQLiteParameter("@ModelName", model) } }.ExecuteScalar();

                        long partId = (long)new SQLiteCommand(
                            @"SELECT PartID FROM PARTS 
                              WHERE PartName=@PartName AND PartNumber=@PartNumber AND Brand=@Brand;", conn)
                        {
                            Parameters =
                            {
                                new SQLiteParameter("@PartName", part.Name),
                                new SQLiteParameter("@PartNumber", part.Number),
                                new SQLiteParameter("@Brand", part.Brand)
                            }
                        }.ExecuteScalar();

                        // Insert inventory only if combination does not exist
                        long? stockId = (long?)new SQLiteCommand(
                            @"SELECT StockID FROM INVENTORY 
                              WHERE PartID=@PartID AND ModelID=@ModelID AND SupplierID=@SupplierID;", conn)
                        {
                            Parameters =
                            {
                                new SQLiteParameter("@PartID", partId),
                                new SQLiteParameter("@ModelID", modelId),
                                new SQLiteParameter("@SupplierID", defaultSupplierId)
                            }
                        }.ExecuteScalar();

                        if (stockId == null)
                        {
                            using var cmdInv = new SQLiteCommand(
                                @"INSERT INTO INVENTORY (PartID, ModelID, SupplierID, SRP, WS_Price, InternalCode) 
                                  VALUES (@PartID, @ModelID, @SupplierID, @SRP, @WS_Price, @InternalCode);
                                  SELECT last_insert_rowid();", conn);
                            cmdInv.Parameters.AddWithValue("@PartID", partId);
                            cmdInv.Parameters.AddWithValue("@ModelID", modelId);
                            cmdInv.Parameters.AddWithValue("@SupplierID", defaultSupplierId);
                            cmdInv.Parameters.AddWithValue("@SRP", rnd.Next(50, 500));
                            cmdInv.Parameters.AddWithValue("@WS_Price", rnd.Next(40, 400));
                            cmdInv.Parameters.AddWithValue("@InternalCode", $"{part.Number}-{model.Replace(" ", "")}");
                            stockId = (long)cmdInv.ExecuteScalar();
                        }

                        // Assign quantity to main warehouse if not exists
                        long? locStockId = (long?)new SQLiteCommand(
                            @"SELECT StockLocationID FROM INVENTORY_LOCATIONS 
                              WHERE StockID=@StockID AND LocationID=@LocationID;", conn)
                        {
                            Parameters =
                            {
                                new SQLiteParameter("@StockID", stockId),
                                new SQLiteParameter("@LocationID", mainWarehouseId)
                            }
                        }.ExecuteScalar();

                        if (locStockId == null)
                        {
                            using var cmdInvLoc = new SQLiteCommand(
                                @"INSERT INTO INVENTORY_LOCATIONS (StockID, LocationID, Quantity) 
                                  VALUES (@StockID, @LocationID, @Quantity);", conn);
                            cmdInvLoc.Parameters.AddWithValue("@StockID", stockId);
                            cmdInvLoc.Parameters.AddWithValue("@LocationID", mainWarehouseId);
                            cmdInvLoc.Parameters.AddWithValue("@Quantity", rnd.Next(5, 50));
                            cmdInvLoc.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show("Seeding complete!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Seeding failed: {ex.Message}");
            }
        }
    }
}
