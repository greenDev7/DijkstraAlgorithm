using System;
using System.Collections.Generic;
using System.Linq;

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
        public double gamma { get; set; }

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

            // Merge test
        }
        
        /// <summary>
        /// Возвращает вес ребра, соединяющего две соседние (смежные) вершины графа
        /// </summary>
        /// <param name="v1">Первая вершина</param>
        /// <param name="v2">Вторая вершина</param>
        /// <returns></returns>
        double Weight(Vertex v1, Vertex v2)
        {
            (double, double) x1y1 = GetRealXY(v1);
            (double, double) x2y2 = GetRealXY(v2);

            double xDiff = x1y1.Item1 - x2y2.Item1;
            double yDiff = x1y1.Item2 - x2y2.Item2;
            double zDiff = v1.Height - v2.Height;

            double sumOfSquares = Math.Pow(xDiff, 2.0) + Math.Pow(yDiff, 2.0) + gamma * Math.Pow(zDiff, 2.0);

            return Math.Sqrt(sumOfSquares);
        }       

        /// <summary>
        /// Возвращает кратчайший путь между двумя заданными вершинами графа
        /// </summary>
        /// <param name="v1">Вершина из которой необходимо найти наикратчайший путь</param>
        /// <param name="v2">Вершина до которой необходимо найти наикратчайший путь</param>
        /// <returns></returns>
        public List<Point2D> FindShortestPathAndLength(Vertex start, Vertex goal, out double shortestPathLength)
        {
            shortestPathLength = 0.0;
            // Вершине из которой будем искать путь присваиваем нулевую метку
            start.Label = 0.0;            
            goal.IsGoal = true;

            Vertex current = start;

            List<Vertex> visitedVertices = new List<Vertex>();

            while (current != null)
            {
                // Находим смежные и не посещенные вершины к текущей вершине
                List<Vertex> neighbors = GetUnvisitedNeighbors(current);

                foreach (Vertex neighbor in neighbors)
                {
                    double currentWeight = current.Label + Weight(current, neighbor);
                    if (currentWeight < neighbor.Label)
                    {
                        neighbor.Label = currentWeight;
                        neighbor.CameFrom = current.Coordinate;
                    }                    
                }

                // После того как все соседи рассмотрены (всем соседям расставлены метки), помечаем текущую вершину как посещенную
                current.IsVisited = true;
                // и добавляем ее в список посещенных вершин
                visitedVertices.Add(current);
                // Используем этот список посещенных вершин для поиска новой текущей вершины
                current = GetCurrent(visitedVertices);
            }

            shortestPathLength = goal.Label;
            return GetShortestPath(goal);
        }

        /// <summary>
        /// Возвращает кратчайший путь
        /// </summary>
        /// <param name="goal">целевая вершина</param>
        /// <returns></returns>
        private List<Point2D> GetShortestPath(Vertex goal)
        {
            List<Point2D> path = new List<Point2D>();
            
            path.Add(goal.Coordinate);
            Point2D cameFrom = goal.CameFrom;

            while (cameFrom != null)
            {
                Vertex vertex = Vertices[cameFrom.i, cameFrom.j];
                path.Add(vertex.Coordinate);
                cameFrom = vertex.CameFrom;
            }

            return path;
        }
        /// <summary>
        /// Возвращает текущую вершину используя список посещенных вершин
        /// </summary>
        /// <param name="visitedVertices">список посещенных вершин</param>
        /// <returns></returns>
        private Vertex GetCurrent(List<Vertex> visitedVertices)
        {
            List<Vertex> unvisitedAndNotGoalNeighbors = new List<Vertex>();

            foreach (Vertex v in visitedVertices)
                if (HasUnvisitedAndNotGoalNeighbors(v, out unvisitedAndNotGoalNeighbors))
                    break;

            // Если не нашлось ни одного подходящего соседа, значит мы дошли до финальной вершины
            if (!unvisitedAndNotGoalNeighbors.Any())
                return null;

            // Находим и возвращаем соседа с минимальной меткой
            double minLabel = unvisitedAndNotGoalNeighbors.Min(v => v.Label);
            Vertex newCurrent = unvisitedAndNotGoalNeighbors.First(v => v.Label == minLabel);

            return newCurrent;
        }

        /// <summary>
        /// Возвращает true, если у текущей вершины имеются непосещенные соседи (среди которых нет целевой вершины), иначе false,
        /// а также сам список непосещенных соседей
        /// </summary>
        /// <param name="vertex">текущая вершина</param>
        /// <param name="unvisitedAndNotGoalNeighbors">список непосещенных соседей</param>
        /// <returns></returns>
        private bool HasUnvisitedAndNotGoalNeighbors(Vertex vertex, out List<Vertex> unvisitedAndNotGoalNeighbors)
        {
            unvisitedAndNotGoalNeighbors = GetUnvisitedNeighbors(vertex).Where(v => !v.IsGoal).ToList();
            return unvisitedAndNotGoalNeighbors.Any();
        }
        /// <summary>
        /// Возвращает все смежные вершины для текущей, которые не посещены
        /// </summary>
        /// <param name="current">текущая вершина</param>
        /// <returns></returns>
        private List<Vertex> GetUnvisitedNeighbors(Vertex current) => GetAllAdjacentVertices(current).Where(v => !v.IsVisited).ToList();

        #region Методы для поиска соседней вершины в зависимости от направления
        private Vertex GetTopVertex(Vertex v) => Vertices[v.Coordinate.i, v.Coordinate.j + 1];
        private Vertex GetRightVertex(Vertex v) => Vertices[v.Coordinate.i + 1, v.Coordinate.j];
        private Vertex GetBottomVertex(Vertex v) => Vertices[v.Coordinate.i, v.Coordinate.j - 1];
        private Vertex GetLeftVertex(Vertex v) => Vertices[v.Coordinate.i - 1, v.Coordinate.j];
        private Vertex GetTopRightVertex(Vertex v) => Vertices[v.Coordinate.i + 1, v.Coordinate.j + 1];
        private Vertex GetBottomRightVertex(Vertex v) => Vertices[v.Coordinate.i + 1, v.Coordinate.j - 1];
        private Vertex GetBottomLeftVertex(Vertex v) => Vertices[v.Coordinate.i - 1, v.Coordinate.j - 1];
        private Vertex GetTopLeftVertex(Vertex v) => Vertices[v.Coordinate.i - 1, v.Coordinate.j + 1];
        #endregion

        #region Методы для определения принадлежности вершины той или иной стороне/углу сетки
        private bool IsTopRightVertex(Vertex v1) => v1.Coordinate.i == N - 1 && v1.Coordinate.j == M - 1;
        private bool IsBottomRightVertex(Vertex v1) => v1.Coordinate.i == N - 1 && v1.Coordinate.j == 0;
        private bool IsBottomLeftVertex(Vertex v1) => v1.Coordinate.i == 0 && v1.Coordinate.j == 0;
        private bool IsTopLeftVertex(Vertex v1) => v1.Coordinate.i == 0 && v1.Coordinate.j == M - 1;

        private bool IsVertexOnTheTopSide(Vertex v1) => v1.Coordinate.j == M - 1;
        private bool IsVertexOnTheRightSide(Vertex v1) => v1.Coordinate.i == N - 1;
        private bool IsVertexOnTheBottomSide(Vertex v1) => v1.Coordinate.j == 0;
        private bool IsVertexOnTheLeftSide(Vertex v1) => v1.Coordinate.i == 0;
        #endregion

        /// <summary>
        /// Возвращает все смежные вершины к рассматриваемой вершине
        /// </summary>
        /// <param name="vertex">рассматриваемая вершина</param>
        /// <returns></returns>
        private List<Vertex> GetAllAdjacentVertices(Vertex vertex)
        {
            #region Рассматриваем угловые вершины

            if (IsTopRightVertex(vertex))
                return new List<Vertex>
                {
                    GetLeftVertex(vertex),
                    GetBottomLeftVertex(vertex),
                    GetBottomVertex(vertex)
                };

            if (IsBottomRightVertex(vertex))
                return new List<Vertex>
                {
                    GetTopVertex(vertex),
                    GetTopLeftVertex(vertex),
                    GetLeftVertex(vertex)
                };

            if (IsBottomLeftVertex(vertex))
                return new List<Vertex>
                {
                    GetTopVertex(vertex),
                    GetTopRightVertex(vertex),
                    GetRightVertex(vertex)
                };

            if (IsTopLeftVertex(vertex))
                return new List<Vertex>
                {
                    GetBottomVertex(vertex),
                    GetBottomRightVertex(vertex),
                    GetRightVertex(vertex)
                };

            #endregion

            #region Рассматриваем боковые вершины

            if (IsVertexOnTheTopSide(vertex))
                return new List<Vertex>
                {
                    GetLeftVertex(vertex),
                    GetBottomLeftVertex(vertex),
                    GetBottomVertex(vertex),
                    GetBottomRightVertex(vertex),
                    GetRightVertex(vertex)
                };

            if (IsVertexOnTheRightSide(vertex))
                return new List<Vertex>
                {
                    GetTopVertex(vertex),
                    GetTopLeftVertex(vertex),
                    GetLeftVertex(vertex),
                    GetBottomLeftVertex(vertex),
                    GetBottomVertex(vertex)
                };

            if (IsVertexOnTheBottomSide(vertex))
                return new List<Vertex>
                {
                    GetLeftVertex(vertex),
                    GetTopLeftVertex(vertex),
                    GetTopVertex(vertex),
                    GetTopRightVertex(vertex),
                    GetRightVertex(vertex)
                };

            if (IsVertexOnTheLeftSide(vertex))
                return new List<Vertex>
                {
                    GetTopVertex(vertex),
                    GetTopRightVertex(vertex),
                    GetRightVertex(vertex),
                    GetBottomRightVertex(vertex),
                    GetBottomVertex(vertex)
                };

            #endregion

            // Иначе вершина лежит "в середине карты" и нужно вернуть все 8 смежных вершин
            return new List<Vertex>
                {
                    GetTopVertex(vertex),
                    GetRightVertex(vertex),
                    GetBottomVertex(vertex),
                    GetLeftVertex(vertex),

                    GetTopRightVertex(vertex),
                    GetBottomRightVertex(vertex),
                    GetBottomLeftVertex(vertex),
                    GetTopLeftVertex(vertex)
                };
        }
        public Graph(double dx, double dy, int N, int M, Func<double, double, double> SurfaceFunc, double gamma = 1.0)
        {
            this.N = N;
            this.M = M;
            this.dx = dx;
            this.dy = dy;
            this.SurfaceFunc = SurfaceFunc;
            this.gamma = gamma;

            Vertices = new Vertex[N, M];

            for (int j = 0; j < M; j++)
                for (int i = 0; i < N; i++)
                {
                    double height = SurfaceFunc(i * dx, j * dy);                   
                    Vertices[i, j] = new Vertex(i, j, Height: height);
                }
        }
    }
}