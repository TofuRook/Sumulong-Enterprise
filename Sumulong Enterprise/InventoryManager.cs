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

        /// <summary>
        /// Loads the full inventory, showing each stock item's quantity at its specific location.
        /// </summary>
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

        /// <summary>
        /// Filters the inventory based on specified criteria.
        /// </summary>
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


        /// <summary>
        /// Populates a ComboBox with values from a specified query (e.g., list of Brands, Models).
        /// </summary>
        public void PopulateComboBox(ComboBox comboBox, string query)
        {
            comboBox.Items.Clear();
            comboBox.Items.Insert(0, " "); // Allow users to select "All" or no filter

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

        /// <summary>
        /// Resets the ComboBox selection to the default "no filter" option.
        /// </summary>
        public void ClearFilters(ComboBox comboBox)
        {
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }
    }
}