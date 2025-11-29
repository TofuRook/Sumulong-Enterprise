using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Sumulong_Enterprise
{
    public partial class SumulongEnterpriseInventory : Form
    {
        private InventoryManager manager = new InventoryManager();

        public SumulongEnterpriseInventory()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadInventory();
            FillComboBoxes();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            ClearFilters();
            LoadInventory();
        }

        private void dataGridViewInventory_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Ensure StockID column exists
                if (!dataGridViewInventory.Columns.Contains("StockID"))
                {
                    MessageBox.Show("StockID not found. Cannot open details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                long stockId = Convert.ToInt64(dataGridViewInventory.Rows[e.RowIndex].Cells["StockID"].Value);

                var itemForm = new ItemDetails(stockId, 0, this);
                itemForm.ShowDialog();
            }
        }

        public void LoadInventory()
        {
            var dt = manager.LoadInventory();
            dataGridViewInventory.DataSource = dt;
            FormatInventoryGrid();
        }

        public void FillComboBoxes()
        {
            manager.PopulateComboBox(modelComboBox,
                "SELECT DISTINCT m.ModelName FROM MOTORCYCLE_MODELS m " +
                "JOIN INVENTORY i ON i.ModelID = m.ModelID;");
            manager.PopulateComboBox(brandComboBox, "SELECT DISTINCT Brand FROM PARTS;");
            manager.PopulateComboBox(partComboBox, "SELECT DISTINCT PartName FROM PARTS;");
        }

        private void ApplyFilters()
        {
            var dt = manager.FilterInventory(
                brandComboBox.Text,
                modelComboBox.Text,
                partComboBox.Text
            );

            dataGridViewInventory.DataSource = dt;
            FormatInventoryGrid();
        }

        private void ClearFilters()
        {
            brandComboBox.SelectedIndex = -1;
            modelComboBox.SelectedIndex = -1;
            partComboBox.SelectedIndex = -1;
        }

        private void FormatInventoryGrid()
        {
            var grid = dataGridViewInventory;

            if (grid.Columns.Count == 0)
                return;

            // Rename columns for display
            if (grid.Columns.Contains("ModelName"))
                grid.Columns["ModelName"].HeaderText = "Model/Motorcycle";

            if (grid.Columns.Contains("Brand"))
                grid.Columns["Brand"].HeaderText = "Brand";

            if (grid.Columns.Contains("PartName"))
                grid.Columns["PartName"].HeaderText = "Part Name";

            if (grid.Columns.Contains("PartNumber"))
                grid.Columns["PartNumber"].HeaderText = "Part Number";

            if (grid.Columns.Contains("Quantity"))
                grid.Columns["Quantity"].HeaderText = "QTY";

            if (grid.Columns.Contains("SRP"))
                grid.Columns["SRP"].HeaderText = "SRP";

            if (grid.Columns.Contains("WS_Price"))
                grid.Columns["WS_Price"].HeaderText = "WS Price";

            if (grid.Columns.Contains("SupplierName"))
                grid.Columns["SupplierName"].HeaderText = "Supplier";

            // Optional: hide StockID column but keep it for reference
            if (grid.Columns.Contains("StockID"))
                grid.Columns["StockID"].Visible = false;

            // Set display order
            grid.Columns["ModelName"].DisplayIndex = 0;
            grid.Columns["Brand"].DisplayIndex = 1;
            grid.Columns["PartName"].DisplayIndex = 2;
            grid.Columns["PartNumber"].DisplayIndex = 3;
            grid.Columns["Quantity"].DisplayIndex = 4;
            grid.Columns["SRP"].DisplayIndex = 5;
            grid.Columns["WS_Price"].DisplayIndex = 6;
            grid.Columns["SupplierName"].DisplayIndex = 7;

            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public void RefreshInventoryGrid()
        {
            dataGridViewInventory.DataSource = manager.LoadInventory(); // or whatever method you use
            FormatInventoryGrid();
        }


        private void addButton_Click(object sender, EventArgs e)
        {
            long stockId = 0;          // Zero = new item
            int startTabIndex = 1;     // Start on Create Items tab

            var itemForm = new ItemDetails(stockId, startTabIndex, this);
            itemForm.ShowDialog();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {

        }

        private void modelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void brandComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }

}
