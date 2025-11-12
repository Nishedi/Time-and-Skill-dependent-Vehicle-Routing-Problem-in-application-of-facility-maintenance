using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCVRPTW
{
    internal class TabuSearch
    {
        public static List<Solutionv2> generateNeighborsv2(List<Location> allLocations, List<Crew> crews, Instance instance)
        {
            var scenarios = new List<Solutionv2>();
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
                    var crewsCopy = new List<Crew>(instance.Crews);

                    var solution = Utils.calculateMetricsv2(new Solutionv2(copy,crewsCopy), instance);
                    solution.move = (i, j);
                    scenarios.Add(solution);
                    var depotNumber = allLocations.Count(loc => loc.Id == 0);
                    for (int k = 0; k < Math.Min(Math.Max(depotNumber*1.1,depotNumber+5), crewsCopy.Count); k++)
                    {

                        for (int l = 0; l < k; l++)
                        {
                            crewsCopy = new List<Crew>(instance.Crews);
                            if (crewsCopy[k].Type == crewsCopy[l].Type && crewsCopy[k].WorkingTimeWindow.startTime == crewsCopy[l].WorkingTimeWindow.startTime) continue;
                            var crewToSwap = crewsCopy[k];
                            crewsCopy[k] = crewsCopy[l];
                            crewsCopy[l] = crewToSwap;
                            solution = Utils.calculateMetricsv2(new Solutionv2(copy, crewsCopy), instance);
                            solution.move = (i, j);
                            scenarios.Add(solution);
                        }

                    }

                }
            }
            return scenarios.OrderBy(s => s.GrandTotal).ToList();
        }

        public static Solutionv2 RunTabuSearch(Instance instance, int iterations, int tabuSize)
        {
            
            //Solution bestSolution = Utils.calculateMetrics(Utils.generateGreedySolution(instance).Tours, instance);
            Solutionv2 bestSolution = Utils.calculateMetricsv2(Utils.generateGreedySolutionv2(instance), instance);


            Solutionv2 greedySolution = new Solutionv2(bestSolution.GTR, bestSolution.Crews, bestSolution.TotalPenalty, bestSolution.TotalDrivingCost, bestSolution.TotalAfterHoursCost, bestSolution.TotalCrewUsageCost, bestSolution.GrandTotal);
            Solutionv2 currentSolution = bestSolution;
            Console.Write("G: " + greedySolution.GrandTotal+" ");
            Queue<Solutionv2> TabuList = new Queue<Solutionv2>();
            int notImprovingIterations = 0;
            for (int i = 0; i < iterations; i++)
            {
                Console.Write(".");
                Solutionv2 bestNeighbor = null;
                var neighborhood = generateNeighborsv2(currentSolution.GTR, currentSolution.Crews, instance);
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
