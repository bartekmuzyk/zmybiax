using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace Zmybiax.ZmybiaxGraphics
{
    class Window
    {
        public string Title;
        public byte Position;
        private List<Object> objects = new List<Object>();
        public Color Background = Color.LightGray;
        private WindowManager wm;

        public Window(WindowManager wm, string title, byte position)
        {
            this.wm = wm;
            this.Title = title;
            this.Position = position;
        }

        public Window(WindowManager wm, string title, byte position, Color background)
        {
            this.wm = wm;
            this.Title = title;
            this.Position = position;
            this.Background = background;
        }

        public int[] GetBoundaries()
        {
            int[] boundaries = new int[2];
            switch (Position)
            {
                case 0x01:
                    boundaries[0] = wm.resolution[0] / 2;
                    boundaries[1] = wm.resolution[1] / 2;
                    break;
            }
            return boundaries;
        }
    }
}
