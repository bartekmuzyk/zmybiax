using System;
using System.Collections.Generic;
using System.Text;

namespace Zmybiax.Graphics
{
    public class CompressedBitmap
    {
        private byte[] data;

        public CompressedBitmap(byte[] data)
        {
            this.data = data;
        }

        public byte[] Decompress()
        {
            List<byte> decompressed = new List<byte>();

            for (int i = 0; i < this.data.Length; i += 2)
            {
                int times = this.data[i];
                byte value = this.data[i + 1];
                for (int t = 0; t < times; t++) decompressed.Add(value);
            }

            return decompressed.ToArray();
        }
    }
}
