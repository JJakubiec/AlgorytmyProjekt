using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorytmyProjekt
{
    enum NeighbourSelectMethod { 
        Random,
        Best
    }

    class IHC
    {
        private DataController dataController;

        public IHC(DataController data)
        {
            this.dataController = data;
        }

        public List<int> Execute(NeighbourSelectMethod neighbourSelectMethod, int iterationsWithoutImprovement, int multiStart)
        {
            var solutionPath = createRandomPath(dataController.dataLength);

            for (var m = 0; m < multiStart; m++)
            {
                var path = createRandomPath(dataController.dataLength);

                for (var iwi = 0; iwi < iterationsWithoutImprovement; iwi++)
                {
                    var neighbour = getNeigbourUsingSelectedMethod(path, neighbourSelectMethod);

                    while (improvementExists(path,neighbour))
                    {
                        path = new List<int>(neighbour);
                        neighbour = getNeigbourUsingSelectedMethod(path, neighbourSelectMethod);
                        iwi = 0;
                    }
                }

                if (improvementExists(solutionPath, path))
                {
                    solutionPath = new List<int>(path);
                }
            }

            return solutionPath;
        }

        private List<int> getNeigbourUsingSelectedMethod(List<int> path, NeighbourSelectMethod neighbourSelectMethod)
        {
            var neighbour = new List<int>();

            switch (neighbourSelectMethod)
            {
                case NeighbourSelectMethod.Best:
                    neighbour = generateBestNeighbour(path);
                    break;
                case NeighbourSelectMethod.Random:
                    neighbour = generateRandomNeighbour(path);
                    break;
            }

            return neighbour;
        }

        private bool improvementExists(List<int> pathCurrent, List<int> pathNew)
        {
            return dataController.countPathDistance(pathCurrent) > dataController.countPathDistance(pathNew);
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

        private List<int> generateRandomNeighbour(List<int> currentPath) 
        {
            var rng = new Random();
            var split = rng.Next(currentPath.Count);
            var leftSide = rng.Next(split);
            var rightSide = rng.Next(split, currentPath.Count);


            return swap(currentPath, leftSide, rightSide);
        }

        private List<int> swap(List<int> path, int x, int y)
        {
            var pathCopy = new List<int>(path);

            pathCopy[y] = path[x];
            pathCopy[x] = path[y];

            return pathCopy;
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
