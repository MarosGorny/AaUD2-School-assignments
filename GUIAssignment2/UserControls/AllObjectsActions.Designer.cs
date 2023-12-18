namespace GUIAssignment2.UserControls
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindAllObjects));
            showAllButton = new Button();
            latNumericUpDown2 = new NumericUpDown();
            latNRadioButton2 = new RadioButton();
            latGroupBox2 = new GroupBox();
            latSRadioButton2 = new RadioButton();
            longNumericUpDown2 = new NumericUpDown();
            longERadioButton2 = new RadioButton();
            longWRadioButton2 = new RadioButton();
            longGroupBox2 = new GroupBox();
            rightTopGPSLabel = new Label();
            searchAllObjectsButton = new Button();
            latGroupBox = new GroupBox();
            latNumericUpDown = new NumericUpDown();
            latNRadioButton = new RadioButton();
            latSRadioButton = new RadioButton();
            longERadioButton = new RadioButton();
            longNumericUpDown = new NumericUpDown();
            longGroupBox = new GroupBox();
            longWRadioButton = new RadioButton();
            allObjectsGridView = new DataGridView();
            leftBottomGPSLabel = new Label();
            Type = new DataGridViewTextBoxColumn();
            PropertyTableID = new DataGridViewTextBoxColumn();
            LeftBottom = new DataGridViewTextBoxColumn();
            RightTop = new DataGridViewTextBoxColumn();
            Search = new DataGridViewImageColumn();
            Edit = new DataGridViewImageColumn();
            Delete = new DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)latNumericUpDown2).BeginInit();
            latGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)longNumericUpDown2).BeginInit();
            longGroupBox2.SuspendLayout();
            latGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)latNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)longNumericUpDown).BeginInit();
            longGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)allObjectsGridView).BeginInit();
            SuspendLayout();
            // 
            // showAllButton
            // 
            showAllButton.Location = new Point(577, 226);
            showAllButton.Name = "showAllButton";
            showAllButton.Size = new Size(493, 28);
            showAllButton.TabIndex = 66;
            showAllButton.Text = "Show All Properties And Parcels";
            showAllButton.UseVisualStyleBackColor = true;
            showAllButton.Click += showAllButton_Click;
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
            latNRadioButton2.Font = new Font("Segoe UI", 15F);
            latNRadioButton2.Location = new Point(6, 23);
            latNRadioButton2.Name = "latNRadioButton2";
            latNRadioButton2.Size = new Size(55, 39);
            latNRadioButton2.TabIndex = 21;
            latNRadioButton2.TabStop = true;
            latNRadioButton2.Text = "N";
            latNRadioButton2.UseVisualStyleBackColor = true;
            // 
            // latGroupBox2
            // 
            latGroupBox2.Controls.Add(latNumericUpDown2);
            latGroupBox2.Controls.Add(latNRadioButton2);
            latGroupBox2.Controls.Add(latSRadioButton2);
            latGroupBox2.Location = new Point(320, 78);
            latGroupBox2.Name = "latGroupBox2";
            latGroupBox2.Size = new Size(238, 68);
            latGroupBox2.TabIndex = 64;
            latGroupBox2.TabStop = false;
            latGroupBox2.Text = "Latitude (decimal degrees)";
            // 
            // latSRadioButton2
            // 
            latSRadioButton2.AutoSize = true;
            latSRadioButton2.Font = new Font("Segoe UI", 15F);
            latSRadioButton2.Location = new Point(65, 23);
            latSRadioButton2.Name = "latSRadioButton2";
            latSRadioButton2.Size = new Size(49, 39);
            latSRadioButton2.TabIndex = 20;
            latSRadioButton2.Text = "S";
            latSRadioButton2.UseVisualStyleBackColor = true;
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
            longERadioButton2.Font = new Font("Segoe UI", 15F);
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
            longWRadioButton2.Font = new Font("Segoe UI", 15F);
            longWRadioButton2.Location = new Point(65, 23);
            longWRadioButton2.Name = "longWRadioButton2";
            longWRadioButton2.Size = new Size(59, 39);
            longWRadioButton2.TabIndex = 20;
            longWRadioButton2.Text = "W";
            longWRadioButton2.UseVisualStyleBackColor = true;
            // 
            // longGroupBox2
            // 
            longGroupBox2.Controls.Add(longNumericUpDown2);
            longGroupBox2.Controls.Add(longERadioButton2);
            longGroupBox2.Controls.Add(longWRadioButton2);
            longGroupBox2.Location = new Point(320, 152);
            longGroupBox2.Name = "longGroupBox2";
            longGroupBox2.Size = new Size(238, 68);
            longGroupBox2.TabIndex = 65;
            longGroupBox2.TabStop = false;
            longGroupBox2.Text = "Longitude (decimal degrees)";
            // 
            // rightTopGPSLabel
            // 
            rightTopGPSLabel.AutoSize = true;
            rightTopGPSLabel.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold);
            rightTopGPSLabel.Location = new Point(322, 34);
            rightTopGPSLabel.Margin = new Padding(0);
            rightTopGPSLabel.Name = "rightTopGPSLabel";
            rightTopGPSLabel.Size = new Size(204, 38);
            rightTopGPSLabel.TabIndex = 63;
            rightTopGPSLabel.Text = "Right Top GPS";
            // 
            // searchAllObjectsButton
            // 
            searchAllObjectsButton.Location = new Point(34, 226);
            searchAllObjectsButton.Name = "searchAllObjectsButton";
            searchAllObjectsButton.Size = new Size(524, 29);
            searchAllObjectsButton.TabIndex = 59;
            searchAllObjectsButton.Text = "Search Properties And Parcels";
            searchAllObjectsButton.UseVisualStyleBackColor = true;
            searchAllObjectsButton.Click += searchAllObjectsButton_Click;
            // 
            // latGroupBox
            // 
            latGroupBox.Controls.Add(latNumericUpDown);
            latGroupBox.Controls.Add(latNRadioButton);
            latGroupBox.Controls.Add(latSRadioButton);
            latGroupBox.Location = new Point(34, 78);
            latGroupBox.Name = "latGroupBox";
            latGroupBox.Size = new Size(238, 68);
            latGroupBox.TabIndex = 60;
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
            latNRadioButton.Font = new Font("Segoe UI", 15F);
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
            latSRadioButton.Font = new Font("Segoe UI", 15F);
            latSRadioButton.Location = new Point(65, 23);
            latSRadioButton.Name = "latSRadioButton";
            latSRadioButton.Size = new Size(49, 39);
            latSRadioButton.TabIndex = 20;
            latSRadioButton.Text = "S";
            latSRadioButton.UseVisualStyleBackColor = true;
            // 
            // longERadioButton
            // 
            longERadioButton.AutoSize = true;
            longERadioButton.Checked = true;
            longERadioButton.Font = new Font("Segoe UI", 15F);
            longERadioButton.Location = new Point(6, 23);
            longERadioButton.Name = "longERadioButton";
            longERadioButton.Size = new Size(49, 39);
            longERadioButton.TabIndex = 21;
            longERadioButton.TabStop = true;
            longERadioButton.Text = "E";
            longERadioButton.UseVisualStyleBackColor = true;
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
            // longGroupBox
            // 
            longGroupBox.Controls.Add(longNumericUpDown);
            longGroupBox.Controls.Add(longERadioButton);
            longGroupBox.Controls.Add(longWRadioButton);
            longGroupBox.Location = new Point(34, 152);
            longGroupBox.Name = "longGroupBox";
            longGroupBox.Size = new Size(238, 68);
            longGroupBox.TabIndex = 61;
            longGroupBox.TabStop = false;
            longGroupBox.Text = "Longitude (decimal degrees)";
            // 
            // longWRadioButton
            // 
            longWRadioButton.AutoSize = true;
            longWRadioButton.Font = new Font("Segoe UI", 15F);
            longWRadioButton.Location = new Point(65, 23);
            longWRadioButton.Name = "longWRadioButton";
            longWRadioButton.Size = new Size(59, 39);
            longWRadioButton.TabIndex = 20;
            longWRadioButton.Text = "W";
            longWRadioButton.UseVisualStyleBackColor = true;
            // 
            // allObjectsGridView
            // 
            allObjectsGridView.AllowUserToAddRows = false;
            allObjectsGridView.AllowUserToDeleteRows = false;
            allObjectsGridView.AllowUserToOrderColumns = true;
            allObjectsGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            allObjectsGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            allObjectsGridView.Columns.AddRange(new DataGridViewColumn[] { Type, PropertyTableID, LeftBottom, RightTop, Search, Edit, Delete });
            allObjectsGridView.Location = new Point(34, 265);
            allObjectsGridView.Name = "allObjectsGridView";
            allObjectsGridView.ReadOnly = true;
            allObjectsGridView.RowHeadersWidth = 51;
            allObjectsGridView.Size = new Size(1036, 406);
            allObjectsGridView.TabIndex = 62;
            allObjectsGridView.CellContentClick += allObjectsGridView_CellContentClick;
            allObjectsGridView.CellMouseClick += allObjectsGridView_CellMouseClick;
            allObjectsGridView.CellMouseDoubleClick += allObjectsGridView_CellMouseDoubleClick_1;
            // 
            // leftBottomGPSLabel
            // 
            leftBottomGPSLabel.AutoSize = true;
            leftBottomGPSLabel.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold);
            leftBottomGPSLabel.Location = new Point(36, 34);
            leftBottomGPSLabel.Margin = new Padding(0);
            leftBottomGPSLabel.Name = "leftBottomGPSLabel";
            leftBottomGPSLabel.Size = new Size(238, 38);
            leftBottomGPSLabel.TabIndex = 58;
            leftBottomGPSLabel.Text = "Left Bottom GPS";
            // 
            // Type
            // 
            Type.FillWeight = 88.78364F;
            Type.HeaderText = "Type";
            Type.MinimumWidth = 6;
            Type.Name = "Type";
            Type.ReadOnly = true;
            // 
            // PropertyTableID
            // 
            PropertyTableID.FillWeight = 88.78364F;
            PropertyTableID.HeaderText = "Property/Parcel Number";
            PropertyTableID.MinimumWidth = 6;
            PropertyTableID.Name = "PropertyTableID";
            PropertyTableID.ReadOnly = true;
            // 
            // LeftBottom
            // 
            LeftBottom.FillWeight = 88.78364F;
            LeftBottom.HeaderText = "Left Bottom Position";
            LeftBottom.MinimumWidth = 6;
            LeftBottom.Name = "LeftBottom";
            LeftBottom.ReadOnly = true;
            // 
            // RightTop
            // 
            RightTop.FillWeight = 88.78364F;
            RightTop.HeaderText = "Right Top Position";
            RightTop.MinimumWidth = 6;
            RightTop.Name = "RightTop";
            RightTop.ReadOnly = true;
            // 
            // Search
            // 
            Search.FillWeight = 25F;
            Search.HeaderText = "";
            Search.Image = (Image)resources.GetObject("Search.Image");
            Search.MinimumWidth = 6;
            Search.Name = "Search";
            Search.ReadOnly = true;
            // 
            // Edit
            // 
            Edit.FillWeight = 25F;
            Edit.HeaderText = "";
            Edit.Image = (Image)resources.GetObject("Edit.Image");
            Edit.MinimumWidth = 6;
            Edit.Name = "Edit";
            Edit.ReadOnly = true;
            // 
            // Delete
            // 
            Delete.FillWeight = 25F;
            Delete.HeaderText = "";
            Delete.Image = (Image)resources.GetObject("Delete.Image");
            Delete.MinimumWidth = 6;
            Delete.Name = "Delete";
            Delete.ReadOnly = true;
            // 
            // FindAllObjects
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(showAllButton);
            Controls.Add(latGroupBox2);
            Controls.Add(longGroupBox2);
            Controls.Add(rightTopGPSLabel);
            Controls.Add(searchAllObjectsButton);
            Controls.Add(latGroupBox);
            Controls.Add(longGroupBox);
            Controls.Add(allObjectsGridView);
            Controls.Add(leftBottomGPSLabel);
            Name = "FindAllObjects";
            Size = new Size(1366, 768);
            ((System.ComponentModel.ISupportInitialize)latNumericUpDown2).EndInit();
            latGroupBox2.ResumeLayout(false);
            latGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)longNumericUpDown2).EndInit();
            longGroupBox2.ResumeLayout(false);
            longGroupBox2.PerformLayout();
            latGroupBox.ResumeLayout(false);
            latGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)latNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)longNumericUpDown).EndInit();
            longGroupBox.ResumeLayout(false);
            longGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)allObjectsGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button showAllButton;
        private NumericUpDown latNumericUpDown2;
        private RadioButton latNRadioButton2;
        private GroupBox latGroupBox2;
        private RadioButton latSRadioButton2;
        private NumericUpDown longNumericUpDown2;
        private RadioButton longERadioButton2;
        private RadioButton longWRadioButton2;
        private GroupBox longGroupBox2;
        private Label rightTopGPSLabel;
        private Button searchAllObjectsButton;
        private GroupBox latGroupBox;
        private NumericUpDown latNumericUpDown;
        private RadioButton latNRadioButton;
        private RadioButton latSRadioButton;
        private RadioButton longERadioButton;
        private NumericUpDown longNumericUpDown;
        private GroupBox longGroupBox;
        private RadioButton longWRadioButton;
        private DataGridView allObjectsGridView;
        private Label leftBottomGPSLabel;
        private DataGridViewTextBoxColumn Type;
        private DataGridViewTextBoxColumn PropertyTableID;
        private DataGridViewTextBoxColumn LeftBottom;
        private DataGridViewTextBoxColumn RightTop;
        private DataGridViewImageColumn Search;
        private DataGridViewImageColumn Edit;
        private DataGridViewImageColumn Delete;
    }
}
