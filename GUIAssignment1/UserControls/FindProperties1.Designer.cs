namespace GUIAssignment1.UserControls
{
    partial class FindProperties
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
            propertyGridView = new DataGridView();
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
            searchPropertiesButton = new Button();
            descriptionLabel = new Label();
            descriptionTextBox = new TextBox();
            conscriptionNumberLabel = new Label();
            conscriptionNumberTextBox = new TextBox();
            searchGroupBox = new GroupBox();
            insertPropertyGroupBox = new GroupBox();
            insertPropertyButton = new Button();
            gps2LatNRadioButton = new RadioButton();
            gps2LatSRadioButton = new RadioButton();
            gps2LatGroupBox = new GroupBox();
            gps2LatNumericUpDown = new NumericUpDown();
            insertGPS2GroupBox = new GroupBox();
            gps2LongGroupBox = new GroupBox();
            gps2LongNumericUpDown = new NumericUpDown();
            gps2LongERadioButton = new RadioButton();
            gps2LongWRadioButton = new RadioButton();
            insertGPS1GroupBox = new GroupBox();
            gps1LongGroupBox = new GroupBox();
            gps1LongNumericUpDown = new NumericUpDown();
            gps1LongERadioButton = new RadioButton();
            gps1LongWRadioButton = new RadioButton();
            gps1LatGroupBox = new GroupBox();
            gps1LatNumericUpDown = new NumericUpDown();
            gps1LatNRadioButton = new RadioButton();
            gps1LatSRadioButton = new RadioButton();
            ((System.ComponentModel.ISupportInitialize)propertyGridView).BeginInit();
            longGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)longNumericUpDown).BeginInit();
            latGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)latNumericUpDown).BeginInit();
            insertPropertyGroupBox.SuspendLayout();
            gps2LatGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gps2LatNumericUpDown).BeginInit();
            insertGPS2GroupBox.SuspendLayout();
            gps2LongGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gps2LongNumericUpDown).BeginInit();
            insertGPS1GroupBox.SuspendLayout();
            gps1LongGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gps1LongNumericUpDown).BeginInit();
            gps1LatGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gps1LatNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // propertyGridView
            // 
            propertyGridView.AllowUserToAddRows = false;
            propertyGridView.AllowUserToDeleteRows = false;
            propertyGridView.AllowUserToOrderColumns = true;
            propertyGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            propertyGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            propertyGridView.Columns.AddRange(new DataGridViewColumn[] { PropertyTableID, PropertyTableDescription, ParcelList, LeftBottom, RightTop });
            propertyGridView.Location = new Point(330, 18);
            propertyGridView.Name = "propertyGridView";
            propertyGridView.ReadOnly = true;
            propertyGridView.RowHeadersWidth = 51;
            propertyGridView.RowTemplate.Height = 29;
            propertyGridView.Size = new Size(758, 740);
            propertyGridView.TabIndex = 41;
            propertyGridView.CellContentClick += dataGridView1_CellContentClick;
            propertyGridView.CellMouseDoubleClick += dataGridView1_CellMouseDoubleClick;
            // 
            // PropertyTableID
            // 
            PropertyTableID.HeaderText = "Conscription Number";
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
            ParcelList.HeaderText = "Parcel List";
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
            longGroupBox.Location = new Point(19, 121);
            longGroupBox.Name = "longGroupBox";
            longGroupBox.Size = new Size(286, 68);
            longGroupBox.TabIndex = 40;
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
            latGroupBox.Location = new Point(19, 47);
            latGroupBox.Name = "latGroupBox";
            latGroupBox.Size = new Size(286, 68);
            latGroupBox.TabIndex = 39;
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
            // searchPropertiesButton
            // 
            searchPropertiesButton.Location = new Point(19, 195);
            searchPropertiesButton.Name = "searchPropertiesButton";
            searchPropertiesButton.Size = new Size(286, 29);
            searchPropertiesButton.TabIndex = 38;
            searchPropertiesButton.Text = "Search Properties";
            searchPropertiesButton.UseVisualStyleBackColor = true;
            searchPropertiesButton.Click += button1_Click;
            // 
            // descriptionLabel
            // 
            descriptionLabel.AutoSize = true;
            descriptionLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            descriptionLabel.Location = new Point(14, 423);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new Size(121, 28);
            descriptionLabel.TabIndex = 45;
            descriptionLabel.Text = "Description";
            // 
            // descriptionTextBox
            // 
            descriptionTextBox.Location = new Point(11, 451);
            descriptionTextBox.Multiline = true;
            descriptionTextBox.Name = "descriptionTextBox";
            descriptionTextBox.Size = new Size(286, 27);
            descriptionTextBox.TabIndex = 44;
            // 
            // conscriptionNumberLabel
            // 
            conscriptionNumberLabel.AutoSize = true;
            conscriptionNumberLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            conscriptionNumberLabel.Location = new Point(14, 372);
            conscriptionNumberLabel.Name = "conscriptionNumberLabel";
            conscriptionNumberLabel.Size = new Size(214, 28);
            conscriptionNumberLabel.TabIndex = 43;
            conscriptionNumberLabel.Text = "Conscription Number";
            conscriptionNumberLabel.Click += label1_Click;
            // 
            // conscriptionNumberTextBox
            // 
            conscriptionNumberTextBox.Location = new Point(19, 644);
            conscriptionNumberTextBox.Name = "conscriptionNumberTextBox";
            conscriptionNumberTextBox.Size = new Size(286, 27);
            conscriptionNumberTextBox.TabIndex = 42;
            // 
            // searchGroupBox
            // 
            searchGroupBox.Location = new Point(8, 9);
            searchGroupBox.Name = "searchGroupBox";
            searchGroupBox.Size = new Size(316, 228);
            searchGroupBox.TabIndex = 48;
            searchGroupBox.TabStop = false;
            searchGroupBox.Text = "Search properties by GPS point";
            // 
            // insertPropertyGroupBox
            // 
            insertPropertyGroupBox.Controls.Add(insertGPS2GroupBox);
            insertPropertyGroupBox.Controls.Add(insertPropertyButton);
            insertPropertyGroupBox.Controls.Add(conscriptionNumberLabel);
            insertPropertyGroupBox.Controls.Add(descriptionLabel);
            insertPropertyGroupBox.Controls.Add(descriptionTextBox);
            insertPropertyGroupBox.Location = new Point(8, 243);
            insertPropertyGroupBox.Name = "insertPropertyGroupBox";
            insertPropertyGroupBox.Size = new Size(316, 515);
            insertPropertyGroupBox.TabIndex = 49;
            insertPropertyGroupBox.TabStop = false;
            insertPropertyGroupBox.Text = "Insert property";
            // 
            // insertPropertyButton
            // 
            insertPropertyButton.Location = new Point(11, 481);
            insertPropertyButton.Name = "insertPropertyButton";
            insertPropertyButton.Size = new Size(286, 29);
            insertPropertyButton.TabIndex = 50;
            insertPropertyButton.Text = "Insert Propety";
            insertPropertyButton.UseVisualStyleBackColor = true;
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
            gps2LatNumericUpDown.Location = new Point(110, 32);
            gps2LatNumericUpDown.Maximum = new decimal(new int[] { 90, 0, 0, 0 });
            gps2LatNumericUpDown.Name = "gps2LatNumericUpDown";
            gps2LatNumericUpDown.Size = new Size(140, 27);
            gps2LatNumericUpDown.TabIndex = 22;
            gps2LatNumericUpDown.Tag = "";
            // 
            // insertGPS2GroupBox
            // 
            insertGPS2GroupBox.Controls.Add(gps2LongGroupBox);
            insertGPS2GroupBox.Controls.Add(gps2LatGroupBox);
            insertGPS2GroupBox.Location = new Point(11, 199);
            insertGPS2GroupBox.Name = "insertGPS2GroupBox";
            insertGPS2GroupBox.Size = new Size(286, 179);
            insertGPS2GroupBox.TabIndex = 51;
            insertGPS2GroupBox.TabStop = false;
            insertGPS2GroupBox.Text = "Right Top GPS Point";
            insertGPS2GroupBox.Enter += groupBox4_Enter;
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
            gps2LongNumericUpDown.Location = new Point(110, 30);
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
            // insertGPS1GroupBox
            // 
            insertGPS1GroupBox.Controls.Add(gps1LongGroupBox);
            insertGPS1GroupBox.Controls.Add(gps1LatGroupBox);
            insertGPS1GroupBox.Location = new Point(19, 264);
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
            gps1LongNumericUpDown.Location = new Point(110, 32);
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
            gps1LatNumericUpDown.Location = new Point(110, 32);
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
            // FindProperties
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            Controls.Add(insertGPS1GroupBox);
            Controls.Add(conscriptionNumberTextBox);
            Controls.Add(propertyGridView);
            Controls.Add(longGroupBox);
            Controls.Add(latGroupBox);
            Controls.Add(searchPropertiesButton);
            Controls.Add(searchGroupBox);
            Controls.Add(insertPropertyGroupBox);
            Name = "FindProperties";
            Size = new Size(1366, 768);
            ((System.ComponentModel.ISupportInitialize)propertyGridView).EndInit();
            longGroupBox.ResumeLayout(false);
            longGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)longNumericUpDown).EndInit();
            latGroupBox.ResumeLayout(false);
            latGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)latNumericUpDown).EndInit();
            insertPropertyGroupBox.ResumeLayout(false);
            insertPropertyGroupBox.PerformLayout();
            gps2LatGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gps2LatNumericUpDown).EndInit();
            insertGPS2GroupBox.ResumeLayout(false);
            gps2LongGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gps2LongNumericUpDown).EndInit();
            insertGPS1GroupBox.ResumeLayout(false);
            gps1LongGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gps1LongNumericUpDown).EndInit();
            gps1LatGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gps1LatNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView propertyGridView;
        private GroupBox longGroupBox;
        private NumericUpDown longNumericUpDown;
        private RadioButton longERadioButton;
        private RadioButton longWRadioButton;
        private GroupBox latGroupBox;
        private NumericUpDown latNumericUpDown;
        private RadioButton latNRadioButton;
        private RadioButton latSRadioButton;
        private Button searchPropertiesButton;
        private DataGridViewTextBoxColumn PropertyTableID;
        private DataGridViewTextBoxColumn PropertyTableDescription;
        private DataGridViewTextBoxColumn ParcelList;
        private DataGridViewTextBoxColumn LeftBottom;
        private DataGridViewTextBoxColumn RightTop;
        private Label descriptionLabel;
        private TextBox descriptionTextBox;
        private Label conscriptionNumberLabel;
        private TextBox conscriptionNumberTextBox;
        private GroupBox searchGroupBox;
        private GroupBox insertPropertyGroupBox;
        private Button insertPropertyButton;
        private RadioButton gps2LatNRadioButton;
        private RadioButton gps2LatSRadioButton;
        private GroupBox gps2LatGroupBox;
        private NumericUpDown gps2LatNumericUpDown;
        private GroupBox insertGPS2GroupBox;
        private GroupBox gps2LongGroupBox;
        private NumericUpDown gps2LongNumericUpDown;
        private RadioButton gps2LongERadioButton;
        private RadioButton gps2LongWRadioButton;
        private GroupBox insertGPS1GroupBox;
        private GroupBox gps1LongGroupBox;
        private NumericUpDown gps1LongNumericUpDown;
        private RadioButton gps1LongERadioButton;
        private RadioButton gps1LongWRadioButton;
        private GroupBox gps1LatGroupBox;
        private NumericUpDown gps1LatNumericUpDown;
        private RadioButton gps1LatNRadioButton;
        private RadioButton gps1LatSRadioButton;
    }
}
