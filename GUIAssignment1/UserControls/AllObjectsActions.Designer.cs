namespace GUIAssignment1.UserControls
{
    partial class FindAllObjects
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            leftBottomGPSLabel = new Label();
            allObjectsGridView = new DataGridView();
            Type = new DataGridViewTextBoxColumn();
            PropertyTableID = new DataGridViewTextBoxColumn();
            PropertyTableDescription = new DataGridViewTextBoxColumn();
            ParcelList = new DataGridViewTextBoxColumn();
            LeftBottom = new DataGridViewTextBoxColumn();
            RightTop = new DataGridViewTextBoxColumn();
            longGroupBox = new GroupBox();
            longNumericUpDown = new NumericUpDown();
            longERadioButton = new RadioButton();
            longWRadioButton = new RadioButton();
            latGroupBox = new GroupBox();
            latNumericUpDown = new NumericUpDown();
            latNRadioButton = new RadioButton();
            latSRadioButton = new RadioButton();
            searchAllObjectsButton = new Button();
            rightTopGPSLabel = new Label();
            longGroupBox2 = new GroupBox();
            longNumericUpDown2 = new NumericUpDown();
            longERadioButton2 = new RadioButton();
            longWRadioButton2 = new RadioButton();
            latGroupBox2 = new GroupBox();
            latNumericUpDown2 = new NumericUpDown();
            latNRadioButton2 = new RadioButton();
            latSRadioButton2 = new RadioButton();
            showAllButton = new Button();
            ((System.ComponentModel.ISupportInitialize)allObjectsGridView).BeginInit();
            longGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)longNumericUpDown).BeginInit();
            latGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)latNumericUpDown).BeginInit();
            longGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)longNumericUpDown2).BeginInit();
            latGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)latNumericUpDown2).BeginInit();
            SuspendLayout();
            // 
            // leftBottomGPSLabel
            // 
            leftBottomGPSLabel.AutoSize = true;
            leftBottomGPSLabel.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point);
            leftBottomGPSLabel.Location = new Point(46, 36);
            leftBottomGPSLabel.Margin = new Padding(0);
            leftBottomGPSLabel.Name = "leftBottomGPSLabel";
            leftBottomGPSLabel.Size = new Size(238, 38);
            leftBottomGPSLabel.TabIndex = 46;
            leftBottomGPSLabel.Text = "Left Bottom GPS";
            leftBottomGPSLabel.Click += label1_Click;
            // 
            // allObjectsGridView
            // 
            allObjectsGridView.AllowUserToAddRows = false;
            allObjectsGridView.AllowUserToDeleteRows = false;
            allObjectsGridView.AllowUserToOrderColumns = true;
            allObjectsGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            allObjectsGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            allObjectsGridView.Columns.AddRange(new DataGridViewColumn[] { Type, PropertyTableID, PropertyTableDescription, ParcelList, LeftBottom, RightTop });
            allObjectsGridView.Location = new Point(44, 267);
            allObjectsGridView.Name = "allObjectsGridView";
            allObjectsGridView.ReadOnly = true;
            allObjectsGridView.RowHeadersWidth = 51;
            allObjectsGridView.RowTemplate.Height = 29;
            allObjectsGridView.Size = new Size(1036, 406);
            allObjectsGridView.TabIndex = 53;
            allObjectsGridView.CellMouseDoubleClick += allObjectsGridView_CellMouseDoubleClick;
            // 
            // Type
            // 
            Type.HeaderText = "Type";
            Type.MinimumWidth = 6;
            Type.Name = "Type";
            Type.ReadOnly = true;
            // 
            // PropertyTableID
            // 
            PropertyTableID.HeaderText = "Conscription/Parcel Number";
            PropertyTableID.MinimumWidth = 6;
            PropertyTableID.Name = "PropertyTableID";
            PropertyTableID.ReadOnly = true;
            // 
            // PropertyTableDescription
            // 
            PropertyTableDescription.HeaderText = "Description";
            PropertyTableDescription.MinimumWidth = 6;
            PropertyTableDescription.Name = "PropertyTableDescription";
            PropertyTableDescription.ReadOnly = true;
            // 
            // ParcelList
            // 
            ParcelList.HeaderText = "Property/Parcel List";
            ParcelList.MinimumWidth = 6;
            ParcelList.Name = "ParcelList";
            ParcelList.ReadOnly = true;
            // 
            // LeftBottom
            // 
            LeftBottom.HeaderText = "Left Bottom Position";
            LeftBottom.MinimumWidth = 6;
            LeftBottom.Name = "LeftBottom";
            LeftBottom.ReadOnly = true;
            // 
            // RightTop
            // 
            RightTop.HeaderText = "Right Top Position";
            RightTop.MinimumWidth = 6;
            RightTop.Name = "RightTop";
            RightTop.ReadOnly = true;
            // 
            // longGroupBox
            // 
            longGroupBox.Controls.Add(longNumericUpDown);
            longGroupBox.Controls.Add(longERadioButton);
            longGroupBox.Controls.Add(longWRadioButton);
            longGroupBox.Location = new Point(44, 154);
            longGroupBox.Name = "longGroupBox";
            longGroupBox.Size = new Size(238, 68);
            longGroupBox.TabIndex = 52;
            longGroupBox.TabStop = false;
            longGroupBox.Text = "Longitude (decimal degrees)";
            // 
            // longNumericUpDown
            // 
            longNumericUpDown.DecimalPlaces = 3;
            longNumericUpDown.Location = new Point(122, 30);
            longNumericUpDown.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
            longNumericUpDown.Name = "longNumericUpDown";
            longNumericUpDown.Size = new Size(77, 27);
            longNumericUpDown.TabIndex = 23;
            longNumericUpDown.Tag = "";
            // 
            // longERadioButton
            // 
            longERadioButton.AutoSize = true;
            longERadioButton.Checked = true;
            longERadioButton.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            longERadioButton.Location = new Point(6, 23);
            longERadioButton.Name = "longERadioButton";
            longERadioButton.Size = new Size(49, 39);
            longERadioButton.TabIndex = 21;
            longERadioButton.TabStop = true;
            longERadioButton.Text = "E";
            longERadioButton.UseVisualStyleBackColor = true;
            // 
            // longWRadioButton
            // 
            longWRadioButton.AutoSize = true;
            longWRadioButton.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            longWRadioButton.Location = new Point(65, 23);
            longWRadioButton.Name = "longWRadioButton";
            longWRadioButton.Size = new Size(59, 39);
            longWRadioButton.TabIndex = 20;
            longWRadioButton.Text = "W";
            longWRadioButton.UseVisualStyleBackColor = true;
            // 
            // latGroupBox
            // 
            latGroupBox.Controls.Add(latNumericUpDown);
            latGroupBox.Controls.Add(latNRadioButton);
            latGroupBox.Controls.Add(latSRadioButton);
            latGroupBox.Location = new Point(44, 80);
            latGroupBox.Name = "latGroupBox";
            latGroupBox.Size = new Size(238, 68);
            latGroupBox.TabIndex = 51;
            latGroupBox.TabStop = false;
            latGroupBox.Text = "Latitude (decimal degrees)";
            // 
            // latNumericUpDown
            // 
            latNumericUpDown.DecimalPlaces = 3;
            latNumericUpDown.Location = new Point(122, 31);
            latNumericUpDown.Maximum = new decimal(new int[] { 90, 0, 0, 0 });
            latNumericUpDown.Name = "latNumericUpDown";
            latNumericUpDown.Size = new Size(77, 27);
            latNumericUpDown.TabIndex = 22;
            latNumericUpDown.Tag = "";
            // 
            // latNRadioButton
            // 
            latNRadioButton.AutoSize = true;
            latNRadioButton.Checked = true;
            latNRadioButton.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            latNRadioButton.Location = new Point(6, 23);
            latNRadioButton.Name = "latNRadioButton";
            latNRadioButton.Size = new Size(55, 39);
            latNRadioButton.TabIndex = 21;
            latNRadioButton.TabStop = true;
            latNRadioButton.Text = "N";
            latNRadioButton.UseVisualStyleBackColor = true;
            // 
            // latSRadioButton
            // 
            latSRadioButton.AutoSize = true;
            latSRadioButton.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            latSRadioButton.Location = new Point(65, 23);
            latSRadioButton.Name = "latSRadioButton";
            latSRadioButton.Size = new Size(49, 39);
            latSRadioButton.TabIndex = 20;
            latSRadioButton.Text = "S";
            latSRadioButton.UseVisualStyleBackColor = true;
            // 
            // searchAllObjectsButton
            // 
            searchAllObjectsButton.Location = new Point(44, 228);
            searchAllObjectsButton.Name = "searchAllObjectsButton";
            searchAllObjectsButton.Size = new Size(524, 29);
            searchAllObjectsButton.TabIndex = 50;
            searchAllObjectsButton.Text = "Search Properties And Parcels";
            searchAllObjectsButton.UseVisualStyleBackColor = true;
            searchAllObjectsButton.Click += searchAllObjectsButton_Click;
            // 
            // rightTopGPSLabel
            // 
            rightTopGPSLabel.AutoSize = true;
            rightTopGPSLabel.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point);
            rightTopGPSLabel.Location = new Point(332, 36);
            rightTopGPSLabel.Margin = new Padding(0);
            rightTopGPSLabel.Name = "rightTopGPSLabel";
            rightTopGPSLabel.Size = new Size(204, 38);
            rightTopGPSLabel.TabIndex = 54;
            rightTopGPSLabel.Text = "Right Top GPS";
            // 
            // longGroupBox2
            // 
            longGroupBox2.Controls.Add(longNumericUpDown2);
            longGroupBox2.Controls.Add(longERadioButton2);
            longGroupBox2.Controls.Add(longWRadioButton2);
            longGroupBox2.Location = new Point(330, 154);
            longGroupBox2.Name = "longGroupBox2";
            longGroupBox2.Size = new Size(238, 68);
            longGroupBox2.TabIndex = 56;
            longGroupBox2.TabStop = false;
            longGroupBox2.Text = "Longitude (decimal degrees)";
            // 
            // longNumericUpDown2
            // 
            longNumericUpDown2.DecimalPlaces = 3;
            longNumericUpDown2.Location = new Point(122, 30);
            longNumericUpDown2.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
            longNumericUpDown2.Name = "longNumericUpDown2";
            longNumericUpDown2.Size = new Size(77, 27);
            longNumericUpDown2.TabIndex = 23;
            longNumericUpDown2.Tag = "";
            // 
            // longERadioButton2
            // 
            longERadioButton2.AutoSize = true;
            longERadioButton2.Checked = true;
            longERadioButton2.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            longERadioButton2.Location = new Point(6, 23);
            longERadioButton2.Name = "longERadioButton2";
            longERadioButton2.Size = new Size(49, 39);
            longERadioButton2.TabIndex = 21;
            longERadioButton2.TabStop = true;
            longERadioButton2.Text = "E";
            longERadioButton2.UseVisualStyleBackColor = true;
            // 
            // longWRadioButton2
            // 
            longWRadioButton2.AutoSize = true;
            longWRadioButton2.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            longWRadioButton2.Location = new Point(65, 23);
            longWRadioButton2.Name = "longWRadioButton2";
            longWRadioButton2.Size = new Size(59, 39);
            longWRadioButton2.TabIndex = 20;
            longWRadioButton2.Text = "W";
            longWRadioButton2.UseVisualStyleBackColor = true;
            // 
            // latGroupBox2
            // 
            latGroupBox2.Controls.Add(latNumericUpDown2);
            latGroupBox2.Controls.Add(latNRadioButton2);
            latGroupBox2.Controls.Add(latSRadioButton2);
            latGroupBox2.Location = new Point(330, 80);
            latGroupBox2.Name = "latGroupBox2";
            latGroupBox2.Size = new Size(238, 68);
            latGroupBox2.TabIndex = 55;
            latGroupBox2.TabStop = false;
            latGroupBox2.Text = "Latitude (decimal degrees)";
            // 
            // latNumericUpDown2
            // 
            latNumericUpDown2.DecimalPlaces = 3;
            latNumericUpDown2.Location = new Point(122, 31);
            latNumericUpDown2.Maximum = new decimal(new int[] { 90, 0, 0, 0 });
            latNumericUpDown2.Name = "latNumericUpDown2";
            latNumericUpDown2.Size = new Size(77, 27);
            latNumericUpDown2.TabIndex = 22;
            latNumericUpDown2.Tag = "";
            // 
            // latNRadioButton2
            // 
            latNRadioButton2.AutoSize = true;
            latNRadioButton2.Checked = true;
            latNRadioButton2.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            latNRadioButton2.Location = new Point(6, 23);
            latNRadioButton2.Name = "latNRadioButton2";
            latNRadioButton2.Size = new Size(55, 39);
            latNRadioButton2.TabIndex = 21;
            latNRadioButton2.TabStop = true;
            latNRadioButton2.Text = "N";
            latNRadioButton2.UseVisualStyleBackColor = true;
            // 
            // latSRadioButton2
            // 
            latSRadioButton2.AutoSize = true;
            latSRadioButton2.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            latSRadioButton2.Location = new Point(65, 23);
            latSRadioButton2.Name = "latSRadioButton2";
            latSRadioButton2.Size = new Size(49, 39);
            latSRadioButton2.TabIndex = 20;
            latSRadioButton2.Text = "S";
            latSRadioButton2.UseVisualStyleBackColor = true;
            // 
            // showAllButton
            // 
            showAllButton.Location = new Point(587, 228);
            showAllButton.Name = "showAllButton";
            showAllButton.Size = new Size(493, 28);
            showAllButton.TabIndex = 57;
            showAllButton.Text = "Show All Properties And Parcels";
            showAllButton.UseVisualStyleBackColor = true;
            showAllButton.Click += showAllButton_Click;
            // 
            // FindAllObjects3
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(showAllButton);
            Controls.Add(longGroupBox2);
            Controls.Add(latGroupBox2);
            Controls.Add(rightTopGPSLabel);
            Controls.Add(allObjectsGridView);
            Controls.Add(longGroupBox);
            Controls.Add(latGroupBox);
            Controls.Add(searchAllObjectsButton);
            Controls.Add(leftBottomGPSLabel);
            Name = "FindAllObjects3";
            Size = new Size(1366, 768);
            ((System.ComponentModel.ISupportInitialize)allObjectsGridView).EndInit();
            longGroupBox.ResumeLayout(false);
            longGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)longNumericUpDown).EndInit();
            latGroupBox.ResumeLayout(false);
            latGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)latNumericUpDown).EndInit();
            longGroupBox2.ResumeLayout(false);
            longGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)longNumericUpDown2).EndInit();
            latGroupBox2.ResumeLayout(false);
            latGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)latNumericUpDown2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label leftBottomGPSLabel;
        private Label label2;
        private DataGridView allObjectsGridView;
        private GroupBox longGroupBox;
        private NumericUpDown longNumericUpDown;
        private RadioButton longERadioButton;
        private RadioButton longWRadioButton;
        private GroupBox latGroupBox;
        private NumericUpDown latNumericUpDown;
        private RadioButton latNRadioButton;
        private RadioButton latSRadioButton;
        private Button searchAllObjectsButton;
        private Label rightTopGPSLabel;
        private GroupBox longGroupBox2;
        private NumericUpDown longNumericUpDown2;
        private RadioButton longERadioButton2;
        private RadioButton longWRadioButton2;
        private GroupBox latGroupBox2;
        private NumericUpDown latNumericUpDown2;
        private RadioButton latNRadioButton2;
        private RadioButton latSRadioButton2;
        private DataGridViewTextBoxColumn Type;
        private DataGridViewTextBoxColumn PropertyTableID;
        private DataGridViewTextBoxColumn PropertyTableDescription;
        private DataGridViewTextBoxColumn ParcelList;
        private DataGridViewTextBoxColumn LeftBottom;
        private DataGridViewTextBoxColumn RightTop;
        private Button showAllButton;
    }
}
