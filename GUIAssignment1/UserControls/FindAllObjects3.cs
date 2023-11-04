using QuadTreeDS.SpatialItems;
using SemesterAssignment1.RealtyObjects;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Rectangle = QuadTreeDS.SpatialItems.Rectangle;

namespace GUIAssignment1.UserControls
{
    public partial class FindAllObjects3 : UserControl
    {
        public FindAllObjects3()
        {
            InitializeComponent();
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void showAllButton_Click(object sender, EventArgs e)
        {
            allObjectsGridView.Rows.Clear();
            allObjectsGridView.Refresh();

            var foundObjects = Program.ApplicationLogic.GetAllRealtyObjects();

            foreach (var foundObject in foundObjects)
            {
                if (foundObject is Property foundProperty)
                {
                    AddPropertyToGrid(foundProperty);
                }
                else if (foundObject is Parcel foundParcel)
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
                if (foundObject is Property foundProperty)
                {
                    AddPropertyToGrid(foundProperty);
                }
                else if (foundObject is Parcel foundParcel)
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

        private void AddPropertyToGrid(Property property)
        {
            string listOfParcelsString = String.Join(", ", property.PositionedOnParcels.Select(parcel => parcel.ParcelNumber));
            string bottomLeftString = property.LowerLeft.ToString();
            string topRightString = property.UpperRight.ToString();

            allObjectsGridView.Rows.Add(
                "Property",
                property.ConscriptionNumber,
                property.Description,
                listOfParcelsString,
                bottomLeftString,
                topRightString);
        }

        private void AddParcelToGrid(Parcel parcel)
        {
            string listOfPropertiesString = String.Join(", ", parcel.OccupiedByProperties.Select(property => property.ConscriptionNumber));
            string bottomLeftString = parcel.LowerLeft.ToString();
            string topRightString = parcel.UpperRight.ToString();

            allObjectsGridView.Rows.Add(
                "Parcel",
                parcel.ParcelNumber,
                parcel.Description,
                listOfPropertiesString,
                bottomLeftString,
                topRightString);
        }

        private void UpdateRowHeader()
        {
            int lastRowIndex = allObjectsGridView.Rows.Count - 1;
            allObjectsGridView.Rows[lastRowIndex].HeaderCell.Value = (lastRowIndex + 1).ToString();
        }


    }
}
