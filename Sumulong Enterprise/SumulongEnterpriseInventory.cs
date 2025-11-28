using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Sumulong_Enterprise
{
    public partial class SumulongEnterpriseInventory : Form
    {
        private DataTable inventoryTable = new DataTable();
        private InventoryManager manager = new InventoryManager();



        public SumulongEnterpriseInventory()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadInventory();
            FillComboBox();
        }


        private void searchButton_Click(object sender, EventArgs e)
        {
            UseFilters();
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
                // Get the StockID of the selected row
                long stockId = Convert.ToInt64(dataGridViewInventory.Rows[e.RowIndex]
                    .Cells["StockID"].Value);

                // Open the ItemDetails form for that stock
                ItemDetails detailsForm = new ItemDetails(stockId);
                detailsForm.ShowDialog();
            }
        }


        public void LoadInventory()
        {
            var dt = manager.LoadInventory();
            dataGridViewInventory.DataSource = dt;
            FormatInventoryGrid();
        }

        public void FillComboBox()
        {
            manager.PopulateComboBox(modelComboBox,
                "SELECT DISTINCT m.ModelName FROM MOTORCYCLE_MODELS m " +
                "JOIN INVENTORY s ON s.ModelID = m.ModelID;");
            manager.PopulateComboBox(brandComboBox, "SELECT DISTINCT Brand FROM PARTS;");
            manager.PopulateComboBox(partComboBox, "SELECT DISTINCT PartName FROM PARTS;");
        }

        public void UseFilters()
        {
            var dt = manager.FilterInventory(
                        brandComboBox.Text,
                        modelComboBox.Text,
                        partComboBox.Text
                    );

            dataGridViewInventory.DataSource = dt;
            FormatInventoryGrid();
        }

        public void ClearFilters()
        {
            manager.ClearFilters(brandComboBox);
            manager.ClearFilters(modelComboBox);
            manager.ClearFilters(partComboBox);
        }

        private void FormatInventoryGrid()
        {
            var grid = dataGridViewInventory;

            if (grid.Columns.Count == 0)
                return;

            grid.Columns["ModelName"].HeaderText = "Model/Motorcycle";
            grid.Columns["Brand"].HeaderText = "Brand";
            grid.Columns["PartName"].HeaderText = "Part Name";
            grid.Columns["PartNumber"].HeaderText = "Part Number";
            grid.Columns["Quantity"].HeaderText = "QTY";
            grid.Columns["SRP"].HeaderText = "SRP";
            grid.Columns["WS_Price"].HeaderText = "WS Price";
            grid.Columns["SupplierName"].HeaderText = "Supplier";

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
    }
}
