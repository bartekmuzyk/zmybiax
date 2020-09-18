using Cosmos.Debug.Kernel;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.Listing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sys = Cosmos.System;

namespace Zmybiax
{
    public class Kernel : Sys.Kernel
    {
        CosmosVFS fs = new Sys.FileSystem.CosmosVFS();
        Debugger debugger = new Debugger("", "");
        public string path;

        private void WriteFile(CosmosVFS fs, string path, string content, bool append = false)
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
                } else
                {
                    toWrite = Encoding.ASCII.GetBytes(content);
                }
                targetFileStream.Write(toWrite, 0, toWrite.Length);
            }
        }

        private Dictionary<string, long> dictionaryListing(CosmosVFS fs, string path)
        {
            Dictionary<string, long> listing = new Dictionary<string, long>();
            try
            {
                List<DirectoryEntry> rawListing = fs.GetDirectoryListing(path);
                foreach (var entry in rawListing)
                {
                    listing[entry.mName] = entry.mSize;
                }
            } catch (Exception e)
            {
                //debugger.Send(e.Message);
            }
            return listing;
        }

        private void Setup()
        {
            Console.WriteLine(" ___       _   __              / __     ( )  ___         ");
            Console.WriteLine("   / /   // ) )  ) ) //   / / //   ) ) / / //   ) ) \\\\ / /");
            Console.WriteLine("  / /   // / /  / / ((___/ / //   / / / / //   / /   \\/ /");
            Console.WriteLine(" / /__ // / /  / /      / / ((___/ / / / ((___( (    / /\\");
            Dictionary<string, long> systemTest = dictionaryListing(fs, "0:/system");
            if (systemTest.Count == 0)
            {
                Console.WriteLine("Przygotowywanie systemu...");
                debugger.Send("1");
                try
                {
                    fs.CreateDirectory("0:/system");
                } catch (Exception e)
                {
                    debugger.Send(e.Message);
                }
                fs.CreateFile(@"0:/system/startup.cmd");
                WriteFile(fs, @"0:/system/startup.cmd", "# Put your commands you want to run at startup here!");
                fs.CreateDirectory("0:/user");
                fs.CreateDirectory("0:/applications");
                fs.CreateDirectory("0:/config");
                fs.CreateDirectory("0:/services");
                Console.WriteLine("Utworz uzytkownika:");
                Console.Write("Nazwa uzytkownika: ");
                var username = Console.ReadLine();
                fs.CreateDirectory("0:/user/" + username);
                Console.WriteLine("Gotowe! System zostanie ponownie uruchomiony.");
                Cosmos.System.Power.Reboot();
            }
        }

        protected override void BeforeRun()
        {
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            Setup();
        }

        protected override void Run()
        {
            Console.Write("Input: ");
            var input = Console.ReadLine();
            Console.Write("Text typed: ");
            Console.WriteLine(input);
        }
    }
}
