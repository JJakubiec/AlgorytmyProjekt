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
            //new Thread(() => { executeClimbing(dataControllerSmallData, pathToSolutionSmallIHC); }).Start();
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

            var tab = new List<(NeighbourSearchMethod, TempDecreaseMethod, float, int)> {  { (NeighbourSearchMethod.Best, TempDecreaseMethod.Geometric, 2.0f, 75) },
                                                                                           { (NeighbourSearchMethod.Best, TempDecreaseMethod.Geometric, 2.6f, 90) },
                                                                                           { (NeighbourSearchMethod.Best, TempDecreaseMethod.QuadraticMultiplicative, 1.5f, 80) },
                                                                                           { (NeighbourSearchMethod.Best, TempDecreaseMethod.Geometric, 1.2f, 100) },
                                                                                           //{ (NeighbourSearchMethod.Insert, TempDecreaseMethod.Geometric, 1.2f, 100) },
                                                                                           //{ (NeighbourSearchMethod.Reverse, TempDecreaseMethod.Geometric, 1.2f, 100) },
                                                                                           //{ (NeighbourSearchMethod.Swap, TempDecreaseMethod.Geometric, 1.2f, 100) },
                                                                                           { (NeighbourSearchMethod.Best, TempDecreaseMethod.QuadraticMultiplicative, 1.4f, 105) },
                                                                                           { (NeighbourSearchMethod.Best, TempDecreaseMethod.QuadraticMultiplicative, 0.7f, 150) },
                                                                                           { (NeighbourSearchMethod.Best, TempDecreaseMethod.Geometric, 2.3f, 70) },
                                                                                           { (NeighbourSearchMethod.Best, TempDecreaseMethod.QuadraticMultiplicative, 3f, 50) }
                                                                                           };

            tab.ForEach(a =>
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

                //var random = new Random();
                //var iter = random.Next(100);
                //var temp = random.NextDouble() * 10;
                //var tempdesc = (TempDecreaseMethod)typeof(TempDecreaseMethod).GetEnumValues().GetValue(random.Next(4));
                //var swapMethod = (NeighbourSearchMethod)typeof(NeighbourSearchMethod).GetEnumValues().GetValue(random.Next(4));

                for (var x = 0; x < 1000; x++)
               {

                    //var candidatePath = sim.Execute(iter, (float)temp, tempdesc, swapMethod);
                    var candidatePath = sim.Execute(a.Item4, a.Item3, a.Item2, a.Item1);
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

                            //Console.WriteLine("iter: " +iter +" temp: " + temp+ " tmepdec: " + tempdesc + " swapMethod: " + swapMethod +" | The best in file " + dataController.dataLength + " : Iteration: " + x + " | distance: " + candidateDistance);
                        }

                        //Console.WriteLine("Current improvement " + dataController.dataLength + " : Iteration: " + x + " | distance: " + candidateDistance);
                    }
               }
               Console.WriteLine(a + " | Best in current run " + dataController.dataLength + " | distance: " + solutionDistance);
           });
        }

        static void executeClimbing(DataController dataController, string pathToTxtSolutionFile)
        {
            var climbing = new IHC(dataController);

            var tab = new List<(NeighbourSelectMethod, int, int)> {  { (NeighbourSelectMethod.Random, 30, 50) },
                                                                     { (NeighbourSelectMethod.Random, 30, 75) },
                                                                     { (NeighbourSelectMethod.Best, 75, 75) },
                                                                     { (NeighbourSelectMethod.Best, 20, 100) } };

            tab.ForEach(a =>
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

                for (var x = 0; x < 1000; x++)
                {
                   var candidatePath = climbing.Execute(a.Item1, a.Item2, a.Item3);
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

                           //Console.WriteLine("The best in file " + dataController.dataLength + " : Iteration: " + x + " | distance: " + candidateDistance);
                       }

                       //Console.WriteLine("Current improvement " + dataController.dataLength + " : Iteration: " + x + " | distance: " + candidateDistance);
                   }
               }

               Console.WriteLine(a + "| Best in current run " + dataController.dataLength + " | distance: " + solutionDistance);
           });
        }

        static void executeTabuSearch(DataController dataController, string pathToTxtSolutionFile)
        {
            var tabu = new TS(dataController);

            var tab = new List<(SwapLimitation, StopMethod, int, int, int)> {  //{ (SwapLimitation.AllPosibilities, StopMethod.IterationWithoutImprovement, 100, 30, 400) },
                                                                               // { (SwapLimitation.Limited, StopMethod.IterationWithoutImprovement, 10, 30, 40) },
                                                                               // { (SwapLimitation.AllPosibilities, StopMethod.Iteration, 50, 40, 40) },
                                                                               // { (SwapLimitation.Limited, StopMethod.Iteration, 100, 30, 50) },
                                                                               // { (SwapLimitation.AllPosibilities, StopMethod.IterationWithoutImprovement, 10, 300, 70) },
                                                                               // { (SwapLimitation.Limited, StopMethod.Iteration, 75, 40, 100) },
                                                                                { (SwapLimitation.AllPosibilities, StopMethod.IterationWithoutImprovement, 1000, 30, 100) }};

            tab.ForEach(s =>
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

                for (var x = 0; x < 1000; x++)
               {
                   var candidatePath = tabu.Execute(s.Item1, s.Item2, s.Item3, s.Item4, s.Item5);
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

                           //Console.WriteLine(s + "| The best in file " + dataController.dataLength + " : Iteration: " + x + " | distance: " + candidateDistance);
                       }

                       //Console.WriteLine(s + "| Current improvement " + dataController.dataLength + " : Iteration: " + x + " | distance: " + candidateDistance);
                   }
               }

               Console.WriteLine(s + "| Best in current run " + dataController.dataLength + " | distance: " + solutionDistance);
            });
        }
    }
}
