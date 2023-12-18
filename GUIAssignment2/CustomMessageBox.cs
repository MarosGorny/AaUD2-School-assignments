using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUIAssignment2
{
    public partial class CustomMessageBox : Form
    {
        private TextBox textBox;

        public CustomMessageBox(string text)
        {
            // Set the size of the form
            this.Width = 600; // Set desired width
            this.Height = 300; // Set desired height

            // Create a TextBox to show the message
            textBox = new TextBox();
            textBox.Multiline = true;
            textBox.ReadOnly = true;
            textBox.ScrollBars = ScrollBars.Vertical;
            textBox.Text = text;
            textBox.Dock = DockStyle.Fill;

            this.Controls.Add(textBox);
        }

        public static void Show(string text)
        {
            CustomMessageBox messageBox = new CustomMessageBox(text);
            messageBox.ShowDialog();
        }
    }
}
