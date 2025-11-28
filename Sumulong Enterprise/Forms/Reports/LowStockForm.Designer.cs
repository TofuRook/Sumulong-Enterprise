namespace Sumulong_Enterprise.Forms.Reports
{
    partial class LowStockForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnReload = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Location = new System.Drawing.Point(20, 20);
            this.dataGridView1.Size = new System.Drawing.Size(650, 350);
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.AllowUserToAddRows = false;
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(280, 390);
            this.btnReload.Size = new System.Drawing.Size(120, 40);
            this.btnReload.Text = "Reload";
            this.btnReload.UseVisualStyleBackColor = true;
            // 
            // LowStockForm
            // 
            this.ClientSize = new System.Drawing.Size(700, 450);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnReload);
            this.Text = "Low Stock Report";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
