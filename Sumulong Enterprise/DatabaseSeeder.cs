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

                using var cmd = new SQLiteCommand("PRAGMA journal_mode=WAL;", conn);
                cmd.ExecuteNonQuery();

                // Seed Locations
                string[] locations = { "Main Warehouse", "Secondary Warehouse" };
                foreach (var loc in locations)
                {
                    using var cmdLoc = new SQLiteCommand(
                        "INSERT OR IGNORE INTO LOCATIONS (LocationName) VALUES (@LocationName);", conn);
                    cmdLoc.Parameters.AddWithValue("@LocationName", loc);
                    cmdLoc.ExecuteNonQuery();
                }

                // Seed Suppliers
                string[] suppliers = { "Default Supplier", "Yamaha Supplier" };
                foreach (var sup in suppliers)
                {
                    using var cmdSup = new SQLiteCommand(
                        "INSERT OR IGNORE INTO SUPPLIERS (SupplierName) VALUES (@SupplierName);", conn);
                    cmdSup.Parameters.AddWithValue("@SupplierName", sup);
                    cmdSup.ExecuteNonQuery();
                }

                // Seed Models
                string[] models = { "Model X", "Model Y", "Model Z" };
                foreach (var model in models)
                {
                    using var cmdModel = new SQLiteCommand(
                        "INSERT OR IGNORE INTO MOTORCYCLE_MODELS (ModelName) VALUES (@ModelName);", conn);
                    cmdModel.Parameters.AddWithValue("@ModelName", model);
                    cmdModel.ExecuteNonQuery();
                }

                // Seed Parts
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Seeding failed: {ex.Message}");
            }
        }
    }
}
