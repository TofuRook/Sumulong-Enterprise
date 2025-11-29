using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Sumulong_Enterprise
{
    public partial class ItemDetails : Form
    {
        private long _stockId;
        private int _startTabIndex = 0;
        private SumulongEnterpriseInventory _mainForm; // store main form reference

        private InventoryManager manager = new InventoryManager();

        public ItemDetails(long stockId, int startTabIndex, SumulongEnterpriseInventory mainForm)
        {
            InitializeComponent();
            _stockId = stockId;
            _startTabIndex = startTabIndex;

            InitializeDataGridView();

            LoadItemData();
            LoadLocations();
            LoadMovementHistory();

            tabControl1.SelectedIndex = _startTabIndex;
            _startTabIndex = startTabIndex;
            _mainForm = mainForm;
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

            if (manager.AddStock(_stockId, locationId, qty, uom))
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

            if (manager.DeductStock(_stockId, locationId, qty, uom))
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

            if (manager.TransferStock(_stockId, fromLocationId, toLocationId, qty, uom, transferCode))
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
            using var conn = new SQLiteConnection("Data Source=SumulongInventory.db;Version=3;");
            conn.Open();

            string query = @"
        SELECT 
            sm.MovementID,
            fl.LocationName AS FromLocation,
            tl.LocationName AS ToLocation,
            sm.Quantity,
            sm.UnitType,
            sm.TransferCode,
            sm.MovementDate,
            CASE
                WHEN sm.FromLocationID IS NULL THEN 'ADD'
                WHEN sm.ToLocationID IS NULL THEN 'DEDUCT'
                ELSE 'TRANSFER'
            END AS MovementType
        FROM STOCK_MOVEMENTS sm
        LEFT JOIN LOCATIONS fl ON sm.FromLocationID = fl.LocationID
        LEFT JOIN LOCATIONS tl ON sm.ToLocationID = tl.LocationID
        WHERE sm.StockID = @StockID
        ORDER BY sm.MovementDate DESC;
    ";

            using var cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@StockID", _stockId);

            using var adapter = new System.Data.SQLite.SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            movementDataGridView.DataSource = dt; // make sure you have a DataGridView named movementDataGridView

            // Optional: hide MovementID column
            if (movementDataGridView.Columns.Contains("MovementID"))
                movementDataGridView.Columns["MovementID"].Visible = false;
        }

        private class ComboBoxItem
        {
            public string Text { get; set; }
            public object Tag { get; set; }
            public override string ToString() => Text;
        }

        private InventoryManager GetManager()
        {
            return manager;
        }

        private void InitializeDataGridView()
        {
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("Model", "Model");
            dataGridView1.Columns.Add("Brand", "Brand");
            dataGridView1.Columns.Add("PartName", "Part Name");
            dataGridView1.Columns.Add("PartNumber", "Part Number");
            dataGridView1.Columns.Add("Quantity", "Quantity");
            dataGridView1.Columns.Add("SRP", "SRP");
            dataGridView1.Columns.Add("WS_Price", "WS Price");
            dataGridView1.Columns.Add("Supplier", "Supplier");

            // Optional: make numeric columns right-aligned
            dataGridView1.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["SRP"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["WS_Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Optional: auto-size columns
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnSaveItems_Click_1(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                string model = row.Cells["Model"].Value?.ToString();
                string brand = row.Cells["Brand"].Value?.ToString();
                string partName = row.Cells["PartName"].Value?.ToString();

                // This is the updated partNumber handling
                string partNumber = row.Cells["PartNumber"].Value?.ToString();
                partNumber = string.IsNullOrWhiteSpace(partNumber) ? "N/A" : partNumber;

                int quantity = int.TryParse(row.Cells["Quantity"].Value?.ToString(), out int q) ? q : 0;
                decimal srp = decimal.TryParse(row.Cells["SRP"].Value?.ToString(), out decimal s) ? s : 0;
                decimal wsPrice = decimal.TryParse(row.Cells["WS_Price"].Value?.ToString(), out decimal w) ? w : 0;
                string supplier = row.Cells["Supplier"].Value?.ToString();

                if (string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(partName))
                    continue;

                try
                {
                    manager.AddOrUpdateItem(model, brand, partName, partNumber, quantity, srp, wsPrice, supplier);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving row: {ex.Message}");
                }
            }

            MessageBox.Show("Items successfully added!");
            dataGridView1.Rows.Clear();

            // Refresh main grid
            _mainForm?.RefreshInventoryGrid();
        }

        private void QuantitytextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
