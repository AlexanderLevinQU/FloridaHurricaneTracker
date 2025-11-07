using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nanoid;

namespace FloridaHurricaneTracker.Model
{
    internal class Hurricane
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public double MaxWindSpeed { get; set; }

        public Hurricane() { }

        //Constructor for hurricane that is Unamed
        public Hurricane(DateTime date, double windSpeed)
        {
            Date = date;
            MaxWindSpeed = windSpeed;
            Name = $"Hurricane_{Nanoid.Nanoid.Generate(size: 5)}_{date.ToShortDateString()}"; 
        }

        //Constructor for hurricane that is typical
        public Hurricane(string name, DateTime date, double windSpeed)
        {
            Name = $"Hurricane_{name}_{date.ToShortDateString()}";
            Date = date;
            MaxWindSpeed = windSpeed;
        }
    }
}
