using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Shippery.Controllers;
using MySql.Data.MySqlClient;
using Shippery.Models.Database;
using Shippery.Models.Resources;

namespace Shippery.Models.Basic
{
    public class Conversation
    {
        User from;
        User to;
        List<Message> messages;
        string newMessage;

        public Conversation() { messages = new List<Message>(); }
        public Conversation(User to, List<Message> messages):this()
        {
            To = to;
            Messages = messages;
        }
        public Conversation(User from, User to, List<Message> messages):this()
        {
            From = from;
            To = to;
            Messages = messages;
        }

        public List<Message> Messages { get => messages; set => messages = value; }
        public User From { get => from; set => from = value; }
        public User To { get => to; set => to = value; }
        public string NewMessage { get => newMessage; set => newMessage = value; }
    }
}
