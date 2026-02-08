using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace Shippery.Models.Database
{
    public class Notification
    {
        string message;
        DateTime date;
        bool watched;
        int associatedId;

        public string Message
        { 
            get => message; 
            set
            {
                Regex id = new Regex("@[\\w\\.]*");
                Console.WriteLine(id.Match(value).Value.Replace("@", ""));
                if (id.Match(value).Success) associatedId = Convert.ToInt32(id.Match(value).Value.Replace("@", ""));

                Regex name = new Regex("#\\w*");
                Regex rgx = new Regex("{{.*}}");
                message = rgx.Replace(value, name.Match(value).Value.Replace("#", ""));
            }
        }
        public DateTime Date { get => date; set => date = value; }
        public bool Watched { get => watched; set => watched = value; }
        public int AssociatedId { get => associatedId; set => associatedId = value; }
    }
}
