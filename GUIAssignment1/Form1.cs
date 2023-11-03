using GUIAssignment1.UserControls;
using QuadTreeDS.SpatialItems;
using SemesterAssignment1;

namespace GUIAssignment1
{
    public partial class Form1 : Form
    {
        NavigationControl _navigationControl;
        NavigationButtons _navigationButtons;

        Color _btnDefaultColor = Color.FromKnownColor(KnownColor.ControlLight);
        Color _btnSelectedColor = Color.FromKnownColor(KnownColor.ControlDark);
        public Form1()
        {
            InitializeComponent();
            InitializeNavigationControl();
            InitializeNavigationButton();
        }

        private void InitializeNavigationControl()
        {
            List<UserControl> userControls = new List<UserControl>()
            {
                new FindProperties1(),
                new FindParcels2(),
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
                AddPropertyNavButton,
                AddParcelNavButton,
                EditPropertyNavButton,
                EditParcelNavButton,
                DeletePropertyNavButton,
                DeleteParcelNavButton
            };
            _navigationButtons = new NavigationButtons(buttons, _btnDefaultColor, _btnSelectedColor);
            _navigationButtons.HighlightButton(FindPropertiesNavButton); // Default button to highlight

        }

        private void button2_Click(object sender, EventArgs e)
        {
            _navigationControl.DisplayUserControl(0);
            _navigationButtons.HighlightButton(FindPropertiesNavButton);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _navigationControl.DisplayUserControl(1);
            _navigationButtons.HighlightButton(FindParcelsNavButton);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _navigationControl.DisplayUserControl(2);
            _navigationButtons.HighlightButton(FindRealtyNavButton);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _navigationControl.DisplayUserControl(3);
            _navigationButtons.HighlightButton(AddPropertyNavButton);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            _navigationControl.DisplayUserControl(4);
            _navigationButtons.HighlightButton(AddParcelNavButton);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            _navigationControl.DisplayUserControl(5);
            _navigationButtons.HighlightButton(EditPropertyNavButton);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            _navigationControl.DisplayUserControl(6);
            _navigationButtons.HighlightButton(EditParcelNavButton);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            _navigationControl.DisplayUserControl(7);
            _navigationButtons.HighlightButton(DeletePropertyNavButton);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            _navigationControl.DisplayUserControl(8);
            _navigationButtons.HighlightButton(DeleteParcelNavButton);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}