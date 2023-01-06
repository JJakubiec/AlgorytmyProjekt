using System.Collections.Generic;

namespace AlgorytmyProjekt
{
    public class NN
    {
        private DataController dataController;
        private int currentNodeIndex;

        public NN(int currentNodeIndex, DataController data) 
        {
            this.dataController = data;
            this.currentNodeIndex = currentNodeIndex;
        }

        public List<int> Execute()
        {
            var path = new List<int>();
            var distanceSum = 0.0;
            path.Add(this.currentNodeIndex);

            for (var y = 1; y < dataController.data.GetLength(0) - 1; y++)
            {
                (int nodeNumber, float distance) nearestFreeNode = (0, float.MaxValue);

                for (var x = 1; x < dataController.data.GetLength(1); x++) 
                {
                    if (!path.Contains(x) && dataController.data[currentNodeIndex, x] < nearestFreeNode.distance)
                    {
                        nearestFreeNode = (x, dataController.data[currentNodeIndex, x]);
                    }
                }

                path.Add(nearestFreeNode.nodeNumber);

                distanceSum += nearestFreeNode.distance;
                currentNodeIndex = nearestFreeNode.nodeNumber;
            }

            return path;
        }

    }
}
