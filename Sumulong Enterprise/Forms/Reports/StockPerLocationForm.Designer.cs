namespace Sumulong_Enterprise.Forms.Reports
{
    partial class StockPerLocationForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnReload;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            btnReload = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Location = new Point(20, 20);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.Size = new Size(650, 350);
            dataGridView1.TabIndex = 0;
            // 
            // btnReload
            // 
            btnReload.Location = new Point(280, 390);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(120, 40);
            btnReload.TabIndex = 1;
            btnReload.Text = "Reload";
            btnReload.UseVisualStyleBackColor = true;
            btnReload.Click += btnReload_Click;
            // 
            // StockPerLocationForm
            // 
            ClientSize = new Size(700, 450);
            Controls.Add(dataGridView1);
            Controls.Add(btnReload);
            Name = "StockPerLocationForm";
            Text = "Stock Per Location";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }
    }
}
