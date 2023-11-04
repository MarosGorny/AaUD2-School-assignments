namespace GUIAssignment1.UserControls
{
    partial class FindParcels
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindParcels));
            parcelGridView = new DataGridView();
            ParcelTableID = new DataGridViewTextBoxColumn();
            PropertyTableDescription = new DataGridViewTextBoxColumn();
            PropertyList = new DataGridViewTextBoxColumn();
            LeftBottom = new DataGridViewTextBoxColumn();
            RightTop = new DataGridViewTextBoxColumn();
            Edit = new DataGridViewImageColumn();
            Delete = new DataGridViewImageColumn();
            searchGroupBox = new GroupBox();
            searchPropertiesButton = new Button();
            longGroupBox = new GroupBox();
            longNumericUpDown = new NumericUpDown();
            longERadioButton = new RadioButton();
            longWRadioButton = new RadioButton();
            latGroupBox = new GroupBox();
            latNumericUpDown = new NumericUpDown();
            latNRadioButton = new RadioButton();
            latSRadioButton = new RadioButton();
            insertGroupBox = new GroupBox();
            parcelNumberLabel = new Label();
            parcelNumberNumericUpDown = new NumericUpDown();
            insertGPS1GroupBox = new GroupBox();
            gps1LongGroupBox = new GroupBox();
            gps1LongNumericUpDown = new NumericUpDown();
            gps1LongERadioButton = new RadioButton();
            gps1LongWRadioButton = new RadioButton();
            gps1LatGroupBox = new GroupBox();
            gps1LatNumericUpDown = new NumericUpDown();
            gps1LatNRadioButton = new RadioButton();
            gps1LatSRadioButton = new RadioButton();
            insertGPS2GroupBox = new GroupBox();
            gps2LongGroupBox = new GroupBox();
            gps2LongNumericUpDown = new NumericUpDown();
            gps2LongERadioButton = new RadioButton();
            gps2LongWRadioButton = new RadioButton();
            gps2LatGroupBox = new GroupBox();
            gps2LatNumericUpDown = new NumericUpDown();
            gps2LatNRadioButton = new RadioButton();
            gps2LatSRadioButton = new RadioButton();
            insertParcelButton = new Button();
            descriptionTextBox = new TextBox();
            ((System.ComponentModel.ISupportInitialize)parcelGridView).BeginInit();
            searchGroupBox.SuspendLayout();
            longGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)longNumericUpDown).BeginInit();
            latGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)latNumericUpDown).BeginInit();
            insertGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)parcelNumberNumericUpDown).BeginInit();
            insertGPS1GroupBox.SuspendLayout();
            gps1LongGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gps1LongNumericUpDown).BeginInit();
            gps1LatGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gps1LatNumericUpDown).BeginInit();
            insertGPS2GroupBox.SuspendLayout();
            gps2LongGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gps2LongNumericUpDown).BeginInit();
            gps2LatGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gps2LatNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // parcelGridView
            // 
            parcelGridView.AllowUserToAddRows = false;
            parcelGridView.AllowUserToDeleteRows = false;
            parcelGridView.AllowUserToOrderColumns = true;
            parcelGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            parcelGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            parcelGridView.Columns.AddRange(new DataGridViewColumn[] { ParcelTableID, PropertyTableDescription, PropertyList, LeftBottom, RightTop, Edit, Delete });
            parcelGridView.Location = new Point(339, 21);
            parcelGridView.Name = "parcelGridView";
            parcelGridView.ReadOnly = true;
            parcelGridView.RowHeadersWidth = 51;
            parcelGridView.RowTemplate.Height = 29;
            parcelGridView.Size = new Size(749, 741);
            parcelGridView.TabIndex = 45;
            parcelGridView.CellContentClick += parcelGridView_CellContentClick;
            parcelGridView.CellMouseClick += parcelGridView_CellMouseClick;
            parcelGridView.CellMouseDoubleClick += parcelGridView_CellMouseDoubleClick;
            // 
            // ParcelTableID
            // 
            ParcelTableID.FillWeight = 88.1550751F;
            ParcelTableID.HeaderText = "Parcel Number";
            ParcelTableID.MinimumWidth = 6;
            ParcelTableID.Name = "ParcelTableID";
            ParcelTableID.ReadOnly = true;
            // 
            // PropertyTableDescription
            // 
            PropertyTableDescription.FillWeight = 88.1550751F;
            PropertyTableDescription.HeaderText = "Description";
            PropertyTableDescription.MinimumWidth = 6;
            PropertyTableDescription.Name = "PropertyTableDescription";
            PropertyTableDescription.ReadOnly = true;
            // 
            // PropertyList
            // 
            PropertyList.FillWeight = 88.1550751F;
            PropertyList.HeaderText = "Property List";
            PropertyList.MinimumWidth = 6;
            PropertyList.Name = "PropertyList";
            PropertyList.ReadOnly = true;
            // 
            // LeftBottom
            // 
            LeftBottom.FillWeight = 88.1550751F;
            LeftBottom.HeaderText = "Left Bottom Position";
            LeftBottom.MinimumWidth = 6;
            LeftBottom.Name = "LeftBottom";
            LeftBottom.ReadOnly = true;
            // 
            // RightTop
            // 
            RightTop.FillWeight = 88.1550751F;
            RightTop.HeaderText = "Right Top Position";
            RightTop.MinimumWidth = 6;
            RightTop.Name = "RightTop";
            RightTop.ReadOnly = true;
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
            // searchGroupBox
            // 
            searchGroupBox.Controls.Add(searchPropertiesButton);
            searchGroupBox.Controls.Add(longGroupBox);
            searchGroupBox.Controls.Add(latGroupBox);
            searchGroupBox.Location = new Point(13, 12);
            searchGroupBox.Name = "searchGroupBox";
            searchGroupBox.Size = new Size(316, 219);
            searchGroupBox.TabIndex = 49;
            searchGroupBox.TabStop = false;
            searchGroupBox.Text = "Search parcels by GPS point";
            // 
            // searchPropertiesButton
            // 
            searchPropertiesButton.Location = new Point(15, 174);
            searchPropertiesButton.Name = "searchPropertiesButton";
            searchPropertiesButton.Size = new Size(286, 29);
            searchPropertiesButton.TabIndex = 50;
            searchPropertiesButton.Text = "Search Parcels";
            searchPropertiesButton.UseVisualStyleBackColor = true;
            searchPropertiesButton.Click += searchPropertiesButton_Click;
            // 
            // longGroupBox
            // 
            longGroupBox.Controls.Add(longNumericUpDown);
            longGroupBox.Controls.Add(longERadioButton);
            longGroupBox.Controls.Add(longWRadioButton);
            longGroupBox.Location = new Point(15, 100);
            longGroupBox.Name = "longGroupBox";
            longGroupBox.Size = new Size(286, 68);
            longGroupBox.TabIndex = 51;
            longGroupBox.TabStop = false;
            longGroupBox.Text = "Longitude (decimal degrees)";
            // 
            // longNumericUpDown
            // 
            longNumericUpDown.DecimalPlaces = 13;
            longNumericUpDown.Location = new Point(125, 30);
            longNumericUpDown.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
            longNumericUpDown.Name = "longNumericUpDown";
            longNumericUpDown.Size = new Size(150, 27);
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
            latGroupBox.Location = new Point(15, 26);
            latGroupBox.Name = "latGroupBox";
            latGroupBox.Size = new Size(286, 68);
            latGroupBox.TabIndex = 50;
            latGroupBox.TabStop = false;
            latGroupBox.Text = "Latitude (decimal degrees)";
            // 
            // latNumericUpDown
            // 
            latNumericUpDown.DecimalPlaces = 13;
            latNumericUpDown.Location = new Point(125, 31);
            latNumericUpDown.Maximum = new decimal(new int[] { 90, 0, 0, 0 });
            latNumericUpDown.Name = "latNumericUpDown";
            latNumericUpDown.Size = new Size(150, 27);
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
            // insertGroupBox
            // 
            insertGroupBox.Controls.Add(parcelNumberLabel);
            insertGroupBox.Controls.Add(parcelNumberNumericUpDown);
            insertGroupBox.Controls.Add(insertGPS1GroupBox);
            insertGroupBox.Controls.Add(insertGPS2GroupBox);
            insertGroupBox.Controls.Add(insertParcelButton);
            insertGroupBox.Controls.Add(descriptionTextBox);
            insertGroupBox.Location = new Point(13, 237);
            insertGroupBox.Name = "insertGroupBox";
            insertGroupBox.Size = new Size(316, 515);
            insertGroupBox.TabIndex = 50;
            insertGroupBox.TabStop = false;
            insertGroupBox.Text = "Insert parcel";
            // 
            // parcelNumberLabel
            // 
            parcelNumberLabel.AutoSize = true;
            parcelNumberLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            parcelNumberLabel.Location = new Point(11, 403);
            parcelNumberLabel.Name = "parcelNumberLabel";
            parcelNumberLabel.Size = new Size(115, 20);
            parcelNumberLabel.TabIndex = 54;
            parcelNumberLabel.Text = "Parcel number:";
            // 
            // parcelNumberNumericUpDown
            // 
            parcelNumberNumericUpDown.Location = new Point(177, 401);
            parcelNumberNumericUpDown.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            parcelNumberNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            parcelNumberNumericUpDown.Name = "parcelNumberNumericUpDown";
            parcelNumberNumericUpDown.Size = new Size(120, 27);
            parcelNumberNumericUpDown.TabIndex = 53;
            parcelNumberNumericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // insertGPS1GroupBox
            // 
            insertGPS1GroupBox.Controls.Add(gps1LongGroupBox);
            insertGPS1GroupBox.Controls.Add(gps1LatGroupBox);
            insertGPS1GroupBox.Location = new Point(11, 22);
            insertGPS1GroupBox.Name = "insertGPS1GroupBox";
            insertGPS1GroupBox.Size = new Size(286, 179);
            insertGPS1GroupBox.TabIndex = 52;
            insertGPS1GroupBox.TabStop = false;
            insertGPS1GroupBox.Text = "Left Bottom GPS Point";
            // 
            // gps1LongGroupBox
            // 
            gps1LongGroupBox.Controls.Add(gps1LongNumericUpDown);
            gps1LongGroupBox.Controls.Add(gps1LongERadioButton);
            gps1LongGroupBox.Controls.Add(gps1LongWRadioButton);
            gps1LongGroupBox.Location = new Point(15, 96);
            gps1LongGroupBox.Name = "gps1LongGroupBox";
            gps1LongGroupBox.Size = new Size(260, 68);
            gps1LongGroupBox.TabIndex = 48;
            gps1LongGroupBox.TabStop = false;
            gps1LongGroupBox.Text = "Longitude (decimal degrees)";
            // 
            // gps1LongNumericUpDown
            // 
            gps1LongNumericUpDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            gps1LongNumericUpDown.DecimalPlaces = 14;
            gps1LongNumericUpDown.Location = new Point(115, 32);
            gps1LongNumericUpDown.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
            gps1LongNumericUpDown.Name = "gps1LongNumericUpDown";
            gps1LongNumericUpDown.Size = new Size(140, 27);
            gps1LongNumericUpDown.TabIndex = 23;
            gps1LongNumericUpDown.Tag = "";
            // 
            // gps1LongERadioButton
            // 
            gps1LongERadioButton.Checked = true;
            gps1LongERadioButton.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            gps1LongERadioButton.Location = new Point(6, 23);
            gps1LongERadioButton.Name = "gps1LongERadioButton";
            gps1LongERadioButton.Size = new Size(43, 39);
            gps1LongERadioButton.TabIndex = 21;
            gps1LongERadioButton.TabStop = true;
            gps1LongERadioButton.Text = "E";
            gps1LongERadioButton.UseVisualStyleBackColor = true;
            // 
            // gps1LongWRadioButton
            // 
            gps1LongWRadioButton.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            gps1LongWRadioButton.Location = new Point(55, 23);
            gps1LongWRadioButton.Name = "gps1LongWRadioButton";
            gps1LongWRadioButton.Size = new Size(54, 39);
            gps1LongWRadioButton.TabIndex = 20;
            gps1LongWRadioButton.Text = "W";
            gps1LongWRadioButton.UseVisualStyleBackColor = true;
            // 
            // gps1LatGroupBox
            // 
            gps1LatGroupBox.Controls.Add(gps1LatNumericUpDown);
            gps1LatGroupBox.Controls.Add(gps1LatNRadioButton);
            gps1LatGroupBox.Controls.Add(gps1LatSRadioButton);
            gps1LatGroupBox.Location = new Point(15, 22);
            gps1LatGroupBox.Name = "gps1LatGroupBox";
            gps1LatGroupBox.Size = new Size(260, 68);
            gps1LatGroupBox.TabIndex = 50;
            gps1LatGroupBox.TabStop = false;
            gps1LatGroupBox.Text = "Latitude (decimal degrees)";
            // 
            // gps1LatNumericUpDown
            // 
            gps1LatNumericUpDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            gps1LatNumericUpDown.DecimalPlaces = 13;
            gps1LatNumericUpDown.Location = new Point(114, 32);
            gps1LatNumericUpDown.Maximum = new decimal(new int[] { 90, 0, 0, 0 });
            gps1LatNumericUpDown.Name = "gps1LatNumericUpDown";
            gps1LatNumericUpDown.Size = new Size(140, 27);
            gps1LatNumericUpDown.TabIndex = 22;
            gps1LatNumericUpDown.Tag = "";
            // 
            // gps1LatNRadioButton
            // 
            gps1LatNRadioButton.Checked = true;
            gps1LatNRadioButton.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            gps1LatNRadioButton.Location = new Point(6, 23);
            gps1LatNRadioButton.Name = "gps1LatNRadioButton";
            gps1LatNRadioButton.Size = new Size(43, 39);
            gps1LatNRadioButton.TabIndex = 21;
            gps1LatNRadioButton.TabStop = true;
            gps1LatNRadioButton.Text = "N";
            gps1LatNRadioButton.UseVisualStyleBackColor = true;
            // 
            // gps1LatSRadioButton
            // 
            gps1LatSRadioButton.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            gps1LatSRadioButton.Location = new Point(55, 23);
            gps1LatSRadioButton.Name = "gps1LatSRadioButton";
            gps1LatSRadioButton.Size = new Size(39, 39);
            gps1LatSRadioButton.TabIndex = 20;
            gps1LatSRadioButton.Text = "S";
            gps1LatSRadioButton.UseVisualStyleBackColor = true;
            // 
            // insertGPS2GroupBox
            // 
            insertGPS2GroupBox.Controls.Add(gps2LongGroupBox);
            insertGPS2GroupBox.Controls.Add(gps2LatGroupBox);
            insertGPS2GroupBox.Location = new Point(11, 216);
            insertGPS2GroupBox.Name = "insertGPS2GroupBox";
            insertGPS2GroupBox.Size = new Size(286, 179);
            insertGPS2GroupBox.TabIndex = 51;
            insertGPS2GroupBox.TabStop = false;
            insertGPS2GroupBox.Text = "Right Top GPS Point";
            // 
            // gps2LongGroupBox
            // 
            gps2LongGroupBox.Controls.Add(gps2LongNumericUpDown);
            gps2LongGroupBox.Controls.Add(gps2LongERadioButton);
            gps2LongGroupBox.Controls.Add(gps2LongWRadioButton);
            gps2LongGroupBox.Location = new Point(15, 96);
            gps2LongGroupBox.Name = "gps2LongGroupBox";
            gps2LongGroupBox.Size = new Size(260, 68);
            gps2LongGroupBox.TabIndex = 48;
            gps2LongGroupBox.TabStop = false;
            gps2LongGroupBox.Text = "Longitude (decimal degrees)";
            // 
            // gps2LongNumericUpDown
            // 
            gps2LongNumericUpDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            gps2LongNumericUpDown.DecimalPlaces = 14;
            gps2LongNumericUpDown.Location = new Point(114, 30);
            gps2LongNumericUpDown.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
            gps2LongNumericUpDown.Name = "gps2LongNumericUpDown";
            gps2LongNumericUpDown.Size = new Size(140, 27);
            gps2LongNumericUpDown.TabIndex = 23;
            gps2LongNumericUpDown.Tag = "";
            // 
            // gps2LongERadioButton
            // 
            gps2LongERadioButton.Checked = true;
            gps2LongERadioButton.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            gps2LongERadioButton.Location = new Point(6, 23);
            gps2LongERadioButton.Name = "gps2LongERadioButton";
            gps2LongERadioButton.Size = new Size(43, 39);
            gps2LongERadioButton.TabIndex = 21;
            gps2LongERadioButton.TabStop = true;
            gps2LongERadioButton.Text = "E";
            gps2LongERadioButton.UseVisualStyleBackColor = true;
            // 
            // gps2LongWRadioButton
            // 
            gps2LongWRadioButton.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            gps2LongWRadioButton.Location = new Point(55, 23);
            gps2LongWRadioButton.Name = "gps2LongWRadioButton";
            gps2LongWRadioButton.Size = new Size(54, 39);
            gps2LongWRadioButton.TabIndex = 20;
            gps2LongWRadioButton.Text = "W";
            gps2LongWRadioButton.UseVisualStyleBackColor = true;
            // 
            // gps2LatGroupBox
            // 
            gps2LatGroupBox.Controls.Add(gps2LatNumericUpDown);
            gps2LatGroupBox.Controls.Add(gps2LatNRadioButton);
            gps2LatGroupBox.Controls.Add(gps2LatSRadioButton);
            gps2LatGroupBox.Location = new Point(15, 22);
            gps2LatGroupBox.Name = "gps2LatGroupBox";
            gps2LatGroupBox.Size = new Size(260, 68);
            gps2LatGroupBox.TabIndex = 50;
            gps2LatGroupBox.TabStop = false;
            gps2LatGroupBox.Text = "Latitude (decimal degrees)";
            // 
            // gps2LatNumericUpDown
            // 
            gps2LatNumericUpDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            gps2LatNumericUpDown.DecimalPlaces = 13;
            gps2LatNumericUpDown.Location = new Point(114, 32);
            gps2LatNumericUpDown.Maximum = new decimal(new int[] { 90, 0, 0, 0 });
            gps2LatNumericUpDown.Name = "gps2LatNumericUpDown";
            gps2LatNumericUpDown.Size = new Size(140, 27);
            gps2LatNumericUpDown.TabIndex = 22;
            gps2LatNumericUpDown.Tag = "";
            // 
            // gps2LatNRadioButton
            // 
            gps2LatNRadioButton.Checked = true;
            gps2LatNRadioButton.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            gps2LatNRadioButton.Location = new Point(6, 23);
            gps2LatNRadioButton.Name = "gps2LatNRadioButton";
            gps2LatNRadioButton.Size = new Size(43, 39);
            gps2LatNRadioButton.TabIndex = 21;
            gps2LatNRadioButton.TabStop = true;
            gps2LatNRadioButton.Text = "N";
            gps2LatNRadioButton.UseVisualStyleBackColor = true;
            // 
            // gps2LatSRadioButton
            // 
            gps2LatSRadioButton.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            gps2LatSRadioButton.Location = new Point(55, 23);
            gps2LatSRadioButton.Name = "gps2LatSRadioButton";
            gps2LatSRadioButton.Size = new Size(39, 39);
            gps2LatSRadioButton.TabIndex = 20;
            gps2LatSRadioButton.Text = "S";
            gps2LatSRadioButton.UseVisualStyleBackColor = true;
            // 
            // insertParcelButton
            // 
            insertParcelButton.Location = new Point(11, 481);
            insertParcelButton.Name = "insertParcelButton";
            insertParcelButton.Size = new Size(286, 29);
            insertParcelButton.TabIndex = 50;
            insertParcelButton.Text = "Insert Parcel";
            insertParcelButton.UseVisualStyleBackColor = true;
            insertParcelButton.Click += insertPropertyButton_Click;
            // 
            // descriptionTextBox
            // 
            descriptionTextBox.Location = new Point(11, 434);
            descriptionTextBox.Multiline = true;
            descriptionTextBox.Name = "descriptionTextBox";
            descriptionTextBox.PlaceholderText = "Description of Parcel";
            descriptionTextBox.Size = new Size(286, 27);
            descriptionTextBox.TabIndex = 44;
            // 
            // FindParcels
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(insertGroupBox);
            Controls.Add(searchGroupBox);
            Controls.Add(parcelGridView);
            Name = "FindParcels";
            Size = new Size(1366, 768);
            ((System.ComponentModel.ISupportInitialize)parcelGridView).EndInit();
            searchGroupBox.ResumeLayout(false);
            longGroupBox.ResumeLayout(false);
            longGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)longNumericUpDown).EndInit();
            latGroupBox.ResumeLayout(false);
            latGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)latNumericUpDown).EndInit();
            insertGroupBox.ResumeLayout(false);
            insertGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)parcelNumberNumericUpDown).EndInit();
            insertGPS1GroupBox.ResumeLayout(false);
            gps1LongGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gps1LongNumericUpDown).EndInit();
            gps1LatGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gps1LatNumericUpDown).EndInit();
            insertGPS2GroupBox.ResumeLayout(false);
            gps2LongGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gps2LongNumericUpDown).EndInit();
            gps2LatGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gps2LatNumericUpDown).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView parcelGridView;
        private GroupBox searchGroupBox;
        private GroupBox longGroupBox;
        private NumericUpDown longNumericUpDown;
        private RadioButton longERadioButton;
        private RadioButton longWRadioButton;
        private GroupBox latGroupBox;
        private NumericUpDown latNumericUpDown;
        private RadioButton latNRadioButton;
        private RadioButton latSRadioButton;
        private Button searchPropertiesButton;
        private GroupBox insertGroupBox;
        private Label parcelNumberLabel;
        private NumericUpDown parcelNumberNumericUpDown;
        private GroupBox insertGPS1GroupBox;
        private GroupBox gps1LongGroupBox;
        private NumericUpDown gps1LongNumericUpDown;
        private RadioButton gps1LongERadioButton;
        private RadioButton gps1LongWRadioButton;
        private GroupBox gps1LatGroupBox;
        private NumericUpDown gps1LatNumericUpDown;
        private RadioButton gps1LatNRadioButton;
        private RadioButton gps1LatSRadioButton;
        private GroupBox insertGPS2GroupBox;
        private GroupBox gps2LongGroupBox;
        private NumericUpDown gps2LongNumericUpDown;
        private RadioButton gps2LongERadioButton;
        private RadioButton gps2LongWRadioButton;
        private GroupBox gps2LatGroupBox;
        private NumericUpDown gps2LatNumericUpDown;
        private RadioButton gps2LatNRadioButton;
        private RadioButton gps2LatSRadioButton;
        private Button insertParcelButton;
        private TextBox descriptionTextBox;
        private DataGridViewTextBoxColumn ParcelTableID;
        private DataGridViewTextBoxColumn PropertyTableDescription;
        private DataGridViewTextBoxColumn PropertyList;
        private DataGridViewTextBoxColumn LeftBottom;
        private DataGridViewTextBoxColumn RightTop;
        private DataGridViewImageColumn Edit;
        private DataGridViewImageColumn Delete;
    }
}
