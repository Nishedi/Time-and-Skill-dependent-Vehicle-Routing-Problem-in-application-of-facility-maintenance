using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCVRPTW
{
    
    internal class Tour
    {
        public List<Location> Locations { get; set; }
        public double Cost { get; set; }
        public double penalty { get; set; }
        public Crew Crew { get; set; }
        public Tour(Crew crew, List<Location> locations)
        {
            Locations = locations;
            Crew = crew;
        }

    }
}
