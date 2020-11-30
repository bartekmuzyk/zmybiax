using System;
using System.Collections.Generic;
using System.Text;

namespace Zmybiax
{
    public static class Builtins
    {
        public static void reg()
        {
            Console.WriteLine("Narzedzie rejestru Zmybiax");
            bool RUN = true;
            while (RUN)
            {
                Console.Write("klucz lub 'help': ");
                string key = Console.ReadLine();

                if (key == "help")
                {
                    Console.WriteLine(
                        "Komendy:\n " +
                        "quit - Zamyka narzedzie\n " +
                        "add <klucz> <wartosc> <typ> - Dodaje wartosc do rejestru o typie Number, Boolean lub String"
                    );
                }
                else if (key.Split(' ')[0] == "add")
                {
                    string[] addTokens = key.Split(' ');
                    Type entryType = typeof(string);
                    if (addTokens.Length < 4) Console.WriteLine("Wprowadz wszystkie wartosci");
                    else
                    {
                        switch (addTokens[3])
                        {
                            case "Number":
                                if (addTokens[2].Contains(".")) entryType = typeof(float);
                                else entryType = typeof(int);
                                break;
                            case "Boolean":
                                entryType = typeof(bool);
                                break;
                            case "String":
                                entryType = typeof(string);
                                break;
                        }
                        Registry.Storage.AddEntry(addTokens[1], entryType, addTokens[2]);
                    }
                }
                else if (Registry.Storage.KeyExists(key))
                {
                    Registry.Entry got = Registry.Storage.GetEntry(key);

                    string type;
                    if (got.EntryType == typeof(int) || got.EntryType == typeof(float)) type = "Number";
                    else if (got.EntryType == typeof(bool)) type = "Boolean";
                    else if (got.EntryType == typeof(string)) type = "String";
                    else type = "Nieznany";

                    Console.WriteLine($"Typ: {type}");
                    Console.WriteLine($"Wartosc: {got.GetString()}");
                }
                else if (key == "quit") RUN = false;
                else Console.WriteLine($"Klucz '{key}' nie istnieje.");
            }
        }
    }
}
