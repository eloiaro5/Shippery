using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Shippery.Models.Basic;
using Shippery.Models.Database;
using Shippery.Models.Resources;

namespace Shippery.Models.Views
{
    public class TripViewModel
    {
        Trip trip;
        bool reserved;

        public TripViewModel() { }
        public TripViewModel(Trip trip):this(trip, false) { }
        public TripViewModel(Trip trip, bool reserved)
        {
            this.trip = trip;
            this.reserved = reserved;
        }

        public User User { get => trip.User; set => trip.User = value; }
        public Currency Currency { get => trip.Currency; set => trip.Currency = value; }
        public string PriceHelper { get => trip.Price % 0.10 >= 0.0999999999 ? trip.Price.ToString() + "0" : trip.Price.ToString(); set => trip.Price = (float)Convert.ToDouble(value.Replace(".", ",")); }
        public City SourceCity { get => trip.SourceCity; set => trip.SourceCity = value; }
        public string SourceString { get => SourceCity is null ? "" : SourceCity.ToString(); set => SourceCity = City.GetCityByJSON(value); }
        [StringLength(63, ErrorMessage = "Must have less than 63 characters")]
        public string SourceStreet { get => trip.SourceStreet; set => trip.SourceStreet = value; }
        public City DestinationCity { get => trip.DestinationCity; set => trip.DestinationCity = value; }
        public string DestinationString { get => DestinationCity is null ? "" : DestinationCity.ToString(); set => DestinationCity = City.GetCityByJSON(value); }
        [StringLength(63, ErrorMessage = "Must have less than 63 characters")]
        public string DestinationStreet { get => trip.DestinationStreet; set => trip.DestinationStreet = value; }
        public int TravelTime { get => trip.TravelTime; set => trip.TravelTime = value; }
        public DateTime Date { get => trip.Date; set => trip.Date = value; }
        public string Description { get => trip.Description; set => trip.Description = value; }
        public bool NeedsCard { get => trip.NeedsCard; set => trip.NeedsCard = value; }
        public bool PersonsAllowed { get => trip.PersonsAllowed; set => trip.PersonsAllowed = value; }
        public bool Reserved { get => reserved; set => reserved = value; }
    }

    public class TripsViewModel
    {
        public enum OrderEnum
        {
            Date,
            SourceStreet,
            DestinationStreet,
            Price
        }
        User? user;
        List<TripViewModel> trips;
        City source;
        City destination;
        Price max;
        Price min;
        DateTime before;
        DateTime after;
        OrderEnum order;

        public TripsViewModel()
        {
            trips = new List<TripViewModel>();

            if (max is null) max = new Price(9999.9999M, Currency.GetCurrency("EUR"));
            else max = new Price(9999.9999M, max.Currency);
            if (min is null) min = new Price(0.0100M, Currency.GetCurrency("EUR"));
            else min = new Price(0.0100M, min.Currency);
            before = DateTime.Today.AddMonths(1);
            after = DateTime.Now;

            //UpdateSession(session);

            //if (Parental) trip.Description = MainController.ChangeBannedWords(trip.Description);
            //List<Trip> trips = new List<Trip>();
            //trips.Add(trip);
            //Trips = trips;

            //UpdateReserve();
        }

        public TripsViewModel(List<Trip> trips):this()
        {
            foreach (Trip trip in trips) this.trips.Add(new TripViewModel(trip));
        }

        public TripsViewModel(TripsViewModel tvm, List<Trip> trips):this(trips)
        {
            user = tvm.user;
            source = tvm.source;
            destination = tvm.destination;
            max = tvm.max;
            min = tvm.min;
            before = tvm.before;
            after = tvm.after;
            order = tvm.order;
        }

        public TripsViewModel(User user, List<Trip> trips, string sessionId, DatabaseDelegate dd) : this()
        {
            foreach (Trip trip in trips) this.trips.Add(new TripViewModel(trip, dd.GetTripReserved(trip.Id, user.Username).Value));
            UpdateSession(sessionId, dd);
        }

        public User? User { get => user; set => user = value; }       
        public City SourceCity { get => source; set => source = value; }
        [StringLength(63, ErrorMessage = "Must have less than 63 characters")]
        public string SourceString { get => SourceCity is null ? "" : SourceCity.ToString(); set => SourceCity = City.GetCityByJSON(value); }
        public City DestinationCity { get => destination; set => destination = value; }
        [StringLength(63, ErrorMessage = "Must have less than 63 characters")]
        public string DestinationString { get => DestinationCity is null ? "" : DestinationCity.ToString(); set => DestinationCity = City.GetCityByJSON(value); }
        public Price Max { get => max; set => max = value; }
        [RegularExpression(@"[0-9]{1,4}[,\.][0-9]{0,4}[A-Z]{3}|[0-9]{1,4}[A-Z]{3}", ErrorMessage = "Price needs to have 1 - 4 numbers and 0 - 4 decimals and the currency code")]
        public string MaxHelper { get => max.ToStringCode(); set => max = Price.SplitPrice(value.Replace(".", ",")); }
        public Price Min { get => min; set => min = value; }
        [RegularExpression(@"[0-9]{1,4}[,\.][0-9]{0,4}[A-Z]{3}|[0-9]{1,4}[A-Z]{3}", ErrorMessage = "Price needs to have 1 - 4 numbers and 0 - 4 decimals and the currency code")]
        public string MinHelper { get => min.ToStringCode(); set => min = Price.SplitPrice(value.Replace(".", ",")); }
        public DateTime DateBefore { get => before; set => before = value; }
        public DateTime DateAfter { get => after; set => after = value; }
        [Browsable(true)]
        public string Order { get => Enum.GetName(typeof(OrderEnum), order); set => Enum.TryParse(value, out order); }
        public List<TripViewModel> Trips { get => trips; set => trips = value; }
        public bool HasTrips { get => trips != null && trips.Count > 0; }
        public bool HasSession { get => !(user is null); }

        /// <summary>
        /// Get the user preferences to manage the view model with them.
        /// </summary>
        /// <param name="sessionId">String saved inside DB that references the user.</param>
        public void UpdateSession(string sessionId, DatabaseDelegate dd)
        {
            User = dd.GetUser(dd.GetSessionUsername(sessionId));

            if (max != null) max.Currency = Currency.GetCurrency("EUR");
            if (min != null) min.Currency = Currency.GetCurrency("EUR");
        }
    }
}
