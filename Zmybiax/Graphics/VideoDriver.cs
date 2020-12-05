using System;
using System.Collections.Generic;
using System.Text;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using Cosmos.HAL;
using System.Drawing;

namespace Zmybiax.Graphics
{
    public class VideoDriver
    {
        private Canvas canvas;
        private Color[] pixelBuffer;
        public int[] Resolution = new int[2];
        public List<Screen> Screens = new List<Screen>();
        private Action renderCallback;

        public VideoDriver(int width, int height, bool transparency, Action renderCallback)
        {
            ColorDepth driverDepth = transparency ? ColorDepth.ColorDepth32 : ColorDepth.ColorDepth24;
            this.canvas = FullScreenCanvas.GetFullScreenCanvas();
            this.Resolution = new int[] { width, height };
            this.pixelBuffer = new Color[(width * height) + width];
            this.renderCallback = renderCallback;
        }

        public void SetRenderCallback(Action callback)
        {
            this.renderCallback = callback;
        }

        public void Disable() { this.canvas.Disable(); }

        #region buffer methods
        public void SetBufferPixel(int x, int y, Color c)
        {
            if (x > this.Resolution[0] || y > this.Resolution[1]) return;
            this.pixelBuffer[(x * y) + x] = c;
        }

        public void ClearBuffer(Color c)
        {
            for (int i = 0; i < this.pixelBuffer.Length; i++) this.pixelBuffer[i] = c;
        }

        public void DrawBuffer()
        {
            this.canvas.DrawArray(this.pixelBuffer, 0, 0, this.Resolution[0], this.Resolution[1]);
        }
        #endregion

        public void SetPixel(int x, int y, Color c)
        {
            canvas.DrawPoint(new Pen(c), x, y);
        }

        private void RenderScreen(Screen screen, bool force)
        {
            if (!screen.NeedsRendering && !force) return;
            this.canvas.Clear(screen.Background);
            foreach (Control control in screen.Controls)
            {
                #region rendering
                Pen pen = new Pen(control.Color);
                int x = control.X;
                int y = control.Y;
                switch (control.Type)
                {
                    case ControlType.Line:
                        Line line = (Line)control;
                        this.canvas.DrawLine(pen, x, y, line.EndX, line.EndY);
                        break;
                    case ControlType.Rectangle:
                        Rectangle rect = (Rectangle)control;
                        if (rect.Filled)
                            this.canvas.DrawFilledRectangle(pen, x, y, x + rect.Width, x + rect.Height);
                        else
                            this.canvas.DrawRectangle(pen, x, y, x + rect.Width, x + rect.Height);
                        break;
                    case ControlType.Circle:
                        Circle circle = (Circle)control;
                        if (circle.RadiusX == circle.RadiusY)
                            if (circle.Filled)
                                this.canvas.DrawFilledCircle(pen, x, y, circle.RadiusX);
                            else
                                this.canvas.DrawCircle(pen, x, y, circle.RadiusX);
                        else
                            if (circle.Filled)
                            this.canvas.DrawFilledEllipse(pen, x - circle.RadiusX, y - circle.RadiusY, x + circle.RadiusX, y + circle.RadiusY);
                        else
                            this.canvas.DrawEllipse(pen, x, y, circle.RadiusX, circle.RadiusY);
                        break;
                    case ControlType.Label:
                        Label label = (Label)control;
                        this.canvas.DrawString(label.Text, PCScreenFont.Default, pen, x, y);
                        break;
                }
                #endregion
            }
            this.renderCallback();
        }

        public void Render(bool force = false)
        {
            foreach (Screen screen in this.Screens) RenderScreen(screen, force);
        }
    }
}
