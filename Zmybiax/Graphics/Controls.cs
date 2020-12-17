using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;

namespace Zmybiax.Graphics
{

    public class Control
    {
        internal int X { get; set; }
        internal int Y { get; set; }
        public Color Color;
        public bool ModifiedSinceLastRender = true;
        internal Action<Canvas, int, int> renderCallback;

        public void Update()
        {
            this.ModifiedSinceLastRender = true;
        }

        public void Render(ref Canvas canvas)
        {
            this.ModifiedSinceLastRender = false;
            this.renderCallback.Invoke(canvas, this.X, this.Y);
        }

        public void Render(ref Canvas canvas, int posX, int posY)
        {
            this.ModifiedSinceLastRender = false;
            this.renderCallback.Invoke(canvas, posX, posY);
        }
    }

    //Control classess
    public class Line : Control
    {
        public int Width;
        public int Height;

        public Line(Color color, int x, int y, int width, int height)
        {
            this.Color = color;
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;

            this.renderCallback = (canvas, posX, posY) =>
            {
                canvas.DrawLine(new Pen(this.Color), posX, posY, posX + this.Width, posY + this.Height);
            };
        }
    }

    public class Rectangle : Control
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Filled;

        public Rectangle(Color color, int x, int y, int width, int height, bool filled)
        {
            this.Color = color;
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Filled = filled;

            this.renderCallback = (canvas, posX, posY) =>
            {
                if (this.Filled)
                    canvas.DrawFilledRectangle(new Pen(this.Color), posX, posY, this.Width, this.Height);
                else
                    canvas.DrawRectangle(new Pen(this.Color), posX, posY, this.Width, this.Height);
            };
        }
    }

    public class Circle : Control
    {
        public int RadiusX { get; set; }
        public int RadiusY { get; set; }
        public bool Filled;

        public Circle(Color color, int centerX, int centerY, int radius, bool filled)
        {
            this.Color = color;
            this.X = centerX;
            this.Y = centerY;
            this.RadiusX = radius;
            this.RadiusY = radius;
            this.Filled = filled;

            this.renderCallback = (canvas, posX, posY) =>
            {
                if (this.Filled)
                    canvas.DrawFilledCircle(new Pen(this.Color), posX, posY, this.RadiusX);
                else
                    canvas.DrawCircle(new Pen(this.Color), posX, posY, this.RadiusX);
            };
        }

        public Circle(Color color, int centerX, int centerY, int radiusX, int radiusY, bool filled)
        {
            this.Color = color;
            this.X = centerX;
            this.Y = centerY;
            this.RadiusX = radiusX;
            this.RadiusY = radiusY;
            this.Filled = filled;

            this.renderCallback = (canvas, posX, posY) =>
            {
                if (this.Filled)
                    canvas.DrawFilledEllipse(new Pen(this.Color), posX - this.RadiusX, posY - this.RadiusY, posX + this.RadiusX, posY + this.RadiusY);
                else
                    canvas.DrawEllipse(new Pen(this.Color), posX, posY, this.RadiusX, this.RadiusY);
            };
        }
    }

    public class Label : Control
    {
        public string Text;
        public Color Background;

        public Label(string text, int x, int y, Color background, Color color)
        {
            this.Text = text;
            this.X = x;
            this.Y = y;
            this.Color = color;
            this.Background = background;

            this.renderCallback = (canvas, posX, posY) =>
            {
                canvas.DrawFilledRectangle(new Pen(this.Background), posX, posY, this.Text.Length * 8, 16);
                canvas.DrawString(this.Text, PCScreenFont.Default, new Pen(this.Color), posX, posY);
            };
        }

        public Label(string text, int x, int y, Color background)
        {
            this.Text = text;
            this.X = x;
            this.Y = y;
            this.Color = Color.White;
            this.Background = background;

            this.renderCallback = (canvas, posX, posY) =>
            {
                canvas.DrawFilledRectangle(new Pen(this.Background), posX, posY, this.Text.Length * 8, 16);
                canvas.DrawString(this.Text, PCScreenFont.Default, new Pen(this.Color), posX, posY);
            };
        }
    }

    public class Form : Control
    {
        const int TITLE_BAR_HEIGHT = 16;

        public string Title;
        public List<Control> Content;
        public int Width;
        public int Height;
        internal int previousControlCount = 0;

        public Form(string title, int x, int y, int width, int height)
        {
            this.Title = title;
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Color = Color.Gray;

            this.renderCallback = (canvas, posX, posY) =>
            {
                canvas.DrawFilledRectangle(new Pen(this.Color), posX, posY, width, height);
                canvas.DrawFilledRectangle(new Pen(Color.Blue), posX, posY, this.Width, TITLE_BAR_HEIGHT);
                canvas.DrawString(this.Title, PCScreenFont.Default, new Pen(Color.White), posX, posY);

                RenderEvent e = this.Content.WhichToRender(this.previousControlCount);
                if (e.Type == RenderEventType.Draw)
                    foreach (Control control in e.Data)
                        control.Render(ref canvas, this.X + control.X, this.Y + TITLE_BAR_HEIGHT + control.Y);
                else if (e.Type == RenderEventType.Undraw)
                    canvas.DrawFilledRectangle(new Pen(this.Color), posX, posY + TITLE_BAR_HEIGHT, this.Width, this.Height - TITLE_BAR_HEIGHT);

                this.previousControlCount = this.Content.Count;
            };
        }
    }
}
