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
using CGUI;

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
            Console.WriteLine("                      ___.   .__               ");
            Console.WriteLine("________ _____ ___.__.\\_ |__ |__|____  ___  ___");
            Console.WriteLine("\\___   //     <   |  | | __ \\|  \\__  \\ \\  \\/  /");
            Console.WriteLine(" /    /|  Y Y /\\___  | | \\_\\ \\  |/ __ \\_>    < ");
            Console.WriteLine("/_____ \\__|_|  / ____| |___  /__(____  /__/\\_ \\");
            Console.WriteLine("      \\/     \\/\\/          \\/        \\/      \\/");
            bool systemTest = fs.DirectoryExists(@"0:\system");
            //bool systemTest = false;
            if (!systemTest)
            {
                fs.CreateDirectory(@"0:\system");
                Console.WriteLine("Przygotowywanie systemu...");
                fs.CreateFile(@"0:\system\startup.cmd");
                fs.WriteFile(@"0:\system\startup.cmd", "# Put your commands you want to run at startup here!");
                fs.CreateDirectory(@"0:\user");
                fs.CreateDirectory(@"0:\applications");
                fs.CreateDirectory(@"0:\config");
                fs.CreateDirectory(@"0:\services");
                Console.WriteLine("Utworz uzytkownika:");
                Console.Write("Nazwa uzytkownika: ");
                var username = Console.ReadLine();
                fs.CreateDirectory(@"0:\user\" + username);
                Console.WriteLine("Gotowe! System zostanie ponownie uruchomiony.");
                Cosmos.System.Power.Reboot();
            }
            else
            {
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
                    foreach (DirectoryEntry user in users)
                    {
                        Console.WriteLine(" * " + user.mName);
                    }
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

                        if (!found)
                        {
                            Console.WriteLine("Uzytkownik " + username + " nie istnieje.");
                        }
                        else
                        {
                            user = username;
                            userPath = new string[] { "0:", "user", user };
                            path = userPath;
                            retry = false;
                        }
                    }
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

        private void interpretCmd(string cmd)
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
                        if (choice == "Y" || choice == "y")
                        {
                            Factory();
                        }
                        break;
                    case "ls":
                        if (tokens.Length > 1)
                        {
                            fs.GetDirectoryListing(tokens[1]).DisplayEntries(tokens[1]);
                        }
                        else
                        {
                            fs.GetDirectoryListing(path.StringifyPath()).DisplayEntries(path.StringifyPath());
                        }
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
                                    if (path.Length > 1) { path = path.Pop(); }
                                    else { Console.WriteLine("Brak nadrzednego folderu."); }
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
                        else
                        {
                            path = userPath;
                        }
                        break;
                    case "loc":
                        if (tokens.Length > 1)
                        {
                            if (tokens[1].ToCharArray()[1] == ':')
                            {
                                //The path is already absolute
                                Console.WriteLine(tokens[1]);
                            }
                            else
                            {
                                switch (tokens[1])
                                {
                                    case "..":
                                        string[] parentDir = path.Pop();
                                        Console.WriteLine(parentDir.StringifyPath());
                                        break;
                                    case ".":
                                        Console.WriteLine(path.StringifyPath());
                                        break;
                                    default:
                                        if (tokens[1].StartsWith('\\'))
                                        {
                                            Console.WriteLine("Zla sciezka.");
                                        }
                                        else
                                        {
                                            string pathToCheck = path.StringifyPath() + tokens[1];
                                            if (fs.DirectoryExists(pathToCheck) || fs.FileExists(pathToCheck))
                                            {
                                                Console.WriteLine(pathToCheck);
                                            }
                                            else
                                            {
                                                Console.WriteLine("Plik/folder " + pathToCheck + " nie istnieje.");
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("loc - Wyswietla pelna sciezke do folderu/pliku.");
                            Console.WriteLine("Uzycie: loc <relatywa/sciezka/do/elementu>");
                        }
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
                            Console.WriteLine("power - Wylacz lub zresetuj komputer.");
                            Console.WriteLine("Uzycie: power [OPCJE]");
                            Console.WriteLine("Opcje:");
                            Console.WriteLine("\t-s - Wylacza komputer");
                            Console.WriteLine("\t-r - Restartuje komputer");
                        }
                        break;
                    case "wm":
                        Console.WriteLine("Uruchamianie sesji...");
                        VGADriver driver = new VGADriver();
                        WindowManager wm = new WindowManager(driver);
                        wm.Init();
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
                interpretCmd(cmd);
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
