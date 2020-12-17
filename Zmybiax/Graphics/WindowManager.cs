using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.Debug.Kernel;

namespace Zmybiax.Graphics
{
    public class WindowManager
    {
        private VideoDriver driver;
        private bool RUN = true;
        private List<Shortcut> desktopShortcuts = new List<Shortcut>();
        private Color bg = Color.Aqua;
        private System.Drawing.Point mousePos;

        public WindowManager()
        {
            this.driver = new VideoDriver();
        }

        private void QUIT() { this.RUN = false; }

        public void Init()
        {
            Sys.MouseManager.ScreenWidth = (uint)driver.Resolution[0];
            Sys.MouseManager.ScreenHeight = (uint)driver.Resolution[1];
            this.mousePos = new System.Drawing.Point(driver.Resolution[0] / 2, driver.Resolution[1] / 2);
            Screen screen = new Screen(this.bg);
            driver.Clear(this.bg);
            BeforeRun(ref screen);
            ulong frame = 1;
            do
            {
                Run(ref screen, frame);
                driver.RenderScreen(screen);
                //screen.ClearControls();
                frame++;
            } while (this.RUN);
            driver.Disable();
        }

        private void DrawMouse(ref Screen screen, int mouseIndex)
        {
            //TODO
            screen.Controls[mouseIndex].X = (int)Sys.MouseManager.X;
            screen.Controls[mouseIndex].Y = (int)Sys.MouseManager.Y;
            screen.Controls[mouseIndex].Update();
        }

        private void BeforeRun(ref Screen screen)
        {
            const short taskbarHeight = 20;
            Rectangle taskbar = new Rectangle(Color.Blue, 0, driver.Resolution[1] - taskbarHeight, driver.Resolution[0], taskbarHeight, true);
            Rectangle startbtn = new Rectangle(Color.Green, 0, driver.Resolution[1] - taskbarHeight, 100, taskbarHeight, true);
            screen.AddControl(taskbar);
            screen.AddControl(startbtn);
            Form form = new Form("Ostrzezenie!", 100, 50, 400, 200);
            form.Content.Add(new Label("testowy napis lol", 10, 10, form.Color));
            form.Update();
            screen.AddControl(form);
        }

        private void Run(ref Screen screen, ulong frame)
        {
            //DrawMouse(ref screen, 2);
        }
    }
}
