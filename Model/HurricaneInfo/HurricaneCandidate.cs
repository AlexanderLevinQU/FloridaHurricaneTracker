using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloridaHurricaneTracker.Model.HurricaneInfo
{
    internal class HurricaneCandidate
    {
        public string Id { get; set; }       
        public string Name { get; set; }              
        public List<HurricaneEntry> Entries { get; set; } = new List<HurricaneEntry>();
        public double MaxWindSpeed { get; set; }
        public bool HitsFloridaAsHurricane { get; set; }
        public DateTime TimeStamp { get; set; }

    }
}
