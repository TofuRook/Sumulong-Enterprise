namespace Sumulong_Enterprise
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            modelcolumn = new DataGridViewTextBoxColumn();
            brandcolumn = new DataGridViewTextBoxColumn();
            partcolumn = new DataGridViewTextBoxColumn();
            partnumcolumn = new DataGridViewTextBoxColumn();
            quantitycolumn = new DataGridViewTextBoxColumn();
            srpcolumn = new DataGridViewTextBoxColumn();
            wspcolumn = new DataGridViewTextBoxColumn();
            suppliercolumn = new DataGridViewTextBoxColumn();
            modelComboBox = new ComboBox();
            brandComboBox = new ComboBox();
            partComboBox = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToOrderColumns = true;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { modelcolumn, brandcolumn, partcolumn, partnumcolumn, quantitycolumn, srpcolumn, wspcolumn, suppliercolumn });
            dataGridView1.Location = new Point(154, 108);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(844, 341);
            dataGridView1.TabIndex = 0;
            // 
            // modelcolumn
            // 
            modelcolumn.HeaderText = "Model/Motorcycle";
            modelcolumn.Name = "modelcolumn";
            // 
            // brandcolumn
            // 
            brandcolumn.HeaderText = "Brand";
            brandcolumn.Name = "brandcolumn";
            // 
            // partcolumn
            // 
            partcolumn.HeaderText = "Part Name";
            partcolumn.Name = "partcolumn";
            // 
            // partnumcolumn
            // 
            partnumcolumn.HeaderText = "Part Number";
            partnumcolumn.Name = "partnumcolumn";
            // 
            // quantitycolumn
            // 
            quantitycolumn.HeaderText = "QTY";
            quantitycolumn.Name = "quantitycolumn";
            // 
            // srpcolumn
            // 
            srpcolumn.HeaderText = "SRP";
            srpcolumn.Name = "srpcolumn";
            // 
            // wspcolumn
            // 
            wspcolumn.HeaderText = "WSP";
            wspcolumn.Name = "wspcolumn";
            // 
            // suppliercolumn
            // 
            suppliercolumn.HeaderText = "Supplier";
            suppliercolumn.Name = "suppliercolumn";
            // 
            // modelComboBox
            // 
            modelComboBox.FormattingEnabled = true;
            modelComboBox.Items.AddRange(new object[] { "Click 125", "Click 150", "Beat Fi", "Mio i 125", "Mio Sporty", "Aerox", "Nmax ", "Scooters", "Underbones" });
            modelComboBox.Location = new Point(194, 79);
            modelComboBox.Name = "modelComboBox";
            modelComboBox.Size = new Size(100, 23);
            modelComboBox.TabIndex = 1;
            // 
            // brandComboBox
            // 
            brandComboBox.FormattingEnabled = true;
            brandComboBox.Items.AddRange(new object[] { "Genuine", "Motul", "Shell", "Yamalube ", "RCB", "Ordinary", "Stainles Hexagonal Bolts", "Replacement" });
            brandComboBox.Location = new Point(300, 79);
            brandComboBox.Name = "brandComboBox";
            brandComboBox.Size = new Size(96, 23);
            brandComboBox.TabIndex = 2;
            // 
            // partComboBox
            // 
            partComboBox.FormattingEnabled = true;
            partComboBox.Items.AddRange(new object[] { "Cylinder Block", "Piston Pin", "Motor Oil", "Mags", "Handle Grip", "Bolt" });
            partComboBox.Location = new Point(402, 79);
            partComboBox.Name = "partComboBox";
            partComboBox.Size = new Size(96, 23);
            partComboBox.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 450);
            Controls.Add(partComboBox);
            Controls.Add(brandComboBox);
            Controls.Add(modelComboBox);
            Controls.Add(dataGridView1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn modelcolumn;
        private DataGridViewTextBoxColumn brandcolumn;
        private DataGridViewTextBoxColumn partcolumn;
        private DataGridViewTextBoxColumn partnumcolumn;
        private DataGridViewTextBoxColumn quantitycolumn;
        private DataGridViewTextBoxColumn srpcolumn;
        private DataGridViewTextBoxColumn wspcolumn;
        private DataGridViewTextBoxColumn suppliercolumn;
        private ComboBox modelComboBox;
        private ComboBox brandComboBox;
        private ComboBox partComboBox;
    }
}
