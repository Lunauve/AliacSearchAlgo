using AISearchSample;
using System;
using System.Collections;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace AliacSearchAlgo
{
    class HillSearch
    {
        private ArrayList nodes;
        private Node startNode;
        private Node goalNode;

        private Node currentNode = null;
        private bool started = false;

        public HillSearch(ArrayList nodes, int startIndex, int goalIndex)
        {
            this.nodes = nodes;

            if (startIndex < 0 || startIndex >= nodes.Count || goalIndex < 0 || goalIndex >= nodes.Count)
                throw new Exception("Invalid start or goal index.");

            startNode = (Node)nodes[startIndex];
            goalNode = (Node)nodes[goalIndex];

            startNode.Start = true;
            goalNode.Goal = true;
        }

        public Node search()
        {
            Node current = startNode;
            current.Expanded = true;

            while (!current.Goal)
            {
                ArrayList neighbors = current.getNeighbor();

                Node bestNeighbor = null;
                double bestHeuristic = double.MaxValue;

                // Heuristic: Neighbor that is closest to goal.
                foreach (Node neighbor in neighbors)
                {
                    if (!neighbor.Expanded)
                    {
                        double h = DistanceToGoal(neighbor, goalNode);
                        if (h < bestHeuristic)
                        {
                            bestHeuristic = h;
                            bestNeighbor = neighbor;
                        }
                    }
                }

                // Local maximum.
                if (bestNeighbor == null || DistanceToGoal(current, goalNode) <= bestHeuristic)
                {
                    MessageBox.Show("Reached local maximum, goal not found.");
                    return null;
                }

                bestNeighbor.Origin = current;
                bestNeighbor.Expanded = true;
                current = bestNeighbor;
            }

            MessageBox.Show("Goal found: " + current.Name);
            return current;
        }

        public Node searchone()
        {
            if (!started)
            {
                currentNode = startNode;
                currentNode.Expanded = true;
                started = true;
                return currentNode;
            }

            if (currentNode.Goal)
                return currentNode;

            ArrayList neighbors = currentNode.getNeighbor();
            Node bestNeighbor = null;
            double bestHeuristic = double.MaxValue;

            // Heuristic: Neighbor that is closest to goal.
            foreach (Node neighbor in neighbors)
            {
                if (!neighbor.Expanded)
                {
                    double h = DistanceToGoal(neighbor, goalNode);
                    if (h < bestHeuristic)
                    {
                        bestHeuristic = h;
                        bestNeighbor = neighbor;
                    }
                }
            }

            // Local maximum.
            if (bestNeighbor == null || DistanceToGoal(currentNode, goalNode) <= bestHeuristic)
            {
                MessageBox.Show("Reached local maximum, cannot proceed further.");
                return null;
            }

            bestNeighbor.Origin = currentNode;
            bestNeighbor.Expanded = true;
            currentNode = bestNeighbor;

            return currentNode;
        }

        // Node distance to goal.
        private double DistanceToGoal(Node node, Node goal)
        {
            int dx = node.X - goal.X;
            int dy = node.Y - goal.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
