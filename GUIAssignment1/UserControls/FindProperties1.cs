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
            var foundProperties = Program.ApplicationLogic.FindProperties(gpsPoint);
            foreach (var foundProperty in foundProperties)
            {
                var conscriptionNumber = foundProperty.ConscriptionNumber;
                var description = foundProperty.Description;
                var listOfParcels = foundProperty.PositionedOnParcels;
                var listOfParcelsString = "";
                for (int i = 0; i < listOfParcels.Count; i++)
                {
                    listOfParcelsString += listOfParcels[i].ParcelNumber;
                    if (i != listOfParcels.Count - 1)
                    {
                        listOfParcelsString += ", ";
                    }
                }
                var bottomLeftString = ((GPSPoint)(foundProperty.LowerLeft)).ToString();
                var topRightString = ((GPSPoint)(foundProperty.UpperRight)).ToString();

                dataGridView1.Rows.Add(conscriptionNumber, description, listOfParcelsString, bottomLeftString, topRightString);
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].HeaderCell.Value = (dataGridView1.Rows.Count).ToString();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Check if the click is on a valid cell
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Optionally, get the value of the cell that was double-clicked
                var cellValue = dataGridView1[e.ColumnIndex, e.RowIndex].Value?.ToString() ?? "N/A";

                // Show the message box
                MessageBox.Show($"Cell at row {e.RowIndex + 1}, column {e.ColumnIndex + 1} \nValue: {cellValue}", "Cell Double-Clicked");
            }
            //MessageBox.Show(dataGridView1.SelectedCells[0].Value.ToString());
        }
    }
}
