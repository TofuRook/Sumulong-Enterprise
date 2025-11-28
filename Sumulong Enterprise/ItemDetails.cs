using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Sumulong_Enterprise
{
    public partial class ItemDetails : Form
    {
        private long _stockId;
        private InventoryManager _manager;

        public ItemDetails(long stockId)
        {
            InitializeComponent();
            _stockId = stockId;
            _manager = new InventoryManager();

            LoadItemData();
            LoadLocations();
            LoadMovementHistory();
        }

        private void Addbutton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(QuantitytextBox.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Enter a valid positive quantity.", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (LocationcomboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a location.", "Missing Location",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int locationId = Convert.ToInt32(((ComboBoxItem)LocationcomboBox.SelectedItem).Tag);
            string uom = "pcs"; // Or fetch from a textbox if needed

            if (_manager.AddStock(_stockId, locationId, qty, uom))
            {
                MessageBox.Show("Quantity added successfully!");
                LoadItemData();
                LoadMovementHistory();
            }
        }

        private void Deductbutton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(QuantitytextBox.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Enter a valid positive quantity.", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (LocationcomboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a location.", "Missing Location",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int locationId = Convert.ToInt32(((ComboBoxItem)LocationcomboBox.SelectedItem).Tag);
            string uom = "pcs";

            if (_manager.DeductStock(_stockId, locationId, qty, uom))
            {
                MessageBox.Show("Quantity deducted successfully!");
                LoadItemData();
                LoadMovementHistory();
            }
        }

        private void transferbutton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(QuantitytextBox.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Invalid quantity!");
                return;
            }

            if (LocationcomboBox.SelectedIndex == -1 || ToLocationcomboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select both source and destination locations.");
                return;
            }

            int fromLocationId = Convert.ToInt32(((ComboBoxItem)LocationcomboBox.SelectedItem).Tag);
            int toLocationId = Convert.ToInt32(((ComboBoxItem)ToLocationcomboBox.SelectedItem).Tag);

            if (fromLocationId == toLocationId)
            {
                MessageBox.Show("Cannot transfer to the same location.");
                return;
            }

            if (string.IsNullOrWhiteSpace(codetextBox.Text))
            {
                MessageBox.Show("Transfer code is required.");
                return;
            }

            string uom = "pcs"; // Unit of measurement
            string transferCode = codetextBox.Text;

            if (_manager.TransferStock(_stockId, fromLocationId, toLocationId, qty, uom, transferCode))
            {
                MessageBox.Show("Transfer completed successfully.");
                LoadItemData();
                LoadMovementHistory();
            }
        }


        private void LoadLocations()
        {
            LocationcomboBox.Items.Clear();
            ToLocationcomboBox.Items.Clear();

            using var conn = new SQLiteConnection("Data Source=SumulongInventory.db;Version=3;");
            conn.Open();

            using var cmd = new SQLiteCommand("SELECT LocationID, LocationName FROM LOCATIONS ORDER BY LocationName;", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var item = new ComboBoxItem
                {
                    Text = reader["LocationName"].ToString(),
                    Tag = Convert.ToInt32(reader["LocationID"])
                };

                LocationcomboBox.Items.Add(item);
                ToLocationcomboBox.Items.Add(item);
            }

            if (LocationcomboBox.Items.Count > 0) LocationcomboBox.SelectedIndex = 0;
            if (ToLocationcomboBox.Items.Count > 0) ToLocationcomboBox.SelectedIndex = 0;
        }

        private void LoadItemData()
        {
            using var conn = new SQLiteConnection("Data Source=SumulongInventory.db;Version=3;");
            conn.Open();

            string query = @"
                SELECT 
                    i.StockID,
                    p.PartNumber,
                    p.PartName,
                    p.Brand,
                    m.ModelName,
                    s.SupplierName,
                    COALESCE(SUM(il.Quantity), 0) AS Quantity,
                    i.SRP,
                    i.WS_Price,
                    i.InternalCode
                FROM INVENTORY i
                JOIN PARTS p ON i.PartID = p.PartID
                JOIN MOTORCYCLE_MODELS m ON i.ModelID = m.ModelID
                JOIN SUPPLIERS s ON i.SupplierID = s.SupplierID
                LEFT JOIN INVENTORY_LOCATIONS il ON il.StockID = i.StockID
                WHERE i.StockID = @StockID
                GROUP BY i.StockID, p.PartNumber, p.PartName, p.Brand, m.ModelName, s.SupplierName, i.SRP, i.WS_Price, i.InternalCode;
            ";

            using var cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@StockID", _stockId);

            using var adapter = new SQLiteDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);

            itemdataGridView.DataSource = table;

            if (table.Columns.Contains("StockID"))
                itemdataGridView.Columns["StockID"].Visible = false;
            if (table.Columns.Contains("InternalCode"))
                itemdataGridView.Columns["InternalCode"].HeaderText = "Code";
        }

        private void LoadMovementHistory()
        {
            // Implement movement history retrieval if needed
        }

        private class ComboBoxItem
        {
            public string Text { get; set; }
            public object Tag { get; set; }
            public override string ToString() => Text;
        }
    }
}
