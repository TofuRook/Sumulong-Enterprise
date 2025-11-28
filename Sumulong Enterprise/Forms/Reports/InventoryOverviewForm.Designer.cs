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
            dataGridView1.Location = new Point(202, 220);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(391, 150);
            dataGridView1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Location = new Point(50, 39);
            panel1.Name = "panel1";
            panel1.Size = new Size(200, 100);
            panel1.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.Location = new Point(536, 39);
            panel2.Name = "panel2";
            panel2.Size = new Size(200, 100);
            panel2.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.Location = new Point(293, 39);
            panel3.Name = "panel3";
            panel3.Size = new Size(200, 100);
            panel3.TabIndex = 3;
            // 
            // InventoryOverviewForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(dataGridView1);
            Name = "InventoryOverviewForm";
            Text = "Inventory Overview";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }
    }
}
