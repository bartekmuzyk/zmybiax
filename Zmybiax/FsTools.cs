using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Cosmos.Debug.Kernel;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.Listing;

namespace Zmybiax
{
    public static class FsTools
    {
        public static void WriteFile(this CosmosVFS fs, string path, string content, bool append = false)
        {
            var targetFile = fs.GetFile(path);
            var targetFileStream = targetFile.GetFileStream();
            if (targetFileStream.CanWrite)
            {
                byte[] toWrite;
                if (append)
                {
                    byte[] currentContent = new byte[targetFileStream.Length];
                    targetFileStream.Read(currentContent, 0, (int)targetFileStream.Length);
                    toWrite = Encoding.ASCII.GetBytes(Encoding.Default.GetString(currentContent) + content);
                }
                else
                {
                    toWrite = Encoding.ASCII.GetBytes(content);
                }
                targetFileStream.Write(toWrite, 0, toWrite.Length);
            }
        }

        public static string GetVerboseEntryType(this DirectoryEntry entry)
        {
            switch (entry.mEntryType)
            {
                case DirectoryEntryTypeEnum.Directory:
                    return "Katalog";
                case DirectoryEntryTypeEnum.File:
                    return "Plik";
                case DirectoryEntryTypeEnum.Unknown:
                    return "Nieznany";
                default:
                    return "Nieznany";
            }
        }

        public static string[] ParsePath(this string source)
        {
            return source.Split("/");
        }

        public static string StringifyPath(this string[] source)
        {
            string final = "";
            foreach (string part in source)
            {
                final = final + part + "/";
            }
            return final;
        }

        public static bool DirectoryExists(this CosmosVFS fs, string path)
        {
            try
            {
                DirectoryEntry entry = fs.GetDirectory(path);
                if (entry.mEntryType == DirectoryEntryTypeEnum.Directory)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool FileExists(this CosmosVFS fs, string path)
        {
            try
            {
                DirectoryEntry entry = fs.GetFile(path);
                if (entry.mEntryType == DirectoryEntryTypeEnum.File)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void DisplayEntries(this List<DirectoryEntry> entries, string path)
        {
            Console.WriteLine("Lista plikow dla katalogu: " + path);
            foreach (DirectoryEntry metadata in entries)
            {
                switch (metadata.mEntryType)
                {
                    case DirectoryEntryTypeEnum.Directory:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;
                    case DirectoryEntryTypeEnum.File:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case DirectoryEntryTypeEnum.Unknown:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                }
                Console.Write("\t");                            //Indentation
                Console.Write(metadata.mName);                  //File name
                Console.ForegroundColor = ConsoleColor.White;   //Information must be in white color
                Console.Write(" -- ");                          //Seperator
                //Console.Write(metadata.mSize);                  //File size on disk
                Console.Write("0");                             //File size on disk
                Console.Write("B");                             //Size unit (bytes)
                Console.Write(Environment.NewLine);             //New line
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
