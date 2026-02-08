using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shippery.Models.Database;

namespace Shippery.Controllers
{
    [Route("CurrencyController")]
    public class CurrencyController : Controller
    {
        [HttpGet("GetCurrencies")]
        public IEnumerable<Currency> GetCurrency() => Currency.currencies;

        [HttpGet("GetCurrency")]
        public Currency GetCurrency(string code) => Currency.GetCurrency(code);
    }
}
