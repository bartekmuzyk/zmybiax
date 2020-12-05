using System;
using System.Collections.Generic;
using System.Text;
using Cosmos.System.Graphics;
using System.Drawing;

namespace Zmybiax.Graphics
{
    class Shortcut
    {
        const short width = 160;
        const short height = 90;
        const short maxLetters = width / 8;

        public Bitmap Icon;
        private string _name;
        private byte nameWidth;
        public string Name
        {
            set { _name = value; }
            get {
                if (_name.Equals("")) return "Unknown";
                else
                    return this._name.Length > maxLetters ?
                        _name.Substring(0, 17) + "..."
                        :
                        _name;
            }
        }
        public string Target;

        private bool selected;

        //public Control[] Controls;

        public Shortcut(byte[] icon, string name, string targetFile, int x, int y, bool selected = false)
        {
            this.Icon = new Bitmap(icon);
            this._name = name;
            this.nameWidth = (byte)(name.Length * 8);
            this.Target = targetFile;
            this.selected = selected;

            int labelX = this._name.Length > maxLetters ? x : x + (width - this.nameWidth) / 2;
            /*this.Controls = new Control[]
            {
                new CGUI.Rectangle(x, y, width, height, Color.LightBlue),
                new Picture(this.Icon, x + (width - 64) / 2, y + 5),
                new Label(this.Name, Color.White, labelX, y + 74)
            };*/
        }
    }
}