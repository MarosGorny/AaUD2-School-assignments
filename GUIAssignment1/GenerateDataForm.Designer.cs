namespace GUIAssignment1
{
    partial class GenerateDataForm
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
            titleLabel = new Label();
            parcelsCountLabel = new Label();
            parcelsCountNumericUpDown = new NumericUpDown();
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
            generateButton = new Button();
            propertiesCountLabel = new Label();
            propertiesCountNumericUpDown = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)parcelsCountNumericUpDown).BeginInit();
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
            ((System.ComponentModel.ISupportInitialize)propertiesCountNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            titleLabel.Location = new Point(12, 9);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(421, 41);
            titleLabel.TabIndex = 68;
            titleLabel.Text = "Area of New Generated Data";
            // 
            // parcelsCountLabel
            // 
            parcelsCountLabel.AutoSize = true;
            parcelsCountLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            parcelsCountLabel.Location = new Point(12, 266);
            parcelsCountLabel.Name = "parcelsCountLabel";
            parcelsCountLabel.Size = new Size(106, 20);
            parcelsCountLabel.TabIndex = 67;
            parcelsCountLabel.Text = "Parcels count:";
            // 
            // parcelsCountNumericUpDown
            // 
            parcelsCountNumericUpDown.Location = new Point(183, 266);
            parcelsCountNumericUpDown.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            parcelsCountNumericUpDown.Name = "parcelsCountNumericUpDown";
            parcelsCountNumericUpDown.Size = new Size(420, 27);
            parcelsCountNumericUpDown.TabIndex = 66;
            parcelsCountNumericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // insertGPS1GroupBox
            // 
            insertGPS1GroupBox.Controls.Add(gps1LongGroupBox);
            insertGPS1GroupBox.Controls.Add(gps1LatGroupBox);
            insertGPS1GroupBox.Location = new Point(12, 65);
            insertGPS1GroupBox.Name = "insertGPS1GroupBox";
            insertGPS1GroupBox.Size = new Size(286, 179);
            insertGPS1GroupBox.TabIndex = 65;
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
            gps1LongNumericUpDown.Location = new Point(114, 32);
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
            insertGPS2GroupBox.Location = new Point(317, 65);
            insertGPS2GroupBox.Name = "insertGPS2GroupBox";
            insertGPS2GroupBox.Size = new Size(286, 179);
            insertGPS2GroupBox.TabIndex = 64;
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
            gps2LatNumericUpDown.Location = new Point(113, 32);
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
            // generateButton
            // 
            generateButton.Location = new Point(12, 329);
            generateButton.Name = "generateButton";
            generateButton.Size = new Size(591, 29);
            generateButton.TabIndex = 63;
            generateButton.Text = "Generate";
            generateButton.UseVisualStyleBackColor = true;
            generateButton.Click += generateButton_Click;
            // 
            // propertiesCountLabel
            // 
            propertiesCountLabel.AutoSize = true;
            propertiesCountLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            propertiesCountLabel.Location = new Point(12, 296);
            propertiesCountLabel.Name = "propertiesCountLabel";
            propertiesCountLabel.Size = new Size(129, 20);
            propertiesCountLabel.TabIndex = 70;
            propertiesCountLabel.Text = "Properties count:";
            // 
            // propertiesCountNumericUpDown
            // 
            propertiesCountNumericUpDown.Location = new Point(183, 296);
            propertiesCountNumericUpDown.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            propertiesCountNumericUpDown.Name = "propertiesCountNumericUpDown";
            propertiesCountNumericUpDown.Size = new Size(420, 27);
            propertiesCountNumericUpDown.TabIndex = 69;
            propertiesCountNumericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // GenerateDataForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(626, 369);
            Controls.Add(propertiesCountLabel);
            Controls.Add(propertiesCountNumericUpDown);
            Controls.Add(titleLabel);
            Controls.Add(parcelsCountLabel);
            Controls.Add(parcelsCountNumericUpDown);
            Controls.Add(insertGPS1GroupBox);
            Controls.Add(insertGPS2GroupBox);
            Controls.Add(generateButton);
            Name = "GenerateDataForm";
            Text = "Generate New Data";
            ((System.ComponentModel.ISupportInitialize)parcelsCountNumericUpDown).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)propertiesCountNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label titleLabel;
        private Label parcelsCountLabel;
        private NumericUpDown parcelsCountNumericUpDown;
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
        private Button generateButton;
        private Label propertiesCountLabel;
        private NumericUpDown propertiesCountNumericUpDown;
    }
}