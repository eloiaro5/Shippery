using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Shippery.Models.Basic;

namespace Shippery.Models.Views
{
    public abstract class BaseViewModel
    {
        //string sessionId;
        //Currency currency;
        //bool parental;
        //bool cardPayer;

        //public string SessionId { get => sessionId; set => sessionId = value; }
        //[DisplayName("Parental protection")]
        //[Description("Protect yourself of bad words. This changes the text showed and can modificate the text to the point that it can't be understand.")]
        //public bool Parental { get => parental; set => parental = value; }
        //[DisplayName("Pays with card")]
        //[Description("The users accepts to pay with card if necessary. This allow the user to access only card trips.")]
        //public bool CardPayer { get => cardPayer; set => cardPayer = value; }
        //[DisplayName("Currency")]
        //[Description("Type of currency used for the transaction.")]
        //public Currency Currency { get => currency; set => currency = value; }
        //public string CurrencyString { get => currency is null ? "" : currency.ToString(); set => currency = Currency.GetCurrency(value); }
        //[Required(ErrorMessage = "Username required")]
        //[StringLength(15, MinimumLength = 3, ErrorMessage = "Must have between 3 - 15 characters")]
        //[DisplayName("Username")]

        //public bool HasSession => !string.IsNullOrEmpty(sessionId);

        //public void UpdateSession(string sessionId)
        //{           
        //    SessionId = sessionId;
        //    string username = Controllers.MainController.SessionUsername(sessionId);
        //    Parental = Controllers.MainController.HasParentalProtectionActivated(username);
        //    CardPayer = Controllers.MainController.PaysWithCard(username);
        //    Currency = Controllers.MainController.PrefferedCurrency(username);
        //}
    }
}
