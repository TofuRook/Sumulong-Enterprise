using System;
using System.Data;
using System.Windows.Forms;

namespace Sumulong_Enterprise.Forms.Reports
{
    public partial class LowStockForm : Form
    {
        private InventoryManager manager = new InventoryManager();

        public LowStockForm()
        {
            InitializeComponent();
            LoadLowStock();
            btnReload.Click += BtnReload_Click; // wire up the button
        }

        private void LoadLowStock()
        {
            try
            {
                int threshold = 5; // Example threshold
                DataTable dt = manager.GetLowStock(threshold);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load low stock data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnReload_Click(object sender, EventArgs e)
        {
            LoadLowStock();
        }
    }
}
