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
        private Color bg = Color.Black;

        public WindowManager(int width, int height)
        {
            this.Resolution[0] = width;
            this.Resolution[1] = height;
            this.driver = new VideoDriver(width, height);
            Sys.MouseManager.ScreenWidth = (uint)width;
            Sys.MouseManager.ScreenHeight = (uint)height;
        }

        private void QUIT() { this.RUN = false; }

        

        public void Init()
        {
            Screen screen = new Screen(this.bg);
            this.driver.Clear(this.bg);
            BeforeDrawLoop(ref screen);
            ulong frame = 1;
            do
            {
                DrawLoop(ref screen, frame);
                this.driver.RenderScreen(screen);
                //screen.ClearControls();
                frame++;
            } while (this.RUN);
            this.driver.Disable();
        }

        private void BeforeDrawLoop(ref Screen screen)
        {
            Label frames = new Label("before draw loop", 0, 0, Color.Black, Color.White);
            screen.AddControl(frames);
            Rectangle box = new Rectangle(Color.Gray, 50, 50, 300, 200, true);
            screen.AddControl(box);
            Rectangle titlebar = new Rectangle(Color.Blue, 50, 50, 300, 16, true);
            screen.AddControl(titlebar);
            Label title = new Label("Ostrzezenie", 50, 50, Color.Blue, Color.White);
            screen.AddControl(title);
        }

        private void DrawLoop(ref Screen screen, ulong frame)
        {
            ((Label)(screen.Controls[0])).Text = $"frame no. {frame}";
            screen.Controls[0].Update();

            if (frame > 500)
            {
                Circle testCircle = new Circle(Color.White, 20, 80, 20, true);
                screen.AddControl(testCircle);
            }

            if (frame > 1000) QUIT();
        }
    }
}
