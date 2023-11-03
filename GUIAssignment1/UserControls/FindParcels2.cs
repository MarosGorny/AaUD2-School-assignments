using QuadTreeDS.SpatialItems;

namespace GUIAssignment1.UserControls
{
    public partial class FindParcels : UserControl
    {
        public FindParcels()
        {
            InitializeComponent();
        }

        private void SearchPropertiesButton_Click(object sender, EventArgs e)
        {

        }

        private void latGroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void latSRadioButton_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void propertyGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void longGroupBox_Enter(object sender, EventArgs e)
        {
        }

        private void longNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
        }

        private void longERadioButton_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void longWRadioButton_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void latNRadioButton_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void latNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
        }

        private void SearchPropertiesButton_Click_1(object sender, EventArgs e)
        {
            parcelGridView.Rows.Clear();
            parcelGridView.Refresh();

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
            var foundParcels = Program.ApplicationLogic.FindParcels(gpsPoint);
            foreach (var foundParcel in foundParcels)
            {
                var parcelNumber = foundParcel.ParcelNumber;
                var parcelDescription = foundParcel.Description;
                var listOfProperties = foundParcel.OccupiedByProperties;
                var listOfPropertiesString = "";
                for (int i = 0; i < listOfProperties.Count; i++)
                {
                    listOfPropertiesString += listOfProperties[i].ConscriptionNumber;
                    if (i != listOfProperties.Count - 1)
                    {
                        listOfPropertiesString += ", ";
                    }
                }
                var bottomLeftString = ((GPSPoint)(foundParcel.LowerLeft)).ToString();
                var topRightString = ((GPSPoint)(foundParcel.UpperRight)).ToString();

                parcelGridView.Rows.Add(parcelNumber, parcelDescription, listOfPropertiesString, bottomLeftString, topRightString);
                parcelGridView.Rows[parcelGridView.Rows.Count - 1].HeaderCell.Value = (parcelGridView.Rows.Count).ToString();
            }
        }

        private void parcelGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Check if the click is on a valid cell
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Optionally, get the value of the cell that was double-clicked
                var cellValue = parcelGridView[e.ColumnIndex, e.RowIndex].Value?.ToString() ?? "N/A";

                // Show the message box
                MessageBox.Show($"Cell at row {e.RowIndex + 1}, column {e.ColumnIndex + 1} \nValue: {cellValue}", "Cell Double-Clicked");
            }
        }
    }
}
