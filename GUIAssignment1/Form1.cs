using QuadTreeDS.SpatialItems;
using SemesterAssignment1;

namespace GUIAssignment1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}