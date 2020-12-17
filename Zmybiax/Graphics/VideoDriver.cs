using System;
using System.Collections.Generic;
using System.Text;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using Cosmos.HAL;
using System.Drawing;

namespace Zmybiax.Graphics
{
    public enum RenderEventType
    {
        Draw,
        Undraw
    }

    public class RenderEvent
    {
        public RenderEventType Type { get; }
        public List<Control> Data;

        public RenderEvent(RenderEventType eventType, List<Control> data)
        {
            this.Type = eventType;
            this.Data = data;
        }
    }

    public class VideoDriver
    {
        private Canvas canvas;
        public int[] Resolution = new int[2];
        internal int previousControlsCount = 0;

        public VideoDriver()
        {
            this.canvas = FullScreenCanvas.GetFullScreenCanvas();
            this.Resolution = new int[] {
                this.canvas.DefaultGraphicMode.Columns,
                this.canvas.DefaultGraphicMode.Rows
            };
        }

        public void Disable() { this.canvas.Disable(); }

        public void SetPixel(int x, int y, Color c) { canvas.DrawPoint(new Pen(c), x, y); }

        private void RenderControls(List<Control> collection)
        {
            collection.ForEach((control) => {
                control.Render(ref this.canvas);
            });
        }

        public void RenderScreen(Screen screen)
        {
            RenderEvent e = screen.Controls.WhichToRender(this.previousControlsCount);
            if (e.Type == RenderEventType.Draw)
                RenderControls(e.Data);
            else if (e.Type == RenderEventType.Undraw)
            {
                Clear(screen.Background);
                RenderControls(screen.Controls);
            }
            this.previousControlsCount = screen.Controls.Count;
        }

        public void Clear(Color c)
        {
            this.canvas.Clear(c);
        }
    }
}
