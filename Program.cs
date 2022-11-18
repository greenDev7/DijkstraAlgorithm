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
            // Создаем матрицу-препятствий из csv-файла
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            int[,] obstacleMatrix = Obstacle.CreateObstacleMatrixFromCSVFile(Path.Combine(docPath, "test2.csv"));

            // Инициализируем граф с помощью этой матрицы
            Graph graph = new Graph(obstacleMatrix);

            // Вычисляем кратчайший путь
            double shortestPathLength = 0.0;           

            Point2D startPoint = new Point2D(0, 4);
            Point2D goalPoint = new Point2D(27, 5);

            List<Point2D> shortestPath = graph.FindShortestPathAndLength(startPoint, goalPoint, out shortestPathLength);

            // Записываем найденный путь в файл
            WriteShortestPathToFile(shortestPath, Path.Combine(docPath, "shortestPath.txt"));

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
