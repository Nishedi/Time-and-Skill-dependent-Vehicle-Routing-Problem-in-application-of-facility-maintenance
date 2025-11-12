using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCVRPTW
{
    internal class TabuSearch
    {
        public static List<Solution> generateNeighbors(List<Tour> t, Instance instance)
        {
            List<Location> allLocations = t.SelectMany(t => t.Locations).ToList();
            for (int rep = 0; rep < allLocations.Count - 1; rep++)
            {
                if (allLocations[rep].Id == 0 && allLocations[rep + 1].Id == 0)
                {
                    allLocations.RemoveAt(rep);
                }


            }
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
                            if (locations.Count > 2)
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
                        tours.Add(new Tour(crewsCopy[toursNoCrew.IndexOf(route)], route.Locations));
                    }
                    var solution = Utils.calculateMetrics(tours,instance);
                    solution.move = (i, j);
                    scenarios.Add(solution);
                    for (int k = 0; k < Math.Min(toursNoCrew.Count, crewsCopy.Count); k++)
                    {

                        for (int l = 0; l < k; l++)
                        {
                            crewsCopy = new List<Crew>(instance.Crews);
                            if (crewsCopy[k].Type == crewsCopy[l].Type && crewsCopy[k].WorkingTimeWindow.startTime == crewsCopy[l].WorkingTimeWindow.startTime) continue;
                            var crewToSwap = crewsCopy[k];
                            crewsCopy[k] = crewsCopy[l];
                            crewsCopy[l] = crewToSwap;

                            tours = new List<Tour>();
                            foreach (var route in toursNoCrew)
                            {
                                tours.Add(new Tour(crewsCopy[toursNoCrew.IndexOf(route)], route.Locations));
                            }
                            solution = Utils.calculateMetrics(tours,instance);
                            solution.move = (i, j);
                            scenarios.Add(solution);
                        }

                    }

                }
            }
            return scenarios.OrderBy(s => s.GrandTotal).ToList();
        }

        public static Solution RunTabuSearch(Instance instance, int iterations, int tabuSize)
        {
            Solutionv2 test = Utils.calculateMetricsv2(Utils.generateGreedySolutionv2(instance), instance);
            Solution bestSolution = Utils.calculateMetrics(Utils.generateGreedySolution(instance).Tours, instance);
            if (Math.Round(test.TotalPenalty,2) != Math.Round(bestSolution.TotalPenalty,2))
            {
                Console.WriteLine("ERROR in TS initial solution Total Penalty");
                Console.WriteLine(test.TotalPenalty + " " + bestSolution.TotalPenalty);
            }
            if(Math.Round(test.TotalDrivingCost, 2) != Math.Round(bestSolution.TotalDrivingCost,2))
            {
                Console.WriteLine("ERROR in TS initial solution TotalDrivingCost");
                Console.WriteLine(test.TotalDrivingCost + " " + Math.Round(bestSolution.TotalDrivingCost, 2));
            }
            if (Math.Round(test.TotalAfterHoursCost, 2) != Math.Round(bestSolution.TotalAfterHoursCost,2))
            {
                Console.WriteLine("ERROR in TS initial solution TOtal after hours");
                Console.WriteLine(test.TotalAfterHoursCost + " " + Math.Round(bestSolution.TotalAfterHoursCost, 2));
            }
            if (Math.Round(test.TotalCrewUsageCost, 2) != Math.Round(bestSolution.TotalCrewUsageCost,2))
            {
                Console.WriteLine("ERROR in TS initial solution total crew usage");
                Console.WriteLine(test.TotalCrewUsageCost + " " + bestSolution.TotalCrewUsageCost);
            }
            if (Math.Round(test.GrandTotal,2) != Math.Round(bestSolution.GrandTotal,2))
            {
                Console.WriteLine("ERROR in TS initial solution grand total");
                Console.WriteLine(test.GrandTotal + " " + bestSolution.GrandTotal);
            }
            return bestSolution;


            Solution greedySolution = new Solution(bestSolution.Tours, bestSolution.TotalPenalty, bestSolution.TotalDrivingCost, bestSolution.TotalAfterHoursCost, bestSolution.TotalCrewUsageCost, bestSolution.GrandTotal);
            Solution currentSolution = bestSolution;
            Console.Write("G: " + greedySolution.GrandTotal+" ");
            Queue<Solution> TabuList = new Queue<Solution>();
            int notImprovingIterations = 0;
            for (int i = 0; i < iterations; i++)
            {
                Console.Write(".");
                Solution bestNeighbor = null;
                var neighborhood = generateNeighbors(currentSolution.Tours, instance);
                foreach (var neighbor in neighborhood.Take(tabuSize * 10))
                {
                    bool isTabu = TabuList.Any(tabuSolution => tabuSolution.Equals(neighbor));
                    if (isTabu && neighbor.GrandTotal > bestSolution.GrandTotal) continue;
                    if (bestNeighbor == null || neighbor.GrandTotal < bestNeighbor.GrandTotal)
                    {
                        bestNeighbor = neighbor;
                    }
                }
                if (bestNeighbor == null) break;
                currentSolution = bestNeighbor;
                if (bestNeighbor.GrandTotal < bestSolution.GrandTotal)
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
            Console.WriteLine(" "+bestSolution.GrandTotal + " "+(1-bestSolution.GrandTotal/greedySolution.GrandTotal));
            return bestSolution;
        }




    }
}
