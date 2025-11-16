using Gurobi;
using HCVRPTW;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GurobiVRP


{
    public List<List<int>> subSets = new List<List<int>>();
    private static GRBEnv env;
    public GurobiVRP()
    {

        env = new GRBEnv();
        env.Set(GRB.IntParam.OutputFlag, 0);
    }

    // Function that generates all possible combinations of customers
    public void GetCombination(List<Location> customers)
    {
        List<int> list = new List<int>();
        foreach (var loc in customers)
        {
            if (loc.Id != 0)
                list.Add(loc.Id);
        }
        double count = Math.Pow(2, list.Count);
        for (int i = 1; i <= count - 1; i++)
        {
            string str = Convert.ToString(i, 2).PadLeft(list.Count, '0');
            List<int> subSet = new List<int>();
            for (int j = 0; j < str.Length; j++)
            {
                if (str[j] == '1')
                {
                    subSet.Add(list[j]);
                }
            }
            if (subSet.Count > 1)
                subSets.Add(subSet);
        }
    }

    // Subset printer for testing purposes
    public void printSubSets()
    {
        foreach (var subSet in subSets)
        {
            foreach (var item in subSet)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
        }
    }
    public Tuple<double, double> gurobi_test(Instance problem)
    {
        // Generate all possible combinations
        GetCombination(problem.Locations);
        // Create a new model
        GRBModel model = new GRBModel(env);
        // Set time limit
        double minutes = 1;
        model.Set(GRB.DoubleParam.TimeLimit, minutes * 60);
        // Definition of variables
        int locationsNumber = problem.Locations.Count;
        GRBVar[,,] x = new GRBVar[locationsNumber, locationsNumber, problem.numberOfCrews];
        GRBVar[,] y = new GRBVar[locationsNumber, problem.numberOfCrews];
        GRBVar[,] t = new GRBVar[locationsNumber, problem.numberOfCrews];
        GRBVar[,] p = new GRBVar[locationsNumber, problem.numberOfCrews];
        GRBVar[,] q = new GRBVar[locationsNumber, problem.numberOfCrews];
        GRBVar[,] wait = new GRBVar[locationsNumber, problem.numberOfCrews];
        GRBVar[] h = new GRBVar[problem.numberOfCrews];
        GRBVar[] a = new GRBVar[problem.numberOfCrews];
        foreach(var v in problem.Crews)
            Console.WriteLine("Crew " + v.Id + " start time: " + v.WorkingTimeWindow.startTime);
        // Create variable x[i, j, v]
        for (int i = 0; i < locationsNumber; i++)
        {
            for (int j = 0; j < locationsNumber; j++)
            {
                for (int v = 0; v < problem.numberOfCrews; v++)
                {
                    x[i, j, v] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "x_" + i + "_" + j + "_" + v);
                }
            }
        }

        // Create variable y[v, i]
        for (int i = 1; i < locationsNumber; i++)
        {
            for (int v = 0; v < problem.numberOfCrews; v++)
            {
                y[i, v] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "y_" + i + "_" + v);

            }
        }

        // Create variable t[v, i]
        // Limited by working hours of given crew
        for (int i = 0; i < locationsNumber; i++)
        {
            for (int v = 0; v < problem.numberOfCrews; v++)
            {
                t[i, v] = model.AddVar(problem.Crews[v].WorkingTimeWindow.startTime, 20000, 0.0, GRB.INTEGER, "t_" + i + "_" + v);
            }
        }

        // Create variable p[v, i] - penalty for arrival before time window (penalty)
        // Limited by total service time
        for (int i = 0; i < locationsNumber; i++)
        {
            for (int v = 0; v < problem.numberOfCrews; v++)
            {
                p[i, v] = model.AddVar(0.0, 2 * problem.Locations[0].TimeWindow.End, 0.0, GRB.INTEGER, "p_" + i + "_" + v);

            }
        }

        // Create variable q[v, i] - penalty for arrival after time window (penalty)
        // Limited by total service time
        for (int i = 0; i < locationsNumber; i++)
        {
            for (int v = 0; v < problem.numberOfCrews; v++)
            {
                q[i, v] = model.AddVar(0.0, 2 * problem.Locations[0].TimeWindow.End, 0.0, GRB.INTEGER, "q_" + i + "_" + v);
            }
        }

        // Create variable wait[v, i] - a time spent before location to avoid penalty
        for (int i = 0; i < locationsNumber; i++)
        {
            for (int v = 0; v < problem.numberOfCrews; v++)
            {
                wait[i, v] = model.AddVar(0.0, 0.0, 0.0, GRB.INTEGER, "wait_" + i + "_" + v);
            }
        }

        // Create variable h[v] - working hours for each crew

        for (int v = 0; v < problem.numberOfCrews; v++)
        {
            h[v] = model.AddVar(0.0, 2 * problem.Locations[0].TimeWindow.End, 0.0, GRB.INTEGER, "h_" + v);
        }

        // Create variable a[v] - overhours for each crew

        for (int v = 0; v < problem.numberOfCrews; v++)
        {
            a[v] = model.AddVar(0.0, 2 * problem.Locations[0].TimeWindow.End, 0.0, GRB.INTEGER, "a_" + v);
        }


        model.Update();

        // Set objective - minimize total distance and penalties -> OK
        GRBLinExpr expr = 0.0;
        for (int i = 0; i < locationsNumber; i++)
        {
            for (int j = 0; j < locationsNumber; j++)
            {
                for (int v = 0; v < problem.numberOfCrews; v++)
                {
                    // First factor
                    expr += x[i, j, v] * problem.DistanceMatrix[i, j];
                }
            }
        }
        for (int v = 0; v < problem.numberOfCrews; v++)
        {
            // Second factor
            expr += a[v] * problem.Crews[v].afterHoursCost;
            for (int i = 1; i < locationsNumber; i++)
            {
                // Third factor
                expr += problem.toEarlyPenaltyMultiplier * p[i, v];
                expr += problem.toLatePenaltyMultiplier * q[i, v];
                //expr += 100000 * wait[i, v];
                // Fourth factor
                expr += x[i, 0 , v] * problem.Crews[v].baseCost; 
            }
            
        }
        model.SetObjective(expr, GRB.MINIMIZE);




        // Constraint 1
        // Calculation of working hours h[v] for each crew
        
        for (int v = 0; v < problem.numberOfCrews; v++)
        {
            GRBLinExpr sum = 0.0;
            // Add travelling time
            for (int i = 0; i < locationsNumber; i++)
            {
                for (int j = 0; j < locationsNumber; j++)
                {
                    sum += x[i, j, v] * problem.DistanceMatrix[i, j];
                }
            }
            //Add service time 
            
            for (int i = 1; i < locationsNumber; i++)
            {
                sum += y[i, v] * problem.Locations[i].ServiceTime * problem.Crews[v].serviceTimeMultiplier;
            }
            for (int i = 0; i < locationsNumber; i++)
            {
                sum += wait[i, v];
            }

            // Checking if the time is less than the vehicle working time. Vehicle 0 is take, because fleet is homogeneous
            model.AddConstr(h[v], GRB.GREATER_EQUAL, sum, "c2");
        }

        // Constraint 2
        // Calculation of overhours a[v] for each crew

        for (int v = 0; v < problem.numberOfCrews; v++)
        {
            model.AddConstr(a[v], GRB.GREATER_EQUAL, h[v] - (problem.Crews[v].WorkingTimeWindow.endTime - problem.Crews[v].WorkingTimeWindow.startTime), "c2");
        }

        // Constraint 3
        // Capacity constraint
        
        for (int v = 0; v < problem.numberOfCrews; v++)
        {
            GRBLinExpr load = 0.0;
            for (int i = 1; i < locationsNumber; i++)
            {
                load += y[i, v] * problem.Locations[i].Demand;
            }
            model.AddConstr(load, GRB.LESS_EQUAL, problem.vehicleCapacity, "c3");
        }
        

        // Constraint 4
        // Assign each location to exactly one vehicle
        for (int i = 1; i < locationsNumber; i++)
        {
            GRBLinExpr sum = 0.0;
            for (int v = 0; v < problem.numberOfCrews; v++)
            {
                sum += y[i, v];
            }
            model.AddConstr(sum, GRB.EQUAL, 1.0, "c3");
        }

        // Constraint 5
        // Set decision variable y[v, i] based on x[i, j, v] (if an edge is pointing to location)
        for (int v = 0; v < problem.numberOfCrews; v++)
        {
            for (int i = 1; i < locationsNumber; i++)
            {
                GRBLinExpr sum = 0.0;
                for (int j = 0; j < locationsNumber; j++)
                {
                    sum += x[j, i, v];
                }
                // Less equal jesli jedna lokalizacja moze byc odwiedzona przez wiecej niz jedno auto
                model.AddConstr(y[i, v], GRB.EQUAL, sum, "c4");
            }
        }

        // Constraint 6
        // Assure that each used vehicle start from depot
        for (int v = 0; v < problem.numberOfCrews; v++)
        {
            for (int i = 1; i < locationsNumber; i++)
            {
                GRBLinExpr sum = 0.0;
                for (int j = 0; j < locationsNumber; j++)
                {
                    sum += x[0, j, v];
                }
                model.AddConstr(y[i, v], GRB.LESS_EQUAL, sum, "c5");
            }

        }

        // Constraint 7
        // Assure that each vehicle returns to depot
        for (int v = 0; v < problem.numberOfCrews; v++)
        {
            GRBLinExpr sum = 0.0;
            for (int i = 0; i < locationsNumber; i++)
            {
                sum += x[i, 0, v];
            }
            model.AddConstr(sum, GRB.LESS_EQUAL, 1.0, "c6");
        }

        // Constraint 8
        // A vehicle must not go to the same location
        for (int v = 0; v < problem.numberOfCrews; v++)
        {
            GRBLinExpr sum = 0.0;
            for (int i = 0; i < locationsNumber; i++)
            {
                sum += x[i, i, v];
            }
            model.AddConstr(sum, GRB.EQUAL, 0.0, "c7");
        }

        // Constraint 9
        // A vehicle must enter and leave a location
        for (int v = 0; v < problem.numberOfCrews; v++)
        {
            for (int i = 0; i < locationsNumber; i++)
            {
                GRBLinExpr sumEnter = 0.0;
                GRBLinExpr sumLeave = 0.0;
                for (int j = 0; j < locationsNumber; j++)
                {
                    sumEnter += x[i, j, v];
                    sumLeave += x[j, i, v];
                }
                model.AddConstr(sumEnter, GRB.EQUAL, sumLeave, "c8");
            }
        }

        // Constraint 10
        // No subtour constraint
        
        for (int v = 0; v < problem.numberOfCrews; v++)
        {
            foreach (var subSet in subSets)
            {
                GRBLinExpr sum = 0.0;
                foreach (var i in subSet)
                {
                    foreach (var j in subSet)
                    {
                        if (i != j)
                            sum += x[i, j, v];
                    }
                }
                model.AddConstr(sum, GRB.LESS_EQUAL, subSet.Count - 1, "c9");
            }
        }
        
        // Constraint 11
        // Set arrival time variables
        for (int v = 0; v < problem.numberOfCrews; v++)
        {
            for (int i = 0; i < locationsNumber; i++)
            {
                for (int j = 1; j < locationsNumber; j++)
                {
                    GRBLinExpr sum = t[i, v] + problem.DistanceMatrix[i, j] + (problem.Locations[i].ServiceTime * problem.Crews[v].serviceTimeMultiplier) - (10000 * (1 - x[i, j, v]));
                    model.AddConstr(sum, GRB.LESS_EQUAL, t[j, v], "c10");
                }
            }
        }


        // Constraint 12
        // Calculation of penalty p variable, that counts too early arrival time
        for (int v = 0; v < problem.numberOfCrews; v++)
        {
            for (int i = 1; i < locationsNumber; i++)
            {
                GRBLinExpr sum = problem.Locations[i].TimeWindow.Start - t[i, v] - 10000 * (1 - y[i, v]);
                model.AddConstr(p[i, v], GRB.GREATER_EQUAL, sum, "c11");

            }
        }

        // Constraint 13
        // Calculation of penalty p variable, that counts too late service finish time
        for (int v = 0; v < problem.numberOfCrews; v++)
        {
            for (int i = 1; i < locationsNumber; i++)
            {
                GRBLinExpr sum = t[i, v] + (problem.Locations[i].ServiceTime * problem.Crews[v].serviceTimeMultiplier) - problem.Locations[i].TimeWindow.End - (10000 * (1 - y[i, v]));
                model.AddConstr(q[i, v], GRB.GREATER_EQUAL, sum, "c12");
            }
        }

        // Constraint 14
        // Calculation of waiting time before services, that may be done by vehicle to avoid penalty for early arrival
        for (int v = 0; v < problem.numberOfCrews; v++)
        {
            for (int i = 0; i < locationsNumber; i++)
            {
                for (int j = 1; j < locationsNumber; j++)
                {
                    GRBLinExpr sum = t[j, v] - (problem.Locations[i].ServiceTime * problem.Crews[v].serviceTimeMultiplier) - problem.DistanceMatrix[i, j] - t[i, v] - (10000 * (1 - x[i, j, v]));
                    model.AddConstr(wait[i, v], GRB.GREATER_EQUAL, sum, "c13");
                }
            }
        }



        // Optimize model
        model.Optimize();

        // Print model variables - to comment for experiments
        GRBVar[] vars = model.GetVars();
        //foreach (var v in vars)
        //    Console.WriteLine(v.VarName + " = " + v.X);

        // Goal function value and operation time in seconds
        double fCelu = model.ObjVal;
        double operationTime = model.Runtime;
        //model.Write("model.lp");
        // Dispose (Clean) of model and environment
        model.Dispose();
        env.Dispose();
        return new Tuple<double, double>(fCelu, operationTime);
    }
}