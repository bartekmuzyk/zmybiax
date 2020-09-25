using Cosmos.Core.IOGroup;
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
        private Window[] windows = new Window[4];
        private List<Layer> layers = new List<Layer>();

        public WindowManager(SVGAIICanvas canvas, byte layout = 0x01)
        {
            this.screen = canvas;
            this.layout = layout;

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

        public void Disable() { this.screen.Disable(); }

        public void Display() { this.screen.Display(); }

        public SVGAIICanvas GetScreen() { return this.screen; }

        private int GetNextFreePosition()
        {
            int index = 0;
            for (int i = 0; i < windows.Length; i++)
            {
                if (windows[i] == null)
                {
                    index = i + 1;
                    break;
                }
            }
            return index;
        }

        public void InitWindow(string title)
        {
            int freePos = GetNextFreePosition();
            if (freePos == 0){ Console.WriteLine("freepos 0"); return; }

            Window window = new Window(this, title, (byte)freePos);

            int[] boundaries = window.GetBoundaries();
            switch (window.Position)
            {
                case 0x01:
                    this.screen.DrawFilledRectangle(new Pen(window.Background), 0, 0, boundaries[0], boundaries[1]);
                    this.screen.DrawFilledRectangle(new Pen(Color.DodgerBlue), 0, 0, boundaries[0], 33);
                    break;
                default:
                    this.screen.DrawFilledRectangle(new Pen(window.Background), 0, 0, boundaries[0], boundaries[1]);
                    this.screen.DrawFilledRectangle(new Pen(Color.DodgerBlue), 0, 0, boundaries[0], 33);
                    break;

            }
        }
    }
}
