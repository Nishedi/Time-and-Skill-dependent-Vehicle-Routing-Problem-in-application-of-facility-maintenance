using HCVRPTW;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HCVRPTW
{
    public class Instance
    {
        public List<Location> Locations { get; set; } = new List<Location>();   // \mathcal{R} - set of all locations
        public double[,] DistanceMatrix;                                        // \mathcal{C} - cost matrix, c_{i,j} - DistanceMatrix[i,j]
        public List<Crew> Crews = new List<Crew>();                             // \mathcal{V} - set of crews, v - Crews.Count
        public string FileName;
        public double seniorsToJuniorsRatio = 0.5;                       // ratio of seniors to juniors in the crew pool
        public int numberOfCrews = 50;                                  //for now, to CHANGE!!!!
        public int toEarlyPenaltyMultiplier = 1;
        public int toLatePenaltyMultiplier = 1;
        public int vehicleCapacity = 900;
        public int endOfWork = 0;
        public Instance(string filename, double seniorsToJuniorsRatio = 0.5, int toEarlyPenaltyMultiplier = 1, int toLatePenaltyMultiplier = 1, int numberOfCrews = 2, int vehicleCapacity = 90)
        {
            FileName = filename;
            ParseSolomonFile(filename);
            endOfWork = Locations[0].TimeWindow.End;
            this.seniorsToJuniorsRatio = seniorsToJuniorsRatio;
            this.toLatePenaltyMultiplier = toLatePenaltyMultiplier;
            this.numberOfCrews = numberOfCrews;
            this.toLatePenaltyMultiplier = toLatePenaltyMultiplier;
            this.vehicleCapacity = vehicleCapacity;
            for(int i = 0; i < numberOfCrews * seniorsToJuniorsRatio; i++)
            {
                Crews.Add(new Crew(i, i % 2, CrewType.Seniors,endOfWork));
            }
            for (int i = 0; i < numberOfCrews * (1 - seniorsToJuniorsRatio); i++)
            {
                Crews.Add(new Crew(i + (int)(numberOfCrews * seniorsToJuniorsRatio), i % 2, CrewType.Juniors, endOfWork));
            }
            var rnd = new Random(1); 
            Crews = Crews.OrderBy(_ => rnd.Next()).ToList();

        }

        public void ParseSolomonFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                string trimmedLine = line.Trim();
                var parts = trimmedLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 7) continue;

                try
                {
                    Locations.Add(new Location(
                        int.Parse(parts[0]) - 1,
                        int.Parse(parts[0]) == 1 ? LocationType.Depot : LocationType.Customer,
                        (int)double.Parse(parts[1], CultureInfo.InvariantCulture),
                        (int)double.Parse(parts[2], CultureInfo.InvariantCulture),
                        double.Parse(parts[3], CultureInfo.InvariantCulture),
                        ((int)double.Parse(parts[4], CultureInfo.InvariantCulture), (int)double.Parse(parts[5], CultureInfo.InvariantCulture)),
                        (int)double.Parse(parts[6], CultureInfo.InvariantCulture)
                    ));

                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Błąd parsowania linii: {trimmedLine}. Szczegóły: {ex.Message}");
                }
            }
            DistanceMatrix = createDistanceMatrix();
        }
        public double[,] createDistanceMatrix() // funkcja generująca macierz odleglosci
        {
            double[,] distanceMatrix = new double[Locations.Count, Locations.Count];
            for (int i = 0; i < Locations.Count; i++)
            {
                for (int j = 0; j < Locations.Count; j++)
                {
                    double deltaX = Locations[j].X - Locations[i].X;
                    double deltaY = Locations[j].Y - Locations[i].Y;
                    double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                    distanceMatrix[i, j] = (int)distance; // dodana konwersja na inta
                }
            }
            return distanceMatrix;
        }

    }


}