using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Shippery.Models.Database;
using Shippery.Models.Resources;

namespace Shippery.Models.Views
{
    public class ReserveTripViewModel
    {
        Trip trip;
        int bond;
        string description;
        bool paysBond;

        public ReserveTripViewModel() { }

        public ReserveTripViewModel(int id, bool paysBond, DatabaseDelegate dd)
        {
            this.Trip = dd.GetTrip(id);
            this.paysBond = paysBond;
        }

        public ReserveTripViewModel(int id, int bond, string description, DatabaseDelegate dd)
        {
            this.Trip = dd.GetTrip(id);
            this.Bond = bond;
            this.Description = description;
        }

        public Trip Trip { get => trip; set => trip = value; }
        [DisplayName("Price")]
        [Required(ErrorMessage = "Bond required")]
        [RegularExpression(@"[0-9]{1,4}", ErrorMessage = "Bond need to:<br/>·\tBe an integer<br/>·\tHave 1 - 4 numbers")]
        [Description("Set the price the driver will pay as a bond in case of object miss")]
        public int Bond { get => bond; set => bond = value; }
        [DisplayName("Description")]
        [StringLength(4096, ErrorMessage = "Must have less than 4096 characters")]
        public string Description { get => description; set => description = value; }
        public bool PaysBond { get => paysBond; set => paysBond = value; }
    }
}
