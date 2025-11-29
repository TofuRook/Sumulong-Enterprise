namespace Sumulong_Enterprise
{
    partial class ItemDetails
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemDetails));
            newItemTab = new TabPage();
            btnSaveItems = new Button();
            dataGridView1 = new DataGridView();
            Model = new DataGridViewTextBoxColumn();
            Brand = new DataGridViewTextBoxColumn();
            PartName = new DataGridViewTextBoxColumn();
            PartNumber = new DataGridViewTextBoxColumn();
            Quantity = new DataGridViewTextBoxColumn();
            SRP = new DataGridViewTextBoxColumn();
            WS_Price = new DataGridViewTextBoxColumn();
            Supplier = new DataGridViewTextBoxColumn();
            stockTab = new TabPage();
            movementDataGridView = new DataGridView();
            ToLocationcomboBox = new ComboBox();
            label1 = new Label();
            codetextBox = new TextBox();
            LocationcomboBox = new ComboBox();
            QuantitytextBox = new TextBox();
            transferbutton = new Button();
            Deductbutton = new Button();
            Addbutton = new Button();
            itemdataGridView = new DataGridView();
            tabControl1 = new TabControl();
            editItemTab = new TabPage();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            newItemTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            stockTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)movementDataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)itemdataGridView).BeginInit();
            tabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // newItemTab
            // 
            newItemTab.Controls.Add(btnSaveItems);
            newItemTab.Controls.Add(dataGridView1);
            newItemTab.Location = new Point(4, 24);
            newItemTab.Name = "newItemTab";
            newItemTab.Padding = new Padding(3);
            newItemTab.Size = new Size(768, 303);
            newItemTab.TabIndex = 1;
            newItemTab.Text = "New Item";
            newItemTab.UseVisualStyleBackColor = true;
            // 
            // btnSaveItems
            // 
            btnSaveItems.Location = new Point(648, 118);
            btnSaveItems.Name = "btnSaveItems";
            btnSaveItems.Size = new Size(97, 23);
            btnSaveItems.TabIndex = 1;
            btnSaveItems.Text = "Create Item";
            btnSaveItems.UseVisualStyleBackColor = true;
            btnSaveItems.Click += btnSaveItems_Click_1;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Model, Brand, PartName, PartNumber, Quantity, SRP, WS_Price, Supplier });
            dataGridView1.Location = new Point(6, 147);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(756, 150);
            dataGridView1.TabIndex = 0;
            // 
            // Model
            // 
            Model.HeaderText = "Model";
            Model.Name = "Model";
            // 
            // Brand
            // 
            Brand.HeaderText = "Brand";
            Brand.Name = "Brand";
            // 
            // PartName
            // 
            PartName.HeaderText = "PartName";
            PartName.Name = "PartName";
            // 
            // PartNumber
            // 
            PartNumber.HeaderText = "PartNumber";
            PartNumber.Name = "PartNumber";
            // 
            // Quantity
            // 
            Quantity.HeaderText = "Quantity";
            Quantity.Name = "Quantity";
            // 
            // SRP
            // 
            SRP.HeaderText = "SRP";
            SRP.Name = "SRP";
            // 
            // WS_Price
            // 
            WS_Price.HeaderText = "WS_Price";
            WS_Price.Name = "WS_Price";
            // 
            // Supplier
            // 
            Supplier.HeaderText = "Supplier";
            Supplier.Name = "Supplier";
            // 
            // stockTab
            // 
            stockTab.Controls.Add(label2);
            stockTab.Controls.Add(pictureBox1);
            stockTab.Controls.Add(movementDataGridView);
            stockTab.Controls.Add(ToLocationcomboBox);
            stockTab.Controls.Add(label1);
            stockTab.Controls.Add(codetextBox);
            stockTab.Controls.Add(LocationcomboBox);
            stockTab.Controls.Add(QuantitytextBox);
            stockTab.Controls.Add(transferbutton);
            stockTab.Controls.Add(Deductbutton);
            stockTab.Controls.Add(Addbutton);
            stockTab.Controls.Add(itemdataGridView);
            stockTab.ForeColor = Color.Black;
            stockTab.Location = new Point(4, 24);
            stockTab.Name = "stockTab";
            stockTab.Padding = new Padding(3);
            stockTab.Size = new Size(768, 303);
            stockTab.TabIndex = 0;
            stockTab.Text = "Control Stock";
            stockTab.UseVisualStyleBackColor = true;
            // 
            // movementDataGridView
            // 
            movementDataGridView.AllowUserToAddRows = false;
            movementDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            movementDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            movementDataGridView.Location = new Point(521, 140);
            movementDataGridView.Name = "movementDataGridView";
            movementDataGridView.ReadOnly = true;
            movementDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            movementDataGridView.Size = new Size(244, 157);
            movementDataGridView.TabIndex = 9;
            // 
            // ToLocationcomboBox
            // 
            ToLocationcomboBox.FormattingEnabled = true;
            ToLocationcomboBox.Location = new Point(641, 19);
            ToLocationcomboBox.Name = "ToLocationcomboBox";
            ToLocationcomboBox.Size = new Size(121, 23);
            ToLocationcomboBox.TabIndex = 8;
            ToLocationcomboBox.Text = "Source";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(135, 86);
            label1.Name = "label1";
            label1.Size = new Size(157, 15);
            label1.TabIndex = 7;
            label1.Text = "CODE for Morong Transfer:";
            // 
            // codetextBox
            // 
            codetextBox.Location = new Point(135, 104);
            codetextBox.Name = "codetextBox";
            codetextBox.Size = new Size(293, 23);
            codetextBox.TabIndex = 6;
            // 
            // LocationcomboBox
            // 
            LocationcomboBox.FormattingEnabled = true;
            LocationcomboBox.Location = new Point(496, 19);
            LocationcomboBox.Name = "LocationcomboBox";
            LocationcomboBox.Size = new Size(121, 23);
            LocationcomboBox.TabIndex = 5;
            LocationcomboBox.Text = "Destination";
            // 
            // QuantitytextBox
            // 
            QuantitytextBox.Location = new Point(196, 19);
            QuantitytextBox.Name = "QuantitytextBox";
            QuantitytextBox.Size = new Size(232, 23);
            QuantitytextBox.TabIndex = 4;
            QuantitytextBox.TextChanged += QuantitytextBox_TextChanged;
            // 
            // transferbutton
            // 
            transferbutton.BackColor = Color.LightSteelBlue;
            transferbutton.Location = new Point(353, 48);
            transferbutton.Name = "transferbutton";
            transferbutton.Size = new Size(75, 23);
            transferbutton.TabIndex = 3;
            transferbutton.Text = "Transfer";
            transferbutton.UseVisualStyleBackColor = false;
            transferbutton.Click += transferbutton_Click;
            // 
            // Deductbutton
            // 
            Deductbutton.BackColor = Color.RosyBrown;
            Deductbutton.Location = new Point(244, 48);
            Deductbutton.Name = "Deductbutton";
            Deductbutton.Size = new Size(103, 23);
            Deductbutton.TabIndex = 2;
            Deductbutton.Text = "Deduct Stock";
            Deductbutton.UseVisualStyleBackColor = false;
            Deductbutton.Click += Deductbutton_Click;
            // 
            // Addbutton
            // 
            Addbutton.BackColor = Color.DarkSeaGreen;
            Addbutton.Location = new Point(135, 48);
            Addbutton.Name = "Addbutton";
            Addbutton.Size = new Size(103, 23);
            Addbutton.TabIndex = 1;
            Addbutton.Text = "Add Stock";
            Addbutton.UseVisualStyleBackColor = false;
            Addbutton.Click += Addbutton_Click;
            // 
            // itemdataGridView
            // 
            itemdataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            itemdataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            itemdataGridView.Location = new Point(6, 140);
            itemdataGridView.Name = "itemdataGridView";
            itemdataGridView.Size = new Size(509, 157);
            itemdataGridView.TabIndex = 0;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(stockTab);
            tabControl1.Controls.Add(newItemTab);
            tabControl1.Controls.Add(editItemTab);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(776, 331);
            tabControl1.TabIndex = 0;
            // 
            // editItemTab
            // 
            editItemTab.Location = new Point(4, 24);
            editItemTab.Name = "editItemTab";
            editItemTab.Size = new Size(768, 303);
            editItemTab.TabIndex = 2;
            editItemTab.Text = "Edit Item";
            editItemTab.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = SystemColors.Control;
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(6, 14);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(123, 113);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 12;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(135, 27);
            label2.Name = "label2";
            label2.Size = new Size(55, 15);
            label2.TabIndex = 13;
            label2.Text = "Quantity";
            // 
            // ItemDetails
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkSlateBlue;
            ClientSize = new Size(800, 350);
            Controls.Add(tabControl1);
            Name = "ItemDetails";
            Text = "ItemDetails";
            newItemTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            stockTab.ResumeLayout(false);
            stockTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)movementDataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)itemdataGridView).EndInit();
            tabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabPage newItemTab;
        private TabPage stockTab;
        private TextBox QuantitytextBox;
        private Button transferbutton;
        private Button Deductbutton;
        private Button Addbutton;
        private DataGridView itemdataGridView;
        private TabControl tabControl1;
        private ComboBox LocationcomboBox;
        private Label label1;
        private TextBox codetextBox;
        private ComboBox ToLocationcomboBox;
        private TabPage editItemTab;
        private DataGridView movementDataGridView;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn Model;
        private DataGridViewTextBoxColumn Brand;
        private DataGridViewTextBoxColumn PartName;
        private DataGridViewTextBoxColumn PartNumber;
        private DataGridViewTextBoxColumn Quantity;
        private DataGridViewTextBoxColumn SRP;
        private DataGridViewTextBoxColumn WS_Price;
        private DataGridViewTextBoxColumn Supplier;
        private Button btnSaveItems;
        private PictureBox pictureBox1;
        private Label label2;
    }
}