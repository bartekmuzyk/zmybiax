using System;
using System.Collections.Generic;
using System.Text;

namespace Zmybiax.Registry
{
    public class Entry
    {
        public Type EntryType;
        private string value;

        public Entry(Type type, string value)
        {
            this.EntryType = type;
            this.value = value;
        }

        public string GetString() { return value; }

        public int GetInteger() { return int.Parse(this.value); }

        public bool GetBoolean() { return this.value == "true"; }
    }
}
