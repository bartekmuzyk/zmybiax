using System;
using System.Collections.Generic;
using System.Text;
using Drw = System.Drawing;
using Sys = Cosmos.System;
using CGUI;

namespace Zmybiax
{
    class WindowManager
    {
        private VGADriver driver;
        public int[] Resolution = new int[2];
        private List<Screen> screens = new List<Screen>() { new Screen() };
        const byte DEFAULT_SCREEN = 0;
        private bool switchMode = false;
        private string wallpaper;

        public WindowManager(int width, int height, string wallpaper = "")
        {
            this.Resolution[0] = width;
            this.Resolution[1] = height;
            this.driver = new VGADriver(width, height);
            this.wallpaper = wallpaper;
        }

        private void ReRender(byte screenIndex)
        {
            this.driver.RenderScreen(this.screens[screenIndex]);
        }

        private void AddControl(Control item, byte screenIndex = DEFAULT_SCREEN)
        {
            this.screens[screenIndex].Controls.Add(item);
            ReRender(screenIndex);
        }

        private void AddControl(Control[] items, byte screenIndex = DEFAULT_SCREEN)
        {
            foreach (Control item in items) this.screens[screenIndex].Controls.Add(item);
            ReRender(screenIndex);
        }

        public void Init()
        {
            Control[] explorer = new Control[]
            {
                new CGUI.Rectangle(0, 0, this.Resolution[0], 18, Drw.Color.White),
                new Label("00:00:00", Drw.Color.Black, 1, 1)
            };

            AddControl(explorer);
        }
    }
}
