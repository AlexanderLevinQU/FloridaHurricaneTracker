using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloridaHurricaneTracker.Model.HurricaneInfo
{
    internal class HurricaneEntry
    {
        public DateTime Timestamp { get; set; }
        public bool isLandFall { get; set; }
        public string Status { get; set; } // Hurricane or tropical storm etc...
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double WindSpeed { get; set; }
    }
}
