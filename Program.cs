using System;
using System.Collections.Generic;
using System.IO;

namespace DijkstraAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создаем несколько экземпляров параметров для Гауссиана для имитации гор (холмов) и одного оврага
            GaussianParameter gaussianParameter1 = new GaussianParameter(1.5, 0.5, 0.5, 2.0, 4.0);
            GaussianParameter gaussianParameter2 = new GaussianParameter(1.0, 0.5, 0.5, 7.5, 1.0);
            GaussianParameter gaussianParameter3 = new GaussianParameter(-0.5, 0.2, 1.0, 5.0, 0.5);
            GaussianParameter gaussianParameter4 = new GaussianParameter(1.0, 0.5, 0.8, 3.5, 2.2);

            // Инициализируем граф
            Graph graph = new Graph(0.1, 0.1, 101, 51, 20.0, gaussianParameter1, gaussianParameter2, gaussianParameter3, gaussianParameter4);

            // Создаем искуственные сооружения на карте
            graph.CreateBuilding(new Point2D(57, 20), 2, 20, 0.3);
            graph.CreateBuilding(new Point2D(64, 16), 3, 5, 0.3);
            graph.CreateBuilding(new Point2D(18, 4), 2, 5, 0.3);
            graph.CreateBuilding(new Point2D(10, 14), 4, 2, 0.4);
            graph.CreateBuilding(new Point2D(64, 29), 5, 2, 0.4);
            graph.CreateBuilding(new Point2D(14, 24), 5, 2, 0.3);

            // Записываем получившуюся поверхность в файл
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            graph.WriteSurfaceToFile(Path.Combine(docPath, "surface.txt"));

            // Вычисляем кратчайший путь
            double shortestPathLength = 0.0;

            Point2D startPoint = new Point2D(92, 7);
            Point2D goalPoint = new Point2D(14, 21);

            List<Point2D> shortestPath = graph.FindShortestPathAndLength(startPoint, goalPoint, out shortestPathLength);

            // Записываем найденный путь в файл
            WriteShortestPathToFile(shortestPath, graph, Path.Combine(docPath, "shortestPath.txt"));

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
