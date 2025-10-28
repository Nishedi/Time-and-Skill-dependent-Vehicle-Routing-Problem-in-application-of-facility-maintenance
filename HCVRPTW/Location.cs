using HCVRPTW;
using System;

public enum LocationType { Depot, Customer } 

namespace HCVRPTW
{
    public class Location
    {
        public int Id { get; set; }
        public LocationType Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public double Demand { get; set; }
        public (int Start, int End) TimeWindow { get; set; }    //b_i, d_i - time window at location i
        public int ServiceTime { get; set; }                    // s_i - service time at location i

        public Location(
            int id, LocationType type, int x, int y, double demand, (int Start, int End) timeWindow,  int serviceTime)
        {
            Id = id;
            Type = type;
            X = x;
            Y = y;
            Demand = demand;
            TimeWindow = timeWindow;
            ServiceTime = serviceTime;

        }

        public override string ToString()
        {
            return $"{Id} {Type} ({X},{Y}) Demand: {Demand:F1} " +
                $"TW: [{TimeWindow.Start}, {TimeWindow.End}]";
        }
    }
}