using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCVRPTW
{
    internal class Utils
    {
        public static Solution calculateMetrics(List<Tour> tours, Instance instance)
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
                double currentLoad = 0.0;

                foreach (var loc in tour.Locations)
                {
                    if (loc.Id != 0)
                    {
                        var dist = instance.DistanceMatrix[prevLocation.Id, loc.Id];
                        penalty += Math.Max(0, loc.TimeWindow.Start - (crew.WorkTime + dist)) * instance.toEarlyPenaltyMultiplier;
                        penalty += Math.Max(0, (crew.WorkTime + dist + loc.ServiceTime * crew.serviceTimeMultiplier) - loc.TimeWindow.End) * instance.toLatePenaltyMultiplier;
                        crew.WorkTime += loc.ServiceTime * crew.serviceTimeMultiplier + dist;
                        drivingCost += dist;
                        currentLoad += loc.Demand;
                    }
                    else if (prevLocation != null)
                    {
                        var dist = instance.DistanceMatrix[prevLocation.Id, loc.Id];
                        crew.WorkTime += dist;
                        drivingCost += dist;
                    }
                    prevLocation = loc;
                }
                if (currentLoad > instance.vehicleCapacity) return new Solution(tours, Double.MaxValue, Double.MaxValue, Double.MaxValue, Double.MaxValue, Double.MaxValue);
                crew.computeAfterHours();
                double sum = penalty + drivingCost + crew.afterHoursWorkTime * crew.afterHoursCost + crew.baseCost;//100 - cost of using the crew
                totalPenalty += penalty;
                totalDrivingCost += drivingCost;
                totalAfterHoursCost += crew.afterHoursWorkTime * crew.afterHoursCost;
                totalCrewUsageCost += 100;
                grandTotal += sum;
            }
            return new Solution(tours, totalPenalty, totalDrivingCost, totalAfterHoursCost, totalCrewUsageCost, grandTotal);

        }

        public static Solution generateGreedySolution(Instance instance) //wprowadzic czekanie jezeli sie oplaca
        {
            var greedyGTR = createGreedyGTR(instance);
            var result = new List<Tour>();
            var current = new List<Location>();
            int numRoutes = 0;
            foreach (var loc in greedyGTR) //split tras względem 0
            {
                current.Add(loc); // dodanie lokalizacji
                if (loc.Id == 0 && current.Count > 1)
                {
                    result.Add(new Tour(instance.Crews[numRoutes], new List<Location>(current)));
                    current.Clear();
                    current.Add(loc); 
                    numRoutes++;
                }
            }
            // Jeśli coś zostało, dodaj jako ostatnią trasę
            if (current.Count > 1)
                result.Add(new Tour(instance.Crews[numRoutes], new List<Location>(current)));

            return new Solution(result);
        }
        private static List<Location> createGreedyGTR(Instance instance)
        {
            double[,] distanceMatrix = instance.DistanceMatrix;
            List<Location> locations = instance.Locations;
            List<Crew> Crews = instance.Crews;
            var initialRoutesSplitted = new List<List<Location>>();
            var initialRoute = new List<Location>();
            bool[] visited = new bool[locations.Count];
            int crewNumber = 0;
            int routeNumber = 0;
            double vehicleTime = 0;
            double currentLoad = 0.0;

            initialRoute.Add(locations[0]);

            while (visited.Contains(false))
            {
                Location current = locations[0]; // Start z bazy
                visited[current.Id] = true;      // Oznaczenie bazy jako odwiedzona
                vehicleTime = Crews[crewNumber].WorkingTimeWindow.startTime;
                currentLoad = 0.0;
                if (crewNumber == 3)
                {
                    int a = 0;
                }
                while (true)
                {
                    Location nextCustomer = null;
                    double minDistance = double.MaxValue;

                    foreach (var location in locations)
                    {
                        if (!visited[location.Id] && location.Id != 0)
                        {
                            double demand = location.Demand;
                            double distance = distanceMatrix[current.Id, location.Id];                                                                                  // Odległość pomiedzy aktualną lokalizacją a potencjalnie najbliższą
                            double estimatedUpperTimeLeft = location.TimeWindow.End - (vehicleTime + location.ServiceTime * Crews[crewNumber].serviceTimeMultiplier);   // Kara za spóźnienie
                            double estimatedLowerTimeLeft = location.TimeWindow.Start - vehicleTime;                                                                    // Kara za zbyt szybki przyjazd
                            estimatedLowerTimeLeft *= instance.toEarlyPenaltyMultiplier;                                                                                // Współczynnik kary za zbyt wczesny przyjazd
                            estimatedUpperTimeLeft *= instance.toLatePenaltyMultiplier;                                                                                 // Wpsółczynnik kary za spóźnienie
                            double estimatedPenalty = Math.Max(0, estimatedUpperTimeLeft);                                                                              // Max bo kary nie są ujemne
                            estimatedPenalty += Math.Max(0, estimatedLowerTimeLeft);
                            distance += estimatedPenalty;

                            if (distance < minDistance &&                                                                                                           // Dystans (odległość + suma kar)
                                location.TimeWindow.Start < Crews[crewNumber].WorkingTimeWindow.endTime &&                                                          // Rozpoczęcie okna czasowego nie może być później niż zakońćzenie pracy ekipy
                                vehicleTime + location.ServiceTime * Crews[crewNumber].serviceTimeMultiplier < Crews[crewNumber].WorkingTimeWindow.endTime + 100 && // Czas ukończenia serwisu lokalizacji nie może być późniejszy niż zakończenie pracy ekipy ale dopuszczane jest małe spóźnienie (Uwaga Radka)
                                currentLoad + demand <= instance.vehicleCapacity)
                            {
                                minDistance = distance; //Aktualizacja "najbliższeg"o sąsiada
                                nextCustomer = location;
                            }
                        }
                    }

                    if (nextCustomer == null && current.Id != 0 || vehicleTime >= locations[0].TimeWindow.End)
                    {
                        initialRoute.Add(locations[0]);
                        (Crews[routeNumber], Crews[crewNumber]) = (Crews[crewNumber], Crews[routeNumber]);
                        crewNumber++;
                        routeNumber++;
                        crewNumber = routeNumber;
                        if (crewNumber >= Crews.Count)
                            return initialRoute;
                        break;
                    }
                    else if (nextCustomer == null && current.Id == 0)
                    {
                        //foreach (var location in locations)
                        //{
                        //    if (!visited[location.Id] && currentLoad + location.Demand <= instance.vehicleCapacity)
                        //    {
                        //        nextCustomer = location;
                        //        break;
                        //    }
                        //}
                        crewNumber++;
                        break;
                    }

                    if (current.Id == 0)
                    {
                        //if (nextCustomer!=null&&nextCustomer.TimeWindow.Start >= 518) vehicleTime = 618;// Ustawienie zmiany -
                        //                                                                                // jeżeli okno pierwszej lokalizacji jest bliskie
                        //                                                                                // rozpoczęciu pracy drugiej zmiany - wyslij druga zmiane
                        //else vehicleTime = 0;                                                           // jezeli nie - pierwsza zmiana
                        //Crews[crewNumber].WorkingTimeWindow = (vehicleTime, vehicleTime + 618);         // ustawienie czasu pracy w zależności od zmiany
                    }
                    vehicleTime += distanceMatrix[current.Id, nextCustomer.Id];                         // Dodanie czasu przejazdu z aktualnej do "najbliższej" lokalizacji
                    vehicleTime += nextCustomer.ServiceTime * Crews[crewNumber].serviceTimeMultiplier;    // Dodanie czasu obsługi lokalizacji "najbliższej" lokalizacji
                    initialRoute.Add(nextCustomer);                                                     // Dodanie "najbliższej" lokalizacji do trasy
                    visited[nextCustomer.Id] = true;                                                    // Oznaczenie "najbliższej" lokalizacji jako odwiedzona
                    currentLoad += nextCustomer.Demand;
                    current = nextCustomer;                                                             // Ustawienie "najbliższej" lokalizacji na aktualną
                }
            }

            if (initialRoute[initialRoute.Count - 1].Id != 0)
            {
                initialRoute.Add(locations[0]);
            }
            return initialRoute;
        }

    }
}
