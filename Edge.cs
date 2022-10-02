using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraAlgorithm
{
    public class Edge
    {
        public double Weight { get; set; }

        public Vertex VertexFrom { get; set; }

        public Vertex VertexTo { get; set; }

        public Edge(double Weight)
        {
            this.Weight = Weight;
        }
    }
}
