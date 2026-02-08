using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shippery.Models.Database;
using Shippery.Models.Responses;

namespace Shippery.Controllers
{
    [Route("CityController")]
    public class CityController : Controller
    {
        [HttpPost("GetCities")]
        public CityResponse GetCities()
        {
            if (City.cities.Count > 0) return new CityResponse(true, City.cities.ToArray());
            else return new CityResponse(false, "The list is empty!");
        }

        [HttpPost("GetMatchings")]
        public CityResponse GetCities(string match)
        {
            try
            {
                if (string.IsNullOrEmpty(match)) Console.WriteLine("Match is Empty!");
                else
                {
                    Console.WriteLine("Cities with: '" + match + "' -> ");
                    foreach (City c in City.cities.Where(c => c.Ascii.StartsWith(match)).ToArray())
                    {
                        Console.Write(c.Name + ", ");
                    }
                }
                if (match is null) return new CityResponse(false, "Value cannot be null");
                else if (match == "") return new CityResponse(true, City.cities.GetRange(0, 10));
                else if (City.cities.Count > 0) return new CityResponse(true, City.cities.Where(c => c.Ascii.StartsWith(match)).ToArray());
                else return new CityResponse(false, "The list is empty!");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new CityResponse(false, e.StackTrace);
            }
        }
    }
}
