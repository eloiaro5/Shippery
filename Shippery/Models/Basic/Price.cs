using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shippery.Models.Database;

namespace Shippery.Models.Basic
{
    public class Price
    {
        decimal cost;
        Currency currency;

        public Price(decimal cost, Currency currency)
        {
            Cost = cost;
            Currency = currency;
        }
        public Price(float cost, Currency currency):this(Convert.ToDecimal(cost), currency) { }

        public decimal Cost { get => cost; set => cost = value; }
        public Currency Currency { get => currency; set => currency = value; }

        public override string ToString()
        {
            return Cost.ToString() + currency.Symbol;
        }
        public string ToStringCode()
        {
            return Cost.ToString() + currency.Code;
        }
        public static Price SplitPrice(string cost)
        {
            string num = "0";
            string code = "";
            for (int i = 0; i < cost.Length; i++)
            {
                if (int.TryParse(cost[i].ToString(), out int n)) num += n.ToString();
                else if (cost[i] == ',') num += cost[i];
                else code += cost[i]; 
            }
            return new Price(Convert.ToDecimal(num), Currency.GetCurrency(code));
        }
    }
}
