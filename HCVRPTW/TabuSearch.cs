using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCVRPTW
{
    internal class TabuSearch
    {
        static List<T> InsertMove<T>(List<T> list, int i, int j)
        {
            var copy = list.ToList();
            var element = copy[i];
            copy.RemoveAt(i);
            if (j > i) j--;
            copy.Insert(j, element);
            return copy;
        }
        static List<T> SwapMove<T>(List<T> list, int i, int j)
        {
            var copy = list.ToList();
            var toSwap = copy[i];
            copy[i] = copy[j];
            copy[j] = toSwap;
            return copy;
        }
        static List<T> ReverseMove<T>(List<T> list, int i, int j)
        {
            var copy = list.ToList();
            copy.Reverse(i, j - i + 1);
            return copy;
        }
        static List<T> TwoOptMove<T>(List<T> list, int i, int j)
        {
            var copy = list.ToList();
            // wszystko między i+1 i j odwracamy – dokładnie 2-opt
            copy.Reverse(i + 1, j - i);
            return copy;
        }

        public static List<Solutionv2> generateNeighborsv2(List<Location> allLocations, List<Crew> crews, Instance instance, int moveOperator)
        {
            var scenarios = new List<Solutionv2>();
            for (int i = 1; i < allLocations.Count - 1; i++)
            {
                for (int j = 1; j < i; j++)
                //for (int i = allLocations.Count - 2; i < allLocations.Count - 1; i++)
                //{
                //    for (int j = i-1; j < i; j++)
                {
                    List<Location> copy = null;
                    if (moveOperator == 0) copy = SwapMove(allLocations, i, j);
                    if (moveOperator == 1) copy = InsertMove(allLocations, i, j);
                    if (moveOperator == 2) copy = ReverseMove(allLocations, j, i);
                    if (moveOperator == 3) copy = TwoOptMove(allLocations, j, i);

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

        public static List<Location> RandomShuttle(List<Location> allLocations)
        {
            var result = new List<Location>();
            for(int n = 0; n < 10; n++)
            {
                for (int i = 1; i < allLocations.Count - 1; i++)
                {
                    for (int j = 1; j < i; j++)
                    {
                        var rnd = new Random();
                        if (rnd.NextDouble() < 0.1)
                        {
                            var copy = allLocations;
                            var toSwap = copy[i];
                            copy[i] = copy[j];
                            copy[j] = toSwap;
                            result = copy;
                        }
                    }
                }
            }
            
            
            
            return result;
        }

        public static Solutionv2 RunTabuSearch(Instance instance, int iterations, int tabuSize, int moveOperator)
        {
            String[] operators = new String[] {"SwapMove", "InsertMove", "ReverseMove", "TwoOptMove", "BlockSwapMOve", "BlockShiftMove" };
            
            //Solution bestSolution = Utils.calculateMetrics(Utils.generateGreedySolution(instance).Tours, instance);
            Solutionv2 bestSolution = Utils.calculateMetricsv2(Utils.generateGreedySolutionv2(instance), instance);
            List<Solutionv2> Elite = new();
            Elite.Add(bestSolution);
            Elite = Elite.OrderBy(s => s.GrandTotal).Take(5).ToList();


            Solutionv2 greedySolution = new Solutionv2(bestSolution.GTR, bestSolution.Crews, bestSolution.TotalPenalty, bestSolution.TotalDrivingCost, bestSolution.TotalAfterHoursCost, bestSolution.TotalCrewUsageCost, bestSolution.GrandTotal);
            Solutionv2 currentSolution = bestSolution;
            //Console.Write("G: " + greedySolution.GrandTotal+" ");
            Queue<Solutionv2> TabuList = new Queue<Solutionv2>();
            int notImprovingIterations = 0;
            int notImprovingIterationsv2 = 0;
            for (int i = 0; i < iterations; i++)
            {
                //Console.Write(".");
                Solutionv2 bestNeighbor = null;
                var neighborhood = generateNeighborsv2(currentSolution.GTR, currentSolution.Crews, instance, moveOperator);
                foreach (var neighbor in neighborhood.Take(tabuSize * 10))
                {
                    bool isTabu = TabuList.Any(tabuSolution => tabuSolution.Equals(neighbor));
                    bool aspiration = neighbor.GrandTotal < bestSolution.GrandTotal;

                    //if (isTabu && !aspiration) continue;
                    if (isTabu && neighbor.GrandTotal >= bestSolution.GrandTotal) continue;
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
                    //Console.Write(" + ");
                    notImprovingIterationsv2 = 0;
                    Elite.Add(bestNeighbor);
                    Elite = Elite.OrderBy(s => s.GrandTotal).Take(5).ToList();

                }
                else
                {
                    notImprovingIterations++;
                    notImprovingIterationsv2++;
                }
                if (notImprovingIterations >= iterations * 0.01)
                {
                    var random = new Random();
                    int x = random.Next(2);
                    if (x % 2 == 0)
                    {

                        currentSolution = Elite[random.Next(Elite.Count)];

                    }
                    else
                    {
                        currentSolution.GTR = RandomShuttle(currentSolution.GTR);
                    }
                        
                    //Console.Write(" - ");
                    notImprovingIterations = 0;
                }
                if (notImprovingIterationsv2 >= iterations * 0.1)
                {
                    Console.Write(" Break at: " + i);
                    break;
                }
                TabuList.Enqueue(currentSolution);
                int dynamicTabu = tabuSize;

                if (notImprovingIterations > 50)
                {
                    dynamicTabu = Math.Min(dynamicTabu * 2, 500);
                }
                else if (notImprovingIterations == 0)
                {
                    dynamicTabu = Math.Max(dynamicTabu / 2, 5);
                }

                if (TabuList.Count > dynamicTabu)
                    TabuList.Dequeue();
            }
            Console.WriteLine(" "+operators[moveOperator]+" "+bestSolution.GrandTotal + " "+(1-bestSolution.GrandTotal/greedySolution.GrandTotal));
            return bestSolution;
        }




    }
}
