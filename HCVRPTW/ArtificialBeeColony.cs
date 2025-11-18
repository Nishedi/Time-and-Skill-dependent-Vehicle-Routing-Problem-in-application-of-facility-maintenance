using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace HCVRPTW
{
    internal class ArtificialBeeColony
    {

        public static List<Solutionv2> population = new List<Solutionv2>();
        public static List<double> fitnessValues = new List<double>();
        public static List<int> noImprovementCounter = new List<int>();
        public static List<double> probability = new List<double>();
        public static Random rnd = new Random();

        public static Solutionv2 bestSolution;

        private static int length = 0;

        private static Solutionv2 crewMutation(Solutionv2 solution, Instance instance)
        {
            var scenarios = new List<Solutionv2>();
            int i= solution.move.i;
            int j = solution.move.j;
            var depotNumber = solution.GTR.Count(loc => loc.Id == 0);
            var crewsCopy = new List<Crew>(instance.Crews);
            for (int k = 0; k < Math.Min(Math.Max(depotNumber * 1.1, depotNumber + 5), crewsCopy.Count); k++)
            {

                for (int l = 0; l < k; l++)
                {
                    crewsCopy = new List<Crew>(instance.Crews);
                    if (crewsCopy[k].Type == crewsCopy[l].Type && crewsCopy[k].WorkingTimeWindow.startTime == crewsCopy[l].WorkingTimeWindow.startTime) continue;
                    var crewToSwap = crewsCopy[k];
                    crewsCopy[k] = crewsCopy[l];
                    crewsCopy[l] = crewToSwap;
                    solution = Utils.calculateMetricsv2(new Solutionv2(solution.GTR, crewsCopy), instance);
                    solution.move = (i, j);
                    scenarios.Add(solution);
                }

            }

            return scenarios.OrderBy(s => s.GrandTotal).ToList()[0];
        }
        public static Solutionv2 Run(Instance instance, int iterations, int noImprovementLimit, int beesNumber, int moveOperator, int maxTime=600, bool isLogging = true)
        {
            List<int> changes = new List<int>();
            String[] operators = new String[] { "SwapMove", "InsertMove", "ReverseMove", "TwoOptMove"};
            Solutionv2 currSolution = Utils.calculateMetricsv2(Utils.generateGreedySolutionv2(instance), instance);
            length = currSolution.GTR.Count - 1;
            for (int i = 0; i < beesNumber; i++)
            {
                var newSolution = new Solutionv2(currSolution);
                newSolution.GTR = TabuSearch.RandomShuttle(newSolution.GTR);
                newSolution = Utils.calculateMetricsv2(newSolution, instance);
                if (i == 0) bestSolution = new Solutionv2(newSolution);
                population.Add(newSolution);
                fitnessValues.Add(newSolution.GrandTotal);
                noImprovementCounter.Add(0);
                if (newSolution.GrandTotal < bestSolution.GrandTotal)
                {
                    bestSolution = newSolution;
                }
            }
            int iter = 0;
            var stopwatch = Stopwatch.StartNew();
            maxTime *= 1000;
            while (stopwatch.ElapsedMilliseconds <= maxTime)
            {
                if (isLogging)
                    Console.Write($"\rDone: {iter}/{iterations} {(iter * 100 / iterations)}");
                for (int j = 0; j < beesNumber; j++)
                {
                    var newSolution = new Solutionv2(population[j]);
                    int idx1 = rnd.Next(1, length);
                    int idx2 = rnd.Next(1, length);
                    if (moveOperator == 0)
                        newSolution.GTR = TabuSearch.SwapMove(newSolution.GTR, idx1, idx2);
                    else if (moveOperator == 1)
                        newSolution.GTR = TabuSearch.InsertMove(newSolution.GTR, idx1, idx2);
                    else if (moveOperator == 2)
                    {
                        if (idx1 > idx2)
                            (idx1, idx2) = (idx2, idx1);
                        newSolution.GTR = TabuSearch.ReverseMove(newSolution.GTR, idx1, idx2);
                    }
                    else if (moveOperator == 3)
                    {
                        if (idx1 > idx2)
                            (idx1, idx2) = (idx2, idx1);
                        newSolution.GTR = TabuSearch.TwoOptMove(newSolution.GTR, idx1, idx2);
                    }
                    else
                    {
                        ErrorEventArgs error = new ErrorEventArgs(new Exception("Invalid move operator"));
                        throw error.GetException();
                    }

                        newSolution = Utils.calculateMetricsv2(newSolution, instance);
                    newSolution.move = (idx1, idx2);
                    newSolution = crewMutation(newSolution, instance);

                    if (newSolution.GrandTotal < population[j].GrandTotal)
                    {
                        population[j] = newSolution;
                        fitnessValues[j] = newSolution.GrandTotal;
                        noImprovementCounter[j] = 0;
                        if (newSolution.GrandTotal < bestSolution.GrandTotal)
                        {
                            bestSolution = newSolution;
                            changes.Add(iter);
                        }
                    }
                    else
                    {
                        noImprovementCounter[j]++;
                    }
                }
                probability.Clear();
                for (int j = 0; j < beesNumber; j++)
                {
                    probability.Add(1 - (fitnessValues[j] / fitnessValues.Sum()));
                }

                //Onlooker Bees Phase
                double fitnessSum = fitnessValues.Sum();
                for (int j = 0; j < beesNumber; j++)
                {

                    var randomValue = rnd.NextDouble();

                    int bee = 0;
                    while (randomValue > 0)
                    {
                        randomValue -= probability[bee];
                        bee++;
                    }

                    var newSolution = new Solutionv2(population[bee]);
                    int idx1 = rnd.Next(1, length);
                    int idx2 = rnd.Next(1, length);
                    if (moveOperator == 0)
                        newSolution.GTR = TabuSearch.SwapMove(newSolution.GTR, idx1, idx2);
                    else if (moveOperator == 1)
                        newSolution.GTR = TabuSearch.InsertMove(newSolution.GTR, idx1, idx2);
                    else if (moveOperator == 2)
                    {
                        if (idx1 > idx2)
                            (idx1, idx2) = (idx2, idx1);
                        newSolution.GTR = TabuSearch.ReverseMove(newSolution.GTR, idx1, idx2);
                    }
                    else if (moveOperator == 3)
                    {
                        if (idx1 > idx2)
                            (idx1, idx2) = (idx2, idx1);
                        newSolution.GTR = TabuSearch.TwoOptMove(newSolution.GTR, idx1, idx2);
                    }
                    else
                    {
                        ErrorEventArgs error = new ErrorEventArgs(new Exception("Invalid move operator"));
                        throw error.GetException();
                    }
                    newSolution = Utils.calculateMetricsv2(newSolution, instance);
                    newSolution.move = (idx1, idx2);
                    newSolution = crewMutation(newSolution, instance);

                    if (newSolution.GrandTotal < population[bee].GrandTotal)
                    {
                        population[bee] = newSolution;
                        fitnessValues[bee] = newSolution.GrandTotal;
                        noImprovementCounter[bee] = 0;
                        if (newSolution.GrandTotal < bestSolution.GrandTotal)
                        {
                            bestSolution = newSolution;
                            changes.Add(iter);
                        }
                    }
                    else
                    {
                        noImprovementCounter[bee]++;
                    }
                    
                }

                //Scout Bees Phase
                for (int j = 0; j < beesNumber; j++)
                {
                    if (noImprovementCounter[j] >= noImprovementLimit)
                    {
                        var newSolution = new Solutionv2(currSolution);
                        newSolution.GTR = TabuSearch.RandomShuttle(newSolution.GTR);
                        newSolution = Utils.calculateMetricsv2(newSolution, instance);
                        population[j] = newSolution;
                        fitnessValues[j] = newSolution.GrandTotal;
                        noImprovementCounter[j] = 0;
                        //Console.WriteLine("Scout bee replaced solution " + j + " with new solution: " + newSolution.GrandTotal);
                        if (newSolution.GrandTotal < bestSolution.GrandTotal)
                        {
                            changes.Add(iter);
                            bestSolution = newSolution;
                        }
                    }
                }
                iter++;
            }

            if (isLogging)
            {
                Console.WriteLine("GREEDY: " + "N/a" + " TABU: " + bestSolution.GrandTotal + " " + "N/A" + " operator:" + operators[moveOperator]);
                Console.WriteLine("Changes at iterations: " + string.Join(", ", changes));
            }
            return bestSolution;
        }
    }
}
