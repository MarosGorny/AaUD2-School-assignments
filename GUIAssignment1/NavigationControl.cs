using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIAssignment1
{
    public class NavigationControl
    {
        List<UserControl> _userControls = new List<UserControl>();
        Panel _panel;

        public NavigationControl(List <UserControl> userControls, Panel panel)
        {
            _userControls = userControls;
            _panel = panel;
            AddUserControls();
        }

        private void AddUserControls()
        {
            for (int i = 0; i < _userControls.Count; i++)
            {
                //Set every user control to fill the panel
                _userControls[i].Dock = DockStyle.Fill;
                _panel.Controls.Add(_userControls[i]);
            }
        }

        public void DisplayUserControl(int index)
        {
            //Or can use bring to front
            _userControls[index].BringToFront();

            ////Hide all user controls
            //foreach (UserControl userControl in _userControls)
            //{
            //    userControl.Hide();
            //}

            ////Display the user control at the specified index
            //_userControls[index].Show();
        }
    }
}
