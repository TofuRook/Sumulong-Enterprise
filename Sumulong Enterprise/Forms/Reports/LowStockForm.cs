using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sumulong_Enterprise.Forms.Reports
{
    public partial class LowStockForm : Form
    {
        public LowStockForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            button1 = new Button();
            ((ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(79, 27);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(529, 293);
            dataGridView1.TabIndex = 3;
            // 
            // button1
            // 
            button1.Location = new Point(275, 358);
            button1.Name = "button1";
            button1.Size = new Size(129, 58);
            button1.TabIndex = 4;
            button1.Text = "Restart";
            button1.UseVisualStyleBackColor = true;
            // 
            // StockPerLocationForm
            // 
            ClientSize = new Size(694, 494);
            Controls.Add(button1);
            Controls.Add(dataGridView1);
            Name = "StockPerLocationForm";
            ((ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);

        }
        private DataGridView dataGridView1;
        private Button button1;
    }
}
