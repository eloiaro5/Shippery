using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shippery.Models.Database
{
    public class Message
    {
        User from;
        User to;
        string msg;
        DateTime date;
        bool watched;

        public Message(User from, User to, string message, DateTime date, bool watched)
        {
            From = from;
            To = to;
            Msg = message;
            Date = date;
            Watched = watched;
        }

        public Message(User to, string message, DateTime date, bool watched)
        {
            To = to;
            Msg = message;
            Date = date;
            Watched = watched;
        }

        public User From { get => from; set => from = value; }
        public User To { get => to; set => to = value; }
        public string Msg { get => msg; set => msg = value; }
        public DateTime Date { get => date; set => date = value; }
        public bool Watched { get => watched; set => watched = value; }
    }
}
