using GUIAssignment1.UserControls;
using QuadTreeDS.SpatialItems;
using SemesterAssignment1;

namespace GUIAssignment1
{
    public partial class MainForm : Form
    {
        NavigationControl _navigationControl;
        NavigationButtons _navigationButtons;

        Color _btnDefaultColor = Color.FromKnownColor(KnownColor.ControlLight);
        Color _btnSelectedColor = Color.FromKnownColor(KnownColor.ControlDark);
        public MainForm()
        {
            InitializeComponent();
            InitializeNavigationControl();
            InitializeNavigationButton();
        }

        private void InitializeNavigationControl()
        {
            List<UserControl> userControls = new List<UserControl>()
            {
                new FindProperties(),
                new FindParcels(),
                new FindAllObjects3(),
                new AddProperty4(),
                //Also add other user controls here
            };

            _navigationControl = new NavigationControl(userControls, MainPanel);
            _navigationControl.DisplayUserControl(0); // Default user control to display
        }

        private void InitializeNavigationButton()
        {
            List<Button> buttons = new List<Button>()
            {
                FindPropertiesNavButton,
                FindParcelsNavButton,
                FindRealtyNavButton,
                //AddPropertyNavButton,
                //AddParcelNavButton,
                //EditPropertyNavButton,
                //EditParcelNavButton,
                //DeletePropertyNavButton,
                //DeleteParcelNavButton
            };
            _navigationButtons = new NavigationButtons(buttons, _btnDefaultColor, _btnSelectedColor);
            _navigationButtons.HighlightButton(FindPropertiesNavButton); // Default button to highlight

        }

        private void button2_Click(object sender, EventArgs e)
        {
            _navigationControl.DisplayUserControl(0);
            _navigationButtons.HighlightButton(FindPropertiesNavButton);
            MainForm.ActiveForm.Text = "Properties";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _navigationControl.DisplayUserControl(1);
            _navigationButtons.HighlightButton(FindParcelsNavButton);
            MainForm.ActiveForm.Text = "Parcels";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _navigationControl.DisplayUserControl(2);
            _navigationButtons.HighlightButton(FindRealtyNavButton);
            MainForm.ActiveForm.Text = "Search Realty Objects";
        }

        //private void button5_Click(object sender, EventArgs e)
        //{
        //    _navigationControl.DisplayUserControl(3);
        //    _navigationButtons.HighlightButton(AddPropertyNavButton);
        //    MainForm.ActiveForm.Text = "Add Property";
        //}

        //private void button6_Click(object sender, EventArgs e)
        //{
        //    _navigationControl.DisplayUserControl(4);
        //    _navigationButtons.HighlightButton(AddParcelNavButton);
        //    MainForm.ActiveForm.Text = "Add Parcel";
        //}

        //private void button7_Click(object sender, EventArgs e)
        //{
        //    _navigationControl.DisplayUserControl(5);
        //    _navigationButtons.HighlightButton(EditPropertyNavButton);
        //}

        //private void button8_Click(object sender, EventArgs e)
        //{
        //    _navigationControl.DisplayUserControl(6);
        //    _navigationButtons.HighlightButton(EditParcelNavButton);
        //}

        //private void button9_Click(object sender, EventArgs e)
        //{
        //    _navigationControl.DisplayUserControl(7);
        //    _navigationButtons.HighlightButton(DeletePropertyNavButton);
        //}

        //private void button10_Click(object sender, EventArgs e)
        //{
        //    _navigationControl.DisplayUserControl(8);
        //    _navigationButtons.HighlightButton(DeleteParcelNavButton);
        //}


        static string ShowInputBox(string title, string promptText, string defaultValue)
        {
            return Microsoft.VisualBasic.Interaction.InputBox(promptText, title, defaultValue);
        }

        private void ActionsTableLayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ImportButton_Click_1(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "CSV files (*.csv)|*.csv";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string filePath = ofd.FileName;
                    var realtyObjects = Program.ApplicationLogic.ImportCSV(filePath);
                    Program.ApplicationLogic.ClearAll();
                    foreach (var realtyObject in realtyObjects)
                    {
                        Program.ApplicationLogic.AddObject(realtyObject);
                    }
                }
                MessageBox.Show("Imported");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ExportButton_Click_1(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string folderPath = folderBrowserDialog.SelectedPath;
                    string fileName = ShowInputBox("Enter file name", "Export as CSV", "export.csv");
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        string fullPath = Path.Combine(folderPath, fileName);

                        Program.ApplicationLogic.ExportCSV(Program.ApplicationLogic.GetAllRealtyObjects(), fullPath);
                        MessageBox.Show("Exported");
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void generateDataButton_Click(object sender, EventArgs e)
        {
            GenerateDataForm generateDataForm = new GenerateDataForm();
            generateDataForm.ShowDialog();

            
        }
    }
}