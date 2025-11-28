namespace Sumulong_Enterprise.Forms.Reports
{
    partial class InventoryOverviewForm
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dataGridView1;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;

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
            panel1 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(20, 150);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(740, 250);
            dataGridView1.TabIndex = 0;
            dataGridView1.ReadOnly = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AllowUserToAddRows = false;
            // 
            // panel1
            // 
            panel1.Location = new Point(50, 20);
            panel1.Size = new Size(200, 100);
            // 
            // panel2
            // 
            panel2.Location = new Point(300, 20);
            panel2.Size = new Size(200, 100);
            // 
            // panel3
            // 
            panel3.Location = new Point(550, 20);
            panel3.Size = new Size(200, 100);
            // 
            // InventoryOverviewForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            Controls.Add(panel2);
            Controls.Add(panel3);
            Controls.Add(dataGridView1);
            Name = "InventoryOverviewForm";
            Text = "Inventory Overview";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }
    }
}
