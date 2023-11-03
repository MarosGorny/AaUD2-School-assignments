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
            ((System.ComponentModel.ISupportInitialize)propertyGridView).BeginInit();
            longGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)longNumericUpDown).BeginInit();
            latGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)latNumericUpDown).BeginInit();
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
            propertyGridView.Location = new Point(330, 47);
            propertyGridView.Name = "propertyGridView";
            propertyGridView.ReadOnly = true;
            propertyGridView.RowHeadersWidth = 51;
            propertyGridView.RowTemplate.Height = 29;
            propertyGridView.Size = new Size(758, 406);
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
            longGroupBox.Location = new Point(58, 121);
            longGroupBox.Name = "longGroupBox";
            longGroupBox.Size = new Size(215, 68);
            longGroupBox.TabIndex = 40;
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
            latGroupBox.Location = new Point(58, 47);
            latGroupBox.Name = "latGroupBox";
            latGroupBox.Size = new Size(215, 68);
            latGroupBox.TabIndex = 39;
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
            // searchPropertiesButton
            // 
            searchPropertiesButton.Location = new Point(58, 195);
            searchPropertiesButton.Name = "searchPropertiesButton";
            searchPropertiesButton.Size = new Size(215, 29);
            searchPropertiesButton.TabIndex = 38;
            searchPropertiesButton.Text = "Search Properties";
            searchPropertiesButton.UseVisualStyleBackColor = true;
            searchPropertiesButton.Click += button1_Click;
            // 
            // FindProperties
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            Controls.Add(propertyGridView);
            Controls.Add(longGroupBox);
            Controls.Add(latGroupBox);
            Controls.Add(searchPropertiesButton);
            Name = "FindProperties";
            Size = new Size(1366, 768);
            ((System.ComponentModel.ISupportInitialize)propertyGridView).EndInit();
            longGroupBox.ResumeLayout(false);
            longGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)longNumericUpDown).EndInit();
            latGroupBox.ResumeLayout(false);
            latGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)latNumericUpDown).EndInit();
            ResumeLayout(false);
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
    }
}
