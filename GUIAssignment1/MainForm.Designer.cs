namespace GUIAssignment1
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
            DeleteParcelNavButton = new Button();
            DeletePropertyNavButton = new Button();
            EditParcelNavButton = new Button();
            EditPropertyNavButton = new Button();
            AddParcelNavButton = new Button();
            AddPropertyNavButton = new Button();
            FindRealtyNavButton = new Button();
            FindParcelsNavButton = new Button();
            FindPropertiesNavButton = new Button();
            FileTableLayoutPanel = new TableLayoutPanel();
            ExportButton = new Button();
            ImportButton = new Button();
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
            panel1.TabIndex = 38;
            // 
            // ActionsTableLayoutPanel
            // 
            ActionsTableLayoutPanel.ColumnCount = 1;
            ActionsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            ActionsTableLayoutPanel.Controls.Add(DeleteParcelNavButton, 0, 8);
            ActionsTableLayoutPanel.Controls.Add(DeletePropertyNavButton, 0, 7);
            ActionsTableLayoutPanel.Controls.Add(EditParcelNavButton, 0, 6);
            ActionsTableLayoutPanel.Controls.Add(EditPropertyNavButton, 0, 5);
            ActionsTableLayoutPanel.Controls.Add(AddParcelNavButton, 0, 4);
            ActionsTableLayoutPanel.Controls.Add(AddPropertyNavButton, 0, 3);
            ActionsTableLayoutPanel.Controls.Add(FindRealtyNavButton, 0, 2);
            ActionsTableLayoutPanel.Controls.Add(FindParcelsNavButton, 0, 1);
            ActionsTableLayoutPanel.Controls.Add(FindPropertiesNavButton, 0, 0);
            ActionsTableLayoutPanel.Controls.Add(FileTableLayoutPanel, 0, 9);
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
            // DeleteParcelNavButton
            // 
            DeleteParcelNavButton.Dock = DockStyle.Fill;
            DeleteParcelNavButton.Location = new Point(3, 579);
            DeleteParcelNavButton.Name = "DeleteParcelNavButton";
            DeleteParcelNavButton.Size = new Size(221, 66);
            DeleteParcelNavButton.TabIndex = 8;
            DeleteParcelNavButton.Text = "9";
            DeleteParcelNavButton.UseVisualStyleBackColor = true;
            DeleteParcelNavButton.Click += button10_Click;
            // 
            // DeletePropertyNavButton
            // 
            DeletePropertyNavButton.Dock = DockStyle.Fill;
            DeletePropertyNavButton.Location = new Point(3, 507);
            DeletePropertyNavButton.Name = "DeletePropertyNavButton";
            DeletePropertyNavButton.Size = new Size(221, 66);
            DeletePropertyNavButton.TabIndex = 7;
            DeletePropertyNavButton.Text = "8";
            DeletePropertyNavButton.UseVisualStyleBackColor = true;
            DeletePropertyNavButton.Click += button9_Click;
            // 
            // EditParcelNavButton
            // 
            EditParcelNavButton.Dock = DockStyle.Fill;
            EditParcelNavButton.Location = new Point(3, 435);
            EditParcelNavButton.Name = "EditParcelNavButton";
            EditParcelNavButton.Size = new Size(221, 66);
            EditParcelNavButton.TabIndex = 6;
            EditParcelNavButton.Text = "7";
            EditParcelNavButton.UseVisualStyleBackColor = true;
            EditParcelNavButton.Click += button8_Click;
            // 
            // EditPropertyNavButton
            // 
            EditPropertyNavButton.Dock = DockStyle.Fill;
            EditPropertyNavButton.Location = new Point(3, 363);
            EditPropertyNavButton.Name = "EditPropertyNavButton";
            EditPropertyNavButton.Size = new Size(221, 66);
            EditPropertyNavButton.TabIndex = 5;
            EditPropertyNavButton.Text = "6";
            EditPropertyNavButton.UseVisualStyleBackColor = true;
            EditPropertyNavButton.Click += button7_Click;
            // 
            // AddParcelNavButton
            // 
            AddParcelNavButton.Dock = DockStyle.Fill;
            AddParcelNavButton.Location = new Point(3, 291);
            AddParcelNavButton.Name = "AddParcelNavButton";
            AddParcelNavButton.Size = new Size(221, 66);
            AddParcelNavButton.TabIndex = 4;
            AddParcelNavButton.Text = "5";
            AddParcelNavButton.UseVisualStyleBackColor = true;
            AddParcelNavButton.Click += button6_Click;
            // 
            // AddPropertyNavButton
            // 
            AddPropertyNavButton.Dock = DockStyle.Fill;
            AddPropertyNavButton.Location = new Point(3, 219);
            AddPropertyNavButton.Name = "AddPropertyNavButton";
            AddPropertyNavButton.Size = new Size(221, 66);
            AddPropertyNavButton.TabIndex = 3;
            AddPropertyNavButton.Text = "4";
            AddPropertyNavButton.UseVisualStyleBackColor = true;
            AddPropertyNavButton.Click += button5_Click;
            // 
            // FindRealtyNavButton
            // 
            FindRealtyNavButton.Dock = DockStyle.Fill;
            FindRealtyNavButton.Location = new Point(3, 147);
            FindRealtyNavButton.Name = "FindRealtyNavButton";
            FindRealtyNavButton.Size = new Size(221, 66);
            FindRealtyNavButton.TabIndex = 2;
            FindRealtyNavButton.Text = "3";
            FindRealtyNavButton.UseVisualStyleBackColor = true;
            FindRealtyNavButton.Click += button4_Click;
            // 
            // FindParcelsNavButton
            // 
            FindParcelsNavButton.Dock = DockStyle.Fill;
            FindParcelsNavButton.Location = new Point(3, 75);
            FindParcelsNavButton.Name = "FindParcelsNavButton";
            FindParcelsNavButton.Size = new Size(221, 66);
            FindParcelsNavButton.TabIndex = 1;
            FindParcelsNavButton.Text = "2";
            FindParcelsNavButton.UseVisualStyleBackColor = true;
            FindParcelsNavButton.Click += button3_Click;
            // 
            // FindPropertiesNavButton
            // 
            FindPropertiesNavButton.Dock = DockStyle.Fill;
            FindPropertiesNavButton.Location = new Point(3, 3);
            FindPropertiesNavButton.Name = "FindPropertiesNavButton";
            FindPropertiesNavButton.Size = new Size(221, 66);
            FindPropertiesNavButton.TabIndex = 0;
            FindPropertiesNavButton.Text = "1";
            FindPropertiesNavButton.UseVisualStyleBackColor = true;
            FindPropertiesNavButton.Click += button2_Click;
            // 
            // FileTableLayoutPanel
            // 
            FileTableLayoutPanel.ColumnCount = 2;
            FileTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            FileTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            FileTableLayoutPanel.Controls.Add(ExportButton, 1, 0);
            FileTableLayoutPanel.Controls.Add(ImportButton, 0, 0);
            FileTableLayoutPanel.Location = new Point(3, 651);
            FileTableLayoutPanel.Name = "FileTableLayoutPanel";
            FileTableLayoutPanel.RowCount = 1;
            FileTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            FileTableLayoutPanel.Size = new Size(221, 67);
            FileTableLayoutPanel.TabIndex = 9;
            // 
            // ExportButton
            // 
            ExportButton.Dock = DockStyle.Fill;
            ExportButton.Location = new Point(113, 3);
            ExportButton.Name = "ExportButton";
            ExportButton.Size = new Size(105, 61);
            ExportButton.TabIndex = 1;
            ExportButton.Text = "EXPORT";
            ExportButton.UseVisualStyleBackColor = true;
            // 
            // ImportButton
            // 
            ImportButton.Dock = DockStyle.Fill;
            ImportButton.Location = new Point(3, 3);
            ImportButton.Name = "ImportButton";
            ImportButton.Size = new Size(104, 61);
            ImportButton.TabIndex = 0;
            ImportButton.Text = "IMPORT";
            ImportButton.UseVisualStyleBackColor = true;
            // 
            // MainPanel
            // 
            MainPanel.Dock = DockStyle.Left;
            MainPanel.Location = new Point(0, 0);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(1118, 821);
            MainPanel.TabIndex = 39;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
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
        private Panel MainPanel;
        private Button DeleteParcelNavButton;
        private Button DeletePropertyNavButton;
        private Button EditParcelNavButton;
        private Button EditPropertyNavButton;
        private Button AddParcelNavButton;
        private Button AddPropertyNavButton;
        private Button FindRealtyNavButton;
        private Button FindParcelsNavButton;
        private Button FindPropertiesNavButton;
        private TableLayoutPanel FileTableLayoutPanel;
        private Button ExportButton;
        private Button ImportButton;
    }
}