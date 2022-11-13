using System;
using System.Collections.Generic;
using System.IO;

namespace DijkstraAlgorithm
{
    /// <summary>
    /// Вспомогательный класс для работы с препятствиями (чтение препятствий из csv-файла-матрицы, формирование самого препятствия, запись его в файл)
    /// </summary>
    public static class Obstacle
    {
        /// <summary>
        /// Возвращает матрицу-препятствие по csv-файлу. Элементы матрицы имеют только значения 0 либо 1, где 1 означает, что
        /// вершина с соответствующими координатами является препятствием. Данная матрица используется для инициализации графа
        /// </summary>
        /// <param name="fileName">полный путь к csv-файлу, в котором значения (матрица из нулей и единиц) разделены точкой с запятой</param>
        /// <returns></returns>
        public static int[,] CreateObstacleMatrixFromCSVFile(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            int N = lines.Length;
            int M = lines[0].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Length;
            int[,] obstacleMatrix = new int[N, M];

            for (int i = N - 1; i >= 0; i--)
            {
                string[] array = lines[i].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < M; j++)
                    obstacleMatrix[(N - 1) - i, j] = int.Parse(array[j]);
            }

            return obstacleMatrix;
        }

        /// <summary>
        /// Возвращает координаты вершин являющихся препятствием из матрицы. Данные координаты препятвий необходимы для их
        /// отображения и построения графика
        /// </summary>
        /// <param name="obstacleMatrix">матрица с препятствиями</param>
        /// <returns></returns>
        private static List<Point2D> CreateObstacleFromMatrix(int[,] obstacleMatrix)
        {
            long rowCount = obstacleMatrix.GetLongLength(0);
            long columnCount = obstacleMatrix.GetLongLength(1);

            List<Point2D> obstaclePoints = new List<Point2D>();

            for (int i = 0; i < rowCount; i++)
                for (int j = 0; j < columnCount; j++)
                    if (obstacleMatrix[i, j] == 1)
                        obstaclePoints.Add(new Point2D(i, j));

            return obstaclePoints;
        }

        /// <summary>
        /// Записывает координаты препятствий в файл
        /// </summary>
        /// <param name="obstacleMatrix">матрица с препятствиями из которой отбираются препятствия</param>
        /// <param name="fileName">полный путь к файлу для записи</param>
        public static void WriteObstacleToFile(int[,] obstacleMatrix, string fileName)
        {
            List<Point2D> obstaclePoints = CreateObstacleFromMatrix(obstacleMatrix);
            List<string> obstacleLines = new List<string>();
            foreach (Point2D point in obstaclePoints)
                obstacleLines.Add(string.Format("{0};{1};1", point.i, point.j));

            File.WriteAllLines(fileName, obstacleLines);
        }
    }
}