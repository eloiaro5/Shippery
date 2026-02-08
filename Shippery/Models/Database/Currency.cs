using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Shippery.Models.Database
{
    public class Currency
    {
        public static List<Currency> currencies = new List<Currency>();
        string name;
        string code;
        char symbol;

        public Currency() { }

        public Currency(string name, string code, char symbol)
        {
            Name = name;
            Code = code;
            Symbol = symbol;
        }

        public string Name { get => name; set => name = value; }
        public string Code { get => code; set => code = value; }
        public char Symbol { get => symbol; set => symbol = value; }
        public Currency Default { get => GetCurrency("EUR"); }

        public override string ToString()
        {
            return Name + " (" + Code + "): " + Symbol;
        }


        public static Currency GetCurrency(string code) => currencies.Where(c => c.Code == code).First();

        public IEnumerator GetEnumerator()
        {
            return currencies.GetEnumerator();
        }
    }
}
