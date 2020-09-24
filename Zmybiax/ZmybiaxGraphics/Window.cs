using System;
using System.Collections.Generic;
using System.Text;

namespace Zmybiax.ZmybiaxGraphics
{
    class Window
    {
        public string Title;
        public int Width;
        public int Height;

        public Window(string title, int[] size)
        {
            this.Title = title;
            this.Width = size[0];
            this.Height = size[1];
        }
    }
}
