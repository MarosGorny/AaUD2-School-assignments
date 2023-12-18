using QuadTreeDS.SpatialItems;

namespace GUIAssignment2;

public partial class GenerateDataForm : Form
{
    public GenerateDataForm()
    {
        InitializeComponent();
    }

    private void generateButton_Click(object sender, EventArgs e)
    {
        try
        {
            var parcelsCount = (int)parcelsCountNumericUpDown.Value;
            var propertiesCount = (int)propertiesCountNumericUpDown.Value;

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

            var mainBlockFactor = (int)mainBlockFactorNumericUpDown.Value;
            var overflowBlockFactor = (int)overflowBlockFactorNumericUpDown.Value;
            var maxHashSize = (int)maxHashSizeNumericUpDown.Value == 0 ? null : (int?)maxHashSizeNumericUpDown.Value;

            Program.ApplicationLogic.GenerateNewData(propertiesCount, parcelsCount, area, mainBlockFactor, overflowBlockFactor, maxHashSize);
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        MessageBox.Show("Data generated successfully.");

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

    private void numericUpDown2_ValueChanged(object sender, EventArgs e)
    {

    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
        hashCheckBox.Checked = !hashCheckBox.Checked;
        if(hashCheckBox.Checked)
        {
            maxHashSizeNumericUpDown.Enabled = true;
            maxHashSizeNumericUpDown.Value = 1;
            maxHashSizeNumericUpDown.Minimum = 1;

        } else
        {
            maxHashSizeNumericUpDown.Enabled = false;
            maxHashSizeNumericUpDown.Minimum = 0;
            maxHashSizeNumericUpDown.Value = 0;
        }
    }
}
