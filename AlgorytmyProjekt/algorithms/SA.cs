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
        Reverse,
        Insert,
        Best
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

            //while (temperature > 0.001f)
            //{

                for (var x = 0; x < iterations; x++)
                {
                    path = getNeighbour(neighbourSearchMethod, path, random.Next(solution.Count), random.Next(solution.Count));
                    var neighbourPathDistance = dataController.countPathDistance(path);
                    var diff = neighbourPathDistance - pathDistance;
                    var metropolis = Math.Exp(-diff / temperature);
                    temperature = decreaseTemperature(tempDecreaseMethod, temperature, iterations);

                    if (diff < 0 || random.NextDouble() < metropolis)
                    {
                        solution = new List<int>(path);
                    }
                }

                //temperature = decreaseTemperature(tempDecreaseMethod, temperature, iterations);
           // }

            return solution;
        }

        private float decreaseTemperature(TempDecreaseMethod method, float temp, int iterations)
        {
            switch (method) {
                case TempDecreaseMethod.Arithmetic:
                    temp = temp - 0.1f;
                    break;
                case TempDecreaseMethod.Geometric:
                    temp = temp * 0.9f;
                    break;
                case TempDecreaseMethod.LinearMultiplicative:
                    temp = temp / (1 + 0.1f * (float)Math.Sqrt(iterations));
                    break;
                case TempDecreaseMethod.QuadraticMultiplicative:
                    temp = temp / (1 + 0.1f * iterations);
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
                    output = reversePartOfPath(path);
                    break;
                case NeighbourSearchMethod.Swap:
                    output = swapElementsInPath(path);
                    break;
                case NeighbourSearchMethod.Insert:
                    output = inserttElementInPath(path);
                    break;
                case NeighbourSearchMethod.Best:
                    output = generateBestNeighbour(path);
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

        private List<int> swapElementsInPath(List<int> path)
        {
            var randomValues = getTwoDifferentRandomValue(path);

            var pathCopy = new List<int>(path);

            pathCopy[randomValues.Item1] = path[randomValues.Item2];
            pathCopy[randomValues.Item2] = path[randomValues.Item1];

            return pathCopy;
        }

        private List<int> inserttElementInPath(List<int> path)
        {
            var randomValues = getTwoDifferentRandomValue(path);

            var pathCopy = new List<int>(path);

            pathCopy.Insert(randomValues.Item1, path[randomValues.Item2]);

            return pathCopy;
        }

        private List<int> reversePartOfPath(List<int> path)
        {
            var randomValues = getTwoDifferentRandomValue(path);
            var pathCopy = new List<int>(path);

            pathCopy.Reverse(randomValues.Item1, randomValues.Item2 - randomValues.Item1);

            return pathCopy;
        }

        private (int, int) getTwoDifferentRandomValue(List<int> path) {
            var rng = new Random();
            var split = rng.Next(path.Count);
            var leftSide = rng.Next(split);
            var rightSide = rng.Next(split, path.Count);

            return (leftSide, rightSide);
        }

        private List<int> generateBestNeighbour(List<int> currentPath)
        {
            var neighbour = new List<int>();
            var neighbourDistance = float.MaxValue;

            for (var y = 0; y < currentPath.Count - 1; y++)
            {
                for (var x = y + 1; x < currentPath.Count; x++)
                {
                    var neighbourTemp = new List<int>(currentPath);
                    neighbourTemp[y] = currentPath[x];
                    neighbourTemp[x] = currentPath[y];

                    var neighbourTempDistance = dataController.countPathDistance(neighbourTemp);

                    if (neighbourDistance > neighbourTempDistance)
                    {
                        neighbourDistance = neighbourTempDistance;
                        neighbour = neighbourTemp;
                    }
                }
            }

            return neighbour;
        }
    }
}
