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

            _navigationControl = new NavigationControl(userControls, panel2);
            _navigationControl.DisplayUserControl(0); // Default user control to display
        }

        private void InitializeNavigationButton()
        {
            List<Button> buttons = new List<Button>()
            {
                button2,
                button3,
                button4,
                button5,
                button6,
                button7,
                button8,
                button9,
                button10
            };
            _navigationButtons = new NavigationButtons(buttons, _btnDefaultColor, _btnSelectedColor);
            _navigationButtons.HighlightButton(button2); // Default button to highlight

        }

        private void button2_Click(object sender, EventArgs e)
        {
            _navigationControl.DisplayUserControl(0);
            _navigationButtons.HighlightButton(button2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _navigationControl.DisplayUserControl(1);
            _navigationButtons.HighlightButton(button3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _navigationControl.DisplayUserControl(2);
            _navigationButtons.HighlightButton(button4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _navigationControl.DisplayUserControl(3);
            _navigationButtons.HighlightButton(button5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            _navigationControl.DisplayUserControl(4);
            _navigationButtons.HighlightButton(button6);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            _navigationControl.DisplayUserControl(5);
            _navigationButtons.HighlightButton(button7);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            _navigationControl.DisplayUserControl(6);
            _navigationButtons.HighlightButton(button8);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            _navigationControl.DisplayUserControl(7);
            _navigationButtons.HighlightButton(button9);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            _navigationControl.DisplayUserControl(8);
            _navigationButtons.HighlightButton(button10);
        }
    }
}