using Cosmos.Core.IOGroup;
using Cosmos.Debug.Kernel;
using Cosmos.HAL;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.Listing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Sys = Cosmos.System;

namespace Zmybiax
{
    public class Kernel : Sys.Kernel
    {
        CosmosVFS fs = new Sys.FileSystem.CosmosVFS();
        //Debugger debugger = new Debugger("", "");
        public string[] path;
        public string user;

        private void Setup()
        {
            Console.WriteLine("                      ___.   .__               ");
            Console.WriteLine("________ _____ ___.__.\\_ |__ |__|____  ___  ___");
            Console.WriteLine("\\___   //     <   |  | | __ \\|  \\__  \\ \\  \\/  /");
            Console.WriteLine(" /    /|  Y Y /\\___  | | \\_\\ \\  |/ __ \\_>    < ");
            Console.WriteLine("/_____ \\__|_|  / ____| |___  /__(____  /__/\\_ \\");
            Console.WriteLine("      \\/     \\/\\/          \\/        \\/      \\/");
            bool systemTest = fs.DirectoryExists("0:/system");
            if (!systemTest)
            {
                fs.CreateDirectory("system");
                Console.WriteLine("Przygotowywanie systemu...");
                fs.CreateFile("system/startup.cmd");
                fs.WriteFile("system/startup.cmd", "# Put your commands you want to run at startup here!");
                fs.CreateDirectory("user");
                fs.CreateDirectory("applications");
                fs.CreateDirectory("config");
                fs.CreateDirectory("services");
                Console.WriteLine("Utworz uzytkownika:");
                Console.Write("Nazwa uzytkownika: ");
                var username = Console.ReadLine();
                fs.CreateDirectory("user/" + username);
                Console.WriteLine("Gotowe! System zostanie ponownie uruchomiony.");
                Cosmos.System.Power.Reboot();
            }
            else
            {
                List<DirectoryEntry> users = new List<DirectoryEntry>();
                try
                {
                    users = fs.GetDirectoryListing("0:/user");
                }
                catch (Exception e)
                {
                    fs.CreateDirectory("user"); 
                }
                if (users.Count == 0)
                {
                    Console.WriteLine("Nie znaleziono zadnych uzytkownikow. Zostales zalogowany na tymczasowe konto.");
                    user = "anon";
                    path = new string[] { "0:" };
                }
                else
                {
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
                            path = new string[] { "0:", "user", user };
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
            Console.WriteLine("Resetowanie do ustawien fabrycznych...");
            //TODO
            //Cosmos.System.Power.Shutdown();
        }

        private void interpretCmd(string cmd)
        {
            string[] tokens = cmd.Split(' ');
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
                                    path = new string[] { "user", user };
                                    break;
                                case "..":
                                    path = path.Pop();
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
                                        Console.WriteLine("Katalog " + tokens[1] + " nie istnieje");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            path = new string[] { "user", user };
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
                                        if (tokens[1].StartsWith('/'))
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
                            switch (tokens[1])
                            {
                                case "-s":
                                    Sys.Power.Shutdown();
                                    break;
                                case "-r":
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
                            Console.WriteLine("\t-r - Resetuje komputer");
                        }
                        break;
                    default:
                        Console.Write(tokens[0]);
                        Console.Write(" nie jest poleceniem ani programem wykonywalnym." + Environment.NewLine);
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
