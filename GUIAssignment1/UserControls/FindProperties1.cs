using QuadTreeDS.SpatialItems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUIAssignment1.UserControls
{
    public partial class FindProperties1 : UserControl
    {
        public FindProperties1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LatitudeDirection latDir = default;
            LongitudeDirection longDir = default;
            double lat = Convert.ToDouble(numericUpDown1.Value);
            double lon = Convert.ToDouble(numericUpDown2.Value);

            if (radioButton5.Checked)
            {
                latDir = LatitudeDirection.N;
            }
            else if (radioButton2.Checked)
            {
                latDir = LatitudeDirection.S;
            }

            if (radioButton1.Checked)
            {
                longDir = LongitudeDirection.E;
            }
            else if (radioButton3.Checked)
            {
                longDir = LongitudeDirection.W;
            }

            GPSPoint gpsPoint = new GPSPoint(latDir, lat, longDir, lon);
            var foundProperties = Program.ApplicationLogic.FindProperties(gpsPoint);
            foreach (var foundProperty in foundProperties)
            {
                //TODO: Need to remake return of findProperties to return a list of properties instead of a list of realty objects
                dataGridView1.Rows.Add(foundProperty.ConscriptionNumber, foundProperty.Description);
            }
        }
    }
}
