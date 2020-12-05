using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.Debug.Kernel;
using IL2CPU.API.Attribs;

namespace Zmybiax.Graphics
{
    public class WindowManager
    {
        private VideoDriver driver;
        public int[] Resolution = new int[2];
        private bool RUN = true;
        private List<Shortcut> desktopShortcuts = new List<Shortcut>();

        public Random random = new Random();

        public WindowManager(int width, int height)
        {
            this.Resolution[0] = width;
            this.Resolution[1] = height;
            this.driver = new VideoDriver(width, height, false, () => { });
            Sys.MouseManager.ScreenWidth = (uint)width;
            Sys.MouseManager.ScreenHeight = (uint)height;
        }

        private void QUIT() { this.RUN = false; }

        private void DrawLoop(ref Screen screen, ulong frame)
        {
            Label timeControl = new Label($"00:{frame.ToString()}", 0, 0, Color.White);
            screen.AddControl(timeControl);

            Rectangle testRect = new Rectangle(Color.Red, 60, 40, 300, 200, false);
            screen.AddControl(testRect);

            if (frame > 20000)
            {
                Circle testCircle = new Circle(Color.DodgerBlue, 20, 20, 20, true);
                screen.AddControl(testCircle);
            }

            if (frame > 80000) QUIT();
        }

        public void Init()
        {
            Screen screen = new Screen(Color.Black);
            this.driver.SetRenderCallback(screen.RenderCallback);
            this.driver.Screens.Add(screen);
            DrawLoop(ref screen, 0);
            this.driver.Render(true);

            ulong frame = 1;
            do
            {
                DrawLoop(ref screen, frame);
                this.driver.Render();
                frame++;
            } while (this.RUN);

            this.driver.Disable();
        }
    }
}
