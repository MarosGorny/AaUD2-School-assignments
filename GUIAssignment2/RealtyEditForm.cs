using QuadTreeDS.SpatialItems;
using SemesterAssignment2.RealtyObjects;

namespace GUIAssignment2;

public partial class RealtyEditForm : Form
{
    private RealtyObject _realtyObject;
    public event EventHandler<RealtyObjectEventArgs> RealtyObjectUpdated;
    public RealtyEditForm(RealtyObject realtyObject)
    {
        InitializeComponent();
        _realtyObject = realtyObject;
        SetupFormBasedOnSpatialItem();
    }

    private void SetupFormBasedOnSpatialItem()
    {
        switch (_realtyObject)
        {
            case Property property:
                SetupPropertyFields(property);
                break;
            case Parcel parcel:
                SetupParcelFields(parcel);
                break;
            default:
                throw new ArgumentException("Unsupported Realty item type.");
        }
    }

    private void SetupPropertyFields(Property property)
    {
        titleLabel.Text = $"Edit Property: {property.ConscriptionNumber}";
        descriptionTextBox.Text = property.Description;
        if (string.IsNullOrEmpty(descriptionTextBox.Text))
            descriptionTextBox.PlaceholderText = "Description of property";

        idNumberNumericUpDown.Value = property.ConscriptionNumber;

        SetupGpsFields(gps1LatNumericUpDown, gps1LongNumericUpDown, gps1LatSRadioButton, gps1LongWRadioButton, property.LowerLeft as GPSPoint);
        SetupGpsFields(gps2LatNumericUpDown, gps2LongNumericUpDown, gps2LatSRadioButton, gps2LongWRadioButton, property.UpperRight as GPSPoint);
    }

    private void SetupParcelFields(Parcel parcel)
    {
        titleLabel.Text = $"Edit Parcel: {parcel.ParcelNumber}";
        descriptionTextBox.Text = parcel.Description;
        if (string.IsNullOrEmpty(descriptionTextBox.Text))
            descriptionTextBox.PlaceholderText = "Description of parcel";
        idNumberNumericUpDown.Value = parcel.ParcelNumber;
        idNumberNumericUpDown.Enabled = false;

        // Assuming Parcel also has GPSPoint LowerLeft and UpperRight fields or similar.
        SetupGpsFields(gps1LatNumericUpDown, gps1LongNumericUpDown, gps1LatSRadioButton, gps1LongWRadioButton, parcel.LowerLeft as GPSPoint);
        SetupGpsFields(gps2LatNumericUpDown, gps2LongNumericUpDown, gps2LatSRadioButton, gps2LongWRadioButton, parcel.UpperRight as GPSPoint);
    }

    private void SetupGpsFields(NumericUpDown latNumericUpDown, NumericUpDown longNumericUpDown, RadioButton latSRadioButton, RadioButton longWRadioButton, GPSPoint gpsPoint)
    {
        latNumericUpDown.Value = (decimal)gpsPoint.X;
        longNumericUpDown.Value = (decimal)gpsPoint.Y;
        latSRadioButton.Checked = gpsPoint.LatitudeDirection == LatitudeDirection.S;
        longWRadioButton.Checked = gpsPoint.LongitudeDirection == LongitudeDirection.W;
    }

    private void updateButton_Click(object sender, EventArgs e)
    {
        UpdateRealtyObject();
    }

    private void UpdateRealtyObject()
    {
        RealtyObject originalObject;

        // Update the GPS points
        GPSPoint leftPoint = CreateGPSPointFromInputs(
            gps1LatNumericUpDown, gps1LongNumericUpDown,
            gps1LatNRadioButton, gps1LongERadioButton
        );

        GPSPoint rightPoint = CreateGPSPointFromInputs(
            gps2LatNumericUpDown, gps2LongNumericUpDown,
            gps2LatNRadioButton, gps2LongERadioButton
        );

        GPSRectangle area = new GPSRectangle(leftPoint, rightPoint);
        GPSRectangle oldArea = new GPSRectangle(_realtyObject.LowerLeft as GPSPoint, _realtyObject.UpperRight as GPSPoint);
        // Update the object with new values from the form fields
        if (_realtyObject is Property property)
        {
            originalObject = new Property(property.PropertyNumber, property.ConscriptionNumber, property.Description, oldArea);
            var conscriptionNumber = (int)idNumberNumericUpDown.Value;
            var description = descriptionTextBox.Text;
            Property newProperty = new Property(property.PropertyNumber, conscriptionNumber, description, area);
            _realtyObject = newProperty;
        }
        else if (_realtyObject is Parcel parcel)
        {

            originalObject = new Parcel(parcel.ParcelNumber, parcel.Description, oldArea);
            var parcenNumber = (int)idNumberNumericUpDown.Value;
            var description = descriptionTextBox.Text;
            Parcel newParcel = new Parcel(parcenNumber, description, area);
            _realtyObject = newParcel;
        }
        else
        {
            MessageBox.Show("Unsupported Realty item type.");
            return;
        }

        // Raise the event to notify the main form
        RealtyObjectUpdated?.Invoke(this, new RealtyObjectEventArgs(originalObject, _realtyObject));
        Close();
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

    // Class to pass event data for the RealtyObjectUpdated event
    public class RealtyObjectEventArgs : EventArgs
    {
        public RealtyObject OldRealtyObject { get; }
        public RealtyObject UpdatedRealtyObject { get; }

        public RealtyObjectEventArgs(RealtyObject oldRealtyObject, RealtyObject updatedRealtyObject)
        {
            OldRealtyObject = oldRealtyObject;
            UpdatedRealtyObject = updatedRealtyObject;
        }
    }
}
