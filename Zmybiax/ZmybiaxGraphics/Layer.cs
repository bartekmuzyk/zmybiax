using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zmybiax.ZmybiaxGraphics
{
    class Layer
    {
        public byte zIndex = 0x00;
        private SVGAIICanvas canv;

        public Layer(WindowManager wm, byte zIndex)
        {
            this.canv = wm.GetScreen();
            this.zIndex = zIndex;
        }

        public Layer()
        {
            //TODO
        }
    }
}
