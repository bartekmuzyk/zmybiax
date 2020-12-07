using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Sys = Cosmos.System;
using Zmybiax.Graphics;

namespace Zmybiax
{
    public static class Extensions
    {
        /// <summary>
        /// Removes last element from an array
        /// </summary>
        /// <param name="source"></param>
        /// <returns>The new modified array</returns>
        public static string[] Pop(this string[] source)
        {
            int lastIndex = source.Length - 1;
            string[] newArray = new string[lastIndex];
            for (int i = 0; i < source.Length; i++)
            {
                if (i == lastIndex)
                {
                    break;
                }
                newArray[i] = source[i];
            }
            return newArray;
        }

        /// <summary>
        /// Add an element to a string array
        /// </summary>
        /// <param name="source">Source string array</param>
        /// <param name="element">The element to be added</param>
        /// <returns>A new array with the element</returns>
        public static string[] AddToArray(this string[] source, string element)
        {
            int len = source.Length;
            string[] final = new string[len + 1];
            for (int i = 0; i < len; i++) final[i] = source[i];
            final[len - 1] = element;
            return final;
        }

        /// <summary>
        /// Get flags that were applied for a specific command
        /// </summary>
        /// <param name="instruction">The command entered</param>
        /// <param name="detector">The character that will be used to seperate flags from arguments</param>
        /// <returns>A char array with all the flags</returns>
        public static char[] GetFlags(this string instruction, char detector)
        {
            string[] tokens = instruction.Split(' ');
            List<char> flags = new List<char>(tokens.Length - 1);

            foreach (string token in tokens)
                if (token[0] == detector && token.Length < 3)
                    flags.Add(token[1]);

            return flags.ToArray();
        }

        private static List<Control> FindAllControlsThatWereModifiedSinceLastRender(List<Control> collection)
        {
            List<Control> result = new List<Control>(collection.Count);
            foreach (Control control in collection)
                if (control.ModifiedSinceLastRender) result.Add(control);
            result.TrimExcess();
            return result;
        }

        public static RenderEvent WhichToRender(this Screen screen, int previousControlCount)
        {
            int countDifference = screen.Controls.Count - previousControlCount;
            if (countDifference >= 0)
                return new RenderEvent(RenderEventType.Draw, FindAllControlsThatWereModifiedSinceLastRender(screen.Controls));
            else
                return new RenderEvent(RenderEventType.Undraw, new List<Control>(0));
        }
    }
}
