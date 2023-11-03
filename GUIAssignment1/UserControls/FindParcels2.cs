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
    public partial class FindParcels2 : UserControl
    {
        public FindParcels2()
        {
            InitializeComponent();
        }

        private void SearchPropertiesButton_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

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
            var foundParcels = Program.ApplicationLogic.FindParcels(gpsPoint);
            foreach (var foundParcel in foundParcels)
            {
                dataGridView1.Rows.Add(foundParcel.ParcelNumber, foundParcel.Description);
            }
        }
    }
}
