using System;
using System.Collections.Generic;
using Sys = Cosmos.System;

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
            //Can't use Linq :(
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
        /// Write colored text with custom foreground color in the console
        /// </summary>
        /// <param name="text">Text to be written</param>
        /// <param name="foreground">Text color</param>
        /// <param name="endl">To write a new line after the text</param>
        public static void WriteColored(string text, ConsoleColor foreground, bool endl)
        {
            ConsoleColor originalFg = Console.ForegroundColor;
            Console.ForegroundColor = foreground;
            Console.Write(text);
            Console.ForegroundColor = originalFg;
            if (endl) { Console.Write(Environment.NewLine); }
        }

        /// <summary>
        /// Write colored text with custom foreground and background in the console
        /// </summary>
        /// <param name="text">Text to be written</param>
        /// <param name="foreground">Text color</param>
        /// <param name="background">Background color</param>
        /// <param name="endl">To write a new line after the text</param>
        public static void WriteColored(string text, ConsoleColor foreground, ConsoleColor background, bool endl)
        {
            ConsoleColor originalFg = Console.ForegroundColor;
            ConsoleColor originalBg = Console.BackgroundColor;
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Write(text);
            Console.ForegroundColor = originalFg;
            Console.BackgroundColor = originalBg;
            if (endl) { Console.Write(Environment.NewLine); }
        }

        /// <summary>
        /// Convert string to a character array
        /// </summary>
        /// <param name="source">Source string</param>
        /// <returns>A char array</returns>
        public static char[] ToCharArray(this string source)
        {
            char[] chars = new char[source.Length];
            int i = 0;
            foreach (char c in source)
            {
                chars[i] = c;
                i++;
            }
            return chars;
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
            for (int i = 0; i < len; i++)
            {
                final[i] = source[i];
            }
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
            {
                if (token[0] == detector && token.Length < 2)
                {
                    flags.Add(token[1]);
                }
            }
            return flags.ToArray();
        }
    }
}
