using Shippery.Controllers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Shippery.Models.Database;
using Shippery.Models.Resources;

namespace Shippery.Models.Views
{
    public class ReserveViewModel
    {
        User user;
        List<Reserve> reservesRecived;
        List<Reserve> reservesSent;
        int bond;
        string description;
        bool answered;
        bool accepted;
        bool paid;

        public ReserveViewModel() { }
        public ReserveViewModel(string sessionid, DatabaseDelegate dd)
        {
            Iterator i = new Iterator();
            User = dd.GetUser(dd.GetSessionUsername(sessionid));

            //Reserves sent
            ReservesSent = new List<Reserve>();

            string sql = "SELECT * FROM reserve WHERE user_username = @user_username ORDER BY trip_id DESC";
            MySqlCommand cmd = new MySqlCommand(sql, dd.GetSqlConnection());
            cmd.Parameters.AddWithValue("@user_username", User.Username);

            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    i.Reset();
                    Reserve r = new Reserve();

                    r.Trip = dd.GetTrip(dr.GetInt32(i.Iterate()));
                    r.Bond = dr.GetInt32(i.Iterate(2));
                    r.Description = dr.IsDBNull(i.Iterate()) ? "" : dr.GetString(i.Position);
                    //if (User.Parental) r.Description = dr.IsDBNull(i.Iterate()) ? "" : MainController.ChangeBannedWords(dr.GetString(i.Position));
                    //else r.Description = dr.IsDBNull(i.Iterate()) ? "" : dr.GetString(i.Position);
                    r.CodeT = dr.GetInt64(i.Iterate());
                    r.ByCard = dr.GetBoolean(i.Iterate());
                    r.Answered = dr.GetBoolean(i.Iterate());
                    r.Accepted = dr.GetBoolean(i.Iterate());
                    r.Paid = dr.GetBoolean(i.Iterate());
                    r.Insured = dr.GetBoolean(i.Iterate());
                    r.RecivedT = dr.GetBoolean(i.Iterate());
                    r.DeliveredT = dr.GetBoolean(i.Iterate());
                    r.ValoratedC = dr.GetBoolean(i.Iterate(3));
                    r.ValoratedT = dr.GetBoolean(i.Iterate());

                    ReservesSent.Add(r);
                }
            }

            //Reserves recived
            ReservesRecived = new List<Reserve>();

            sql = "SELECT r.* FROM reserve r JOIN trip t WHERE r.trip_id IN (SELECT t.id FROM trip WHERE t.user_username = @user_username)";
            cmd = new MySqlCommand(sql, dd.GetSqlConnection());
            cmd.Parameters.AddWithValue("@user_username", User.Username);

            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    i.Reset();
                    Reserve r = new Reserve();

                    r.Trip = dd.GetTrip(dr.GetInt32(i.Iterate()));
                    r.Bond = dr.GetInt32(i.Iterate(2));
                    r.Description = dr.IsDBNull(i.Iterate()) ? "" : dr.GetString(i.Position);
                    //if (user.Parental) r.Description = dr.IsDBNull(i.Iterate()) ? "" : MainController.ChangeBannedWords(dr.GetString(i.Position));
                    //else r.Description = dr.IsDBNull(i.Iterate()) ? "" : dr.GetString(i.Position);
                    r.CodeT = dr.GetInt64(i.Iterate());
                    r.ByCard = dr.GetBoolean(i.Iterate());
                    r.Answered = dr.GetBoolean(i.Iterate());
                    r.Accepted = dr.GetBoolean(i.Iterate());
                    r.Paid = dr.GetBoolean(i.Iterate());
                    r.Insured = dr.GetBoolean(i.Iterate());
                    r.RecivedT = dr.GetBoolean(i.Iterate());
                    r.DeliveredT = dr.GetBoolean(i.Iterate());
                    r.ValoratedC = dr.GetBoolean(i.Iterate(3));
                    r.ValoratedT = dr.GetBoolean(i.Iterate());

                    ReservesRecived.Add(r);
                }
            }
        }

        public User User { get => user; set => user = value; }
        [DisplayName("Bond")]
        public int Bond { get => bond; set => bond = value; }
        [DisplayName("Description")]
        public string Description { get => description; set => description = value; }
        [DisplayName("Accepted")]
        public bool Accepted { get => accepted; set => accepted = value; }
        public List<Reserve> ReservesRecived { get => reservesRecived; set => reservesRecived = value; }
        public List<Reserve> ReservesSent { get => reservesSent; set => reservesSent = value; }
        public bool Answered { get => answered; set => answered = value; }
        public bool Paid { get => paid; set => paid = value; }
        public double TotalCost => ReservesSent.Where(r => r.Accepted && !r.Paid && r.ByCard && r.Trip.Currency == user.Currency && r.Trip.Date > DateTime.Now).Sum(r => r.Trip.Price) + ReservesRecived.Where(r => r.Accepted && !r.Insured && r.ByCard && r.Trip.Currency == user.Currency && r.Trip.Date > DateTime.Now).Sum(r => r.Bond);
        public bool HasCurrencyMissmatch => ReservesSent.Where(r => r.Accepted && !r.Paid && r.ByCard && r.Trip.Currency != user.Currency && r.Trip.Date > DateTime.Now).Sum(r => r.Trip.Price) + ReservesRecived.Where(r => r.Accepted && !r.Insured && r.ByCard && r.Trip.Currency != user.Currency && r.Trip.Date > DateTime.Now).Sum(r => r.Bond) > 0;
    }
}
