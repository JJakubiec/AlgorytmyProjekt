using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorytmyProjekt
{
    public enum TempDecreaseMethod { 
        Arithmetic,
        Geometric,
        QuadraticMultiplicative,
        LinearMultiplicative
    }

    public enum NeighbourSearchMethod { 
        Swap,
        Reverse
    }

    public class SA
    {
        private DataController dataController;

        public SA(DataController data) {
            this.dataController = data;
        }

        public List<int> Execute(int iterations, float temperature, TempDecreaseMethod tempDecreaseMethod, NeighbourSearchMethod neighbourSearchMethod)
        {
            var random = new Random();
            var path = createRandomPath(dataController.dataLength);
            var pathDistance = dataController.countPathDistance(path);
            var solution = new List<int>(path);

            while (temperature > 0.1f)
            {

                for (var x = 0; x < iterations; x++)
                {
                    path = getNeighbour(neighbourSearchMethod, path, random.Next(solution.Count), random.Next(solution.Count));
                    var neighbourPathDistance = dataController.countPathDistance(path);
                    var diff = pathDistance - neighbourPathDistance;

                    if (diff > 0)
                    {
                        solution = new List<int>(path);
                    }
                    else
                    {
                        var metropolis = Math.Exp(diff / temperature);

                        if (random.NextDouble() < metropolis)
                        {
                            solution = new List<int>(path);
                        }
                    }
                }

                temperature = decreaseTemperature(tempDecreaseMethod, temperature, iterations);
            }

            return solution;
        }

        private float decreaseTemperature(TempDecreaseMethod method, float temp, int iterations)
        {
            switch (method) {
                case TempDecreaseMethod.Arithmetic:
                    temp = temp - 1;
                    break;
                case TempDecreaseMethod.Geometric:
                    temp = temp * 0.9999f;
                    break;
                case TempDecreaseMethod.LinearMultiplicative:
                    temp = temp / (1 + 0.0001f * (float)Math.Sqrt(iterations));
                    break;
                case TempDecreaseMethod.QuadraticMultiplicative:
                    temp = temp / (1 + 0.0001f * iterations);
                    break;
            }

            return temp;
        }

        private List<int> getNeighbour(NeighbourSearchMethod method, List<int> path, int x, int y)
        {
            var output = new List<int>();
            switch (method)
            {
                case NeighbourSearchMethod.Reverse:
                    output = reversePartOfPath(path,x,y);
                    break;
                case NeighbourSearchMethod.Swap:
                    output = swapElementsInPath(path,x,y);
                    break;
            }

            return output;
        }

        private List<int> createRandomPath(int dataLength)
        {
            var rng = new Random();
            var pathCopy = new List<int>();

            for (var x = 1; x <= dataLength; x++)
            {
                pathCopy.Add(x);
            }

            return pathCopy.OrderBy(a => rng.Next()).ToList();
        }

        private List<int> swapElementsInPath(List<int> path, int x, int y)
        {
            var pathCopy = new List<int>(path);

            pathCopy[y] = path[x];
            pathCopy[x] = path[y];

            return pathCopy;
        }

        private List<int> reversePartOfPath(List<int> path, int startIndex, int endIndex)
        {
            var pathCopy = new List<int>(path);

            if (startIndex > endIndex) {
                var temp = startIndex;
                startIndex = endIndex;
                endIndex = temp;
            }

            pathCopy.Reverse(startIndex, endIndex - startIndex);

            return pathCopy;
        }
    }
}
