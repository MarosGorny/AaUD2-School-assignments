using QuadTreeDS.SpatialItems;
using SemesterAssignment1.RealtyObjects;

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
            ClearPropertyGridView();

            GPSPoint searchPoint = CreateSearchPointFromInputs();
            var foundProperties = Program.ApplicationLogic.FindProperties(searchPoint);

            DisplayFoundProperties(foundProperties);
        }

        private void ClearPropertyGridView()
        {
            propertyGridView.Rows.Clear();
            propertyGridView.Refresh();
        }

        private GPSPoint CreateSearchPointFromInputs()
        {
            LatitudeDirection latDirection = GetSelectedLatitudeDirection(latNRadioButton, latSRadioButton);
            LongitudeDirection longDirection = GetSelectedLongitudeDirection(longERadioButton, longWRadioButton);

            double latitude = Convert.ToDouble(latNumericUpDown.Value);
            double longitude = Convert.ToDouble(longNumericUpDown.Value);

            return new GPSPoint(latDirection, latitude, longDirection, longitude);
        }

        private LatitudeDirection GetSelectedLatitudeDirection(RadioButton northButton, RadioButton southButton)
        {
            return northButton.Checked ? LatitudeDirection.N : LatitudeDirection.S;
        }

        private LongitudeDirection GetSelectedLongitudeDirection(RadioButton eastButton, RadioButton westButton)
        {
            return eastButton.Checked ? LongitudeDirection.E : LongitudeDirection.W;
        }

        private void DisplayFoundProperties(IEnumerable<Property> properties)
        {
            foreach (var property in properties)
            {
                string listOfParcelsString = string.Join(", ", property.PositionedOnParcels.Select(parcel => parcel.ParcelNumber));
                string bottomLeftString = property.LowerLeft.ToString();
                string topRightString = property.UpperRight.ToString();

                propertyGridView.Rows.Add(new object[]
                {
                    property.ConscriptionNumber,
                    property.Description,
                    listOfParcelsString,
                    bottomLeftString,
                    topRightString
                });

                int newRowIdx = propertyGridView.Rows.Count - 1;
                propertyGridView.Rows[newRowIdx].HeaderCell.Value = (newRowIdx + 1).ToString();
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void insertPropertyButton_Click(object sender, EventArgs e)
        {
            GPSPoint leftPoint = CreateGPSPointFromInputs(
                gps1LatNumericUpDown, gps1LongNumericUpDown,
                gps1LatNRadioButton, gps1LongERadioButton
            );

            GPSPoint rightPoint = CreateGPSPointFromInputs(
                gps2LatNumericUpDown, gps2LongNumericUpDown,
                gps2LatNRadioButton, gps2LongERadioButton
            );

            GPSRectangle area = new GPSRectangle(leftPoint, rightPoint);

            int conscriptionNumber = int.Parse(conscriptionNumberTextBox.Text);
            string description = descriptionTextBox.Text;

            Property property = new Property(conscriptionNumber, description, area);
            Program.ApplicationLogic.AddProperty(property);

            ResetInputFields();
        }

        // Resets the numeric up/downs and textboxes to their default values.
        private void ResetInputFields()
        {
            gps1LatNumericUpDown.Value = 0;
            gps1LongNumericUpDown.Value = 0;
            gps2LatNumericUpDown.Value = 0;
            gps2LongNumericUpDown.Value = 0;
            conscriptionNumberTextBox.Text = "";
            descriptionTextBox.Text = "";
        }

        // Creates a GPSPoint based on user inputs from the form.
        private GPSPoint CreateGPSPointFromInputs(NumericUpDown numericUpDownLatitude,
                                                  NumericUpDown numericUpDownLongitude,
                                                  RadioButton radioButtonLatitudeNorth,
                                                  RadioButton radioButtonLongitudeEast)
        {
            // Determine the latitude and longitude directions based on the radio button selections.
            LatitudeDirection latitudeDirection = radioButtonLatitudeNorth.Checked ? LatitudeDirection.N : LatitudeDirection.S;
            LongitudeDirection longitudeDirection = radioButtonLongitudeEast.Checked ? LongitudeDirection.E : LongitudeDirection.W;

            // Convert the numeric up/down values to doubles for latitude and longitude.
            double latitude = Convert.ToDouble(numericUpDownLatitude.Value);
            double longitude = Convert.ToDouble(numericUpDownLongitude.Value);

            // Return a new GPSPoint with the specified values.
            return new GPSPoint(latitudeDirection, latitude, longitudeDirection, longitude);
        }
    }
}
