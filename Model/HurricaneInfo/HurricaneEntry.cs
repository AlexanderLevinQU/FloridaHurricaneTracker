using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloridaHurricaneTracker.Model.HurricaneInfo
{
    internal class HurricaneEntry
    {
        // each hurricane has multiple entries. Easier to parse
        public DateTime Timestamp { get; set; }
        public bool IsLandFall { get; set; }
        public bool IsHurricane { get; set; } // Hurricane 
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double WindSpeed { get; set; }
    }
}
