
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorytmyProjekt
{
    enum StopMethod { 
        IterationWithoutImprovement,
        Iteration
    }

    enum SwapLimitation
    {
        AllPosibilities,
        Limited
    }

    class TS
    {
        private DataController dataController;

        public TS(DataController data)
        {
            this.dataController = data;
        }

        public List<int> Execute(SwapLimitation swapLimitation, StopMethod stopMethod, int iteration, int tabuListLength, int swapLimit) {
            var random = new Random();
            var dataLength = this.dataController.dataLength;
            var minPathGlobal = createRandomPath(dataLength);
            var minPathGlobalDistance = dataController.countPathDistance(minPathGlobal);
            var tabu = new List<(int, int)>();
            var i = 0;
            var improvement = false;

            while (i < iteration) 
            {
                var minPathLocal = new List<int>(minPathGlobal);
                var minPathLocalDistance = dataController.countPathDistance(minPathLocal);
                var swapPossiblities = new List<(int, int)>();

                switch (swapLimitation) 
                {
                    case SwapLimitation.AllPosibilities:
                        swapPossiblities = generateAllSwapPossibilities(minPathGlobal.Count);
                        break;
                    case SwapLimitation.Limited:
                        swapPossiblities = generateLimitedSwapPossibilities(minPathGlobal.Count, swapLimit);
                        break;
                }

                (int, int) pair = (0, 0);

                while (swapPossiblities.Any()) 
                {
                    var randomIndex = random.Next(swapPossiblities.Count);
                    pair = swapPossiblities[randomIndex];
                    swapPossiblities.RemoveAt(randomIndex);
                    var minPathLocalCopy = new List<int>(minPathLocal);

                    if (!tabu.Contains(pair)) 
                    {
                        swapElementsInPath(minPathLocalCopy, pair.Item1, pair.Item2);
                        var newMinPathLocalhDistance = dataController.countPathDistance(minPathLocalCopy);

                        if (newMinPathLocalhDistance < minPathLocalDistance) 
                        {
                            minPathLocal = new List<int>(minPathLocalCopy);
                            minPathLocalDistance = newMinPathLocalhDistance;
                        }
                    }
                }

                if (minPathLocalDistance < minPathGlobalDistance)
                {
                    minPathGlobal = new List<int>(minPathLocal);
                    minPathGlobalDistance = minPathLocalDistance;
                    tabu.Add(pair);
                    improvement = true;
                }

                if (tabu.Count > tabuListLength) 
                {
                    tabu.RemoveAt(0);
                }

                if (stopMethod == StopMethod.IterationWithoutImprovement && improvement)
                {
                    i = 0;
                    improvement = false;
                }
                else
                {
                    i++;
                }
            }

            return minPathGlobal;
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

        private void swapElementsInPath(List<int> currentPath, int x, int y)
        {
            var temp = currentPath[x];
            currentPath[x] = currentPath[y];
            currentPath[y] = temp;
        }

        private List<(int,int)> generateAllSwapPossibilities(int pathLength) // indeksy
        {
            var possibilities = new List<(int,int)>();

            for (var y = 0; y < pathLength - 1; y++)
            {
                for (var x = y + 1; x < pathLength; x++)
                {
                    possibilities.Add((y,x));
                }
            }

            return possibilities;
        }

        private List<(int, int)> generateLimitedSwapPossibilities(int pathLength, int limit)
        {
            var allPossibilities = generateAllSwapPossibilities(pathLength);
            var possibilities = new List<(int, int)>();
            var random = new Random();
            var index = 0;

            while (allPossibilities.Any() && index++ < limit)
            {
                var rnd = random.Next(allPossibilities.Count); // indeks
                possibilities.Add(allPossibilities[rnd]);
                allPossibilities.RemoveAt(rnd);
            }

            return possibilities;
        }
    }
}
