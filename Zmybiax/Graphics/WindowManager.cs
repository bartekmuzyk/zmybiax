using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using CGUI;
using System.Drawing;

namespace Zmybiax
{
    class WindowManager
    {
        private VGADriver driver;
        private Screen screen = new Screen();

        public WindowManager(VGADriver driver)
        {
            this.driver = driver;
        }

        private void AddControl(Control item)
        {
            this.screen.Controls.Add(item);
            this.driver.RenderScreen(this.screen);
        }

        private void AddControl(Control[] items)
        {
            foreach (Control item in items) this.screen.Controls.Add(item);
            this.driver.RenderScreen(this.screen);
        }

        public void Init()
        {

        }
    }
}
