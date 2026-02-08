using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Shippery.Models.Database
{
    public enum CityType
    {
        primary,
        administrative,
        minor
    }
    public class City : IEnumerable
    {
        public static List<City> cities = new List<City>();

        int id;
        string name;
        string ascii;
        double latitude;
        double longitude;
        string country;
        string iso2;
        string iso3;
        string adminName;
        CityType type;
        int population;

        public City() { id = -1; }
        public City(string name):this() { this.name = name; }
        public City(int id, string name) : this(name) { this.id = id; }
        public City(int id, string name, string ascii, double latitude, double longitude, string country, string iso2, string iso3, string adminName, CityType type, int population)
        {
            Id = id;
            Name = name;
            Ascii = ascii;
            Latitude = latitude;
            Longitude = longitude;
            Country = country;
            Iso2 = iso2;
            Iso3 = iso3;
            AdminName = adminName;
            Type = type;
            Population = population;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Ascii { get => ascii; set => ascii = value; }
        public double Latitude { get => latitude; set => latitude = value; }
        public double Longitude { get => longitude; set => longitude = value; }
        public string Country { get => country; set => country = value; }
        public string Iso2 { get => iso2; set => iso2 = value; }
        public string Iso3 { get => iso3; set => iso3 = value; }
        public string AdminName { get => adminName; set => adminName = value; }
        public CityType Type { get => type; set => type = value; }
        public int Population { get => population; set => population = value; }

        public override string ToString()
        {
            if (Id == -1) return "";
            else return Ascii + ", " + AdminName + ", " + Country;
        }

        public static string Cities2JSON() => Cities2JSON(cities);
        public static string Cities2JSON(List<City> cities) => Cities2JSON(cities.ToArray());
        public static string Cities2JSON(City[] cities)
        {
            string json = "[";
            foreach (City city in cities) json += "\"" + city.Name + ", " + city.AdminName + ", " + city.Country + "\",";
            return json + "]";
        }
        public static City GetCityById(int id) => cities.Where(c => c.Id == id).First();
        public static City GetCityByName(string name) => cities.Where(c => c.Name == name).First();
        public static City GetCityByAscii(string ascii) => cities.Where(c => c.Ascii == ascii).First();
        public static City GetCityByJSON(string jsonFormatted)
        {
            string[] jsonParts = jsonFormatted.Split(",");
            string name = jsonParts[0];
            string admin = jsonParts[1].Trim();
            string country = jsonParts[2].Trim();
            return cities.Where(c => c.Name == name && c.AdminName == admin && c.Country == country).First();
        }

        public IEnumerator GetEnumerator()
        {
            return cities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
