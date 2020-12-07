using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Zmybiax.Graphics
{
    public class Screen
    {
        public List<Control> Controls = new List<Control>();
        public Color Background;

        public Screen(Color bg)
        {
            this.Background = bg;
        }

        public void ClearControls()
        {
            this.Controls.RemoveRange(0, this.Controls.Count);
        }

        public void AddControl(Control control)
        {
            this.Controls.Add(control);
        }

        public bool RemoveControl(Control control)
        {
            return this.Controls.Remove(control);
        }

        public bool RemoveControl(int index)
        {
            this.Controls.RemoveAt(index);
            return true;
        }
    }
}
