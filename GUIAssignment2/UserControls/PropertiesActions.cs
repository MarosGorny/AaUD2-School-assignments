using QuadTreeDS.SpatialItems;
using SemesterAssignment2.RealtyObjects;
using System.Data;
using static GUIAssignment2.RealtyEditForm;

namespace GUIAssignment2.UserControls;

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

    private void DisplayFoundProperties(IEnumerable<PropertyQuadObject> properties)
    {
        //NEW CODE | After PROF. Jankovic wanted the change
        foreach (var property in properties)
        {
            string finalString = "";
            //foreach (var parcel in property.PositionedOnParcels)
            //{
            //    string numberParcel = parcel.ParcelNumber.ToString() + " ";
            //    string descriptionParcel = parcel.Description + " ";
            //    string bottomLeftParcel = parcel.LowerLeft.ToString() + " ";
            //    string topRightParcel = parcel.UpperRight.ToString() + " ";
            //    finalString += numberParcel + descriptionParcel + bottomLeftParcel + topRightParcel + "\n";
            //}

            //string listOfParcelsString = string.Join(", ", property.PositionedOnParcels.Select(parcel => parcel.ParcelNumber));
            string bottomLeftString = property.LowerLeft.ToString();
            string topRightString = property.UpperRight.ToString();

            propertyGridView.Rows.Add(new object[]
            {
                -66, //property.ConscriptionNumber,
                property.Description,
                finalString, //instead of listOfParcelsString
                bottomLeftString,
                topRightString
            });

            int newRowIdx = propertyGridView.Rows.Count - 1;
            propertyGridView.Rows[newRowIdx].HeaderCell.Value = (newRowIdx + 1).ToString();
        }


        ///OLD CODE | BEFORE PROF. Jankovic wanted the change
        //foreach (var property in properties)
        //{
        //    string listOfParcelsString = string.Join(", ", property.PositionedOnParcels.Select(parcel => parcel.ParcelNumber));
        //    string bottomLeftString = property.LowerLeft.ToString();
        //    string topRightString = property.UpperRight.ToString();

        //    propertyGridView.Rows.Add(new object[]
        //    {
        //        property.ConscriptionNumber,
        //        property.Description,
        //        listOfParcelsString,
        //        bottomLeftString,
        //        topRightString
        //    });

        //    int newRowIdx = propertyGridView.Rows.Count - 1;
        //    propertyGridView.Rows[newRowIdx].HeaderCell.Value = (newRowIdx + 1).ToString();
        //}
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

            int propertyNumber = (int)propertyNumberNumericUpDown.Value;
            int conscriptionNumber = (int)conscriptionNumberNumericUpDown.Value;
            string description = descriptionTextBox.Text;

            Property property = new Property(propertyNumber, conscriptionNumber, description, area);
            Program.ApplicationLogic.AddObject(property);

            ResetInputFields();
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message);
        }

    }

    // Resets the numeric up/downs and textboxes to their default values.
    private void ResetInputFields()
    {
        gps1LatNumericUpDown.Value = gps1LatNumericUpDown.Minimum;
        gps1LongNumericUpDown.Value = gps1LongNumericUpDown.Minimum;
        gps2LatNumericUpDown.Value = gps2LatNumericUpDown.Minimum;
        gps2LongNumericUpDown.Value = gps2LongNumericUpDown.Minimum;
        conscriptionNumberNumericUpDown.Value = conscriptionNumberNumericUpDown.Minimum;
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
            MessageBox.Show($"Cell at row {e.RowIndex + 1}, column {e.ColumnIndex + 1} \nValue\n: {cellValue}", "Cell Double-Clicked");
        }
    }

    private void propertyGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        // Check if the click is on a valid cell
        if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex == propertyGridView.Columns.Count - 1)
        //DELETE PROPERTY
        {
            int propertyNumber = int.Parse(GetCellValue(propertyGridView, 0, e.RowIndex));
            //int conscriptionNumber = int.Parse(GetCellValue(propertyGridView, 0, e.RowIndex));

            DialogResult dialogResult = MessageBox.Show($"Are you sure you want to delete Property {propertyNumber}?", "Delete Property", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.No)
            {
                return;
            }

            // Parse GPS points from cells
            GPSPoint leftGPSPoint = ParseGPSPointFromCell(propertyGridView, 1, e.RowIndex);
            GPSPoint rightGPSPoint = ParseGPSPointFromCell(propertyGridView, 2, e.RowIndex);

            GPSRectangle area = new GPSRectangle(leftGPSPoint, rightGPSPoint);

            //string description = GetCellValue(propertyGridView, 1, e.RowIndex);
            //Property property = new Property(propertyNumber,-1, "", area);

            if (Program.ApplicationLogic.DeleteProperty(propertyNumber))
                propertyGridView.Rows.RemoveAt(e.RowIndex);
        }
        else if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex == propertyGridView.Columns.Count - 2)
        //EDIT PROPERTY
        {

            int propertyNumber = int.Parse(GetCellValue(propertyGridView, 0, e.RowIndex));
            //string description = GetCellValue(propertyGridView, 1, e.RowIndex);

            // Parse GPS points from cells
            GPSPoint leftGPSPoint = ParseGPSPointFromCell(propertyGridView, 1, e.RowIndex);
            GPSPoint rightGPSPoint = ParseGPSPointFromCell(propertyGridView, 2, e.RowIndex);
            GPSRectangle area = new GPSRectangle(leftGPSPoint, rightGPSPoint);

            //Property property = new Property(propertyNumber, -1, "", area);
            var result = Program.ApplicationLogic.TryFindProperty(propertyNumber);
            var foundProperty = result as Property;

            if (foundProperty is null)
            {
                MessageBox.Show($"Property {propertyNumber} does not exist", "Property not found");
                return;
            }
            RealtyEditForm editForm = new RealtyEditForm(foundProperty);
            editForm.RealtyObjectUpdated += EditForm_RealtyObjectUpdated;
            editForm.Show();
        }
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

        //if (Program.ApplicationLogic.SearchKey(oldProperty, updatedProperty.ConscriptionNumber))
        //{
        //    MessageBox.Show("Consription number cannot be changed");
        //    return false;
        //}

        UpdatePropertyData(oldProperty, updatedProperty);
        return true;
    }

    private bool ValidateParcelNumberChange(Parcel oldParcel, Parcel updatedParcel)
    {
        if (oldParcel.ParcelNumber == updatedParcel.ParcelNumber)
            return true;

        MessageBox.Show("Parcel number cannot be changed");
        return false;
        //if (Program.ApplicationLogic.SearchKey(oldParcel, updatedParcel.ParcelNumber))
        //{
        //    MessageBox.Show("Parcel number cannot be changed");
        //    return false;
        //}

        //UpdateParcelData(oldParcel, updatedParcel);
        //return true;
    }

    private bool HasBoundaryChanged(RealtyObject oldRealtyObject, RealtyObject updatedRealtyObject)
    {
        return oldRealtyObject.LowerLeft != updatedRealtyObject.LowerLeft ||
               oldRealtyObject.UpperRight != updatedRealtyObject.UpperRight;
    }

    private void UpdatePropertyData(Property oldProperty, Property updatedProperty)
    {
        Program.ApplicationLogic.EditProperty(updatedProperty);
        //Program.ApplicationLogic.DeleteProperty(oldProperty);
        //Program.ApplicationLogic.AddProperty(updatedProperty);
        RefreshPropertyDisplay();
    }

    private void UpdateParcelData(Parcel oldParcel, Parcel updatedParcel)
    {
        Program.ApplicationLogic.EditParcel(updatedParcel);
        //Program.ApplicationLogic.DeleteParcel(oldParcel);
        //Program.ApplicationLogic.AddParcel(updatedParcel);
        RefreshPropertyDisplay();
    }

    private void RefreshPropertyDisplay()
    {
        ClearPropertyGridView();
        GPSPoint searchPoint = CreateSearchPointFromInputs();
        var foundProperties = Program.ApplicationLogic.FindProperties(searchPoint);
        DisplayFoundProperties(foundProperties);
    }

    private void label2_Click(object sender, EventArgs e)
    {

    }
}
