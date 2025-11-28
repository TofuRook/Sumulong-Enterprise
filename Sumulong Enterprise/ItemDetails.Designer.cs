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
            tabPage2 = new TabPage();
            tabPage1 = new TabPage();
            label1 = new Label();
            codetextBox = new TextBox();
            LocationcomboBox = new ComboBox();
            QuantitytextBox = new TextBox();
            transferbutton = new Button();
            Deductbutton = new Button();
            Addbutton = new Button();
            itemdataGridView = new DataGridView();
            tabControl1 = new TabControl();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)itemdataGridView).BeginInit();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(768, 303);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(codetextBox);
            tabPage1.Controls.Add(LocationcomboBox);
            tabPage1.Controls.Add(QuantitytextBox);
            tabPage1.Controls.Add(transferbutton);
            tabPage1.Controls.Add(Deductbutton);
            tabPage1.Controls.Add(Addbutton);
            tabPage1.Controls.Add(itemdataGridView);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(768, 303);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(462, 75);
            label1.Name = "label1";
            label1.Size = new Size(147, 15);
            label1.TabIndex = 7;
            label1.Text = "CODE for Morong Transfer";
            // 
            // codetextBox
            // 
            codetextBox.Location = new Point(462, 96);
            codetextBox.Name = "codetextBox";
            codetextBox.Size = new Size(100, 23);
            codetextBox.TabIndex = 6;
            // 
            // LocationcomboBox
            // 
            LocationcomboBox.FormattingEnabled = true;
            LocationcomboBox.Location = new Point(462, 37);
            LocationcomboBox.Name = "LocationcomboBox";
            LocationcomboBox.Size = new Size(121, 23);
            LocationcomboBox.TabIndex = 5;
            // 
            // QuantitytextBox
            // 
            QuantitytextBox.Location = new Point(6, 36);
            QuantitytextBox.Name = "QuantitytextBox";
            QuantitytextBox.Size = new Size(100, 23);
            QuantitytextBox.TabIndex = 4;
            // 
            // transferbutton
            // 
            transferbutton.Location = new Point(621, 36);
            transferbutton.Name = "transferbutton";
            transferbutton.Size = new Size(75, 23);
            transferbutton.TabIndex = 3;
            transferbutton.Text = "Transfer";
            transferbutton.UseVisualStyleBackColor = true;
            transferbutton.Click += transferbutton_Click;
            // 
            // Deductbutton
            // 
            Deductbutton.Location = new Point(138, 75);
            Deductbutton.Name = "Deductbutton";
            Deductbutton.Size = new Size(103, 23);
            Deductbutton.TabIndex = 2;
            Deductbutton.Text = "Deduct Stock";
            Deductbutton.UseVisualStyleBackColor = true;
            Deductbutton.Click += Deductbutton_Click;
            // 
            // Addbutton
            // 
            Addbutton.Location = new Point(138, 35);
            Addbutton.Name = "Addbutton";
            Addbutton.Size = new Size(103, 23);
            Addbutton.TabIndex = 1;
            Addbutton.Text = "Add Stock";
            Addbutton.UseVisualStyleBackColor = true;
            Addbutton.Click += Addbutton_Click;
            // 
            // itemdataGridView
            // 
            itemdataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            itemdataGridView.Location = new Point(6, 140);
            itemdataGridView.Name = "itemdataGridView";
            itemdataGridView.Size = new Size(756, 157);
            itemdataGridView.TabIndex = 0;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(776, 331);
            tabControl1.TabIndex = 0;
            // 
            // ItemDetails
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 350);
            Controls.Add(tabControl1);
            Name = "ItemDetails";
            Text = "ItemDetails";
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)itemdataGridView).EndInit();
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabPage tabPage2;
        private TabPage tabPage1;
        private TextBox QuantitytextBox;
        private Button transferbutton;
        private Button Deductbutton;
        private Button Addbutton;
        private DataGridView itemdataGridView;
        private TabControl tabControl1;
        private ComboBox LocationcomboBox;
        private Label label1;
        private TextBox codetextBox;
    }
}