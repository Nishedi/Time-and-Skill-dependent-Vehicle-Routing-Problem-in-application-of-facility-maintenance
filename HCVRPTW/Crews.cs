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

        public double WorkTime { get; set; }
        public double afterHoursWorkTime { get; set; }
        public double afterHoursCost { get; set; }
        public double serviceTimeMultiplier { get; set; } = 1.0;
        public double baseCost { get; set; } = 100;
        public Crew(int id, int shift/*first or second*/, CrewType crewType)
        {
            this.Id = id;

            if (shift == 0)
            {
                this.WorkingTimeWindow = (0, 1236 / 2);
            }
            else
            {
                this.WorkingTimeWindow = (1236 / 2, 1236);
            }
            if (crewType == CrewType.Seniors)
            {
                afterHoursCost = 2;
                baseCost = 200;
            }
            else
            {
                afterHoursCost = 1;
                serviceTimeMultiplier = 1.5;
                baseCost = 100;
            }
            Type = crewType;
        }
        public void computeAfterHours()
        {
            afterHoursWorkTime = Math.Max(0, WorkTime - WorkingTimeWindow.endTime);
        }
    }
}

