namespace GUIAssignment1
{
    public class NavigationButtons
    {
        List<Button> _buttons;
        Color _defaultColor;
        Color _selectedColor;

        public NavigationButtons(List<Button> buttons, Color defaultColor, Color selectedColor)
        {
            _buttons = buttons;
            _defaultColor = defaultColor;
            _selectedColor = selectedColor;
            SetButtonColor();
        }

        private void SetButtonColor()
        {
            foreach (Button button in _buttons)
            {
                button.BackColor = _defaultColor;
            }
        }

        public void HighlightButton(Button selectedButton)
        {
            foreach (Button button in _buttons)
            {
                if (button == selectedButton)
                {
                    button.BackColor = _selectedColor;
                }
                else
                {
                    button.BackColor = _defaultColor;
                }
            }
        }
    }
}
