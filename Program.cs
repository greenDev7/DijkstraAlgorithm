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
            GaussianParameter gaussianParameter1 = new GaussianParameter(1.5, 0.5, 0.5, 5.0, 2.5);

            // Инициализируем граф
            Graph graph = new Graph(0.1, 0.1, 101, 51, 0.1, gaussianParameter1);

            // Записываем получившуюся поверхность в файл
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            graph.WriteSurfaceToFile(Path.Combine(docPath, "surface.txt"));

            // Вычисляем кратчайший путь
            double shortestPathLength = 0.0;

            Point2D startPoint = new Point2D(85, 25);
            Point2D goalPoint = new Point2D(15, 25);

            List<Point2D> shortestPath = graph.FindShortestPathAndLength(startPoint, goalPoint, out shortestPathLength);

            // Записываем найденный путь в файл
            WriteShortestPathToFile(shortestPath, graph, Path.Combine(docPath, "shortestPath_max_slope_0_1_degrees.txt"));

            Console.ReadLine();
        }

        private static void WriteShortestPathToFile(List<Point2D> shortestPath, string fileName)
        {
            List<string> lines = new List<string>();
            foreach (Point2D point in shortestPath)
                lines.Add(string.Format("{0};{1};1", point.i, point.j));

            File.WriteAllLines(fileName, lines);
        }

        private static void WriteShortestPathToFile(List<Point2D> shortestPath, Graph graph, string fileName)
        {
            List<string> lines = new List<string>();
            foreach (Point2D point in shortestPath)
                lines.Add(string.Format("{0};{1};{2}", point.i * 0.1, point.j * 0.1, graph.Vertices[point.i, point.j].Height));

            File.WriteAllLines(fileName, lines);
        }
    }
}
