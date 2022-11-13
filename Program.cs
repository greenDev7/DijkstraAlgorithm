using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DijkstraAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            string directoryPath = Environment.CurrentDirectory;
            int[,] obstacleMatrix = Obstacle.CreateObstacleMatrixFromCSVFile(directoryPath + "Maze1.csv");
            Obstacle.WriteObstacleToFile(obstacleMatrix, directoryPath + "Maze1Obstacle.txt");

            //Graph graph = new Graph(1, 1, 2, 3, Surface.Plane);
            Graph graph = new Graph(obstacleMatrix);


            double shortestPathLength;

            Point2D startPoint = new Point2D(4, 46);
            Point2D goalPoint = new Point2D(95, 4);

            List<Point2D> path = graph.FindShortestPathAndLength(startPoint, goalPoint, out shortestPathLength);

            WriteShortestPathToFile(path, directoryPath + "Maze1ShortestPath.txt");

            Console.ReadLine();
        }

        private static void WriteShortestPathToFile(List<Point2D> shortestPath, string fileName)
        {
            List<string> lines = new List<string>();
            foreach (Point2D point in shortestPath)
                lines.Add(string.Format("{0};{1};1", point.i, point.j));

            File.WriteAllLines(fileName, lines);
        }
    }
}
