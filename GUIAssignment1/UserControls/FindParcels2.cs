using QuadTreeDS.SpatialItems;
using SemesterAssignment1.RealtyObjects;

namespace GUIAssignment1.UserControls
{
    public partial class FindParcels : UserControl
    {
        public FindParcels()
        {
            InitializeComponent();
        }


        private void propertyGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

        private void insertParcelButton_Click(object sender, EventArgs e)
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

            int parcelNumber = int.Parse(parcelNumberTextBox.Text);
            string description = descriptionTextBox.Text;

            Parcel parcel = new Parcel(parcelNumber, description, area);
            Program.ApplicationLogic.AddParcel(parcel);

            ResetInputFields();
        }

        private void searchParcelsButoon_Click(object sender, EventArgs e)
        {
            ClearParcelGridView();

            GPSPoint searchPoint = CreateSearchPointFromInputs();
            var foundParcels = Program.ApplicationLogic.FindParcels(searchPoint);

            DisplayFoundParcels(foundParcels);
        }

        private void ClearParcelGridView()
        {
            parcelGridView.Rows.Clear();
            parcelGridView.Refresh();
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

        private void DisplayFoundParcels(IEnumerable<Parcel> parcels)
        {
            foreach (var parcel in parcels)
            {
                string listOfPropertiesString = string.Join(", ", parcel.OccupiedByProperties.Select(property => property.ConscriptionNumber));
                string bottomLeftString = parcel.LowerLeft.ToString();
                string topRightString = parcel.UpperRight.ToString();

                parcelGridView.Rows.Add(new object[]
                {
                    parcel.ParcelNumber,
                    parcel.Description,
                    listOfPropertiesString,
                    bottomLeftString,
                    topRightString
                });

                int newRowIdx = parcelGridView.Rows.Count - 1;
                parcelGridView.Rows[newRowIdx].HeaderCell.Value = (newRowIdx + 1).ToString();
            }
        }

        // Resets the numeric up/downs and textboxes to their default values.
        private void ResetInputFields()
        {
            gps1LatNumericUpDown.Value = 0;
            gps1LongNumericUpDown.Value = 0;
            gps2LatNumericUpDown.Value = 0;
            gps2LongNumericUpDown.Value = 0;
            parcelNumberTextBox.Text = "";
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
