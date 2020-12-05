using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Zmybiax.Graphics.Vector
{
    public enum Type
    {
        Line,
        Rectangle
    }

    public class Shape
    {
        public Type Type;
        public Color Color;
        public Point Position;
        public Size Size;

        public Shape(Type shapeType, Color color, Point position, Size size)
        {
            this.Type = shapeType;
            this.Color = color;
            this.Position = position;
            this.Size = size;
        }
    }
}
