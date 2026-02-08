using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shippery.Models.Database
{
    public class UserValoration
    {
        int id;
        User user;
        int qualification;
        string comment;
        DateTime date;
        bool asCostumer;

        public UserValoration() { }

        public int Id { get => id; set => id = value; }
        public User User { get => user; set => user = value; }
        public int Qualification { get => qualification; set => qualification = value; }
        public string Comment { get => comment; set => comment = value; }
        public DateTime Date { get => date; set => date = value; }
        public bool AsCostumer { get => asCostumer; set => asCostumer = value; }
    }
}
