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
        /// Возвращает (физические) координаты вершины с учетом координаты узла и шага регулярной сетки
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

            double xDiff = x1y1.Item1 - x2y2.Item1;
            double yDiff = x1y1.Item2 - x2y2.Item2;
            double zDiff = v1.Height - v2.Height;

            double sumOfSquares = Math.Pow(xDiff, 2.0) + Math.Pow(yDiff, 2.0) + Math.Pow(zDiff, 2.0);

            return Math.Sqrt(sumOfSquares);
        }

        /// <summary>
        /// Возвращает наикратчайший путь между двумя заданными вершинами графа
        /// </summary>
        /// <param name="v1">Вершина из которой необходимо найти наикратчайший путь</param>
        /// <param name="v2">Вершина до которой необходимо найти наикратчайший путь</param>
        /// <returns></returns>
        public List<Label> FindShortestPath(Vertex v1, Vertex v2)
        {
            // Вершине из которой будем искать путь присваиваем нулевую метку
            Label zeroLabel = new Label(v1.Coordinate, 0.0);
            v1.Labels.Clear();
            v1.Labels.Add(zeroLabel);

            List<Label> labels = new List<Label>();

            Vertex currentVertex = v1;

            while (!AllVerticesAreVisited())
            {
                // Находим смежные вершины к текущей (рассматриваемой) вершине
                List<Vertex> vertices = GetAllAdjacentVertices(currentVertex);
            }

            return labels;
        }    

        private Vertex GetTopVertex(Vertex v) => Vertices[v.Coordinate.i, v.Coordinate.j + 1];
        private Vertex GetRightVertex(Vertex v) => Vertices[v.Coordinate.i + 1, v.Coordinate.j];
        private Vertex GetBottomVertex(Vertex v) => Vertices[v.Coordinate.i, v.Coordinate.j - 1];
        private Vertex GetLeftVertex(Vertex v) => Vertices[v.Coordinate.i - 1, v.Coordinate.j];
        private Vertex GetTopRightVertex(Vertex v) => Vertices[v.Coordinate.i + 1, v.Coordinate.j + 1];
        private Vertex GetBottomRightVertex(Vertex v) => Vertices[v.Coordinate.i + 1, v.Coordinate.j - 1];
        private Vertex GetBottomLeftVertex(Vertex v) => Vertices[v.Coordinate.i - 1, v.Coordinate.j - 1];
        private Vertex GetTopLeftVertex(Vertex v) => Vertices[v.Coordinate.i - 1, v.Coordinate.j + 1];

        private bool IsTopRightVertex(Vertex v1) => v1.Coordinate.i == N - 1 && v1.Coordinate.j == M - 1;
        private bool IsBottomRightVertex(Vertex v1) => v1.Coordinate.i == N - 1 && v1.Coordinate.j == 0;
        private bool IsBottomLeftVertex(Vertex v1) => v1.Coordinate.i == 0 && v1.Coordinate.j == 0;
        private bool IsTopLeftVertex(Vertex v1) => v1.Coordinate.i == 0 && v1.Coordinate.j == M - 1;


        /// <summary>
        /// Возвращает все смежные вершины к рассматриваемой вершине
        /// </summary>
        /// <param name="vertex">рассматриваемая вершина</param>
        /// <returns></returns>
        private List<Vertex> GetAllAdjacentVertices(Vertex vertex)
        {
            List<Vertex> adjacentVertices = new List<Vertex>();

            // Рассматриваем угловые вершины
            if (vertex.HasCoordinates(0, 0))
                return new List<Vertex> { Vertices[0, 1], Vertices[1, 1], Vertices[1, 0] };

            if (vertex.HasCoordinates(N - 1, 0))
                return new List<Vertex> { Vertices[N - 2, 0], Vertices[N - 1, 1], Vertices[N - 2, 1] };

            if (vertex.HasCoordinates(0, M - 1))
                return new List<Vertex> { Vertices[0, M - 2], Vertices[1, M - 1], Vertices[1, M - 2] };

            if (vertex.HasCoordinates(N - 1, M - 1))
                return new List<Vertex> { Vertices[N - 2, M - 1], Vertices[N - 1, M - 2], Vertices[N - 2, M - 2] };

            // Рассматриваем боковые вершины



            return adjacentVertices;
        }

        /// <summary>
        /// Возвращает true, если все вершины графа посещены, иначе false
        /// </summary>
        /// <returns></returns>
        private bool AllVerticesAreVisited()
        {
            if (this.Vertices == null)
                return true;

            for (int j = 0; j < M; j++)
                for (int i = 0; i < N; i++)
                    if (!Vertices[i, j].IsVisited)
                        return false;

            return true;
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