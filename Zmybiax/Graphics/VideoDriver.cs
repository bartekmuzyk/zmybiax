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
        public RenderEventType Type;
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
        private int previousControlsCount = 0;

        public VideoDriver(int width, int height)
        {
            this.canvas = FullScreenCanvas.GetFullScreenCanvas();
            this.Resolution = new int[] { width, height };
        }

        public void Disable() { this.canvas.Disable(); }

        public void SetPixel(int x, int y, Color c) { canvas.DrawPoint(new Pen(c), x, y); }

        private void RenderControls(List<Control> collection)
        {
            foreach (Control control in collection)
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
                            this.canvas.DrawFilledRectangle(pen, x, y, rect.Width, rect.Height);
                        else
                            this.canvas.DrawRectangle(pen, x, y, rect.Width, rect.Height);
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
                        this.canvas.DrawFilledRectangle(new Pen(label.Background), x, y, label.Text.Length * 8, 16);
                        this.canvas.DrawString(label.Text, PCScreenFont.Default, pen, x, y);
                        break;
                }
                control.ModifiedSinceLastRender = false;
                #endregion
            }
        }

        public void RenderScreen(Screen screen)
        {
            RenderEvent e = screen.WhichToRender(this.previousControlsCount);
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

        public void Clear(Color c, int startx, int starty, int endx, int endy)
        {
            this.canvas.DrawFilledRectangle(new Pen(c), startx, starty, endx - startx, endy - starty);
        }
    }
}
