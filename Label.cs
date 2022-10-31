namespace DijkstraAlgorithm
{
    public class Label
    {
        public Point2D Coordinate { get; set; }
        public double Value { get; set; }

        public Label(Point2D Coordinate, double Value = double.MaxValue)
        {
            this.Coordinate = Coordinate;
            this.Value = Value;
        }
    }
}