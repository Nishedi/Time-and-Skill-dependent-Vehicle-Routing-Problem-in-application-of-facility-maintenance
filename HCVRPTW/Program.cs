using HCVRPTW;
using System.Diagnostics;

var filePath = "pliki//CTEST.txt";
//filePath = "pliki//100 lokacji//C101.txt";
Instance instance = new Instance(filePath);


var toursNoCrew = new List<Tour>();
toursNoCrew.Add(new Tour(null, new List<Location> { instance.Locations[0], instance.Locations[5], instance.Locations[3], instance.Locations[7], instance.Locations[8], instance.Locations[9] }));
toursNoCrew.Add(new Tour(null, new List<Location> { instance.Locations[0], instance.Locations[6], instance.Locations[4], instance.Locations[2], instance.Locations[1], instance.Locations[0] } ));
var allLocations = toursNoCrew.SelectMany(t => t.Locations).ToList();

Crew createCrew(Tour route, List<Crew> crews, int index)
{
    Crew crew = null;
    if (route.Locations[1].TimeWindow.Start >= 618-150)
    {
        crew = new Crew(crews[index].Id, 1, crews[index].Type);
    }
    else
    {
        crew = new Crew(crews[index].Id, 0, crews[index].Type);
    }
    return crew;
}


List<Solution> generateNeighbors(List<Tour> t, Instance instance) {
    List<Location> allLocations = t.SelectMany(t => t.Locations).ToList();
    for (int rep = 0; rep < allLocations.Count - 1; rep++)
    {
        if (allLocations[rep].Id == 0 && allLocations[rep + 1].Id == 0)
        {
            allLocations.RemoveAt(rep);
        }


    }
    //var toursNoCrew = new List<Tour>();
    var scenarios = new List<Solution>();
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
            var crewsCopy = new List<Crew>(instance.Crews);
            foreach (var route in toursNoCrew)
            {
                Crew crew = createCrew(route, crewsCopy, toursNoCrew.IndexOf(route));
                tours.Add(new Tour(crew, route.Locations));
            }
            var solution = calculateMetrics(tours);
            solution.move = (i, j);
            scenarios.Add(solution);

            //for(int k = 0; k < crewsCopy.Count; k++)
            for (int k = 0; k < Math.Min(toursNoCrew.Count,crewsCopy.Count); k++)
            {
            
                for (int l = 0; l < k; l++)
                {
                    crewsCopy = new List<Crew>(instance.Crews);
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
                    solution = calculateMetrics(tours);
                    solution.move = (i, j);
                    scenarios.Add(solution);
                }
            
            }

        }
    }

    return scenarios.OrderBy(s=>s.GrandTotal).ToList();
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
                penalty+=Math.Max(0, (crew.WorkTime+dist + loc.ServiceTime * crew.serviceTimeMultiplier) - loc.TimeWindow.End);
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

//var scenarios = generateNeighbors(allLocations, instance);

//List<Solution> bestsolutions = new List<Solution>();
//foreach (var tours in scenarios)
//{
//   bestsolutions.Add(calculateMetrics(tours));
//}

//bestsolutions = bestsolutions.OrderBy(s => s.TotalPenalty).ToList();
//foreach (var sol in bestsolutions.Take(5))
//{
//    foreach(var tour in sol.Tours)
//    {
//        foreach (var loc in tour.Locations)
//        {
//            Console.Write(loc.Id + " ");
//        }
//        Console.WriteLine();
//    }
//    Console.WriteLine("Total Penalty: " + sol.TotalPenalty);
//    Console.WriteLine("Total Driving Cost: " + sol.TotalDrivingCost);
//    Console.WriteLine("Total After Hours Cost: " + sol.TotalAfterHoursCost);
//    Console.WriteLine("Total Crew Usage Cost: " + sol.TotalCrewUsageCost);
//    Console.WriteLine("Grand Total: " + sol.GrandTotal);
//    Console.WriteLine();
//}

Solution greedy = generateGreedySolution(instance);
greedy  = calculateMetrics(greedy.Tours);
var scenarios = generateNeighbors(greedy.Tours, instance);

//List<Solution> bestsolutions = new List<Solution>();
//foreach (var tours in scenarios)
//{
//    bestsolutions.Add(calculateMetrics(tours));
//}

//bestsolutions = bestsolutions.OrderBy(s => s.GrandTotal).ToList();
//bestsolutions.Add(greedy);
//bestsolutions = bestsolutions.OrderBy(s => s.TotalPenalty).ToList();

//foreach (var sol in bestsolutions.Take(5))
//{
//    foreach(var tour in sol.Tours)
//    {
//        foreach (var loc in tour.Locations)
//        {
//            Console.Write(loc.Id + " ");
//        }
//        Console.WriteLine();
//    }
//    Console.WriteLine("Total Penalty: " + sol.TotalPenalty);
//    Console.WriteLine("Total Driving Cost: " + sol.TotalDrivingCost);
//    Console.WriteLine("Total After Hours Cost: " + sol.TotalAfterHoursCost);
//    Console.WriteLine("Total Crew Usage Cost: " + sol.TotalCrewUsageCost);
//    Console.WriteLine("Grand Total: " + sol.GrandTotal);
//    Console.WriteLine();
//}

Solution generateGreedySolution(Instance instance) //wprowadzic czekanie jezeli sie oplaca
{
    var greedyGTR = createGreedyGTR(instance);
    var result = new List<Tour>();
    var current = new List<Location>();
    int numRoutes = 0;
    var currentLoad = 0.0;
    foreach (var loc in greedyGTR)
    {
        current.Add(loc);
        currentLoad += loc.Demand;
        if (loc.Id == 0 && current.Count > 1)
        {
            result.Add(new Tour(instance.Crews[numRoutes], new List<Location>(current)));
            current.Clear();
            currentLoad = 0.0;
            current.Add(loc); // zaczynamy nową trasę od bazy
            numRoutes++;
        }
    }
    // Jeśli coś zostało, dodaj jako ostatnią trasę
    if (current.Count > 1)
        result.Add(new Tour(instance.Crews[numRoutes], new List<Location>(current)));

    return new Solution(result);
}
List<Location>  createGreedyGTR(Instance instance)
{
    double[,] distanceMatrix = instance.DistanceMatrix;
    List<Location> locations = instance.Locations;
    List<Crew> Crews = instance.Crews;
    var initialRoutesSplitted = new List<List<Location>>();
    var initialRoute = new List<Location>();
    bool[] visited = new bool[locations.Count];
    int crewNumber = 0;
    double vehicleTime = 0;
    double currentLoad = 0.0;

    initialRoute.Add(locations[0]); 

    while (visited.Contains(false))
    {
        Location current = locations[0]; // Start z bazy
        visited[current.Id] = true;
        vehicleTime = 0;
        //currentLoad = 0.0; // Reset załadunku przy starcie nowego pojazdu

        while (true)
        {
            Location nextCustomer = null;
            double minDistance = double.MaxValue;

            foreach (var location in locations)
            {
                if (!visited[location.Id] && location.Id != 0)
                {
                    double demand = location.Demand;
                    //double vehicleCapacity = Crews[crewNumber].Capacity;

                    //if (currentLoad + demand > vehicleCapacity)
                    //    continue; 

                    double distance = distanceMatrix[current.Id, location.Id];
                    double estimatedUpperTimeLeft = location.TimeWindow.End - (vehicleTime + location.ServiceTime * Crews[crewNumber].serviceTimeMultiplier);
                    double estimatedLowerTimeLeft = location.TimeWindow.Start - vehicleTime;
                    double estimatedPenalty = Math.Max(0, estimatedUpperTimeLeft);
                    estimatedPenalty += Math.Max(0, estimatedLowerTimeLeft);
                    distance += estimatedPenalty;

                    if (distance < minDistance && 
                        location.TimeWindow.Start < Crews[crewNumber].WorkingTimeWindow.endTime && 
                        vehicleTime + location.ServiceTime * Crews[crewNumber].serviceTimeMultiplier < Crews[crewNumber].WorkingTimeWindow.endTime)
                    {
                        minDistance = distance;
                        nextCustomer = location;
                    }
                }
            }

            if (nextCustomer == null && current.Id != 0 || vehicleTime >= locations[0].TimeWindow.End)
            {
                initialRoute.Add(locations[0]);
                crewNumber++;
                if (crewNumber >= Crews.Count)
                    return initialRoute;
                break;
            }
            else if (nextCustomer == null && current.Id == 0)
            {
                foreach (var location in locations)
                {
                    if (!visited[location.Id] && currentLoad + location.Demand <= Crews[crewNumber].Capacity)
                    {
                        nextCustomer = location;
                        break;
                    }
                }
            }

            if (current.Id == 0)
            {
                if (nextCustomer!=null&&nextCustomer.TimeWindow.Start >= 518) vehicleTime = 618;
                else vehicleTime = 0;
                Crews[crewNumber].WorkingTimeWindow = (vehicleTime, vehicleTime + 618);
                //vehicleTime = Math.Max(nextCustomer.TimeWindow.Start - distanceMatrix[current.Id, nextCustomer.Id], 0);
                //vehicleStarts.Add(Math.Max(nextCustomer.TimeWindow.Start - distanceMatrix[current.Id, nextCustomer.Id], 0.0));
            }
            vehicleTime += distanceMatrix[current.Id, nextCustomer.Id];
            //double upperTimeLeft = nextCustomer.TimeWindow.End - vehicleTime;
            //double lowerTimeLeft = nextCustomer.TimeWindow.Start - vehicleTime;
            //double penalty = Math.Max(0, nextCustomer.ServiceTime - upperTimeLeft);
            //penalty += Math.Max(0, Math.Min(lowerTimeLeft, nextCustomer.ServiceTime));
            vehicleTime += nextCustomer.ServiceTime*Crews[crewNumber].serviceTimeMultiplier;
            //vehicleTime += penalty;

            initialRoute.Add(nextCustomer);
            visited[nextCustomer.Id] = true;
            //currentLoad += nextCustomer.Demand; 

            current = nextCustomer;
        }
    }

    if (initialRoute[initialRoute.Count - 1].Id != 0)
    {
        initialRoute.Add(locations[0]);
    }
    return initialRoute;
}

//var x = bestsolutions[0];
//calculateMetrics(x.Tours);

Solution TabuSearch(Instance instance, int iterations, int tabuSize )
{
    Solution bestSolution = calculateMetrics(generateGreedySolution(instance).Tours);
    Solution greedySolution = new Solution(bestSolution.Tours, bestSolution.TotalPenalty, bestSolution.TotalDrivingCost, bestSolution.TotalAfterHoursCost, bestSolution.TotalCrewUsageCost, bestSolution.GrandTotal);
    Solution currentSolution = bestSolution;
    Queue<Solution> TabuList = new Queue<Solution>();
    int notImprovingIterations = 0;
    for (int i = 0; i < iterations; i++)
    {
        Solution bestNeighbor = null;
        var neighborhood = generateNeighbors(currentSolution.Tours, instance);
        foreach (var neighbor in neighborhood.Take(tabuSize * 10))
        {
            bool isTabu = TabuList.Any(tabuSolution => tabuSolution.Equals(neighbor));
            if (isTabu && neighbor.GrandTotal > bestSolution.GrandTotal) continue;
            if(bestNeighbor == null || neighbor.GrandTotal <  bestNeighbor.GrandTotal)
            {
                bestNeighbor = neighbor;
            }
        }
        if (bestNeighbor == null) break;
        currentSolution = bestNeighbor;
        if(bestNeighbor.GrandTotal < bestSolution.GrandTotal)
        {
            bestSolution = bestNeighbor;
            notImprovingIterations = 0;
        }
        else
        {
            notImprovingIterations++;
        }
        TabuList.Enqueue(currentSolution);
        if (TabuList.Count > tabuSize)
            TabuList.Dequeue();
    }
    return bestSolution;
}

var bestSolution = TabuSearch(instance, 10, 10);
calculateMetrics(bestSolution.Tours);
;
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