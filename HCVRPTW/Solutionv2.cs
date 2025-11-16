using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCVRPTW
{
    internal class Solutionv2
    {
        public List<Location> GTR { get; set; }
        public List<Crew> Crews { get; set; }
        public double TotalCost { get; set; }
        public double TotalPenalty = double.MaxValue;
        public double TotalDrivingCost = double.MaxValue;
        public double TotalAfterHoursCost = double.MaxValue;
        public double TotalCrewUsageCost = double.MaxValue;
        public double GrandTotal = double.MaxValue;
        public (int i, int j) move = (0, 0);


        public Solutionv2(List<Location> tours, List<Crew> crews, double totalPenalty = double.MaxValue,
            double totalDrivingCost = double.MaxValue,
            double totalAfterHoursCost = double.MaxValue,
            double totalCrewUsageCost = double.MaxValue,
            double grandTotal = double.MaxValue,
            int I = 0,
            int J = 0
            ) 
        { 
            GTR = tours;
            Crews = crews;
            TotalPenalty = totalPenalty;
            TotalDrivingCost = totalDrivingCost;
            TotalAfterHoursCost = totalAfterHoursCost;
            TotalCrewUsageCost = totalCrewUsageCost;
            GrandTotal = grandTotal;
            move.i = I;
            move.j = J;
        }
        public Solutionv2(List<Location> tours, int i, int j)
        {
            GTR = tours;
            move.i = i;
            move.j = j;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            Solutionv2 other = (Solutionv2)obj;

            //if (other.GrandTotal == this.GrandTotal) return true;
            if (other.move.i != this.move.i) return false;
            if (other.move.j != this.move.j) return false;
            return true;
        }
    }
}
