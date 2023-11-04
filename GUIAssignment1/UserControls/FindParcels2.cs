using QuadTreeDS.SpatialItems;
using SemesterAssignment1.RealtyObjects;
using static GUIAssignment1.RealtyEditForm;

namespace GUIAssignment1.UserControls
{
    public partial class FindParcels : UserControl
    {
        public FindParcels()
        {
            InitializeComponent();
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

        // Resets the numeric up/downs and textboxes to their default values.
        private void ResetInputFields()
        {
            gps1LatNumericUpDown.Value = gps1LatNumericUpDown.Minimum;
            gps1LongNumericUpDown.Value = gps1LongNumericUpDown.Minimum;
            gps2LatNumericUpDown.Value = gps2LatNumericUpDown.Minimum;
            gps2LongNumericUpDown.Value = gps2LongNumericUpDown.Minimum;
            parcelNumberNumericUpDown.Value = parcelNumberNumericUpDown.Minimum;
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

        private void insertPropertyButton_Click(object sender, EventArgs e)
        {
            try
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

                int parcelNumber = (int)parcelNumberNumericUpDown.Value;
                string description = descriptionTextBox.Text;

                Parcel parcel = new Parcel(parcelNumber, description, area);
                Program.ApplicationLogic.AddParcel(parcel);

                ResetInputFields();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void parcelGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private GPSPoint ParseGPSPointFromCell(DataGridView gridView, int columnIndex, int rowIndex)
        {
            var pointString = GetCellValue(gridView, columnIndex, rowIndex);
            if (TryParseCoordinates(pointString, out LatitudeDirection latitudeDirection, out double latitude, out LongitudeDirection longitudeDirection, out double longitude))
            {
                return new GPSPoint(latitudeDirection, latitude, longitudeDirection, longitude);
            }
            throw new FormatException("Invalid GPS data");
        }

        private void EditForm_RealtyObjectUpdated(object sender, RealtyObjectEventArgs e)
        {
            if (UpdatePropertyIfValid(e.OldRealtyObject, e.UpdatedRealtyObject)) return;
            UpdateParcelIfValid(e.OldRealtyObject, e.UpdatedRealtyObject);
        }

        private bool UpdatePropertyIfValid(RealtyObject oldRealtyObject, RealtyObject updatedRealtyObject)
        {
            if (!(oldRealtyObject is Property oldProperty) || !(updatedRealtyObject is Property updatedProperty))
                return false;

            if (!ValidateConscriptionNumberChange(oldProperty, updatedProperty)) return true;

            if (HasBoundaryChanged(oldProperty, updatedProperty) || oldProperty.Description != updatedProperty.Description)
            {
                UpdatePropertyData(oldProperty, updatedProperty);
            }

            return true;
        }

        private void UpdateParcelIfValid(RealtyObject oldRealtyObject, RealtyObject updatedRealtyObject)
        {
            if (!(oldRealtyObject is Parcel oldParcel) || !(updatedRealtyObject is Parcel updatedParcel))
                return;

            if (!ValidateParcelNumberChange(oldParcel, updatedParcel)) return;

            if (HasBoundaryChanged(oldParcel, updatedParcel) || oldParcel.Description != updatedParcel.Description)
            {
                UpdateParcelData(oldParcel, updatedParcel);
            }
        }

        private bool ValidateConscriptionNumberChange(Property oldProperty, Property updatedProperty)
        {
            if (oldProperty.ConscriptionNumber == updatedProperty.ConscriptionNumber)
                return true;

            if (Program.ApplicationLogic.SearchKey(oldProperty, updatedProperty.ConscriptionNumber))
            {
                MessageBox.Show("Consription number cannot be changed");
                return false;
            }

            UpdatePropertyData(oldProperty, updatedProperty);
            return true;
        }

        private bool ValidateParcelNumberChange(Parcel oldParcel, Parcel updatedParcel)
        {
            if (oldParcel.ParcelNumber == updatedParcel.ParcelNumber)
                return true;

            if (Program.ApplicationLogic.SearchKey(oldParcel, updatedParcel.ParcelNumber * -1))
            {
                MessageBox.Show("Parcel number cannot be changed");
                return false;
            }

            UpdateParcelData(oldParcel, updatedParcel);
            return true;
        }

        private bool HasBoundaryChanged(RealtyObject oldRealtyObject, RealtyObject updatedRealtyObject)
        {
            return oldRealtyObject.LowerLeft != updatedRealtyObject.LowerLeft ||
                   oldRealtyObject.UpperRight != updatedRealtyObject.UpperRight;
        }

        private void UpdatePropertyData(Property oldProperty, Property updatedProperty)
        {
            Program.ApplicationLogic.DeleteProperty(oldProperty);
            Program.ApplicationLogic.AddProperty(updatedProperty);
            RefreshPropertyDisplay();
        }

        private void UpdateParcelData(Parcel oldParcel, Parcel updatedParcel)
        {
            Program.ApplicationLogic.DeleteParcel(oldParcel);
            Program.ApplicationLogic.AddParcel(updatedParcel);
            RefreshPropertyDisplay();
        }

        private void DisplayFoundParcels(IEnumerable<Parcel> parcels)
        {
            foreach (var parcel in parcels)
            {
                string listOfParcelsString = string.Join(", ", parcel.OccupiedByProperties.Select(property => property.ConscriptionNumber));
                string bottomLeftString = parcel.LowerLeft.ToString();
                string topRightString = parcel.UpperRight.ToString();

                parcelGridView.Rows.Add(new object[]
                {
                    parcel.ParcelNumber,
                    parcel.Description,
                    listOfParcelsString,
                    bottomLeftString,
                    topRightString
                });

                int newRowIdx = parcelGridView.Rows.Count - 1;
                parcelGridView.Rows[newRowIdx].HeaderCell.Value = (newRowIdx + 1).ToString();
            }
        }

        private void RefreshPropertyDisplay()
        {
            ClearParcelGridView();
            GPSPoint searchPoint = CreateSearchPointFromInputs();
            var foundParcels = Program.ApplicationLogic.FindParcels(searchPoint);
            DisplayFoundParcels(foundParcels);
        }

        // Class level constants
        const int IconsSize = 2;

        private string GetCellValue(DataGridView gridView, int columnIndex, int rowIndex)
        {
            return gridView[columnIndex, rowIndex].Value?.ToString() ?? "N/A";
        }

        public static bool TryParseCoordinates(string coordinates, out LatitudeDirection latitudeDirection, out double latitude, out LongitudeDirection longitudeDirection, out double longitude)
        {
            // Initialize out parameters
            latitudeDirection = default;
            latitude = 0;
            longitudeDirection = default;
            longitude = 0;


            try
            {
                string[] parts = coordinates.Split(',');
                // Split the first part by spaces to get latitude info
                string[] latParts = parts[0].Trim().Split(' ');
                Enum.TryParse(latParts[0], out latitudeDirection); // Get the direction (N/S)

                // Split the second part by spaces to get longitude info
                string[] lonParts = parts[1].Trim().Split(' ');
                Enum.TryParse(lonParts[0], out longitudeDirection); // Get the direction (E/W)

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        private void searchPropertiesButton_Click(object sender, EventArgs e)
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

        private void parcelGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Check if the click is on a valid cell
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex == parcelGridView.Columns.Count - 1)
            {
                int parcelNumber = int.Parse(GetCellValue(parcelGridView, 0, e.RowIndex));

                DialogResult dialogResult = MessageBox.Show($"Are you sure you want to delete Parcel {parcelNumber}?", "Delete Parcel", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.No)
                {
                    return;
                }

                // Parse GPS points from cells
                GPSPoint leftGPSPoint = ParseGPSPointFromCell(parcelGridView, 3, e.RowIndex);
                GPSPoint rightGPSPoint = ParseGPSPointFromCell(parcelGridView, 4, e.RowIndex);

                GPSRectangle area = new GPSRectangle(leftGPSPoint, rightGPSPoint);

                string description = GetCellValue(parcelGridView, 1, e.RowIndex);
                Parcel parcel = new Parcel(parcelNumber, description, area);

                if (Program.ApplicationLogic.DeleteParcel(parcel))
                    parcelGridView.Rows.RemoveAt(e.RowIndex);
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex == parcelGridView.Columns.Count - 2)
            {
                int parcelNumber = int.Parse(GetCellValue(parcelGridView, 0, e.RowIndex));
                string description = GetCellValue(parcelGridView, 1, e.RowIndex);

                // Parse GPS points from cells
                GPSPoint leftGPSPoint = ParseGPSPointFromCell(parcelGridView, 3, e.RowIndex);
                GPSPoint rightGPSPoint = ParseGPSPointFromCell(parcelGridView, 4, e.RowIndex);
                GPSRectangle area = new GPSRectangle(leftGPSPoint, rightGPSPoint);

                Parcel parcel = new Parcel(parcelNumber, description, area);
                var result = Program.ApplicationLogic.FindObject(parcel);
                var foundParcel = result.foundObject as Parcel;

                if (foundParcel is null)
                {
                    MessageBox.Show($"Parcel {parcelNumber} does not exist", "Parcel not found");
                    return;
                }
                RealtyEditForm editForm = new RealtyEditForm(foundParcel);
                editForm.RealtyObjectUpdated += EditForm_RealtyObjectUpdated;
                editForm.Show();
            }
        }
    }
}
