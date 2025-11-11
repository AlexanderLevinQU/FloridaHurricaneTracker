using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nanoid;

namespace FloridaHurricaneTracker.Model.HurricaneInfo
{
    internal class Hurricane
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public double MaxWindSpeed { get; set; }

        public Hurricane() { }

        //Constructor for hurricane
        public Hurricane(string name, DateTime date, double windSpeed)
        {
            Name = $"Hurricane_{name}_{Nanoid.Nanoid.Generate(size: 5)}"; //Multiple Hurricanes with the same name. Can add id. Could also do date for quick identifier
            Date = date;
            MaxWindSpeed = windSpeed;
        }
    }
}
