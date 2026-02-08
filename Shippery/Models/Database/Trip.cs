using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MySql.Data.MySqlClient;

namespace Shippery.Models.Database
{
    public class Trip
    {
        int id;
        User user;
        City sourceCity;
        string sourceStreet;
        City destinationCity;
        string destinationStreet;
        float price;
        Currency currency;
        DateTime date;
        int travelTime;
        string description;
        bool needsCard;
        bool personsAllowed;
        bool full;

        public Trip() { }
        public Trip(Currency currency) { Currency = currency; }
        public Trip(City sourceCity, string sourceStreet, City destinationCity, string destinationStreet, float price, Currency currency, DateTime date)
        {
            SourceCity = sourceCity;
            SourceStreet = sourceStreet;
            DestinationCity = destinationCity;
            DestinationStreet = destinationStreet;
            Price = price;
            Currency = currency;
            Date = date;
        }

        public int Id { get => id; set => id = value; }
        public User User { get => user; set => user = value; }
        public City SourceCity { get => sourceCity; set => sourceCity = value; }
        public string SourceStreet { get => sourceStreet; set => sourceStreet = value; }
        public City DestinationCity { get => destinationCity; set => destinationCity = value; }
        public string DestinationStreet { get => destinationStreet; set => destinationStreet = value; }
        public float Price { get => price; set => price = value; }
        public Currency Currency { get => currency; set => currency = value; }
        public string CurrencyString { get => currency.ToString(); set => currency = Currency.GetCurrency(value); }
        public DateTime Date { get => date; set => date = value; }
        public int TravelTime { get => travelTime; set => travelTime = value; }
        public DateTime ArrivalDate { get => date.AddMinutes(travelTime); }
        public string TravelTimeHour { get => (travelTime / 60 < 10 ? "0" + travelTime / 60 : "" + travelTime / 60) + ":" + ((travelTime % 60).ToString().Length == 1 ? travelTime % 60 + "0" : "" + travelTime % 60); }
        public string Description { get => description; set => description = value; }
        public bool NeedsCard { get => needsCard; set => needsCard = value; }
        public bool PersonsAllowed { get => personsAllowed; set => personsAllowed = value; }
        public bool Full { get => full; set => full = value; }
    }
}
