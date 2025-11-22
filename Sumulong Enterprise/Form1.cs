using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Sumulong_Enterprise
{
    public partial class Form1 : Form
    {
        private DataTable inventoryTable = new DataTable();
        private InventoryManager manager = new InventoryManager();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Load inventory into DataGridView using the InventoryManager
            inventoryTable = manager.LoadInventory();
            dataGridViewInventory.DataSource = inventoryTable;

            // Populate ComboBoxes from the database
            manager.PopulateComboBox(modelComboBox,
                "SELECT DISTINCT m.ModelName FROM MOTORCYCLE_MODELS m JOIN INVENTORY s ON s.ModelID = m.ModelID;");
            manager.PopulateComboBox(brandComboBox, "SELECT DISTINCT Brand FROM PARTS;");
            manager.PopulateComboBox(partComboBox, "SELECT DISTINCT PartName FROM PARTS;");
        }


        private void searchButton_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void LoadInventoryData()
        {
            if (dataGridViewInventory == null)
            {
                MessageBox.Show("DataGridView not initialized.");
                return;
            }

            dataGridViewInventory.AutoGenerateColumns = true;

            try
            {
                using (var conn = new SQLiteConnection("Data Source=SumulongInventory.db;Version=3;"))
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
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridViewInventory.DataSource = table;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        private void ApplyFilters()
        {
            string filter = "";

            if (modelComboBox.SelectedItem != null)
                filter += $"ModelName = '{modelComboBox.SelectedItem}'";

            if (brandComboBox.SelectedItem != null)
                filter += (filter.Length > 0 ? " AND " : "") + $"Brand = '{brandComboBox.SelectedItem}'";

            if (partComboBox.SelectedItem != null)
                filter += (filter.Length > 0 ? " AND " : "") + $"PartName = '{partComboBox.SelectedItem}'";

            if (inventoryTable != null)
                inventoryTable.DefaultView.RowFilter = filter;
        }
    }
}
