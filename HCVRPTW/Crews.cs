using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public enum CrewType { Seniors, Juniors }// k - list of crew skills

namespace HCVRPTW
{
    public class Crew
    {
        public double Capacity; // vehicle load limit
        public int Id;
        public CrewType Type;
        public (double startTime, double endTime) WorkingTimeWindow; // (e_v, l_v) - working time window of crew

        public Crew(int id, double capacity, CrewType crewType)
        {
            this.Id = id;
            this.Capacity = capacity;
            this.Type = crewType;
        }
    }
}
