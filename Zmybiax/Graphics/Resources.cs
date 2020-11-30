using System;
using System.Collections.Generic;
using System.Text;

namespace Zmybiax.Graphics
{
    public static class Resources
    {
        public static CompressedBitmap CompressBitmap(this byte[] source)
        {
            List<byte> compressed = new List<byte>();

            byte previousByte = compressed[0];
            int times = 1;
            for (int i = 1; i < source.Length; i++)
            {
                byte current = source[i];
                if (current == previousByte) times++;
                else
                {
                    compressed.Add((byte)times);
                    compressed.Add(previousByte);
                    times = 1;
                }
                previousByte = current;
            }


            return new CompressedBitmap(compressed.ToArray());
        }
    }
}
