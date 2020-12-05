using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Zmybiax.Graphics
{
    public class Screen
    {
        private int oldControlsCount = 0;
        private List<Control> controls = new List<Control>();
        public Control[] Controls
        {
            get
            {
                return this.controls.ToArray();
            }
        }
        public Color Background;
        public bool NeedsRendering
        {
            get
            {
                return this.controls.Count != this.oldControlsCount;
            }
        }

        public Screen(Color bg)
        {
            this.Background = bg;
        }

        public void RenderCallback()
        {
            this.oldControlsCount = this.controls.Count;
            this.ClearControls();
        }

        public void ClearControls()
        {
            this.controls.RemoveRange(0, this.controls.Count);
        }

        public void AddControl(Control control)
        {
            this.controls.Add(control);
        }

        public bool RemoveControl(Control control)
        {
            return this.controls.Remove(control);
        }

        public bool RemoveControl(int index)
        {
            this.controls.RemoveAt(index);
            return true;
        }
    }
}
