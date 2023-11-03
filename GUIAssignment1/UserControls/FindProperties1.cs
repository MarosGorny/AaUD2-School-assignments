using QuadTreeDS.SpatialItems;

namespace GUIAssignment1.UserControls
{
    public partial class FindProperties : UserControl
    {
        public FindProperties()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            propertyGridView.Rows.Clear();
            propertyGridView.Refresh();

            LatitudeDirection latDir = default;
            LongitudeDirection longDir = default;
            double lat = Convert.ToDouble(latNumericUpDown.Value);
            double lon = Convert.ToDouble(longNumericUpDown.Value);

            if (latNRadioButton.Checked)
            {
                latDir = LatitudeDirection.N;
            }
            else if (latSRadioButton.Checked)
            {
                latDir = LatitudeDirection.S;
            }

            if (longERadioButton.Checked)
            {
                longDir = LongitudeDirection.E;
            }
            else if (longWRadioButton.Checked)
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

                propertyGridView.Rows.Add(conscriptionNumber, description, listOfParcelsString, bottomLeftString, topRightString);
                propertyGridView.Rows[propertyGridView.Rows.Count - 1].HeaderCell.Value = (propertyGridView.Rows.Count).ToString();
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
                var cellValue = propertyGridView[e.ColumnIndex, e.RowIndex].Value?.ToString() ?? "N/A";

                // Show the message box
                MessageBox.Show($"Cell at row {e.RowIndex + 1}, column {e.ColumnIndex + 1} \nValue: {cellValue}", "Cell Double-Clicked");
            }
        }
    }
}
