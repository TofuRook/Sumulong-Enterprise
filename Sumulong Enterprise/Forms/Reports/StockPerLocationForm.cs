using System;
using System.Data;
using System.Windows.Forms;

namespace Sumulong_Enterprise.Forms.Reports
{
    public partial class StockPerLocationForm : Form
    {
        private InventoryManager manager = new InventoryManager();

        public StockPerLocationForm()
        {
            InitializeComponent();
            LoadStockPerLocation();
        }

        private void LoadStockPerLocation()
        {
            try
            {
                DataTable dt = manager.GetStockPerLocation();
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadStockPerLocation();
        }
    }
}
