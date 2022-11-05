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
            Graph graph = new Graph(1, 1, 2, 3, Surface.Plane);

            graph.FindShortestPath(graph.Vertices[0, 0], graph.Vertices[1, 1]);            

            Console.ReadLine();            
        }
    }
}
