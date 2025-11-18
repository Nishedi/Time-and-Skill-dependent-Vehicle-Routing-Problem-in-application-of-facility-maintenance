using HCVRPTW;
using System.Diagnostics;
using System.Globalization;

//var filePath = "pliki//CTEST.txt";
//string[] filePaths = new string[] { "pliki//100 lokacji//C101.txt", "pliki//100 lokacji//C201.txt", "pliki//100 lokacji//R101.txt", "pliki//100 lokacji//R201.txt",
//"pliki//100 lokacji//RC101.txt", "pliki//100 lokacji//RC201.txt"};
//filePath = "pliki//100 lokacji//C101.txt";
////string[] filePaths = new string[] { filePath };
//int maxTabuSize = 10;
//int maxN = 1;
//foreach (string filePath1 in filePaths)
//{
//    Console.WriteLine(filePath1);
//    Instance instance = new Instance(filePath1, vehicleCapacity: 90);
//    var bestTabuSize = 10;
//    var bestOperator = 0;
//    Solutionv2 bestSolution = null;
//    for (int operatorMove = 0; operatorMove < 1; operatorMove++)
//    {
//        for (int tabuSize = 10; tabuSize <= maxTabuSize; tabuSize += 20)
//        {
//            var currentSolution = TabuSearch.RunTabuSearch(instance, 100, tabuSize, operatorMove);

//            //Console.WriteLine("Gurobi Result: " + gurobiResult.Item1);

//        }
//    }
//    //Console.WriteLine("Best tabu size: " + bestTabuSize + " Best tabu operator: " + bestOperator + ": " + bestSolution.GrandTotal);
//    Console.WriteLine("----------------------");
//}

//var filePath = "pliki//100 lokacji//C201.txt";
//////var filePath = @"C:\Users\radek\Source\Repos\Time-and-Skill-dependent-Vehicle-Routing-Problem-in-application-of-facility-maintenance\HCVRPTW\pliki\CTEST.txt";
//Instance instance2 = new Instance(filePath, vehicleCapacity: 90, numberOfCrews: 50);

////GurobiVRP gurobi = new GurobiVRP();
////var gurobiResult = gurobi.gurobi_test(instance2);
////Console.WriteLine("Gurobi Result: " + gurobiResult.Item1);
//for (int i = 0; i < 1; i++)
//{
//    var stopwatch = Stopwatch.StartNew();
//    TabuSearch.RunTabuSearch(instance2, 100, 10, i, isParallel: false, maxTime: 600);
//    stopwatch.Stop();
//    Console.WriteLine("Time elapsed Tabu Search: " + stopwatch.ElapsedMilliseconds + " ms");

//    stopwatch = Stopwatch.StartNew();
//    ArtificialBeeColony.Run(instance2, 10000, 50, 50, i, maxTime:600);
//    stopwatch.Stop();
//    Console.WriteLine("Time elapsed ABC: " + stopwatch.ElapsedMilliseconds + " ms");

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


//var stopwatch = Stopwatch.StartNew();
//TabuTuning.RunFullTabuTuning();
//stopwatch.Stop();
//Console.WriteLine("time: " + stopwatch.ElapsedMilliseconds);

public static class Program
{
    static void Main(string[] args)
    {
        var exp = new AllFilesExperiments(args[0], args[1], int.Parse(args[2]), int.Parse(args[3]));
        









        // STEROWANIE
        //string filename = args[0];
        //string choice = args[1];
        //if(choice == "tabu")
        //{
        //    Console.WriteLine("Starting Tabu Tuning..." + args[0]);
        //    var stopwatch = Stopwatch.StartNew();
        //    TabuTuning.RunFullTabuTuning(filename);
        //    stopwatch.Stop();
        //    Console.WriteLine("time: " + stopwatch.ElapsedMilliseconds);
        //}
        //else if(choice == "bee")
        //{
        //    Console.WriteLine("Starting Bee Tuning..." + args[0]);
        //    var stopwatch = Stopwatch.StartNew();
        //    BeeTuning.RunBeeTuningSequential(filename);
        //    stopwatch.Stop();
        //    Console.WriteLine("time: " + stopwatch.ElapsedMilliseconds);
        //}
        //else
        //{
        //    Console.WriteLine("Invalid choice. Please specify 'tabu' or 'bee'.");
        //}
        //Console.WriteLine("Starting Bee Tuning..." + args[0]);
        //var stopwatch = Stopwatch.StartNew();
        //BeeTuning.RunBeeTuningSequential();
        //stopwatch.Stop();
        //Console.WriteLine("time: " + stopwatch.ElapsedMilliseconds);
    }
}


public class AllFilesExperiments
{
    string[] C1fileNames = {
        "100 lokacji/C101.txt", "100 lokacji/C102.txt", "100 lokacji/C103.txt",
        "100 lokacji/C104.txt", "100 lokacji/C105.txt", "100 lokacji/C106.txt", "100 lokacji/C107.txt",
        "100 lokacji/C108.txt", "100 lokacji/C109.txt", };

    string[] C2fileNames = {
    "100 lokacji/C201.txt", "100 lokacji/C202.txt",
        "100 lokacji/C203.txt", "100 lokacji/C204.txt", "100 lokacji/C205.txt", "100 lokacji/C206.txt",
        "100 lokacji/C207.txt", "100 lokacji/C208.txt", };

    string[] R1fileNames = {
    "100 lokacji/R101.txt", "100 lokacji/R102.txt",
        "100 lokacji/R103.txt", "100 lokacji/R104.txt", "100 lokacji/R105.txt", "100 lokacji/R106.txt",
        "100 lokacji/R107.txt", "100 lokacji/R108.txt", "100 lokacji/R109.txt", "100 lokacji/R110.txt",
        "100 lokacji/R111.txt", "100 lokacji/R112.txt", };
    string[] R2fileNames = {
    "100 lokacji/R201.txt", "100 lokacji/R202.txt",
        "100 lokacji/R203.txt", "100 lokacji/R204.txt", "100 lokacji/R205.txt", "100 lokacji/R206.txt",
        "100 lokacji/R207.txt", "100 lokacji/R208.txt", "100 lokacji/R209.txt", "100 lokacji/R210.txt",
        "100 lokacji/R211.txt", };
    string[] RC1fileNames = {
    "100 lokacji/RC101.txt", "100 lokacji/RC102.txt", "100 lokacji/RC103.txt",
        "100 lokacji/RC104.txt", "100 lokacji/RC105.txt", "100 lokacji/RC106.txt", "100 lokacji/RC107.txt",
        "100 lokacji/RC108.txt", };
    string[] RC2fileNames = {
    "100 lokacji/RC201.txt", "100 lokacji/RC202.txt", "100 lokacji/RC203.txt",
        "100 lokacji/RC204.txt", "100 lokacji/RC205.txt", "100 lokacji/RC206.txt", "100 lokacji/RC207.txt", "100 lokacji/RC208.txt"
    };
    int moveOperator;
    int numberOfRepetition;
    string[] choosenFileNames;
    string fileTypes;
    int duration;

    public AllFilesExperiments(string fileTypes, string choosenAlgorithm, int numberOfRepetition, int duration)
    {
        this.numberOfRepetition = numberOfRepetition;
        this.fileTypes = fileTypes;
        this.duration = duration;
        if (fileTypes == "C1")
        {
            moveOperator = 0;
            choosenFileNames = C1fileNames;
        }
        else if (fileTypes == "C2")
        {
            moveOperator = 1;
            choosenFileNames = C2fileNames;
        }
        else if (fileTypes == "R1")
        {
            moveOperator = 1;
            choosenFileNames = R1fileNames;
        }
        else if (fileTypes == "R2")
        {
            moveOperator = 1;
            choosenFileNames = R2fileNames;
        }
        else if (fileTypes == "RC1")
        {
            moveOperator = 1;
            choosenFileNames = RC1fileNames;
        }
        else if (fileTypes == "RC2")
        {
            moveOperator = 1;
            choosenFileNames = RC2fileNames;
        }
        if (choosenAlgorithm == "tabu")
        {
            Console.WriteLine("Starting Tabu Experiments for file type: " + fileTypes);
            var stopwatch = Stopwatch.StartNew();
            runTabuExperiments();
            stopwatch.Stop();
            Console.WriteLine("End Tabu Experiments. Time: " + stopwatch.ElapsedMilliseconds);
        }
        else if (choosenAlgorithm == "bee")
        {
            Console.WriteLine("Starting Bee Experiments for file type: " + fileTypes);
            var stopwatch = Stopwatch.StartNew();
            runBeeExperiments();
            stopwatch.Stop();
            Console.WriteLine("End Bee Experiments. Time: " + stopwatch.ElapsedMilliseconds);
        }

        void runBeeExperiments()
        {
            foreach (var fileName in choosenFileNames)
            {
                string csvPath = $"bee_results_{fileTypes}.csv";
                // jeśli plik nie istnieje → dodaj nagłówek
                if (!File.Exists(csvPath))
                {
                    File.AppendAllText(csvPath,
                        "File,Iterations,BeesNumber,Limit,Operator,MaxTime,TotalCost,TotalPenalty,DrivingCost,AfterHoursCost,CrewCost,GrandTotal,ExecutionTime\n");
                }
                Solutionv2 bestSol = null;
                for (int i = 0; i < numberOfRepetition; i++)
                {
                    Instance instance = new Instance("pliki//" + fileName, vehicleCapacity: 90, numberOfCrews: 50);
                    Solutionv2 sol = ArtificialBeeColony.Run(
                        instance,
                        iterations: 100,
                        noImprovementLimit: 40,
                        beesNumber: 75,
                        moveOperator: this.moveOperator,
                        maxTime: duration,
                        isLogging: true
                    );
                    if (bestSol == null || sol.GrandTotal < bestSol.GrandTotal)
                    {
                        bestSol = sol;
                    }
                }
                string line = string.Join(",",
                    Path.GetFileName(fileName),
                    100,
                    75,
                    40,
                    moveOperator,
                    duration,
                    bestSol.TotalCost.ToString(CultureInfo.InvariantCulture),
                    bestSol.TotalPenalty.ToString(CultureInfo.InvariantCulture),
                    bestSol.TotalDrivingCost.ToString(CultureInfo.InvariantCulture),
                    bestSol.TotalAfterHoursCost.ToString(CultureInfo.InvariantCulture),
                    bestSol.TotalCrewUsageCost.ToString(CultureInfo.InvariantCulture),
                    bestSol.GrandTotal.ToString(CultureInfo.InvariantCulture),
                    string.Join(";", bestSol.GTR.Select(x => x.Id)),
                    "N/A"
                );
            }
        }

        void runTabuExperiments()
        {
            foreach (string fileName in choosenFileNames)
            {
                string csvPath = $"tabu_results_{fileTypes}.csv";

                // jeśli plik nie istnieje → dodaj nagłówek
                if (!File.Exists(csvPath))
                {
                    File.AppendAllText(csvPath,
                        "File,Iterations,TabuSize,Operator,MaxTime,BestCost,TotalPenalty,DrivingCost,AfterHoursCost,CrewCost,GrandTotal,GTR,ExecutionTime\n");
                }
                Solutionv2 bestSol = null;
                for (int i = 0; i < numberOfRepetition; i++)
                {
                    Instance instance = new Instance("pliki//" + fileName, vehicleCapacity: 90, numberOfCrews: 50);
                    Solutionv2 sol = TabuSearch.RunTabuSearch(
                        instance,
                        iterations: 100,
                        tabuSize: 80,
                        moveOperator: this.moveOperator,
                        maxTime: duration,
                        isLogging: true,
                        isParallel: false
                    );
                    if (bestSol == null || sol.GrandTotal < bestSol.GrandTotal)
                    {
                        bestSol = sol;
                    }
                }
                string line = string.Join(",",
                    Path.GetFileName(fileName),
                    100,
                    80,
                    moveOperator,
                    600,
                    bestSol.TotalCost.ToString(CultureInfo.InvariantCulture),
                    bestSol.TotalPenalty.ToString(CultureInfo.InvariantCulture),
                    bestSol.TotalDrivingCost.ToString(CultureInfo.InvariantCulture),
                    bestSol.TotalAfterHoursCost.ToString(CultureInfo.InvariantCulture),
                    bestSol.TotalCrewUsageCost.ToString(CultureInfo.InvariantCulture),
                    string.Join(";", bestSol.GTR.Select(x => x.Id)),
                    bestSol.GrandTotal.ToString(CultureInfo.InvariantCulture),
                    "N/A"
                );
                File.AppendAllText(csvPath, line + Environment.NewLine);
            }
        }
    }

    public class TabuTuning
    {
        public static void RunFullTabuTuning(string filename)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string[] filePaths = new string[]
            {
            "pliki//100 lokacji//"+filename+".txt"
                //"pliki//100 lokacji//C101.txt",
                //"pliki//100 lokacji//C201.txt",
                //"pliki//100 lokacji//R101.txt",
                //"pliki//100 lokacji//R201.txt",
                //"pliki//100 lokacji//RC101.txt",
                //"pliki//100 lokacji//RC201.txt"
            };

            // ❗ PARAMETRY DO PRZETESTOWANIA
            int[] iterationsList = { 100 };
            int[] tabuSizes = { 20, 40, 80 };
            int[] moveOperators = { 0, 1, 2, 3 };
            int[] maxTimes = { 600 };

            string csvPath = $"tabu_results_{filename}.csv";

            // jeśli plik nie istnieje → dodaj nagłówek
            if (!File.Exists(csvPath))
            {
                File.AppendAllText(csvPath,
                    "File,Iterations,TabuSize,Operator,MaxTime,BestCost,TotalPenalty,DrivingCost,AfterHoursCost,CrewCost,GrandTotal,ExecutionTime\n");
            }

            int totalRuns =
                filePaths.Length *
                iterationsList.Length *
                tabuSizes.Length *
                moveOperators.Length *
                maxTimes.Length;

            int currentRun = 0;

            Console.WriteLine("=== START PARAMETER TUNING TABU SEARCH ===");
            Console.WriteLine($"Total experiment runs: {totalRuns}");
            Console.WriteLine("==========================================");
            String[] operators = new String[] { "SwapMove", "InsertMove", "ReverseMove", "TwoOptMove", "BlockSwapMOve", "BlockShiftMove" };


            foreach (string path in filePaths)
            {
                Instance instance = new Instance(path, vehicleCapacity: 90, numberOfCrews: 50);

                foreach (int iterations in iterationsList)
                    foreach (int tabuSize in tabuSizes)
                        foreach (int op in moveOperators)
                            foreach (int maxTime in maxTimes)
                            {

                                currentRun++;
                                double progress = (double)currentRun / totalRuns * 100;

                                Console.Write(
                                    $"\rProgress: {currentRun}/{totalRuns} ({progress:0.0}%)    Running: {Path.GetFileName(path)} " +
                                    $"iter={iterations} tabu={tabuSize} op={op} time={maxTime}s"
                                );

                                var sw = Stopwatch.StartNew();

                                Solutionv2 sol = TabuSearch.RunTabuSearch(
                                    instance,
                                    iterations,
                                    tabuSize,
                                    op,
                                    maxTime,
                                    isLogging: false,
                                    isParallel: false
                                );

                                sw.Stop();

                                string line = string.Join(",",
                                    Path.GetFileName(path),
                                    iterations,
                                    tabuSize,
                                    operators[op],
                                    maxTime,
                                    sol.TotalCost.ToString(CultureInfo.InvariantCulture),
                                    sol.TotalPenalty.ToString(CultureInfo.InvariantCulture),
                                    sol.TotalDrivingCost.ToString(CultureInfo.InvariantCulture),
                                    sol.TotalAfterHoursCost.ToString(CultureInfo.InvariantCulture),
                                    sol.TotalCrewUsageCost.ToString(CultureInfo.InvariantCulture),
                                    sol.GrandTotal.ToString(CultureInfo.InvariantCulture),
                                    sw.ElapsedMilliseconds.ToString()
                                );

                                File.AppendAllText(csvPath, line + Environment.NewLine);
                            }
            }

            Console.WriteLine("\n\n=== FINISHED ===");
            Console.WriteLine("Results saved to: tabu_results.csv");
        }
        public static void RunFullTabuTuningParallel()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            string[] filePaths = new string[]
            {
        "pliki//100 lokacji//C101.txt",
        "pliki//100 lokacji//C201.txt",
        "pliki//100 lokacji//R101.txt",
        "pliki//100 lokacji//R201.txt",
        "pliki//100 lokacji//RC101.txt",
        "pliki//100 lokacji//RC201.txt"
            };

            int[] iterationsList = { 100 };
            int[] tabuSizes = { 20, 40, 80 };
            int[] moveOperators = { 0, 1, 2, 3 };
            int[] maxTimes = { 600 };

            string csvPath = "tabu_results.csv";
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = 4
            };

            // --------------------------
            //  Zabezpieczenie pliku CSV
            // --------------------------
            object csvLock = new object();

            if (!File.Exists(csvPath))
            {
                File.AppendAllText(csvPath,
                    "File,Iterations,TabuSize,Operator,MaxTime,TotalCost,TotalPenalty,DrivingCost,AfterHoursCost,CrewCost,GrandTotal,ExecutionTime\n");
            }

            // Tworzymy pełną listę zadań do wykonania równolegle
            var tasks = new List<(string file, int it, int tabu, int op, int max)>();

            foreach (string file in filePaths)
                foreach (int it in iterationsList)
                    foreach (int tabu in tabuSizes)
                        foreach (int op in moveOperators)
                            foreach (int max in maxTimes)
                                tasks.Add((file, it, tabu, op, max));

            int totalRuns = tasks.Count;
            int currentRun = 0;

            Console.WriteLine($"Total runs: {totalRuns}");

            // --------------------------
            //  Wersja równoległa
            // --------------------------
            Console.WriteLine("number of cpus: " + Environment.ProcessorCount + "-5");
            Parallel.ForEach(tasks, new ParallelOptions { MaxDegreeOfParallelism = 4 }, task =>
            {
                var (file, iterations, tabuSize, op, maxTime) = task;

                Interlocked.Increment(ref currentRun);
                double progress = (double)currentRun / totalRuns * 100;

                Console.Write(
                    $"\rProgress: {currentRun}/{totalRuns} ({progress:0.0}%)  Running: {Path.GetFileName(file)}  " +
                    $"it={iterations} tabu={tabuSize} op={op} t={maxTime}|"
                );

                Instance instance = new Instance(file, vehicleCapacity: 90, numberOfCrews: 50);

                var sw = Stopwatch.StartNew();
                Solutionv2 sol = TabuSearch.RunTabuSearch(
                    instance,
                    iterations,
                    tabuSize,
                    op,
                    maxTime,
                    isLogging: false,
                    isParallel: true
                );
                sw.Stop();

                // --------------------------
                //     Zapis do CSV
                // --------------------------
                lock (csvLock)
                {
                    string line = string.Join(",",
                        Path.GetFileName(file),
                        iterations,
                        tabuSize,
                        op,
                        maxTime,
                        sol.TotalCost.ToString(CultureInfo.InvariantCulture),
                        sol.TotalPenalty.ToString(CultureInfo.InvariantCulture),
                        sol.TotalDrivingCost.ToString(CultureInfo.InvariantCulture),
                        sol.TotalAfterHoursCost.ToString(CultureInfo.InvariantCulture),
                        sol.TotalCrewUsageCost.ToString(CultureInfo.InvariantCulture),
                        sol.GrandTotal.ToString(CultureInfo.InvariantCulture),
                        sw.ElapsedMilliseconds.ToString()
                    );

                    File.AppendAllText(csvPath, line + Environment.NewLine);
                }
            });

            Console.WriteLine("\n\n=== FINISHED PARALLEL RUN ===");
        }
    }


    public class BeeTuning
    {
        public static void RunBeeTuningSequential(string filename)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            string[] filePaths = new string[]
            {
            "pliki//100 lokacji//"+filename+".txt"
                //"pliki//100 lokacji//C101.txt",
                //"pliki//100 lokacji//C201.txt",
                //"pliki//100 lokacji//R101.txt",
                //"pliki//100 lokacji//R201.txt",
                //"pliki//100 lokacji//RC101.txt",
                //"pliki//100 lokacji//RC201.txt"
            };

            // ❗ PARAMETRY dla ABC – możesz tu dodać więcej
            int[] iterationsList = { 100 };
            int[] beesNumberList = { 25, 50, 75 };
            int[] noImprovementLimitList = { 20, 40 };
            int[] moveOperators = { 0, 1, 2, 3 };
            int[] maxTimes = { 600 };

            string csvPath = $"bee_results_{filename}.csv";
            string[] operators = new string[] { "SwapMove", "InsertMove", "ReverseMove", "TwoOptMove" };

            // nagłówek CSV
            if (!File.Exists(csvPath))
            {
                File.AppendAllText(csvPath,
                    "File,Iterations,BeesNumber,Limit,Operator,MaxTime," +
                    "TotalCost,TotalPenalty,DrivingCost,AfterHoursCost,CrewCost,GrandTotal,ExecutionTime\n");
            }

            int totalRuns =
                filePaths.Length *
                iterationsList.Length *
                beesNumberList.Length *
                noImprovementLimitList.Length *
                moveOperators.Length *
                maxTimes.Length;

            int currentRun = 0;

            Console.WriteLine("=== START PARAMETER TUNING ABC ===");
            Console.WriteLine($"Total experiment runs: {totalRuns}");
            Console.WriteLine("==========================================");


            foreach (string path in filePaths)
            {
                Instance instance = new Instance(path, vehicleCapacity: 90, numberOfCrews: 50);

                foreach (int iterations in iterationsList)
                    foreach (int beesNumber in beesNumberList)
                        foreach (int limit in noImprovementLimitList)
                            foreach (int op in moveOperators)
                                foreach (int maxTime in maxTimes)
                                {
                                    currentRun++;
                                    double progress = (double)currentRun / totalRuns * 100;

                                    Console.Write(
                                        $"\rProgress: {currentRun}/{totalRuns} ({progress:0.0}%)    Running: {Path.GetFileName(path)} " +
                                        $"iter={iterations} bees={beesNumber} limit={limit} op={op} time={maxTime}s"
                                    );

                                    var sw = Stopwatch.StartNew();

                                    Solutionv2 sol = ArtificialBeeColony.Run(
                                        instance,
                                        iterations,
                                        limit,
                                        beesNumber,
                                        op,
                                        maxTime,
                                        isLogging: false
                                    );

                                    sw.Stop();

                                    string line = string.Join(",",
                                        Path.GetFileName(path),
                                        iterations,
                                        beesNumber,
                                        limit,
                                        operators[op],
                                        maxTime,
                                        sol.TotalCost.ToString(CultureInfo.InvariantCulture),
                                        sol.TotalPenalty.ToString(CultureInfo.InvariantCulture),
                                        sol.TotalDrivingCost.ToString(CultureInfo.InvariantCulture),
                                        sol.TotalAfterHoursCost.ToString(CultureInfo.InvariantCulture),
                                        sol.TotalCrewUsageCost.ToString(CultureInfo.InvariantCulture),
                                        sol.GrandTotal.ToString(CultureInfo.InvariantCulture),
                                        sw.ElapsedMilliseconds
                                    );

                                    File.AppendAllText(csvPath, line + Environment.NewLine);
                                }
            }

            Console.WriteLine("\n\n=== FINISHED ABC TUNING ===");
            Console.WriteLine("Results saved to: bee_results.csv");
        }
    }
}
