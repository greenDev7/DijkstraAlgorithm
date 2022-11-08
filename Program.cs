using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph(1, 1, 2, 3, Surface.Plane, gamma: 1.01);

            double shortestPathLength;

            Vertex start = graph.Vertices[0, 0];
            Vertex goal = graph.Vertices[0, 2];

            List<Point2D> path = graph.FindShortestPathAndLength(start, goal, out shortestPathLength);            

            Console.ReadLine();
        }
    }
}
