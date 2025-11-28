using System;
using System.Data;
using System.Windows.Forms;

namespace Sumulong_Enterprise.Forms.Reports
{
    public partial class InventoryOverviewForm : Form
    {
        private InventoryManager manager = new InventoryManager();

        public InventoryOverviewForm()
        {
            InitializeComponent();
            LoadInventoryOverview();
        }

        private void LoadInventoryOverview()
        {
            try
            {
                DataTable dt = manager.LoadInventory();
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load inventory overview: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
