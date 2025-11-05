using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCVRPTW
{
    internal class Solution
    {
        public List<Tour> Tours { get; set; }
        public double TotalCost { get; set; }
        public double TotalPenalty = 0.0;
        public double TotalDrivingCost = 0.0;
        public double TotalAfterHoursCost = 0.0;
        public double TotalCrewUsageCost = 0.0;
        public double GrandTotal = 0.0;
        public Solution(List<Tour> tours, double totalPenalty=0.0,
            double totalDrivingCost=0.0,
            double totalAfterHoursCost=0.0,
            double totalCrewUsageCost=0.0,
            double grandTotal= 0.0
            ) 
        { 
            Tours = tours;
            TotalPenalty = totalPenalty;
            TotalDrivingCost = totalDrivingCost;
            TotalAfterHoursCost = totalAfterHoursCost;
            TotalCrewUsageCost = totalCrewUsageCost;
            GrandTotal = grandTotal;
        }
    }
}
