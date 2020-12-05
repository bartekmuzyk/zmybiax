using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Zmybiax.Graphics
{
    public enum ControlType
    {
        Line,
        Rectangle,
        Circle,
        Label,
        TextBox,
        Button,
        Picture
    }

    public class Control
    {
        internal ControlType Type;
        internal int X { get; set; }
        internal int Y { get; set; }
        public Color Color;
        internal bool CheckBaseEquality(Control c)
        {
            return
                (this.Type == c.Type) &&
                (this.X == c.X) &&
                (this.Y == c.Y) &&
                (this.Color == c.Color);
        }
    }

    //Control classess

    public class Line : Control
    {
        internal int EndX { get; set; }
        internal int EndY { get; set; }

        public Line(Color color, int startx, int starty, int endx, int endy)
        {
            this.Type = ControlType.Line;
            this.Color = color;
            this.X = startx;
            this.Y = starty;
            this.EndX = endx;
            this.EndY = endy;
        }

        public bool Equals(Line c)
        {
            return this.CheckBaseEquality(c) &&
                (this.EndX == c.EndX) &&
                (this.EndY == c.EndY);
        }
    }

    public class Rectangle : Control
    {
        internal int Width { get; set; }
        internal int Height { get; set; }
        public bool Filled;

        public Rectangle(Color color, int x, int y, int width, int height, bool filled)
        {
            this.Type = ControlType.Rectangle;
            this.Color = color;
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Filled = filled;
        }

        public bool Equals(Rectangle c)
        {
            return this.CheckBaseEquality(c) &&
                (this.Width == c.Width) &&
                (this.Height == c.Height) &&
                (this.Filled == c.Filled);
        }
    }

    public class Circle : Control
    {
        internal int RadiusX { get; set; }
        internal int RadiusY { get; set; }
        public bool Filled;

        public Circle(Color color, int centerX, int centerY, int radius, bool filled)
        {
            this.Type = ControlType.Circle;
            this.Color = color;
            this.X = centerX;
            this.Y = centerY;
            this.RadiusX = radius;
            this.RadiusY = radius;
            this.Filled = filled;
        }

        public Circle(Color color, int centerX, int centerY, int radiusX, int radiusY, bool filled)
        {
            this.Type = ControlType.Circle;
            this.Color = color;
            this.X = centerX;
            this.Y = centerY;
            this.RadiusX = radiusX;
            this.RadiusY = radiusY;
            this.Filled = filled;
        }

        public bool Equals(Circle c)
        {
            return this.CheckBaseEquality(c) &&
                (this.RadiusX == c.RadiusX) &&
                (this.RadiusY == c.RadiusY) &&
                (this.Filled == c.Filled);
        }
    }

    public class Label : Control
    {
        public string Text;

        public Label(string text, int x, int y, Color color)
        {
            this.Type = ControlType.Label;
            this.Text = text;
            this.X = x;
            this.Y = y;
            this.Color = color;
        }

        public Label(string text, int x, int y)
        {
            this.Type = ControlType.Label;
            this.Text = text;
            this.X = x;
            this.Y = y;
        }

        public bool Equals(Label c)
        {
            return this.CheckBaseEquality(c) &&
                (this.Text == c.Text);
        }
    }
}
