using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shippery.Models.Database
{
    public class Reserve
    {
        Trip trip;
        int bond;
        string description;
        Int64 codeT;
        bool byCard;
        bool accepted;
        bool answered;
        bool paid;
        bool insured;
        bool recivedT;
        bool deliveredT;
        bool moneyGot;
        bool bondGot;
        bool valoratedC;
        bool valoratedT;

        public Reserve() { }

        public Trip Trip { get => trip; set => trip = value; }
        public int Bond { get => bond; set => bond = value; }
        public string Description { get => description; set => description = value; }
        public long CodeT { get => codeT; set => codeT = value; }
        public bool ByCard { get => byCard; set => byCard = value; }
        public bool Accepted { get => accepted; set => accepted = value; }
        public bool Answered { get => answered; set => answered = value; }
        public bool Paid { get => paid; set => paid = value; }
        public bool Insured { get => insured; set => insured = value; }
        public bool RecivedT { get => recivedT; set => recivedT = value; }
        public bool DeliveredT { get => deliveredT; set => deliveredT = value; }
        public bool MoneyGot { get => moneyGot; set => moneyGot = value; }
        public bool BondGot { get => bondGot; set => bondGot = value; }
        public bool ValoratedC { get => valoratedC; set => valoratedC = value; }
        public bool ValoratedT { get => valoratedT; set => valoratedT = value; }
    }
}
