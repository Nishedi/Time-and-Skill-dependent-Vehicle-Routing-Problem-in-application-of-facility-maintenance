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
        public double TotalPenalty = double.MaxValue;
        public double TotalDrivingCost = double.MaxValue;
        public double TotalAfterHoursCost = double.MaxValue;
        public double TotalCrewUsageCost = double.MaxValue;
        public double GrandTotal = double.MaxValue;
        public (int i, int j) move = (0, 0);
        public Solution(List<Tour> tours, double totalPenalty = double.MaxValue,
            double totalDrivingCost = double.MaxValue,
            double totalAfterHoursCost = double.MaxValue,
            double totalCrewUsageCost = double.MaxValue,
            double grandTotal = double.MaxValue,
            int I = 0,
            int J = 0
            ) 
        { 
            Tours = tours;
            TotalPenalty = totalPenalty;
            TotalDrivingCost = totalDrivingCost;
            TotalAfterHoursCost = totalAfterHoursCost;
            TotalCrewUsageCost = totalCrewUsageCost;
            GrandTotal = grandTotal;
            move.i = I;
            move.j = J;
        }
        public Solution(List<Tour> tours, int i, int j)
        {
            Tours = tours;
            move.i = i;
            move.j = j;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            Solution other = (Solution)obj;

            if (other.GrandTotal == this.GrandTotal) return true;
            if (other.move.i!=this.move.i) return false;
            if(other.move.j!=this.move.j) return false;
            return true;
        }
    }
}
