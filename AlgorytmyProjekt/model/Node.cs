using System;

namespace AlgorytmyProjekt.model
{
    public class Node
    {
        public float distance;
        public int nodeNumber;

        public Node(int nodeNumber, float distance) {
            this.distance = distance;
            this.nodeNumber = nodeNumber;
        }

        public override string ToString()
        {
            return "DistanceTo: "+ nodeNumber + "| Distance: " + distance ;
        }
    }
}
