using Cosmos.Common.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Zmybiax.ZmybiaxGraphics
{
    class FontManager
    {
        private Dictionary<string, Font> fonts = new Dictionary<string, Font>();
        public readonly char[] AvailableCharacters = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ`~!@#$%^&*()-_=+|\\[{]};:'\",./<>?".ToCharArray();
        public Font DefaultFont;

        public FontManager(){}

        public FontManager(Font defaultFont)
        {
            this.DefaultFont = defaultFont;
        }

        public void SetDefault(Font font)
        {
            this.DefaultFont = font;
        }

        public void LoadFont(Font font)
        {
            fonts[font.Name] = font;
        }

        public Dictionary<char, byte[]> GetData(Font font)
        {
            return font.Characters;
        }
    }
}
