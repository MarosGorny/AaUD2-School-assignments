﻿using QuadTreeDS.SpatialItems;
using SemesterAssignment1.RealtyObjects;
using static GUIAssignment1.RealtyEditForm;

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

            int conscriptionNumber = (int)conscriptionNumberNumericUpDown.Value;
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
            conscriptionNumberNumericUpDown.Value = 0;
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

        // Class level constants
        const int IconsSize = 2;

        private string GetCellValue(DataGridView gridView, int columnIndex, int rowIndex)
        {
            return gridView[columnIndex, rowIndex].Value?.ToString() ?? "N/A";
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

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Check if the click is on a valid cell
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex < propertyGridView.Columns.Count - IconsSize)
            {
                var cellValue = GetCellValue(propertyGridView, e.ColumnIndex, e.RowIndex);
                // Show the message box
                MessageBox.Show($"Cell at row {e.RowIndex + 1}, column {e.ColumnIndex + 1} \nValue: {cellValue}", "Cell Double-Clicked");
            }
        }

        private void propertyGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Check if the click is on a valid cell
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex == propertyGridView.Columns.Count - 1)
            {
                int conscriptionNumber = int.Parse(GetCellValue(propertyGridView, 0, e.RowIndex));

                DialogResult dialogResult = MessageBox.Show($"Are you sure you want to delete Property {conscriptionNumber}?", "Delete Property", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.No)
                {
                    return;
                }

                // Parse GPS points from cells
                GPSPoint leftGPSPoint = ParseGPSPointFromCell(propertyGridView, 3, e.RowIndex);
                GPSPoint rightGPSPoint = ParseGPSPointFromCell(propertyGridView, 4, e.RowIndex);

                GPSRectangle area = new GPSRectangle(leftGPSPoint, rightGPSPoint);

                string description = GetCellValue(propertyGridView, 1, e.RowIndex);
                Property property = new Property(conscriptionNumber, description, area);

                if (Program.ApplicationLogic.DeleteProperty(property))
                    propertyGridView.Rows.RemoveAt(e.RowIndex);
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex == propertyGridView.Columns.Count - 2)
            {
                int conscriptionNumber = int.Parse(GetCellValue(propertyGridView, 0, e.RowIndex));
                string description = GetCellValue(propertyGridView, 1, e.RowIndex);

                // Parse GPS points from cells
                GPSPoint leftGPSPoint = ParseGPSPointFromCell(propertyGridView, 3, e.RowIndex);
                GPSPoint rightGPSPoint = ParseGPSPointFromCell(propertyGridView, 4, e.RowIndex);
                GPSRectangle area = new GPSRectangle(leftGPSPoint, rightGPSPoint);

                Property property = new Property(conscriptionNumber, description, area);
                var result = Program.ApplicationLogic.FindObject(property);
                var foundProperty = result.foundObject as Property;

                if (foundProperty is null)
                {
                    MessageBox.Show($"Property {conscriptionNumber} does not exist", "Property not found");
                    return;
                }
                RealtyEditForm editForm = new RealtyEditForm(foundProperty);
                editForm.RealtyObjectUpdated += EditForm_RealtyObjectUpdated;
                editForm.Show();
            }
        }

        private void EditForm_RealtyObjectUpdated(object sender, RealtyObjectEventArgs e)
        {
            // Handle the updated RealtyObject, e.g., update the grid view or internal list
            var oldRealtyObject = e.OldRealtyObject;
            var updatedRealtyObject = e.UpdatedRealtyObject;

            if(oldRealtyObject is Property oldProperty && updatedRealtyObject is Property updatedProperty)
            {
                //check if key is the same
                //check if gps points are the same
                //check if the description is the same
            }
            else if(oldRealtyObject is Parcel oldParcel && updatedRealtyObject is Parcel updatedParcel)
            {

            }
        }

    }
}
