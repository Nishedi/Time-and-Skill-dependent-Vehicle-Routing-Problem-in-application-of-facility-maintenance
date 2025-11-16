using HCVRPTW;
using System.Diagnostics;

var filePath = "pliki//CTEST.txt";
//string[] filePaths = new string[] { /*"pliki//100 lokacji//C101.txt",*/ "pliki//100 lokacji//C201.txt", "pliki//100 lokacji//R101.txt", "pliki//100 lokacji//R201.txt",
//"pliki//100 lokacji//RC101.txt", "pliki//100 lokacji//RC201.txt"};
////filePath = "pliki//100 lokacji//C101.txt";
////string[] filePaths = new string [] {filePath};
//int maxTabuSize = 50;
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
//            var currentSolution = TabuSearch.RunTabuSearch(instance, 100000, tabuSize, operatorMove);

//            if (bestSolution == null || currentSolution.GrandTotal < bestSolution.GrandTotal)
//            {
//                bestSolution = currentSolution;
//                bestTabuSize = tabuSize;
//                bestOperator = operatorMove;
//            }

//        }
//    }
//    Console.WriteLine("Best tabu size: "+bestTabuSize + " Best tabu operator: " + bestOperator + ": " + bestSolution.GrandTotal);
//    Console.WriteLine("----------------------");
//}

;

Instance instance2 = new Instance(filePath, vehicleCapacity: 90);

GurobiVRP gurobi = new GurobiVRP();
var gurobiResult = gurobi.gurobi_test(instance2);
Console.WriteLine("Gurobi Result: " + gurobiResult.Item1);
//for (int i = 0; i < 5; i++)
//     TabuSearch.RunTabuSearch(instance2, 100000, 10,i);
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