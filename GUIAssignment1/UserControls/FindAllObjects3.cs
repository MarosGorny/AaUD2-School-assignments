using QuadTreeDS.SpatialItems;
using SemesterAssignment1.RealtyObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rectangle = QuadTreeDS.SpatialItems.Rectangle;

namespace GUIAssignment1.UserControls
{
    public partial class FindAllObjects3 : UserControl
    {
        public FindAllObjects3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LatitudeDirection latDirLeft = default;
            LongitudeDirection longDirLeft = default;
            LatitudeDirection latDirRight = default;
            LongitudeDirection longDirRight = default;
            double latLeft = Convert.ToDouble(numericUpDown1.Value);
            double lonLeft = Convert.ToDouble(numericUpDown2.Value);
            double latRight = Convert.ToDouble(numericUpDown4.Value);
            double lonRight = Convert.ToDouble(numericUpDown3.Value);

            if (radioButton5.Checked)
            {
                latDirLeft = LatitudeDirection.N;
            }
            else if (radioButton2.Checked)
            {
                latDirLeft = LatitudeDirection.S;
            }

            if (radioButton1.Checked)
            {
                longDirLeft = LongitudeDirection.E;
            }
            else if (radioButton3.Checked)
            {
                longDirLeft = LongitudeDirection.W;
            }

            if (radioButton7.Checked)
            {
                latDirRight = LatitudeDirection.N;
            }
            else if (radioButton8.Checked)
            {
                latDirRight = LatitudeDirection.S;
            }

            if(radioButton4.Checked)
            {
                longDirRight = LongitudeDirection.E;
            }
            else if(radioButton6.Checked)
            {
                longDirRight = LongitudeDirection.W;
            }

            GPSPoint gpsPoint = new GPSPoint(latDirLeft, latLeft, longDirLeft, lonLeft);
            GPSPoint gpsPoint2 = new GPSPoint(latDirRight, latRight, longDirRight, lonRight);
            Rectangle area = new Rectangle(gpsPoint, gpsPoint2);
            var foundOjects = Program.ApplicationLogic.FindObjectsInArea(area);
            foreach (var foundObject in foundOjects)
            {
                if(foundObject is Property foundProperty)
                {
                    dataGridView1.Rows.Add(foundProperty.ConscriptionNumber, foundProperty.Description);
                }
                else if (foundObject is Parcel foundParcel)
                {
                    dataGridView1.Rows.Add(foundParcel.ParcelNumber, foundParcel.Description);
                }
            }
        }
    }
}
