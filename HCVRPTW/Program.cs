using HCVRPTW;

Instance instance = new Instance("pliki//CTEST.txt");


var toursNoCrew = new List<Tour>();
toursNoCrew.Add(new Tour(null, new List<Location> { instance.Locations[0], instance.Locations[5], instance.Locations[3], instance.Locations[7], instance.Locations[8], instance.Locations[9] }));
toursNoCrew.Add(new Tour(null, new List<Location> { instance.Locations[0], instance.Locations[6], instance.Locations[4], instance.Locations[2], instance.Locations[1], instance.Locations[0] } ));
var allLocations = toursNoCrew.SelectMany(t => t.Locations).ToList();

Crew createCrew(Tour route, List<Crew> crews, int index)
{
    Crew crew = null;
    if (route.Locations[1].TimeWindow.Start >= 618)
    {
        crew = new Crew(crews[index].Id, 1, crews[index].Type);
    }
    else
    {
        crew = new Crew(crews[index].Id, 0, crews[index].Type);
    }
    return crew;
}


List<List<Tour>> generateNeighbors(List<Location> allLocations, Instance instance) {
    //var toursNoCrew = new List<Tour>();
    var scenarios = new List<List<Tour>>();
    for (int i = 1; i < allLocations.Count - 1; i++)
    {
        for (int j = 1; j < i; j++)

    //for (int i = allLocations.Count - 2; i < allLocations.Count - 1; i++)
    //{
    //    for (int j = i-1; j < i; j++)
        {
            var copy = allLocations.ToList();
            var toSwap = copy[i];
            copy[i] = copy[j];
            copy[j] = toSwap;
            var locations = new List<Location>();
            var toursNoCrew = new List<Tour>();
            foreach (var loc in copy)
            {
                if (loc.Id != 0)
                {
                    locations.Add(loc);
                }
                else
                {
                    locations.Add(loc);
                    if(locations.Count > 2)
                    {
                        toursNoCrew.Add(new Tour(null, new List<Location>(locations)));
                        locations.Clear();
                        locations.Add(loc);
                    }
                }
            }
            var tours = new List<Tour>();
            var crewsCopy = new List<Crew>(instance.crews);
            foreach (var route in toursNoCrew)
            {
                Crew crew = createCrew(route, crewsCopy, toursNoCrew.IndexOf(route));
                tours.Add(new Tour(crew, route.Locations));
            }
            scenarios.Add(tours);
        
            for(int k = 0; k < crewsCopy.Count; k++)
            {
            
                for (int l = 0; l < k; l++)
                {
                    crewsCopy = new List<Crew>(instance.crews);
                    if (crewsCopy[k].Type == crewsCopy[l].Type) continue;
                    var crewToSwap = crewsCopy[k];
                    crewsCopy[k] = crewsCopy[l];
                    crewsCopy[l] = crewToSwap;

                    tours = new List<Tour>();
                    foreach (var route in toursNoCrew)
                    {
                        Crew crew = createCrew(route, crewsCopy, toursNoCrew.IndexOf(route));
                        tours.Add(new Tour(crew, route.Locations));
                    }

                    scenarios.Add(tours);
                }
            
            }

        }
    }
    return scenarios;
}


Solution calculateMetrics(List<Tour> tours)
{
    double totalPenalty = 0.0;
    double totalDrivingCost = 0.0;
    double totalAfterHoursCost = 0.0;
    double totalCrewUsageCost = 0.0;
    double grandTotal = 0.0;
    foreach (var tour in tours)
    {
        var penalty = 0.0;
        var drivingCost = 0.0;
        var crew = tour.Crew;
        crew.WorkTime = crew.WorkingTimeWindow.startTime;
        Location prevLocation = null;
        
        foreach (var loc in tour.Locations)
        {
            if (loc.Id != 0)
            {
                var dist = instance.DistanceMatrix[prevLocation.Id, loc.Id];
                penalty+= Math.Max(0, loc.TimeWindow.Start - (crew.WorkTime + dist));
                penalty+=Math.Max(0, (crew.WorkTime + dist) - loc.TimeWindow.End);
                crew.WorkTime += loc.ServiceTime * crew.serviceTimeMultiplier + dist;
                drivingCost+= dist;
            }
            else if (prevLocation != null)
            {
                var dist = instance.DistanceMatrix[prevLocation.Id, loc.Id];
                crew.WorkTime += dist;
                drivingCost += dist;
            }
            prevLocation = loc;
        }
        crew.computeAfterHours();
        double sum = penalty + drivingCost + crew.afterHoursWorkTime * crew.afterHoursCost+100;//100 - cost of using the crew
        totalPenalty += penalty;
        totalDrivingCost += drivingCost;
        totalAfterHoursCost += crew.afterHoursWorkTime * crew.afterHoursCost;
        totalCrewUsageCost += 100;
        grandTotal += sum;
    }
    return new Solution(tours,totalPenalty,totalDrivingCost,totalAfterHoursCost,totalCrewUsageCost,grandTotal);
    
}

var scenarios = generateNeighbors(allLocations, instance);

List<Solution> bestsolutions = new List<Solution>();
foreach (var tours in scenarios)
{
   bestsolutions.Add(calculateMetrics(tours));
}

bestsolutions = bestsolutions.OrderBy(s => s.TotalPenalty).ToList();
foreach (var sol in bestsolutions.Take(5))
{
    foreach(var tour in sol.Tours)
    {
        foreach (var loc in tour.Locations)
        {
            Console.Write(loc.Id + " ");
        }
        Console.WriteLine();
    }
    Console.WriteLine("Total Penalty: " + sol.TotalPenalty);
    Console.WriteLine("Total Driving Cost: " + sol.TotalDrivingCost);
    Console.WriteLine("Total After Hours Cost: " + sol.TotalAfterHoursCost);
    Console.WriteLine("Total Crew Usage Cost: " + sol.TotalCrewUsageCost);
    Console.WriteLine("Grand Total: " + sol.GrandTotal);
    Console.WriteLine();
}

for(int i = 0; i < 15; i++)
{
    var randomIndex = new Random().Next(1, 5);
    allLocations = bestsolutions[randomIndex].Tours.SelectMany(t => t.Locations).ToList();
    for(int rep = 0; rep<allLocations.Count-1; rep++)
    {
        if(allLocations[rep].Id == 0 && allLocations[rep + 1].Id == 0)
        {
            allLocations.RemoveAt(rep);
        }
        
        
    }
    scenarios = generateNeighbors(allLocations, instance);
    
    bestsolutions.Clear();
    foreach (var tours in scenarios)
    {
        bestsolutions.Add(calculateMetrics(tours));
    }

    bestsolutions = bestsolutions.OrderBy(s => s.TotalPenalty).ToList();
    Console.WriteLine("Iteration " + (i+1));
    Console.WriteLine("TotalPenalty: " + bestsolutions[0].TotalPenalty);
    Console.WriteLine("Grand Total: " + bestsolutions[0].GrandTotal);
    Console.WriteLine();

}

//foreach (var tours in scenarios)
//{
//    foreach (var tour in tours)
//    {
//        var crew = tour.Crew;
//        crew.WorkTime = crew.WorkingTimeWindow.startTime;
//        Location prevLocation = null;
//        foreach (var loc in tour.Locations)
//        {
//            if (loc.Id != 0)
//            {
//                var dist = instance.DistanceMatrix[prevLocation.Id, loc.Id];
//                var waitingTime = Math.Max(0, Math.Min(loc.ServiceTime, loc.TimeWindow.Start - (crew.WorkTime + dist)));
//                waitingTime = 0;
//                crew.WorkTime += loc.ServiceTime * crew.serviceTimeMultiplier + dist + waitingTime;
//            }
//            else if (prevLocation != null)
//            {
//                var dist = instance.DistanceMatrix[prevLocation.Id, loc.Id];
//                crew.WorkTime += dist;
//            }
//            prevLocation = loc;
//        }
//        crew.computeAfterHours();
//        foreach (var loc in tour.Locations)
//        {
//            Console.Write(loc.Id+" ");
//        }
//        Console.WriteLine("| "+crew.Type+" "+crew.WorkTime + " " + crew.afterHoursWorkTime);
//    }
//    if (scenarios.IndexOf(tours) % 2 == 1) Console.WriteLine();
//}





/*
0 Depot(40, 50) Demand: 0.0 TW: [0, 1236]
5 Customer(42, 65) Demand: 10.0 TW: [15, 67]
3 Customer(42, 66) Demand: 10.0 TW: [65, 146]
7 Customer(40, 66) Demand: 20.0 TW: [170, 225]
8 Customer(38, 68) Demand: 20.0 TW: [255, 324]
9 Customer(38, 70) Demand: 10.0 TW: [534, 605]

0 Depot(40, 50) Demand: 0.0 TW: [0, 1236]
6 Customer(40, 69) Demand: 20.0 TW: [621, 702]
4 Customer(42, 68) Demand: 10.0 TW: [727, 782]
2 Customer(45, 70) Demand: 30.0 TW: [825, 870]
1 Customer(45, 68) Demand: 10.0 TW: [912, 967]
0 Depot(40, 50) Demand: 0.0 TW: [0, 1236]
*/