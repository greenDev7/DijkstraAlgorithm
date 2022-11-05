using System.Collections.Generic;

namespace DijkstraAlgorithm
{
    public class Vertex
    {
        public Point2D Coordinate { get; set; }
        public double Height { get; set; }
        public List<Point2D> Path { get; set; }
        public double Label { get; set; }
        public bool IsVisited { get; set; }

        public Vertex(int i, int j, List<Point2D> Path, double Height = 0.0, double Label = double.MaxValue, bool IsVisited = false)
        {
            Coordinate = new Point2D(i, j);
            this.Path = Path;
            this.Height = Height;           
            this.Label = Label;
            this.IsVisited = IsVisited;
        }
    }
}