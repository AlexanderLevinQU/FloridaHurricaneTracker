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
        public int EntryCount { get; set; }  
        public List<HurricaneEntry> Entries { get; set; } = new();

    }
}
