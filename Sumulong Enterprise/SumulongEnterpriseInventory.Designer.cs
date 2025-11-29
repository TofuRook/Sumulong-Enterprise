namespace Sumulong_Enterprise
{
    partial class SumulongEnterpriseInventory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SumulongEnterpriseInventory));
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
            clearButton = new Button();
            addButton = new Button();
            deleteButton = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            pictureBox1 = new PictureBox();
            label4 = new Label();
            label5 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewInventory).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewInventory
            // 
            dataGridViewInventory.AllowUserToAddRows = false;
            dataGridViewInventory.AllowUserToDeleteRows = false;
            dataGridViewInventory.AllowUserToOrderColumns = true;
            dataGridViewInventory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewInventory.BackgroundColor = Color.Gainsboro;
            dataGridViewInventory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewInventory.Location = new Point(12, 208);
            dataGridViewInventory.MultiSelect = false;
            dataGridViewInventory.Name = "dataGridViewInventory";
            dataGridViewInventory.ReadOnly = true;
            dataGridViewInventory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewInventory.Size = new Size(1240, 701);
            dataGridViewInventory.TabIndex = 0;
            dataGridViewInventory.CellContentDoubleClick += dataGridViewInventory_CellContentDoubleClick;
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
            modelComboBox.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            modelComboBox.FormattingEnabled = true;
            modelComboBox.Items.AddRange(new object[] { "Click 125", "Click 150", "Beat Fi", "Mio i 125", "Mio Sporty", "Aerox", "Nmax ", "Scooters", "Underbones" });
            modelComboBox.Location = new Point(12, 45);
            modelComboBox.Name = "modelComboBox";
            modelComboBox.Size = new Size(421, 40);
            modelComboBox.TabIndex = 1;
            modelComboBox.SelectedIndexChanged += modelComboBox_SelectedIndexChanged;
            // 
            // brandComboBox
            // 
            brandComboBox.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            brandComboBox.FormattingEnabled = true;
            brandComboBox.Items.AddRange(new object[] { "Genuine", "Motul", "Shell", "Yamalube ", "RCB", "Ordinary", "Stainles Hexagonal Bolts", "Replacement" });
            brandComboBox.Location = new Point(12, 130);
            brandComboBox.Name = "brandComboBox";
            brandComboBox.Size = new Size(350, 33);
            brandComboBox.TabIndex = 2;
            brandComboBox.SelectedIndexChanged += brandComboBox_SelectedIndexChanged;
            // 
            // partComboBox
            // 
            partComboBox.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            partComboBox.FormattingEnabled = true;
            partComboBox.Items.AddRange(new object[] { "Cylinder Block", "Piston Pin", "Motor Oil", "Mags", "Handle Grip", "Bolt" });
            partComboBox.Location = new Point(411, 130);
            partComboBox.Name = "partComboBox";
            partComboBox.Size = new Size(350, 33);
            partComboBox.TabIndex = 3;
            // 
            // searchButton
            // 
            searchButton.BackColor = Color.LightSteelBlue;
            searchButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            searchButton.ForeColor = SystemColors.ControlText;
            searchButton.Location = new Point(449, 45);
            searchButton.Name = "searchButton";
            searchButton.Size = new Size(171, 40);
            searchButton.TabIndex = 4;
            searchButton.Text = "Search";
            searchButton.UseVisualStyleBackColor = false;
            searchButton.Click += searchButton_Click;
            // 
            // clearButton
            // 
            clearButton.BackColor = Color.LightSteelBlue;
            clearButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            clearButton.Location = new Point(12, 179);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(128, 23);
            clearButton.TabIndex = 5;
            clearButton.Text = "Clear";
            clearButton.UseVisualStyleBackColor = false;
            clearButton.Click += clearButton_Click;
            // 
            // addButton
            // 
            addButton.BackColor = Color.DarkSeaGreen;
            addButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            addButton.Location = new Point(146, 179);
            addButton.Name = "addButton";
            addButton.Size = new Size(156, 23);
            addButton.TabIndex = 6;
            addButton.Text = "Add Item";
            addButton.UseVisualStyleBackColor = false;
            addButton.Click += addButton_Click;
            // 
            // deleteButton
            // 
            deleteButton.BackColor = Color.RosyBrown;
            deleteButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            deleteButton.Location = new Point(308, 179);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(148, 23);
            deleteButton.TabIndex = 7;
            deleteButton.Text = "Delete Item";
            deleteButton.UseVisualStyleBackColor = false;
            deleteButton.Click += deleteButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.Control;
            label1.Location = new Point(12, 21);
            label1.Name = "label1";
            label1.Size = new Size(147, 21);
            label1.TabIndex = 8;
            label1.Text = "Search for Vehicle";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.Control;
            label2.Location = new Point(411, 106);
            label2.Name = "label2";
            label2.Size = new Size(48, 21);
            label2.TabIndex = 9;
            label2.Text = "Parts";
            label2.Click += label2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = SystemColors.Control;
            label3.Location = new Point(12, 106);
            label3.Name = "label3";
            label3.Size = new Size(55, 21);
            label3.TabIndex = 10;
            label3.Text = "Brand";
            label3.Click += label3_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.DarkSlateBlue;
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(1001, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(251, 190);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 11;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = SystemColors.Control;
            label4.Location = new Point(825, 15);
            label4.Name = "label4";
            label4.Size = new Size(170, 30);
            label4.TabIndex = 12;
            label4.Text = "Welcome User 1";
            label4.Click += label4_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.ForeColor = SystemColors.Control;
            label5.Location = new Point(874, 45);
            label5.Name = "label5";
            label5.Size = new Size(121, 20);
            label5.TabIndex = 13;
            label5.Text = "Manage Account";
            // 
            // SumulongEnterpriseInventory
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkSlateBlue;
            ClientSize = new Size(1264, 921);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(pictureBox1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(deleteButton);
            Controls.Add(addButton);
            Controls.Add(clearButton);
            Controls.Add(searchButton);
            Controls.Add(partComboBox);
            Controls.Add(brandComboBox);
            Controls.Add(modelComboBox);
            Controls.Add(dataGridViewInventory);
            Name = "SumulongEnterpriseInventory";
            Text = "SumulongEnterpriseInventory";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewInventory).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
        private Button clearButton;
        private Button addButton;
        private Button deleteButton;
        private Label label1;
        private Label label2;
        private Label label3;
        private PictureBox pictureBox1;
        private Label label4;
        private Label label5;
    }
}
