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
          if (!int.TryParse(quantityTextbox.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Enter a valid positive quantity.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SQLiteConnection con = new SQLiteConnection("Data Source=SumulongInventory.db;Version=3;"))
            {
                con.Open();

                string updateQuery = "UPDATE INVENTORY SET Quantity = Quantity + @Qty WHERE StockID = @StockID";
                SQLiteCommand cmd = new SQLiteCommand(updateQuery, con);
                cmd.Parameters.AddWithValue("@Qty", qty);
                cmd.Parameters.AddWithValue("@StockID", _stockId);
                cmd.ExecuteNonQuery();

                
                string logQuery = "INSERT INTO MOVEMENTS (StockID, ActionType, Quantity, Date) VALUES (@StockID, 'ADD', @Qty, DATETIME('now'))";
                SQLiteCommand logCmd = new SQLiteCommand(logQuery, con);
                logCmd.Parameters.AddWithValue("@StockID", _stockId);
                logCmd.Parameters.AddWithValue("@Qty", qty);
                logCmd.ExecuteNonQuery();
            }

            MessageBox.Show("Quantity added successfully!");
            LoadItemData();
            LoadMovementHistory();
        
        }

        private void Deductbutton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(quantityTextbox.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Enter a valid positive quantity.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check current stock
            int currentQty = Convert.ToInt32(((DataTable)itemdataGridView.DataSource).Rows[0]["Quantity"]);
            if (qty > currentQty)
            {
                MessageBox.Show("Insufficient stock!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SQLiteConnection con = new SQLiteConnection("Data Source=SumulongInventory.db;Version=3;"))
            {
                con.Open();

                string updateQuery = "UPDATE INVENTORY SET Quantity = Quantity - @Qty WHERE StockID = @StockID";
                SQLiteCommand cmd = new SQLiteCommand(updateQuery, con);
                cmd.Parameters.AddWithValue("@Qty", qty);
                cmd.Parameters.AddWithValue("@StockID", _stockId);
                cmd.ExecuteNonQuery();

                
                string logQuery = "INSERT INTO MOVEMENTS (StockID, ActionType, Quantity, Date) VALUES (@StockID, 'DEDUCT', @Qty, DATETIME('now'))";
                SQLiteCommand logCmd = new SQLiteCommand(logQuery, con);
                logCmd.Parameters.AddWithValue("@StockID", _stockId);
                logCmd.Parameters.AddWithValue("@Qty", qty);
                logCmd.ExecuteNonQuery();
            }

            MessageBox.Show("Quantity deducted successfully!");
            LoadItemData();
            LoadMovementHistory()
        }

        private void transferbutton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(quantityTextbox.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Invalid quantity!");
                return;
            }

            if (LocationcomboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a location.");
                return;
            }

            if (string.IsNullOrWhiteSpace(transferCodeTextbox.Text))
            {
                MessageBox.Show("Transfer code is required.");
                return;
            }

            
            int currentQty = Convert.ToInt32(((DataTable)itemdataGridView.DataSource).Rows[0]["Quantity"]);
            if (qty > currentQty)
            {
                MessageBox.Show("Insufficient stock!");
                return;
            }

            string newLocation = LocationcomboBox.SelectedItem.ToString();

            using (SQLiteConnection con = new SQLiteConnection("Data Source=SumulongInventory.db;Version=3;"))
            {
                con.Open();

                
                string deductQuery = "UPDATE INVENTORY SET Quantity = Quantity - @Qty WHERE StockID = @StockID";
                SQLiteCommand cmdDeduct = new SQLiteCommand(deductQuery, con);
                cmdDeduct.Parameters.AddWithValue("@Qty", qty);
                cmdDeduct.Parameters.AddWithValue("@StockID", _stockId);
                cmdDeduct.ExecuteNonQuery();

               
                string addQuery = @"
            INSERT INTO INVENTORY (PartID, ModelID, SupplierID, SRP, WS_Price, InternalCode, LocationName, Quantity)
            SELECT PartID, ModelID, SupplierID, SRP, WS_Price, InternalCode, @LocationName, @Qty
            FROM INVENTORY WHERE StockID = @StockID
            ON CONFLICT(PartID, ModelID, SupplierID, LocationName)
            DO UPDATE SET Quantity = Quantity + @Qty;
        ";

                SQLiteCommand cmdAdd = new SQLiteCommand(addQuery, con);
                cmdAdd.Parameters.AddWithValue("@LocationName", newLocation);
                cmdAdd.Parameters.AddWithValue("@Qty", qty);
                cmdAdd.Parameters.AddWithValue("@StockID", _stockId);
                cmdAdd.ExecuteNonQuery();

                // Log transfer
                string logQuery = @"
            INSERT INTO MOVEMENTS (StockID, ActionType, Quantity, Location, TransferCode, UnitType, Date)
            VALUES (@StockID, 'TRANSFER', @Qty, @Loc, @Code, @Unit, @Date)
        ";

                SQLiteCommand logCmd = new SQLiteCommand(logQuery, con);
                logCmd.Parameters.AddWithValue("@StockID", _stockId);
                logCmd.Parameters.AddWithValue("@Qty", qty);
                logCmd.Parameters.AddWithValue("@Loc", newLocation);
                logCmd.Parameters.AddWithValue("@Code", transferCodeTextbox.Text);
                logCmd.Parameters.AddWithValue("@Unit", unitTypeTextbox.Text);
                logCmd.Parameters.AddWithValue("@Date", transferDatePicker.Value);
                logCmd.ExecuteNonQuery();
            }

            MessageBox.Show("Transfer completed successfully.");
            LoadItemData();
            LoadMovementHistory();
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
