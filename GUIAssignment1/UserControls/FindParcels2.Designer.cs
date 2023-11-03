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
            parcelGridView = new DataGridView();
            longGroupBox = new GroupBox();
            longNumericUpDown = new NumericUpDown();
            longERadioButton = new RadioButton();
            longWRadioButton = new RadioButton();
            latGroupBox = new GroupBox();
            latNumericUpDown = new NumericUpDown();
            latNRadioButton = new RadioButton();
            latSRadioButton = new RadioButton();
            searchParcelButton = new Button();
            ParcelTableID = new DataGridViewTextBoxColumn();
            PropertyTableDescription = new DataGridViewTextBoxColumn();
            PropertyList = new DataGridViewTextBoxColumn();
            LeftBottom = new DataGridViewTextBoxColumn();
            RightTop = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)parcelGridView).BeginInit();
            longGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)longNumericUpDown).BeginInit();
            latGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)latNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // parcelGridView
            // 
            parcelGridView.AllowUserToAddRows = false;
            parcelGridView.AllowUserToDeleteRows = false;
            parcelGridView.AllowUserToOrderColumns = true;
            parcelGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            parcelGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            parcelGridView.Columns.AddRange(new DataGridViewColumn[] { ParcelTableID, PropertyTableDescription, PropertyList, LeftBottom, RightTop });
            parcelGridView.Location = new Point(330, 47);
            parcelGridView.Name = "parcelGridView";
            parcelGridView.ReadOnly = true;
            parcelGridView.RowHeadersWidth = 51;
            parcelGridView.RowTemplate.Height = 29;
            parcelGridView.Size = new Size(758, 406);
            parcelGridView.TabIndex = 45;
            parcelGridView.CellContentClick += propertyGridView_CellContentClick;
            parcelGridView.CellMouseDoubleClick += parcelGridView_CellMouseDoubleClick;
            // 
            // longGroupBox
            // 
            longGroupBox.Controls.Add(longNumericUpDown);
            longGroupBox.Controls.Add(longERadioButton);
            longGroupBox.Controls.Add(longWRadioButton);
            longGroupBox.Location = new Point(58, 121);
            longGroupBox.Name = "longGroupBox";
            longGroupBox.Size = new Size(215, 68);
            longGroupBox.TabIndex = 44;
            longGroupBox.TabStop = false;
            longGroupBox.Text = "Longitude (decimal degrees)";
            longGroupBox.Enter += longGroupBox_Enter;
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
            longNumericUpDown.ValueChanged += longNumericUpDown_ValueChanged;
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
            longERadioButton.CheckedChanged += longERadioButton_CheckedChanged;
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
            longWRadioButton.CheckedChanged += longWRadioButton_CheckedChanged;
            // 
            // latGroupBox
            // 
            latGroupBox.Controls.Add(latNumericUpDown);
            latGroupBox.Controls.Add(latNRadioButton);
            latGroupBox.Controls.Add(latSRadioButton);
            latGroupBox.Location = new Point(58, 47);
            latGroupBox.Name = "latGroupBox";
            latGroupBox.Size = new Size(215, 68);
            latGroupBox.TabIndex = 43;
            latGroupBox.TabStop = false;
            latGroupBox.Text = "Latitude (decimal degrees)";
            latGroupBox.Enter += latGroupBox_Enter;
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
            latNumericUpDown.ValueChanged += latNumericUpDown_ValueChanged;
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
            latNRadioButton.CheckedChanged += latNRadioButton_CheckedChanged;
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
            latSRadioButton.CheckedChanged += latSRadioButton_CheckedChanged;
            // 
            // searchParcelButton
            // 
            searchParcelButton.Location = new Point(58, 195);
            searchParcelButton.Name = "searchParcelButton";
            searchParcelButton.Size = new Size(215, 29);
            searchParcelButton.TabIndex = 42;
            searchParcelButton.Text = "Search Parcels";
            searchParcelButton.UseVisualStyleBackColor = true;
            searchParcelButton.Click += SearchPropertiesButton_Click_1;
            // 
            // ParcelTableID
            // 
            ParcelTableID.HeaderText = "Parcel Number";
            ParcelTableID.MinimumWidth = 6;
            ParcelTableID.Name = "ParcelTableID";
            ParcelTableID.ReadOnly = true;
            // 
            // PropertyTableDescription
            // 
            PropertyTableDescription.HeaderText = "Description";
            PropertyTableDescription.MinimumWidth = 6;
            PropertyTableDescription.Name = "PropertyTableDescription";
            PropertyTableDescription.ReadOnly = true;
            // 
            // PropertyList
            // 
            PropertyList.HeaderText = "Property List";
            PropertyList.MinimumWidth = 6;
            PropertyList.Name = "PropertyList";
            PropertyList.ReadOnly = true;
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
            // FindParcels
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(parcelGridView);
            Controls.Add(longGroupBox);
            Controls.Add(latGroupBox);
            Controls.Add(searchParcelButton);
            Name = "FindParcels";
            Size = new Size(1366, 768);
            ((System.ComponentModel.ISupportInitialize)parcelGridView).EndInit();
            longGroupBox.ResumeLayout(false);
            longGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)longNumericUpDown).EndInit();
            latGroupBox.ResumeLayout(false);
            latGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)latNumericUpDown).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView parcelGridView;
        private GroupBox longGroupBox;
        private NumericUpDown longNumericUpDown;
        private RadioButton longERadioButton;
        private RadioButton longWRadioButton;
        private GroupBox latGroupBox;
        private NumericUpDown latNumericUpDown;
        private RadioButton latNRadioButton;
        private RadioButton latSRadioButton;
        private Button searchParcelButton;
        private DataGridViewTextBoxColumn ParcelTableID;
        private DataGridViewTextBoxColumn PropertyTableDescription;
        private DataGridViewTextBoxColumn PropertyList;
        private DataGridViewTextBoxColumn LeftBottom;
        private DataGridViewTextBoxColumn RightTop;
    }
}
