using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Zmybiax.ZmybiaxGraphics
{
    class Object
    {
        public enum ObjectType
        {
            Label,
            Button,
            TextBox
        }

        public enum AlignType
        {
            Free,
            Top,
            Left,
            Down,
            Right
        }

        public ObjectType Type;
        public Color BackgroundColor;
        public Color ForegroundColor;
        public Color BorderColor;
        public short BorderSize;
        public AlignType Align;
        private int[] Position;
        public string Value;

        public Object(ObjectType type, int x, int y)
        {
            this.Type = type;
            switch (type)
            {
                case ObjectType.Label:
                    this.BackgroundColor = Color.Transparent;
                    this.ForegroundColor = Color.Black;
                    this.Value = "Label";
                    break;
                case ObjectType.Button:
                    this.BackgroundColor = Color.DarkGray;
                    this.ForegroundColor = Color.White;
                    this.BorderSize = 1;
                    this.Value = "Button";
                    break;
                case ObjectType.TextBox:
                    this.BackgroundColor = Color.White;
                    this.ForegroundColor = Color.Black;
                    this.BorderSize = 2;
                    this.Value = "TextBox";
                    break;
            }
            this.Align = AlignType.Free;
            this.Position = new int[] { x, y };
        }

        public Object(ObjectType type, AlignType align)
        {
            this.Type = type;
            switch (type)
            {
                case ObjectType.Label:
                    this.BackgroundColor = Color.Transparent;
                    this.ForegroundColor = Color.Black;
                    this.Value = "Label";
                    break;
                case ObjectType.Button:
                    this.BackgroundColor = Color.DarkGray;
                    this.ForegroundColor = Color.White;
                    this.BorderSize = 1;
                    this.Value = "Button";
                    break;
                case ObjectType.TextBox:
                    this.BackgroundColor = Color.White;
                    this.ForegroundColor = Color.Black;
                    this.BorderSize = 2;
                    this.Value = "TextBox";
                    break;
            }
            this.Align = align;
            this.Position = new int[] { 0, 0 };
        }

        public Object(ObjectType type, int x, int y, string value)
        {
            this.Type = type;
            switch (type)
            {
                case ObjectType.Label:
                    this.BackgroundColor = Color.Transparent;
                    this.ForegroundColor = Color.Black;
                    break;
                case ObjectType.Button:
                    this.BackgroundColor = Color.DarkGray;
                    this.ForegroundColor = Color.White;
                    this.BorderSize = 1;
                    break;
                case ObjectType.TextBox:
                    this.BackgroundColor = Color.White;
                    this.ForegroundColor = Color.Black;
                    this.BorderSize = 2;
                    break;
            }
            this.Value = value;
            this.Align = AlignType.Free;
            this.Position = new int[] { x, y };
        }

        public Object(ObjectType type, AlignType align, string value)
        {
            this.Type = type;
            switch (type)
            {
                case ObjectType.Label:
                    this.BackgroundColor = Color.Transparent;
                    this.ForegroundColor = Color.Black;
                    break;
                case ObjectType.Button:
                    this.BackgroundColor = Color.DarkGray;
                    this.ForegroundColor = Color.White;
                    this.BorderSize = 1;
                    break;
                case ObjectType.TextBox:
                    this.BackgroundColor = Color.White;
                    this.ForegroundColor = Color.Black;
                    this.BorderSize = 2;
                    break;
            }
            this.Value = value;
            this.Align = align;
            this.Position = new int[] { 0, 0 };
        }

        public void SetPosition(int x, int y)
        {
            Position[0] = x;
            Position[1] = y;
        }
    }
}
