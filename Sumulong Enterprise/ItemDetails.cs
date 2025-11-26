using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Sumulong_Enterprise
{
    public partial class ItemDetails : Form
    {
        private long _stockId;
        public ItemDetails(long stockId)
        {
            InitializeComponent();
            _stockId = stockId;

            LoadItemData();
            LoadLocations();
        }


        private void Addbutton_Click(object sender, EventArgs e)
        {

        }

        private void Deductbutton_Click(object sender, EventArgs e)
        {

        }

        private void transferbutton_Click(object sender, EventArgs e)
        {

        }

        private void LoadLocations()
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=SumulongInventory.db;Version=3;"))
            {
                con.Open();
                string query = "SELECT DISTINCT LocationName FROM LOCATIONS";
                SQLiteCommand cmd = new SQLiteCommand(query, con);
                SQLiteDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    LocationcomboBox.Items.Add(reader["LocationName"].ToString());
                }
            }
        }

        private void LoadItemData()
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=SumulongInventory.db;Version=3;"))
            {
                con.Open();
                string query = @"
            SELECT 
                i.StockID,
                p.PartNumber,
                p.PartName,
                p.Brand,
                m.ModelName,
                s.SupplierName,
                i.Quantity,
                i.SRP,
                i.WS_Price,
                i.InternalCode
            FROM INVENTORY i
            JOIN PARTS p ON i.PartID = p.PartID
            JOIN MOTORCYCLE_MODELS m ON i.ModelID = m.ModelID
            JOIN SUPPLIERS s ON i.SupplierID = s.SupplierID
            WHERE i.StockID = @StockID
        ";

                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StockID", _stockId);

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        // Bind to DataGridView
                        itemdataGridView.DataSource = table;

                        // Optional: adjust column headers
                        itemdataGridView.Columns["StockID"].Visible = false; // hide internal ID
                        itemdataGridView.Columns["InternalCode"].HeaderText = "Code";
                        itemdataGridView.Columns["PartNumber"].HeaderText = "Part Number";
                    }
                }
            }
        }

    }
}
