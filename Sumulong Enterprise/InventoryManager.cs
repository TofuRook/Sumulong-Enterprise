using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
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
                        s.Quantity,
                        s.SRP,
                        s.WS_Price,
                        s.InternalCode
                    FROM INVENTORY s
                    JOIN PARTS p ON s.PartID = p.PartID
                    JOIN MOTORCYCLE_MODELS m ON s.ModelID = m.ModelID
                    JOIN SUPPLIERS sup ON s.SupplierID = sup.SupplierID
                    JOIN LOCATIONS l ON s.LocationID = l.LocationID;
                ";

                using (var cmd = new SQLiteCommand(query, conn))
                using (var adapter = new SQLiteDataAdapter(cmd))
                {
                    adapter.Fill(table);
                }
            }

            return table;
        }

        public void PopulateComboBox(ComboBox comboBox, string query)
        {
            comboBox.Items.Clear();

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
    }
}
