using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraAlgorithm
{
    public class Vertex
    {
        public double Height { get; set; }
        public bool IsVisited { get; set; }
        public double Label { get; set; }        

        public Vertex(double Height, bool IsVisited = false, double Label = double.PositiveInfinity)
        {
            this.Height = Height;
            this.IsVisited = IsVisited;
            this.Label = Label;            
        }
    }
}