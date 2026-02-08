using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MySql.Data.MySqlClient;
using Shippery.Models.Basic;
using Shippery.Models.Database;
using Shippery.Models.Resources;

namespace Shippery.Models.Views
{
    public class UserViewModel
    {
        User user;
        List<Trip> trips;
        List<Reserve> reserves;
        List<UserValoration> valorations;
        Dictionary<User, Conversation> chats;
        string password;
        bool visiting;

        public UserViewModel()
        {
            user = new User();
            trips = new List<Trip>();
            reserves = new List<Reserve>();
            Valorations = new List<UserValoration>();
            chats = new Dictionary<User, Conversation>();
        }
        public UserViewModel(User user) { this.user = user; }
        public UserViewModel(string sessionID, DatabaseDelegate dd, bool visiting = false) : this(dd.GetUser(dd.GetSessionUsername(sessionID)), dd, visiting) { }
        public UserViewModel(User user, DatabaseDelegate dd, bool visiting = false) : this()
        {
            this.visiting = visiting;
            this.user = user;
            trips = dd.GetTrip(user.Username);

            if (!visiting)
            {
                reserves = dd.GetReserve(user.Username);
                valorations = dd.GetValoration(user.Username);
            }
            foreach (User u in dd.GetActiveChats(user.Username))
            {
                chats.Add(u, new Conversation(u, dd.GetConversationMessages(user.Username, u.Username, parental: user.Parental)));
            }
        }

        //User properties
        public User Root { get => user; }

        [Required(ErrorMessage = "Username required")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Must have between 3 - 15 characters")]
        public string Username { get => user.Username; set => user.Username = value; }
        [Required(ErrorMessage = "Password required")]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "Must have between 8 - 255 characters")]
        public string UserPassword { get => user.Password; set => user.Password = value; }
        [Required(ErrorMessage = "Email required")]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "Must have between 8 - 255 characters")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Is not a valid email")]
        public string Mail { get => user.Mail; set => user.Mail = value; }
        public string Image { get => string.IsNullOrEmpty(user.Image) ? "" : user.Image.Split('/')[user.Image.Split('/').Length - 1].Split('.')[0].Replace("-icon", ""); }
        public string ImageSRC { get => user.Image; set => user.Image = "assets/user-icon/" + value + ".png"; }
        [StringLength(65536, ErrorMessage = "Must have less than 65536 characters")]
        public string Description { get => user.Description; set => user.Description = value; }
        [StringLength(65536, ErrorMessage = "Must have less than 65536 characters")]
        public string VehicleDescription { get => user.VehicleDescription; set => user.VehicleDescription = value; }
        [Required(ErrorMessage = "Plate number required")]
        public string Plate { get => user.Plate; set => user.Plate = value; }
        public Currency Currency { get => user.Currency; set => user.Currency = value; }
        [Description("You accept to pay a bond if necessary to ensure the costumer that his product will not be broken or lost.")]
        public bool PaysBond { get => user.PaysBond; set => user.PaysBond = value; }
        [Description("You will be protected from bad words.")]
        public bool Parental { get => user.Parental; set => user.Parental = value; }
        [Description("You accept to pay with credit card, if necessary.")]
        public bool CardPayer { get => user.CardPayer; set => user.CardPayer = value; }
        [Description("You will recieve email notifications about all new sessions that are created.")]
        public bool SendNewSession { get => user.SendNewSession; set => user.SendNewSession = value; }
        [Description("You will be notified whenever a new promotion takes place .")]
        public bool SendPromotion { get => user.SendPromotion; set => user.SendPromotion = value; }
        [Description("Check if the user is a male.")]
        public bool IsMale { get => user.IsMale; }

        //Trips properties
        public Dictionary<DateTime, List<Trip>> TripsByDate { get => trips.GroupBy(t => t.Date.Date).ToDictionary(g => g.Key, g => g.ToList()).OrderBy(d => d.Key).ToDictionary(d => d.Key, d => d.Value); }
        public Dictionary<City, List<Trip>> TripsBySource { get => trips.GroupBy(t => t.SourceCity).ToDictionary(g => g.Key, g => g.ToList()).OrderBy(d => d.Key).ToDictionary(d => d.Key, d => d.Value); }

        //Chats properties
        public Dictionary<User, Conversation> Chats { get => chats; set => chats = value; }

        //This properties
        [Description("List of valorations done by the costumers.")]
        public List<UserValoration> Valorations { get => valorations; set => valorations = value; }
        [Description("The user is checking other user's profile.")]
        public bool Visiting { get => visiting; set => visiting = value; }
        [Description("The password to check when updating the user.")]
        public string Password { get => password; set => password = User.ToSha256(value); }
        [Description("Check if the password and the user passoword are the same.")]
        public bool CorrectPassword { get => Password == UserPassword; }
        [Description("Check if the ViewModel has password indicating that updates had been done.")]
        public bool HasPassword { get => !string.IsNullOrEmpty(password); }
        public bool HasBeenValorated { get => Valorations is null ? false : Valorations.Where(v => v.AsCostumer).Count() > 0 || Valorations.Where(v => !v.AsCostumer).Count() > 0; }
        public bool HasBeenCostumerValorated { get => Valorations is null ? false : Valorations.Where(v => v.AsCostumer).Count() > 0; }
        public bool HasBeenTransporterValorated { get => Valorations is null ? false : Valorations.Where(v => !v.AsCostumer).Count() > 0; }
        public bool HasLoginFields { get => user.HasLoginFields; }
        public bool HasRegisterFields { get => user.HasRegisterFields; }
        public bool HasForgotFields { get => user.HasForgotFields; }
        public bool HasUser { get => !(user is null); }
        public int AverageValoration 
        {
            get
            {
                if (Valorations is null) return 0;
                else
                {
                    int avg = 0;
                    int added = 0;
                    if (Valorations.Where(v => v.AsCostumer).Count() > 0)
                    {
                        avg += (int)Math.Round(Valorations.Where(v => !v.AsCostumer).Average(x => x.Qualification));
                        added++;
                    }
                    if (Valorations.Where(v => !v.AsCostumer).Count() > 0)
                    {
                        avg += (int)Math.Round(Valorations.Where(v => !v.AsCostumer).Average(x => x.Qualification));
                        added++;
                    }
                    return added == 2 ? avg / 2 : avg;
                }
            }
        }
        public int AverageCostumerValoration
        {
            get
            {
                if (Valorations is null) return 0;
                else
                {
                    int avg = 0;
                    if (Valorations.Where(v => v.AsCostumer).Count() > 0) avg = (int)Math.Round(Valorations.Where(v => v.AsCostumer).Average(x => x.Qualification));
                    return avg;
                }
            }
        }
        public int AverageTransporterValoration
        {
            get
            {
                if (Valorations is null) return 0;
                else
                {
                    int avg = 0;
                    if (Valorations.Where(v => !v.AsCostumer).Count() > 0) avg = (int)Math.Round(Valorations.Where(v => !v.AsCostumer).Average(x => x.Qualification));
                    return avg;
                }
            }
        }
    }
}
