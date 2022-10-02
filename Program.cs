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
            int N = 3;
            int M = 5;

            double dx = 1;
            double dy = 1;


            Graph graph = new Graph(dx, dy, N, M, Surface.Plane);

            Console.ReadLine();
        }
    }
}
