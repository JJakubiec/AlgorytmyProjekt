using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace AlgorytmyProjekt
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataControllerSmallestData = new DataController(DataSize.SmallestData);
            var dataControllerSmallData = new DataController(DataSize.SmallData);
            var dataControllerMediumData = new DataController(DataSize.MediumData);
            var dataControllerLargeData = new DataController(DataSize.LargeData);

            var pathToSolutionSmallestTS = "C:\\Users\\jaros\\source\\repos\\AlgorytmyProjekt\\AlgorytmyProjekt\\solutions\\solution_Smallest_TS.txt";
            var pathToSolutionSmallestNN = "C:\\Users\\jaros\\source\\repos\\AlgorytmyProjekt\\AlgorytmyProjekt\\solutions\\solution_Smallest_NN.txt";
            var pathToSolutionSmallestSA = "C:\\Users\\jaros\\source\\repos\\AlgorytmyProjekt\\AlgorytmyProjekt\\solutions\\solution_Smallest_SA.txt";
            var pathToSolutionSmallestIHC = "C:\\Users\\jaros\\source\\repos\\AlgorytmyProjekt\\AlgorytmyProjekt\\solutions\\solution_Smallest_IHC.txt";

            var pathToSolutionSmallTS = "C:\\Users\\jaros\\source\\repos\\AlgorytmyProjekt\\AlgorytmyProjekt\\solutions\\solution_Small_TS.txt";
            var pathToSolutionSmallNN = "C:\\Users\\jaros\\source\\repos\\AlgorytmyProjekt\\AlgorytmyProjekt\\solutions\\solution_Small_NN.txt";
            var pathToSolutionSmallSA = "C:\\Users\\jaros\\source\\repos\\AlgorytmyProjekt\\AlgorytmyProjekt\\solutions\\solution_Small_SA.txt";
            var pathToSolutionSmallIHC = "C:\\Users\\jaros\\source\\repos\\AlgorytmyProjekt\\AlgorytmyProjekt\\solutions\\solution_Small_IHC.txt";

            var pathToSolutionMediumTS = "C:\\Users\\jaros\\source\\repos\\AlgorytmyProjekt\\AlgorytmyProjekt\\solutions\\solution_Medium_TS.txt";
            var pathToSolutionMediumNN = "C:\\Users\\jaros\\source\\repos\\AlgorytmyProjekt\\AlgorytmyProjekt\\solutions\\solution_Medium_NN.txt";
            var pathToSolutionMediumSA = "C:\\Users\\jaros\\source\\repos\\AlgorytmyProjekt\\AlgorytmyProjekt\\solutions\\solution_Medium_SA.txt";
            var pathToSolutionMediumIHC = "C:\\Users\\jaros\\source\\repos\\AlgorytmyProjekt\\AlgorytmyProjekt\\solutions\\solution_Medium_IHC.txt";

            var pathToSolutionLargeTS = "C:\\Users\\jaros\\source\\repos\\AlgorytmyProjekt\\AlgorytmyProjekt\\solutions\\solution_Large_TS.txt";
            var pathToSolutionLargeNN = "C:\\Users\\jaros\\source\\repos\\AlgorytmyProjekt\\AlgorytmyProjekt\\solutions\\solution_Large_NN.txt";
            var pathToSolutionLargeSA = "C:\\Users\\jaros\\source\\repos\\AlgorytmyProjekt\\AlgorytmyProjekt\\solutions\\solution_Large_SA.txt";
            var pathToSolutionLargeIHC = "C:\\Users\\jaros\\source\\repos\\AlgorytmyProjekt\\AlgorytmyProjekt\\solutions\\solution_Large_IHC.txt";

            //new Thread(() => { executeTabuSearch(dataControllerSmallestData, pathToSolutionSmallestTS); }).Start();
            //new Thread(() => { executeTabuSearch(dataControllerSmallData, pathToSolutionSmallTS); }).Start();
            //new Thread(() => { executeTabuSearch(dataControllerMediumData, pathToSolutionMediumTS); }).Start();
            //new Thread(() => { executeTabuSearch(dataControllerLargeData, pathToSolutionLargeTS); }).Start();

            //new Thread(() => { executeNN(dataControllerSmallestData, pathToSolutionSmallestNN); }).Start();
            //new Thread(() => { executeNN(dataControllerSmallData, pathToSolutionSmallNN); }).Start();
            //new Thread(() => { executeNN(dataControllerMediumData, pathToSolutionMediumNN); }).Start();
            //new Thread(() => { executeNN(dataControllerLargeData, pathToSolutionLargeNN); }).Start();

            new Thread(() => { executeSA(dataControllerSmallestData, pathToSolutionSmallestSA); }).Start();
            new Thread(() => { executeSA(dataControllerSmallData, pathToSolutionSmallSA); }).Start();
            new Thread(() => { executeSA(dataControllerMediumData, pathToSolutionMediumSA); }).Start();
            new Thread(() => { executeSA(dataControllerLargeData, pathToSolutionLargeSA); }).Start();

            //new Thread(() => { executeClimbing(dataControllerSmallestData, pathToSolutionSmallestIHC); }).Start();
           // new Thread(() => { executeClimbing(dataControllerSmallData, pathToSolutionSmallIHC); }).Start();
            //new Thread(() => { executeClimbing(dataControllerMediumData, pathToSolutionMediumIHC); }).Start();
            //new Thread(() => { executeClimbing(dataControllerLargeData, pathToSolutionLargeIHC); }).Start();
        }

        static void executeNN(DataController dataController, string pathToTxtSolutionFile)
        {
            var solutionPath = new List<int>();
            var solutionDistance = float.MaxValue;
            var bestFromFile = float.MaxValue;

            using (var reader = new StreamReader(pathToTxtSolutionFile))
            {
                string line;
                var list = new List<int>();
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(Int32.Parse(line));
                }

                bestFromFile = dataController.countPathDistance(list);
            }

            for (var x = 1; x < dataController.data.GetLength(0); x++)
            {
                var nn = new NN(x, dataController);
                var candidatePath = nn.Execute();
                var candidateDistance = dataController.countPathDistance(candidatePath);

                if (candidateDistance < solutionDistance)
                {
                    solutionDistance = candidateDistance;
                    solutionPath = candidatePath;

                    if (solutionDistance < bestFromFile)
                    {
                        bestFromFile = solutionDistance;
                        using (StreamWriter writer = new StreamWriter(pathToTxtSolutionFile))
                        {
                            solutionPath.ForEach(i =>
                            {
                                writer.WriteLine(i);
                            });
                        }

                        Console.WriteLine("The best in file " + dataController.dataLength + " : Iteration: " + x + " | distance: " + candidateDistance);
                    }

                    Console.WriteLine("Current improvement " + dataController.dataLength + " : Iteration: " + x + " | distance: " + candidateDistance);
                }
            }

            Console.WriteLine("Best in current run " + dataController.dataLength + " | distance: " + solutionDistance);
        }

        static void executeSA(DataController dataController, string pathToTxtSolutionFile)
        {
            var sim = new SA(dataController);

            var solutionPath = new List<int>();
            var solutionDistance = float.MaxValue;
            var bestFromFile = float.MaxValue;

            using (var reader = new StreamReader(pathToTxtSolutionFile))
            {
                string line;
                var list = new List<int>();
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(Int32.Parse(line));
                }

                bestFromFile = dataController.countPathDistance(list);
            }

            for (var x = 0; x < 1000; x++)
            {
                var candidatePath = sim.Execute(100, 100, TempDecreaseMethod.LinearMultiplicative, NeighbourSearchMethod.Reverse);
                var candidateDistance = dataController.countPathDistance(candidatePath);

                if (candidateDistance < solutionDistance)
                {
                    solutionDistance = candidateDistance;
                    solutionPath = candidatePath;

                    if (solutionDistance < bestFromFile)
                    {
                        bestFromFile = solutionDistance;
                        using (StreamWriter writer = new StreamWriter(pathToTxtSolutionFile))
                        {
                            solutionPath.ForEach(i =>
                            {
                                writer.WriteLine(i);
                            });
                        }

                        Console.WriteLine("The best in file " + dataController.dataLength + " : Iteration: " + x + " | distance: " + candidateDistance);
                    }

                    Console.WriteLine("Current improvement " + dataController.dataLength + " : Iteration: " + x + " | distance: " + candidateDistance);
                }
            }

            Console.WriteLine("Best in current run " + dataController.dataLength + " | distance: " + solutionDistance);
        }

        static void executeClimbing(DataController dataController, string pathToTxtSolutionFile)
        {
            var climbing = new IHC(dataController);

            var solutionPath = new List<int>();
            var solutionDistance = float.MaxValue;
            var bestFromFile = float.MaxValue;

            using (var reader = new StreamReader(pathToTxtSolutionFile))
            {
                string line;
                var list = new List<int>();
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(Int32.Parse(line));
                }

                bestFromFile = dataController.countPathDistance(list);
            }

            for (var x = 0; x < 1000; x++)
            {
                var candidatePath = climbing.Execute(NeighbourSelectMethod.Random, 100, 100);
                var candidateDistance = dataController.countPathDistance(candidatePath);

                if (candidateDistance < solutionDistance)
                {
                    solutionDistance = candidateDistance;
                    solutionPath = candidatePath;

                    if (solutionDistance < bestFromFile)
                    {
                        bestFromFile = solutionDistance;
                        using (StreamWriter writer = new StreamWriter(pathToTxtSolutionFile))
                        {
                            solutionPath.ForEach(i =>
                            {
                                writer.WriteLine(i);
                            });
                        }

                        Console.WriteLine("The best in file " + dataController.dataLength + " : Iteration: " + x + " | distance: " + candidateDistance);
                    }

                    Console.WriteLine("Current improvement " + dataController.dataLength + " : Iteration: " + x + " | distance: " + candidateDistance);
                }
            }

            Console.WriteLine("Best in current run " + dataController.dataLength + " | distance: " + solutionDistance);
        }

        static void executeTabuSearch(DataController dataController, string pathToTxtSolutionFile)
        {
            var tabu = new TS(dataController);

            var solutionPath = new List<int>();
            var solutionDistance = float.MaxValue;
            var bestFromFile = float.MaxValue;

            using (var reader = new StreamReader(pathToTxtSolutionFile))
            {
                string line;
                var list = new List<int>();
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(Int32.Parse(line));
                }

                bestFromFile = dataController.countPathDistance(list);
            }

            for (var x = 0; x < 1000; x++)
            {
                var candidatePath = tabu.Execute(SwapLimitation.AllPosibilities, StopMethod.IterationWithoutImprovement, 1000, 30, 4000);
                var candidateDistance = dataController.countPathDistance(candidatePath);

                if (candidateDistance < solutionDistance)
                {
                    solutionDistance = candidateDistance;
                    solutionPath = candidatePath;

                    if (solutionDistance < bestFromFile)
                    {
                        bestFromFile = solutionDistance;
                        using (StreamWriter writer = new StreamWriter(pathToTxtSolutionFile))
                        {
                            solutionPath.ForEach(i =>
                            {
                                writer.WriteLine(i);
                            });
                        }

                        Console.WriteLine("The best in file " + dataController.dataLength + " : Iteration: " + x + " | distance: " + candidateDistance);
                    }

                    Console.WriteLine("Current improvement " + dataController.dataLength + " : Iteration: " + x + " | distance: " + candidateDistance);
                }
            }

            Console.WriteLine("Best in current run " + dataController.dataLength + " | distance: " + solutionDistance);
        }
    }
}
