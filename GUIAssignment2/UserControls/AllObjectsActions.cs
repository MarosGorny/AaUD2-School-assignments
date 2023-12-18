using QuadTreeDS.SpatialItems;
using SemesterAssignment2.RealtyObjects;
using System.Data;
using System.Windows.Forms.VisualStyles;
using static GUIAssignment2.RealtyEditForm;
using Rectangle = QuadTreeDS.SpatialItems.Rectangle;

namespace GUIAssignment2.UserControls;

public partial class FindAllObjects : UserControl
{
    public FindAllObjects()
    {
        InitializeComponent();
    }


    private void label1_Click(object sender, EventArgs e)
    {

    }

    private void DisplayFoundObjects(IEnumerable<RealtyObject> objects)
    {
        foreach (var foundObject in objects)
        {
            if (foundObject is PropertyQuadObject foundProperty)
            {
                AddPropertyToGrid(foundProperty);
            }
            else if (foundObject is ParcelQuadObject foundParcel)
            {
                AddParcelToGrid(foundParcel);
            }

            UpdateRowHeader();
        }
    }

    private void showAllButton_Click(object sender, EventArgs e)
    {
        allObjectsGridView.Rows.Clear();
        allObjectsGridView.Refresh();

        var foundObjects = Program.ApplicationLogic.GetAllQuadTreeRealtyObjects();

        foreach (var foundObject in foundObjects)
        {
            if (foundObject is PropertyQuadObject foundProperty)
            {
                AddPropertyToGrid(foundProperty);
            }
            else if (foundObject is ParcelQuadObject foundParcel)
            {
                AddParcelToGrid(foundParcel);
            }

            UpdateRowHeader();
        }
    }

    private void searchAllObjectsButton_Click(object sender, EventArgs e)
    {
        allObjectsGridView.Rows.Clear();
        allObjectsGridView.Refresh();

        var area = CreateSearchAreaFromInput();
        var foundObjects = Program.ApplicationLogic.FindObjectsInArea(area);

        foreach (var foundObject in foundObjects)
        {
            if (foundObject is PropertyQuadObject foundProperty)
            {
                AddPropertyToGrid(foundProperty);
            }
            else if (foundObject is ParcelQuadObject foundParcel)
            {
                AddParcelToGrid(foundParcel);
            }

            UpdateRowHeader();
        }
    }

    private void allObjectsGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        // Check if the click is on a valid cell
        if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        {
            // Optionally, get the value of the cell that was double-clicked
            var cellValue = allObjectsGridView[e.ColumnIndex, e.RowIndex].Value?.ToString() ?? "N/A";

            // Show the message box
            MessageBox.Show($"Cell at row {e.RowIndex + 1}, column {e.ColumnIndex + 1} \nValue: {cellValue}", "Cell Double-Clicked");
        }
    }

    private Rectangle CreateSearchAreaFromInput()
    {
        var latDirLeft = latNRadioButton.Checked ? LatitudeDirection.N : LatitudeDirection.S;
        var longDirLeft = longERadioButton.Checked ? LongitudeDirection.E : LongitudeDirection.W;
        var latDirRight = latNRadioButton2.Checked ? LatitudeDirection.N : LatitudeDirection.S;
        var longDirRight = longERadioButton2.Checked ? LongitudeDirection.E : LongitudeDirection.W;

        var latLeftPoint = latNumericUpDown.Value * (int)latDirLeft;
        var longLeftPoint = longNumericUpDown.Value * (int)longDirLeft;
        var latRightPoint = latNumericUpDown2.Value * (int)latDirRight;
        var longRightPoint = longNumericUpDown2.Value * (int)longDirRight;

        var gpsPoint1 = new GPSPoint(latDirLeft, Convert.ToDouble(latLeftPoint), longDirLeft, Convert.ToDouble(longLeftPoint));
        var gpsPoint2 = new GPSPoint(latDirRight, Convert.ToDouble(latRightPoint), longDirRight, Convert.ToDouble(longRightPoint));

        return new Rectangle(gpsPoint1, gpsPoint2);
    }

    private void AddPropertyToGrid(PropertyQuadObject property)
    {
        // NEW CODE JUST FOR PROF JANKOVIC REQUEST
        //string finalString = "";
        //foreach (var parcel in property.PositionedOnParcels)
        //{
        //    string numberParcel = parcel.ParcelNumber.ToString() + " ";
        //    string descriptionParcel = parcel.Description + " ";
        //    string bottomLeftParcel = parcel.LowerLeft.ToString() + " ";
        //    string topRightParcel = parcel.UpperRight.ToString() + " ";
        //    finalString += numberParcel + descriptionParcel + bottomLeftParcel + topRightParcel + "\n";
        //}
        //END OF CODE

        //string listOfParcelsString = String.Join(", ", property.PositionedOnParcels.Select(parcel => parcel.ParcelNumber));
        string bottomLeftString = property.LowerLeft.ToString();
        string topRightString = property.UpperRight.ToString();

        allObjectsGridView.Rows.Add(
            "Property",
            property.PropertyNumber,
            //property.Description,
            //finalString, //INSTEAD of listOfParcelsString
            bottomLeftString,
            topRightString);
    }

    private void AddParcelToGrid(ParcelQuadObject parcel)
    {
        // NEW CODE JUST FOR PROF JANKOVIC REQUEST
        //string finalString = "";
        //foreach (var property in parcel.OccupiedByProperties)
        //{
        //    string numberParcel = property.ConscriptionNumber.ToString() + " ";
        //    string descriptionParcel = property.Description + " ";
        //    string bottomLeftParcel = property.LowerLeft.ToString() + " ";
        //    string topRightParcel = property.UpperRight.ToString() + " ";
        //    finalString += numberParcel + descriptionParcel + bottomLeftParcel + topRightParcel + "\n";
        //}

        //string listOfPropertiesString = String.Join(", ", parcel.OccupiedByProperties.Select(property => property.ConscriptionNumber));
        string bottomLeftString = parcel.LowerLeft.ToString();
        string topRightString = parcel.UpperRight.ToString();

        allObjectsGridView.Rows.Add(
            "Parcel",
            parcel.ParcelNumber,
            //parcel.Description,
            //finalString, //INSTEAD of listOfPropertiesString
            bottomLeftString,
            topRightString);
    }

    private void UpdateRowHeader()
    {
        int lastRowIndex = allObjectsGridView.Rows.Count - 1;
        allObjectsGridView.Rows[lastRowIndex].HeaderCell.Value = (lastRowIndex + 1).ToString();
    }

    private void allObjectsGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }

    private void allObjectsGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        
        // Check if the click is on a valid cell
        if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex == allObjectsGridView.Columns.Count - 1)
        //DELETE PROPERTY
        {
            bool isProperty = GetCellValue(allObjectsGridView, 0, e.RowIndex) == "Property";

            int propertyNumber = int.Parse(GetCellValue(allObjectsGridView, 1, e.RowIndex));
            //int conscriptionNumber = int.Parse(GetCellValue(propertyGridView, 0, e.RowIndex));

            DialogResult dialogResult;
            if(isProperty)
            {
                dialogResult = MessageBox.Show($"Are you sure you want to delete Property {propertyNumber}?", "Delete Property", MessageBoxButtons.YesNo);
            } else
            {
                dialogResult = MessageBox.Show($"Are you sure you want to delete Parcel {propertyNumber}?", "Delete Parcel", MessageBoxButtons.YesNo);
            }


            if (dialogResult == DialogResult.No)
            {
                return;
            }

            // Parse GPS points from cells
            GPSPoint leftGPSPoint = ParseGPSPointFromCell(allObjectsGridView, 2, e.RowIndex);
            GPSPoint rightGPSPoint = ParseGPSPointFromCell(allObjectsGridView, 3, e.RowIndex);

            GPSRectangle area = new GPSRectangle(leftGPSPoint, rightGPSPoint);

            //string description = GetCellValue(propertyGridView, 1, e.RowIndex);
            //Property property = new Property(propertyNumber,-1, "", area);

            if(isProperty)
            {
                if (Program.ApplicationLogic.DeleteProperty(propertyNumber))
                    allObjectsGridView.Rows.RemoveAt(e.RowIndex);
            } 
            else
            {
                if (Program.ApplicationLogic.DeleteParcel(propertyNumber))
                    allObjectsGridView.Rows.RemoveAt(e.RowIndex);
            }



        }
        else if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex == allObjectsGridView.Columns.Count - 2)
        //EDIT PROPERTY
        {
            bool isProperty = GetCellValue(allObjectsGridView, 0, e.RowIndex) == "Property";

            int propertyNumber = int.Parse(GetCellValue(allObjectsGridView, 1, e.RowIndex));
            //string description = GetCellValue(propertyGridView, 1, e.RowIndex);

            // Parse GPS points from cells
            GPSPoint leftGPSPoint = ParseGPSPointFromCell(allObjectsGridView, 2, e.RowIndex);
            GPSPoint rightGPSPoint = ParseGPSPointFromCell(allObjectsGridView, 3, e.RowIndex);
            GPSRectangle area = new GPSRectangle(leftGPSPoint, rightGPSPoint);

            //Property property = new Property(propertyNumber, -1, "", area);

            RealtyEditForm editForm;
            if (isProperty)
            {
                var result = Program.ApplicationLogic.TryFindProperty(propertyNumber);
                var foundProperty = result as Property;

                if (foundProperty is null)
                {
                    MessageBox.Show($"Property {propertyNumber} does not exist", "Property not found");
                    return;
                }
                editForm = new RealtyEditForm(foundProperty);
            } 
            else
            {
                var result = Program.ApplicationLogic.TryFindParcel(propertyNumber);
                var foundParcel = result as Parcel;

                if (foundParcel is null)
                {
                    MessageBox.Show($"Parcel {propertyNumber} does not exist", "Parcel not found");
                    return;
                }
                editForm = new RealtyEditForm(foundParcel);
            }

            editForm.RealtyObjectUpdated += EditForm_RealtyObjectUpdated;
            editForm.Show();
        }
        else if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex == allObjectsGridView.Columns.Count - 3)
        //LOOK INTO Parcel/PROPERTY
        {
            bool isProperty = GetCellValue(allObjectsGridView, 0, e.RowIndex) == "Property";
            int idNumber = int.Parse(GetCellValue(allObjectsGridView, 1, e.RowIndex));


            if(isProperty)
            {
                var foundProperty = Program.ApplicationLogic.TryFindProperty(idNumber);
                if (foundProperty is null)
                {
                    MessageBox.Show($"Property {idNumber} does not exist", "Property not found");
                    return;
                }
                else
                {
                    MessageBox.Show($"Property: {idNumber}" +
                                    $"\nConscription Number: {foundProperty.ConscriptionNumber}" +
                                    $"\nDsecription: {foundProperty.Description}" +
                                    $"\nPositioned on parcels: {string.Join(", ", foundProperty.PositionedOnParcels)}" +
                                    $"\nGPS {foundProperty.Bounds}", "Property found");
                    return;
                }
            }else
            {
                var foundParcel = Program.ApplicationLogic.TryFindParcel(idNumber);
                if (foundParcel is null)
                {
                    MessageBox.Show($"Parcel {idNumber} does not exist", "Parcel not found");
                    return;
                }
                else
                {
                    MessageBox.Show($"Parcel: {idNumber}" +
                                    $"\nDsecription: {foundParcel.Description}" +
                                    $"\nOccupied By Properties: {string.Join(", ", foundParcel.OccupiedByProperties)}" +
                                    $"\nGPS {foundParcel.Bounds}", "Parcel found");
                    return;
                }
            }
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

    private bool HasBoundaryChanged(RealtyObject oldRealtyObject, RealtyObject updatedRealtyObject)
    {
        return oldRealtyObject.LowerLeft != updatedRealtyObject.LowerLeft ||
               oldRealtyObject.UpperRight != updatedRealtyObject.UpperRight;
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

    private void UpdatePropertyData(Property oldProperty, Property updatedProperty)
    {
        Program.ApplicationLogic.EditProperty(oldProperty, updatedProperty);
        //Program.ApplicationLogic.DeleteProperty(oldProperty);
        //Program.ApplicationLogic.AddProperty(updatedProperty);
        RefreshPropertyDisplay();
    }

    private void UpdateParcelData(Parcel oldParcel, Parcel updatedParcel)
    {
        Program.ApplicationLogic.EditParcel(oldParcel, updatedParcel);
        //Program.ApplicationLogic.DeleteParcel(oldParcel);
        //Program.ApplicationLogic.AddParcel(updatedParcel);
        RefreshPropertyDisplay();
    }

    private void RefreshPropertyDisplay()
    {
        ClearPropertyGridView();
        Rectangle area = CreateSearchAreaFromInput();
        var foundProperties = Program.ApplicationLogic.FindObjectsInArea(area);
        DisplayFoundObjects(foundProperties);
    }

    private void ClearPropertyGridView()
    {
        allObjectsGridView.Rows.Clear();
        allObjectsGridView.Refresh();
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

    private GPSPoint ParseGPSPointFromCell(DataGridView gridView, int columnIndex, int rowIndex)
    {
        var pointString = GetCellValue(gridView, columnIndex, rowIndex);
        if (TryParseCoordinates(pointString, out LatitudeDirection latitudeDirection, out double latitude, out LongitudeDirection longitudeDirection, out double longitude))
        {
            return new GPSPoint(latitudeDirection, latitude, longitudeDirection, longitude);
        }
        throw new FormatException("Invalid GPS data");
    }

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

    private void allObjectsGridView_CellMouseDoubleClick_1(object sender, DataGridViewCellMouseEventArgs e)
    {

    }
}
