﻿using System;
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

            double shortestPathLength;

            Vertex start = graph.Vertices[1, 2];
            Vertex goal = graph.Vertices[1, 1];

            List<Point2D> path = graph.FindShortestPathAndLength(start, goal, out shortestPathLength);            

            Console.ReadLine();
        }
    }
}
