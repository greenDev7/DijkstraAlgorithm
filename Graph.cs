using System;
using System.Collections.Generic;

namespace DijkstraAlgorithm
{
    public class Graph
    {
        /// <summary>
        /// Шаг сетки по оси Ox
        /// </summary>
        public double dx { get; set; }
        /// <summary>
        /// Шаг сетки по оси Oy
        /// </summary>
        public double dy { get; set; }
        /// <summary>
        /// Количество вершин по оси Ox
        /// </summary>
        public int N { get; set; }
        /// <summary>
        /// Количество вершин по оси Oy
        /// </summary>
        public int M { get; set; }
        public Vertex[,] Vertices { get; }
        public Func<double, double, double> SurfaceFunc { get; set; }

        /// <summary>
        /// Возвращает координаты вершины с учетом координаты узла и шага регулярной сетки
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        (double, double) GetRealXY(Vertex vertex)
        {
            double x = vertex.Coordinate.i * dx;
            double y = vertex.Coordinate.j * dy;

            return (x, y);
        }
        
        /// <summary>
        /// Возвращает вес ребра, соединяющего две соседние (смежные) вершины графа
        /// </summary>
        /// <param name="v1">Первая вершина</param>
        /// <param name="v2">Вторая вершина</param>
        /// <returns></returns>
        double Weight(Vertex v1, Vertex v2)
        {
            #region Блок для тестирования

            int x1 = v1.Coordinate.i;
            int y1 = v1.Coordinate.j;

            int x2 = v2.Coordinate.i;
            int y2 = v2.Coordinate.j;


            if (x1 == 0 && y1 == 0 &&
                x2 == 0 && y2 == 1)
                return 2.0;
            else if (x1 == 0 && y1 == 0 &&
                     x2 == 1 && y2 == 0)
                return 1.0;
            else if (x1 == 1 && y1 == 0 &&
                     x2 == 1 && y2 == 1)
                return 100.0;
            else if (x1 == 0 && y1 == 1 &&
                    x2 == 1 && y2 == 1)
                return 3.0;
            else if (x1 == 0 && y1 == 0 &&
                    x2 == 1 && y2 == 1)
                return 50.0;
            else return 80.0;

                #endregion

            (double, double) x1y1 = GetRealXY(v1);
            (double, double) x2y2 = GetRealXY(v2);

            double xDiff = x1y1.Item1 = x2y2.Item1;
            double yDiff = x1y1.Item2 = x2y2.Item2;
            double zDiff = v1.Height = v2.Height;

            double sumOfSquares = Math.Pow(xDiff, 2.0) + Math.Pow(yDiff, 2.0) + Math.Pow(zDiff, 2.0);

            return Math.Sqrt(sumOfSquares);
        }

        /// <summary>
        /// Возвращает наикратчайший путь между двумя заданными вершинами графа
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public List<Label> FindShortestPath(Vertex v1, Vertex v2)
        {
            List<Label> labels = new List<Label>();

            return labels;
        }

        public Graph(double dx, double dy, int N, int M, Func<double, double, double> SurfaceFunc)
        {
            this.N = N;
            this.M = M;
            this.dx = dx;
            this.dy = dy;
            this.SurfaceFunc = SurfaceFunc;

            Vertices = new Vertex[N, M];

            for (int j = 0; j < M; j++)
                for (int i = 0; i < N; i++)
                {
                    double height = SurfaceFunc(i * dx, j * dy);
                    Label label = new Label(new Point2D(i, j));
                    List<Label> labels = new List<Label>() { label };
                    Vertices[i, j] = new Vertex(i, j, height, labels);
                }
        }

    }
}