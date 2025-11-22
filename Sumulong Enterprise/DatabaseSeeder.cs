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

                    // --- Reference Data ---
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

                    // --- Insert Inventory ---
                    Random rnd = new Random();

                    foreach (var model in models)
                    {
                        foreach (var part in parts)
                        {
                            // Get reference IDs
                            long locationId = (long)new SQLiteCommand(
                                "SELECT LocationID FROM LOCATIONS WHERE LocationName='Main Warehouse';", conn).ExecuteScalar();

                            long supplierId = (long)new SQLiteCommand(
                                "SELECT SupplierID FROM SUPPLIERS WHERE SupplierName='Default Supplier';", conn).ExecuteScalar();

                            long modelId;
                            using (var cmdModelId = new SQLiteCommand(
                                "SELECT ModelID FROM MOTORCYCLE_MODELS WHERE ModelName=@ModelName;", conn))
                            {
                                cmdModelId.Parameters.AddWithValue("@ModelName", model);
                                modelId = (long)cmdModelId.ExecuteScalar();
                            }

                            long partId;
                            using (var cmdPartId = new SQLiteCommand(
                                "SELECT PartID FROM PARTS WHERE PartName=@PartName AND PartNumber=@PartNumber AND Brand=@Brand;", conn))
                            {
                                cmdPartId.Parameters.AddWithValue("@PartName", part.Name);
                                cmdPartId.Parameters.AddWithValue("@PartNumber", part.Number);
                                cmdPartId.Parameters.AddWithValue("@Brand", part.Brand);
                                partId = (long)cmdPartId.ExecuteScalar();
                            }

                            // Insert into inventory
                            using (var cmdInv = new SQLiteCommand(
                                @"INSERT OR IGNORE INTO INVENTORY 
                                (PartID, ModelID, SupplierID, LocationID, Quantity, SRP, WS_Price, InternalCode)
                                VALUES (@PartID, @ModelID, @SupplierID, @LocationID, @Quantity, @SRP, @WS_Price, @InternalCode);", conn))
                            {
                                cmdInv.Parameters.AddWithValue("@PartID", partId);
                                cmdInv.Parameters.AddWithValue("@ModelID", modelId);
                                cmdInv.Parameters.AddWithValue("@SupplierID", supplierId);
                                cmdInv.Parameters.AddWithValue("@LocationID", locationId);
                                cmdInv.Parameters.AddWithValue("@Quantity", rnd.Next(5, 50));
                                cmdInv.Parameters.AddWithValue("@SRP", rnd.Next(50, 500));
                                cmdInv.Parameters.AddWithValue("@WS_Price", rnd.Next(40, 400));
                                cmdInv.Parameters.AddWithValue("@InternalCode", $"{part.Number}-{model}");
                                cmdInv.ExecuteNonQuery();
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

