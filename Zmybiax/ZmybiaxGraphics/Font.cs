using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Zmybiax.ZmybiaxGraphics
{
    public class Font
    {
        private string path;
        public string Name;
        private byte[] data;
        private List<byte[]> charData;
        public Dictionary<char, byte[]> Characters;

        public Font(byte[] data, char[] characters)
        {
            this.data = data.Splice(20);
            this.charData = data.SpliceEvery(661);

            List<char> nameCharList = new List<char>();
            for (int i = 0; i < 20; i++)
            {
                char c = (char)data[i];
                if (c == ' ') { break; }
                nameCharList.Add(c);
            }

            this.Name = new string(nameCharList.ToArray());

            for (int i = 0; i < characters.Length; i++)
            {
                char c = characters[i];
                this.Characters[c] = charData[i];
            }
        }

        public Font(string path, char[] characters)
        {
            this.path = path;

            byte[] bytes = File.ReadAllBytes(path);
            this.data = bytes.Splice(20);
            this.charData = data.SpliceEvery(661);

            List<char> nameCharList = new List<char>();
            for (int i = 0; i < 20; i++)
            {
                char c = (char)bytes[i];
                if (c == ' ') { break; }
                nameCharList.Add(c);
            }

            this.Name = new string(nameCharList.ToArray());

            for (int i = 0; i < characters.Length; i++)
            {
                char c = characters[i];
                this.Characters[c] = charData[i];
            }
        }

        public void SetData(byte[] data)
        {
            this.data = data;
        }
        
        public byte[] GetData()
        {
            return this.data;
        }
    }
}
