using System;
using System.Collections.Generic;
using System.Text;

namespace Zmybiax.Registry
{
    public static class Storage
    {
        private static Dictionary<string, Entry> storage = new Dictionary<string, Entry>();

        public static void AddEntry(string key, Type entryType, string value)
        {
            storage[key] = new Entry(entryType, value);
        }

        public static void AddEntry(string key, Entry entry)
        {
            storage[key] = entry;
        }

        public static Entry GetEntry(string key)
        {
            return storage[key];
        }

        public static bool KeyExists(string key)
        {
            return storage.ContainsKey(key);
        }
    }
}
