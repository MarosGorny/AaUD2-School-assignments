using QuadTreeDS.SpatialItems;
using SemesterAssignment1.RealtyObjects;
using Rectangle = QuadTreeDS.SpatialItems.Rectangle;

namespace GUIAssignment1.UserControls
{
    public partial class AddProperty4 : UserControl
    {
        public AddProperty4()
        {
            //InitializeComponent();
        }

        private void reset()
        {
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            numericUpDown3.Value = 0;
            numericUpDown4.Value = 0;
            textBox1.Text = "";
            textBox2.Text = "";
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

            int conscriptionNumber = int.Parse(textBox1.Text);
            string description = textBox2.Text;

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

            if (radioButton4.Checked)
            {
                longDirRight = LongitudeDirection.E;
            }
            else if (radioButton6.Checked)
            {
                longDirRight = LongitudeDirection.W;
            }

            GPSPoint gpsPoint = new GPSPoint(latDirLeft, latLeft, longDirLeft, lonLeft);
            GPSPoint gpsPoint2 = new GPSPoint(latDirRight, latRight, longDirRight, lonRight);
            GPSRectangle area = new GPSRectangle(gpsPoint, gpsPoint2);

            Property property = new Property(conscriptionNumber, description, area);
            Program.ApplicationLogic.AddProperty(property);
            reset();
        }
    }
}
