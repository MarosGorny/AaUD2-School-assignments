namespace GUIAssignment2
{
    partial class MainForm
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
            panel1 = new Panel();
            ActionsTableLayoutPanel = new TableLayoutPanel();
            FindRealtyNavButton = new Button();
            FindParcelsNavButton = new Button();
            FindPropertiesNavButton = new Button();
            FileTableLayoutPanel = new TableLayoutPanel();
            ExportButton = new Button();
            ImportButton = new Button();
            generateDataButton = new Button();
            MainPanel = new Panel();
            panel1.SuspendLayout();
            ActionsTableLayoutPanel.SuspendLayout();
            FileTableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(ActionsTableLayoutPanel);
            panel1.Dock = DockStyle.Right;
            panel1.Location = new Point(1121, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(227, 821);
            panel1.TabIndex = 40;
            // 
            // ActionsTableLayoutPanel
            // 
            ActionsTableLayoutPanel.ColumnCount = 1;
            ActionsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            ActionsTableLayoutPanel.Controls.Add(FindRealtyNavButton, 0, 2);
            ActionsTableLayoutPanel.Controls.Add(FindParcelsNavButton, 0, 1);
            ActionsTableLayoutPanel.Controls.Add(FindPropertiesNavButton, 0, 0);
            ActionsTableLayoutPanel.Controls.Add(FileTableLayoutPanel, 0, 3);
            ActionsTableLayoutPanel.Controls.Add(generateDataButton, 0, 4);
            ActionsTableLayoutPanel.Dock = DockStyle.Top;
            ActionsTableLayoutPanel.Location = new Point(0, 0);
            ActionsTableLayoutPanel.Name = "ActionsTableLayoutPanel";
            ActionsTableLayoutPanel.RowCount = 10;
            ActionsTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10.0000982F));
            ActionsTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10.0001F));
            ActionsTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10.0001F));
            ActionsTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10.0001F));
            ActionsTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10.0001F));
            ActionsTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10.0001F));
            ActionsTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10.0001F));
            ActionsTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10.0001F));
            ActionsTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10.0001F));
            ActionsTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 9.9991F));
            ActionsTableLayoutPanel.Size = new Size(227, 721);
            ActionsTableLayoutPanel.TabIndex = 0;
            // 
            // FindRealtyNavButton
            // 
            FindRealtyNavButton.Dock = DockStyle.Fill;
            FindRealtyNavButton.Location = new Point(3, 147);
            FindRealtyNavButton.Name = "FindRealtyNavButton";
            FindRealtyNavButton.Size = new Size(221, 66);
            FindRealtyNavButton.TabIndex = 2;
            FindRealtyNavButton.Text = "Search Realty Objects";
            FindRealtyNavButton.UseVisualStyleBackColor = true;
            FindRealtyNavButton.Click += SwitchToAllObjects_Click;
            // 
            // FindParcelsNavButton
            // 
            FindParcelsNavButton.Dock = DockStyle.Fill;
            FindParcelsNavButton.Location = new Point(3, 75);
            FindParcelsNavButton.Name = "FindParcelsNavButton";
            FindParcelsNavButton.Size = new Size(221, 66);
            FindParcelsNavButton.TabIndex = 1;
            FindParcelsNavButton.Text = "Parcels";
            FindParcelsNavButton.UseVisualStyleBackColor = true;
            FindParcelsNavButton.Click += SwitchToParcels_Click;
            // 
            // FindPropertiesNavButton
            // 
            FindPropertiesNavButton.Dock = DockStyle.Fill;
            FindPropertiesNavButton.Location = new Point(3, 3);
            FindPropertiesNavButton.Name = "FindPropertiesNavButton";
            FindPropertiesNavButton.Size = new Size(221, 66);
            FindPropertiesNavButton.TabIndex = 0;
            FindPropertiesNavButton.Text = "Properties";
            FindPropertiesNavButton.UseVisualStyleBackColor = true;
            FindPropertiesNavButton.Click += SwitchToProperties_Click;
            // 
            // FileTableLayoutPanel
            // 
            FileTableLayoutPanel.ColumnCount = 2;
            FileTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            FileTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            FileTableLayoutPanel.Controls.Add(ExportButton, 1, 0);
            FileTableLayoutPanel.Controls.Add(ImportButton, 0, 0);
            FileTableLayoutPanel.Location = new Point(3, 219);
            FileTableLayoutPanel.Name = "FileTableLayoutPanel";
            FileTableLayoutPanel.RowCount = 1;
            FileTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            FileTableLayoutPanel.Size = new Size(221, 66);
            FileTableLayoutPanel.TabIndex = 11;
            // 
            // ExportButton
            // 
            ExportButton.Dock = DockStyle.Fill;
            ExportButton.Location = new Point(113, 3);
            ExportButton.Name = "ExportButton";
            ExportButton.Size = new Size(105, 60);
            ExportButton.TabIndex = 1;
            ExportButton.Text = "EXPORT";
            ExportButton.UseVisualStyleBackColor = true;
            ExportButton.Click += ExportButton_Click;
            // 
            // ImportButton
            // 
            ImportButton.Dock = DockStyle.Fill;
            ImportButton.Location = new Point(3, 3);
            ImportButton.Name = "ImportButton";
            ImportButton.Size = new Size(104, 60);
            ImportButton.TabIndex = 0;
            ImportButton.Text = "IMPORT";
            ImportButton.UseVisualStyleBackColor = true;
            ImportButton.Click += ImportButton_Click;
            // 
            // generateDataButton
            // 
            generateDataButton.Dock = DockStyle.Fill;
            generateDataButton.Location = new Point(3, 291);
            generateDataButton.Name = "generateDataButton";
            generateDataButton.Size = new Size(221, 66);
            generateDataButton.TabIndex = 12;
            generateDataButton.Text = "Generate New Data";
            generateDataButton.UseVisualStyleBackColor = true;
            generateDataButton.Click += generateDataButton_Click;
            // 
            // MainPanel
            // 
            MainPanel.Dock = DockStyle.Left;
            MainPanel.Location = new Point(0, 0);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(1118, 821);
            MainPanel.TabIndex = 41;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            ClientSize = new Size(1348, 821);
            Controls.Add(panel1);
            Controls.Add(MainPanel);
            Name = "MainForm";
            Text = "Find properties";
            panel1.ResumeLayout(false);
            ActionsTableLayoutPanel.ResumeLayout(false);
            FileTableLayoutPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private TableLayoutPanel ActionsTableLayoutPanel;
        private Button FindRealtyNavButton;
        private Button FindParcelsNavButton;
        private Button FindPropertiesNavButton;
        private TableLayoutPanel FileTableLayoutPanel;
        private Button ExportButton;
        private Button ImportButton;
        private Button generateDataButton;
        private Panel MainPanel;
    }
}
