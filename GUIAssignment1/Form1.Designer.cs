﻿namespace GUIAssignment1
{
    partial class Form1
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
            groupBox2 = new GroupBox();
            numericUpDown2 = new NumericUpDown();
            radioButton1 = new RadioButton();
            radioButton3 = new RadioButton();
            groupBox1 = new GroupBox();
            numericUpDown1 = new NumericUpDown();
            radioButton5 = new RadioButton();
            radioButton2 = new RadioButton();
            button1 = new Button();
            dataGridView1 = new DataGridView();
            PropertyTableID = new DataGridViewTextBoxColumn();
            PropertyTableDescription = new DataGridViewTextBoxColumn();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(numericUpDown2);
            groupBox2.Controls.Add(radioButton1);
            groupBox2.Controls.Add(radioButton3);
            groupBox2.Location = new Point(12, 95);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(215, 68);
            groupBox2.TabIndex = 36;
            groupBox2.TabStop = false;
            groupBox2.Text = "Longitude (decimal degrees)";
            // 
            // numericUpDown2
            // 
            numericUpDown2.DecimalPlaces = 3;
            numericUpDown2.Location = new Point(122, 30);
            numericUpDown2.Maximum = new decimal(new int[] { 90, 0, 0, 0 });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(77, 27);
            numericUpDown2.TabIndex = 23;
            numericUpDown2.Tag = "";
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            radioButton1.Location = new Point(6, 23);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(49, 39);
            radioButton1.TabIndex = 21;
            radioButton1.TabStop = true;
            radioButton1.Text = "E";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            radioButton3.Location = new Point(65, 23);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(59, 39);
            radioButton3.TabIndex = 20;
            radioButton3.TabStop = true;
            radioButton3.Text = "W";
            radioButton3.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(numericUpDown1);
            groupBox1.Controls.Add(radioButton5);
            groupBox1.Controls.Add(radioButton2);
            groupBox1.Location = new Point(12, 21);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(215, 68);
            groupBox1.TabIndex = 35;
            groupBox1.TabStop = false;
            groupBox1.Text = "Latitude (decimal degrees)";
            // 
            // numericUpDown1
            // 
            numericUpDown1.DecimalPlaces = 3;
            numericUpDown1.Location = new Point(122, 31);
            numericUpDown1.Maximum = new decimal(new int[] { 90, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(77, 27);
            numericUpDown1.TabIndex = 22;
            numericUpDown1.Tag = "";
            // 
            // radioButton5
            // 
            radioButton5.AutoSize = true;
            radioButton5.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            radioButton5.Location = new Point(6, 23);
            radioButton5.Name = "radioButton5";
            radioButton5.Size = new Size(55, 39);
            radioButton5.TabIndex = 21;
            radioButton5.TabStop = true;
            radioButton5.Text = "N";
            radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            radioButton2.Location = new Point(65, 23);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(49, 39);
            radioButton2.TabIndex = 20;
            radioButton2.TabStop = true;
            radioButton2.Text = "S";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(12, 169);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 34;
            button1.Text = "Search";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { PropertyTableID, PropertyTableDescription });
            dataGridView1.Location = new Point(284, 21);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(812, 406);
            dataGridView1.TabIndex = 37;
            // 
            // PropertyTableID
            // 
            PropertyTableID.Frozen = true;
            PropertyTableID.HeaderText = "Conscription Number";
            PropertyTableID.MinimumWidth = 6;
            PropertyTableID.Name = "PropertyTableID";
            PropertyTableID.ReadOnly = true;
            PropertyTableID.Width = 180;
            // 
            // PropertyTableDescription
            // 
            PropertyTableDescription.Frozen = true;
            PropertyTableDescription.HeaderText = "Description";
            PropertyTableDescription.MinimumWidth = 6;
            PropertyTableDescription.Name = "PropertyTableDescription";
            PropertyTableDescription.ReadOnly = true;
            PropertyTableDescription.Width = 125;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1121, 450);
            Controls.Add(dataGridView1);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Vyhladavenie nehnutelnosti";
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox2;
        private NumericUpDown numericUpDown2;
        private RadioButton radioButton1;
        private RadioButton radioButton3;
        private GroupBox groupBox1;
        private NumericUpDown numericUpDown1;
        private RadioButton radioButton5;
        private RadioButton radioButton2;
        private Button button1;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn PropertyTableID;
        private DataGridViewTextBoxColumn PropertyTableDescription;
    }
}