using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shippery.Models.Database
{
    public partial class User
    {
        string username;
        string password;
        string mail;
        string image;
        Currency currency;
        string description;
        string vehicleDescription;
        string plate;
        bool paysBond;
        bool parental;
        bool cardPayer;
        bool sendNewSession;
        bool sendPromotion;

        public User() { }
        public User(string username, string password)
        {
            Username = username;
            this.password = password;
        }

        public string Username
        {
            get => username;
            set
            {
                username = value;
                //if (Controllers.MainController.CheckBannedWords(value)) throw new Controllers.MainController.BadWordException();
                //else username = value;
            }
        }
        public string Password { get => password; set => password = ToSha256(value); }
        public string Mail { get => mail; set => mail = value; }
        public string Image { get => image is null ? "" : image; set => image = value; }
        public Currency Currency { get => currency; set => currency = value; }
        public string CurrencyString { get => currency.ToString(); set => currency = Currency.GetCurrency(value); }
        public string Description { get => description; set => description = value; }
        public string VehicleDescription { get => vehicleDescription; set => vehicleDescription = value; }
        public string Plate { get => plate; set => plate = value; }
        public bool PaysBond { get => paysBond; set => paysBond = value; }
        public bool Parental { get => parental; set => parental = value; }
        public bool CardPayer { get => cardPayer; set => cardPayer = value; }
        public bool SendNewSession { get => sendNewSession; set => sendNewSession = value; }
        public bool SendPromotion { get => sendPromotion; set => sendPromotion = value; }
        public bool HasLoginFields { get => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password); }
        public bool HasRegisterFields { get => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(Mail); }
        public bool HasForgotFields { get => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Mail); }
        public bool IsMale { get => image is null ? false : image.Contains("male"); }

        public override bool Equals(object obj)
        {
            if (obj is User) return username.Equals(((User)obj).username);
            else if (obj is string) return username.Equals((string)obj);
            else if (obj is null) return string.IsNullOrEmpty(username);
            else return false;
        }

        public static bool operator==(User u1, User u2)
        {
            return u1.Equals(u2);
        }

        public static bool operator !=(User u1, User u2)
        {
            return !u1.Equals(u2);
        }

        public static bool operator ==(User u1, string u2)
        {
            return u1.Equals(u2);
        }

        public static bool operator !=(User u1, string u2)
        {
            return !u1.Equals(u2);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(username);
            hash.Add(password);
            hash.Add(mail);
            hash.Add(image);
            hash.Add(currency);
            hash.Add(description);
            hash.Add(vehicleDescription);
            hash.Add(plate);
            hash.Add(paysBond);
            hash.Add(parental);
            hash.Add(cardPayer);
            hash.Add(sendNewSession);
            hash.Add(sendPromotion);
            hash.Add(Username);
            hash.Add(Password);
            hash.Add(Mail);
            hash.Add(Image);
            hash.Add(Currency);
            hash.Add(CurrencyString);
            hash.Add(Description);
            hash.Add(VehicleDescription);
            hash.Add(Plate);
            hash.Add(PaysBond);
            hash.Add(Parental);
            hash.Add(CardPayer);
            hash.Add(SendNewSession);
            hash.Add(SendPromotion);
            hash.Add(HasLoginFields);
            hash.Add(HasRegisterFields);
            hash.Add(HasForgotFields);
            hash.Add(IsMale);
            return hash.ToHashCode();
        }

        #region Helper Methods
        static public string ToSha256(string rawData)
        {
            // Create a SHA256   
            using (System.Security.Cryptography.SHA256 sha256Hash = System.Security.Cryptography.SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        #endregion
    }
}
