using Cosmos.Core.IOGroup;
using Cosmos.Debug.Kernel;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.Listing;
using Cosmos.System.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using Sys = Cosmos.System;
using IL2CPU.API.Attribs;
using Zmybiax.Graphics;

namespace Zmybiax
{
    public class Kernel : Sys.Kernel
    {
        CosmosVFS fs = new Sys.FileSystem.CosmosVFS();
        Cosmos.Debug.Kernel.Debugger debugger = new Cosmos.Debug.Kernel.Debugger("", "");
        public string[] path;
        public string user;
        public string[] userPath;

        private void Setup()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("                      ___.   .__               ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("________ _____ ___.__.\\_ |__ |__|____  ___  ___");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\\___   //     <   |  | | __ \\|  \\__  \\ \\  \\/  /");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" /    /|  Y Y /\\___  | | \\_\\ \\  |/ __ \\_>    < ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("/_____ \\__|_|  / ____| |___  /__(____  /__/\\_ \\");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("      \\/     \\/\\/          \\/        \\/      \\/");
            Console.ForegroundColor = ConsoleColor.White;
            
            bool userTest = fs.DirectoryExists(@"0:\user");
            if (!userTest)
            {
                Console.WriteLine("Nie znaleziono zadnych uzytkownikow. Zostales zalogowany na tymczasowe konto.");
                user = "anon";
                path = new string[] { "0:" };
            }
            else
            {
                List<DirectoryEntry> users = fs.GetDirectoryListing(@"0:\user");
                Console.WriteLine("Uzytkownicy:");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                foreach (DirectoryEntry user in users) Console.WriteLine(" * " + user.mName);
                Console.ForegroundColor = ConsoleColor.White;

                bool retry = true;
                while (retry)
                {
                    Console.Write("Nazwa uzytkownika: ");
                    var username = Console.ReadLine();
                    bool found = false;
                    foreach (DirectoryEntry userDir in users)
                    {
                        if (userDir.mName == username && userDir.mEntryType == DirectoryEntryTypeEnum.Directory)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                    {
                        user = username;
                        userPath = new string[] { "0:", "user", user };
                        path = userPath;
                        retry = false;
                    } else Console.WriteLine("Uzytkownik " + username + " nie istnieje.");
                }
            }
        }

        private void Factory()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Przywracanie systemu do ustawien fabrycznych...");
            Console.WriteLine("NIE WYLACZAJ KOMPUTERA");
            fs.DeleteDirectoryRecursively(@"0:\");
            Cosmos.System.Power.Shutdown();
        }

        private void InterpretCommandLine(string cmd)
        {
            string[] tokens = cmd.Split(' ');
            char[] flags = cmd.GetFlags('-');
            if (tokens.Length != 0)
            {
                switch (tokens[0])
                {
                    case "factory":
                        Console.Write("UWAGA! Ta operacja zniszczy wszystkie pliki konfiguracyjne oraz dokumenty uzytkownika! Jestes pewien? (Y/n): ");
                        var choice = Console.ReadLine();
                        if (choice == "Y" || choice == "y") Factory();
                        break;
                    case "ls":
                        if (tokens.Length > 1)
                            fs.GetDirectoryListing(tokens[1]).DisplayEntries(tokens[1]);
                        else
                            fs.GetDirectoryListing(path.StringifyPath()).DisplayEntries(path.StringifyPath());
                        break;
                    case "cd":
                        if (tokens.Length > 1)
                        {
                            switch (tokens[1])
                            {
                                case "~":
                                    path = userPath;
                                    break;
                                case "..":
                                    if (path.Length > 1) path = path.Pop();
                                    else Console.WriteLine("Brak nadrzednego folderu.");
                                    break;
                                case ".":
                                    break;
                                default:
                                    string[] newPath = path.AddToArray(tokens[1]);
                                    if (fs.DirectoryExists(newPath.StringifyPath()))
                                    {
                                        path = newPath;
                                    }
                                    else if (fs.FileExists(newPath.StringifyPath()))
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine(tokens[1] + " jest plikiem.");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Katalog " + tokens[1] + " nie istnieje.");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    break;
                            }
                        }
                        else path = userPath;
                        break;
                    case "power":
                        if (tokens.Length > 1)
                        {
                            switch (flags[0])
                            {
                                case 's':
                                    Sys.Power.Shutdown();
                                    break;
                                case 'r':
                                    Sys.Power.Reboot();
                                    break;
                                default:
                                    Console.WriteLine("Niepoprawna opcja " + tokens[1]);
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine(
                                "power - Wylacz lub zresetuj komputer.\n" +
                                "Uzycie: power [OPCJE]\n" +
                                "Opcje:\n" +
                                "\t-s - Wylacza komputer\n" +
                                "\t-r - Restartuje komputer"
                                );
                        }
                        break;
                    case "zwm":
                        Console.WriteLine("Uruchamianie sesji...");
                        WindowManager wm = new WindowManager();
                        wm.Init();
                        break;
                    case "reg":
                        Builtins.reg();
                        break;
                    case "mousetest":
                        Builtins.mousetest();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(tokens[0]);
                        Console.WriteLine(" nie jest poleceniem ani programem wykonywalnym.");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
            }
        }

        protected override void BeforeRun()
        {
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            Setup();
        }

        protected override void Run()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(user);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("|");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(path.StringifyPath());
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" >>");
                var cmd = Console.ReadLine();
                InterpretCommandLine(cmd);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wystapil blad");
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
