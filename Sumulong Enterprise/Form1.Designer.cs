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
            dataGridViewInventory = new DataGridView();
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
            searchButton = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewInventory).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewInventory
            // 
            dataGridViewInventory.AllowUserToOrderColumns = true;
            dataGridViewInventory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewInventory.Location = new Point(12, 208);
            dataGridViewInventory.Name = "dataGridViewInventory";
            dataGridViewInventory.Size = new Size(1240, 701);
            dataGridViewInventory.TabIndex = 0;
            // 
            // modelcolumn
            // 
            modelcolumn.HeaderText = "Model/Motorcycle";
            modelcolumn.Name = "modelcolumn";
            modelcolumn.Width = 175;
            // 
            // brandcolumn
            // 
            brandcolumn.HeaderText = "Brand";
            brandcolumn.Name = "brandcolumn";
            brandcolumn.Width = 175;
            // 
            // partcolumn
            // 
            partcolumn.HeaderText = "Part Name";
            partcolumn.Name = "partcolumn";
            partcolumn.Width = 175;
            // 
            // partnumcolumn
            // 
            partnumcolumn.HeaderText = "Part Number";
            partnumcolumn.Name = "partnumcolumn";
            partnumcolumn.Width = 175;
            // 
            // quantitycolumn
            // 
            quantitycolumn.HeaderText = "QTY";
            quantitycolumn.Name = "quantitycolumn";
            quantitycolumn.Width = 75;
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
            suppliercolumn.Width = 220;
            // 
            // modelComboBox
            // 
            modelComboBox.FormattingEnabled = true;
            modelComboBox.Items.AddRange(new object[] { "Click 125", "Click 150", "Beat Fi", "Mio i 125", "Mio Sporty", "Aerox", "Nmax ", "Scooters", "Underbones" });
            modelComboBox.Location = new Point(51, 179);
            modelComboBox.Name = "modelComboBox";
            modelComboBox.Size = new Size(100, 23);
            modelComboBox.TabIndex = 1;
            // 
            // brandComboBox
            // 
            brandComboBox.FormattingEnabled = true;
            brandComboBox.Items.AddRange(new object[] { "Genuine", "Motul", "Shell", "Yamalube ", "RCB", "Ordinary", "Stainles Hexagonal Bolts", "Replacement" });
            brandComboBox.Location = new Point(229, 179);
            brandComboBox.Name = "brandComboBox";
            brandComboBox.Size = new Size(96, 23);
            brandComboBox.TabIndex = 2;
            // 
            // partComboBox
            // 
            partComboBox.FormattingEnabled = true;
            partComboBox.Items.AddRange(new object[] { "Cylinder Block", "Piston Pin", "Motor Oil", "Mags", "Handle Grip", "Bolt" });
            partComboBox.Location = new Point(404, 180);
            partComboBox.Name = "partComboBox";
            partComboBox.Size = new Size(96, 23);
            partComboBox.TabIndex = 3;
            // 
            // searchButton
            // 
            searchButton.Location = new Point(1035, 179);
            searchButton.Name = "searchButton";
            searchButton.Size = new Size(75, 23);
            searchButton.TabIndex = 4;
            searchButton.Text = "Search";
            searchButton.UseVisualStyleBackColor = true;
            searchButton.Click += searchButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 921);
            Controls.Add(searchButton);
            Controls.Add(partComboBox);
            Controls.Add(brandComboBox);
            Controls.Add(modelComboBox);
            Controls.Add(dataGridViewInventory);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewInventory).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridViewInventory;
        private ComboBox modelComboBox;
        private ComboBox brandComboBox;
        private ComboBox partComboBox;
        private Button searchButton;
        private DataGridViewTextBoxColumn modelcolumn;
        private DataGridViewTextBoxColumn brandcolumn;
        private DataGridViewTextBoxColumn partcolumn;
        private DataGridViewTextBoxColumn partnumcolumn;
        private DataGridViewTextBoxColumn quantitycolumn;
        private DataGridViewTextBoxColumn srpcolumn;
        private DataGridViewTextBoxColumn wspcolumn;
        private DataGridViewTextBoxColumn suppliercolumn;
    }
}
