using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Zmybiax.ZmybiaxGraphics
{
    class WindowManager
    {
        private SVGAIICanvas screen;
        public int[] resolution = new int[2];
        public byte layout = 0x01;

        public WindowManager(SVGAIICanvas canvas)
        {
            this.screen = canvas;

            Console.WriteLine(canvas.DefaultGraphicMode);
            resolution[0] = canvas.DefaultGraphicMode.Columns;
            resolution[1] = canvas.DefaultGraphicMode.Rows;
            screen.Display();

            Pen pen = new Pen(Color.FromArgb(33, 33, 33));
            screen.DrawFilledRectangle(pen, 0, 0, resolution[0], resolution[1]);

            pen.Color = Color.White;
            screen.DrawFilledRectangle(pen, resolution[0] / 2, 0, 2, resolution[1]);
            screen.DrawFilledRectangle(pen, 0, resolution[1] / 2, resolution[0], 2);
        }

        public void Disable()
        {
            this.screen.Disable();
        }
    }
}
