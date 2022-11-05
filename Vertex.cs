using System.Collections.Generic;

namespace DijkstraAlgorithm
{
    public class Vertex
    {
        public Point2D Coordinate { get; set; }
        public double Height { get; set; }
        public Point2D CameFrom { get; set; }
        public double Label { get; set; }
        public bool IsVisited { get; set; }
        public bool IsGoal { get; set; }

        public Vertex(int i, int j, Point2D CameFrom = null, double Height = 0.0, double Label = double.MaxValue, bool IsVisited = false, bool IsGoal = false)
        {
            Coordinate = new Point2D(i, j);
            this.CameFrom = CameFrom;
            this.Height = Height;           
            this.Label = Label;
            this.IsVisited = IsVisited;
            this.IsGoal = IsGoal;
        }
    }
}