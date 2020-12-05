using System;
using System.Collections.Generic;
using System.Text;
using Cosmos.System.Graphics;
using System.Drawing;

namespace Zmybiax.Graphics.Vector
{
    public class Image
    {
        private List<Shape> _shapes = new List<Shape>();
        public Shape[] Shapes
        {
            get
            {
                return this._shapes.ToArray(); 
            }
        }
        public int Width;
        public int Height;
        
        public Image(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public void AddShape(Shape shape)
        {
            this._shapes.Add(shape);
        }

        public Bitmap Save()
        {
            //TODO
            return new Bitmap(new byte[0]);
        }

        /*public Control[] RenderCGUI(System.Drawing.Point position)
        {
            List<Control> result = new List<Control>(this._shapes.Count);
            foreach (Shape shape in this._shapes)
            {
                int x1 = position.X + shape.Position.X;
                int y1 = position.Y + shape.Position.Y;
                int x2 = position.X + shape.Size.Width;
                int y2 = position.Y + shape.Size.Height;
                switch (shape.Type)
                {
                    case Type.Line:
                        Line line = new Line(shape.Color, x1, y1, x2, y2);
                        result.Add(line);
                        break;
                    case Type.Rectangle:
                        CGUI.Rectangle rect = new CGUI.Rectangle(x1, y1, x2, y2, shape.Color);
                        result.Add(rect);
                        break;
                }
            }
            return result.ToArray();
        }*/
    }
}
