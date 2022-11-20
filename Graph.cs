using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConcurrentPriorityQueue;


namespace DijkstraAlgorithm
{
    public class Graph
    {
        /// <summary>
        /// Шаг сетки по оси Ox
        /// </summary>
        public double dx { get; }
        /// <summary>
        /// Шаг сетки по оси Oy
        /// </summary>
        public double dy { get; }
        /// <summary>
        /// Количество вершин по оси Ox
        /// </summary>
        public int N { get; }
        /// <summary>
        /// Количество вершин по оси Oy
        /// </summary>
        public int M { get; }
        /// <summary>
        /// Матрица вершин графа
        /// </summary>
        public Vertex[,] Vertices { get; }
        /// <summary>
        /// Предельная величина уклона, необходимая для обхода препятствий, в градусах
        /// </summary>
        public double MaxSlope { get; }

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
        /// Возвращает вес ребра, соединяющего две соседние (смежные) вершины графа (по факту это расстояние между точками поверхности)
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

            double sumOfSquares = Math.Pow(xDiff, 2.0) + Math.Pow(yDiff, 2.0) + Math.Pow(zDiff, 2.0);

            return Math.Sqrt(sumOfSquares);
        }   

        /// <summary>
        /// Возвращает кратчайший путь между двумя заданными вершинами графа и его длину
        /// </summary>
        /// <param name="startPoint">координаты стартовой вершины</param>
        /// <param name="goalPoint">координаты целевой вершины</param>
        /// <param name="shortestPathLength">длина пути</param>
        /// <returns></returns>
        public List<Point2D> FindShortestPathAndLength(Point2D startPoint, Point2D goalPoint, out double shortestPathLength)
        {
            shortestPathLength = 0.0; // Здесь будет храниться длина искомого пути

            // Стартовой вершине присваиваем нулевую метку
            Vertex start = Vertices[startPoint.i, startPoint.j];
            start.Label = 0.0;

            // Сохраним отдельно целевую вершину
            Vertex goal = Vertices[goalPoint.i, goalPoint.j];

            // Очередь с приоритетом
            ConcurrentPriorityQueue<Vertex, double> priorityQueue = new ConcurrentPriorityQueue<Vertex, double>(new MyDoubleComparer());
            priorityQueue.Enqueue(start, start.Label);

            // Цикл заканчивает свою работу, когда очередь пустая, либо когда целевая вершина оказалась посещена
            while (priorityQueue.Any() && !goal.IsVisited)
            {
                // Получаем из очереди с приоритетом вершину с минимальной меткой (и одновременно удаляем эту вершину из очереди)
                Vertex current = priorityQueue.Dequeue();

                if (current.IsVisited)
                    continue;

                current.IsVisited = true;

                // Находим подходящих соседей: которые еще не посещены, не являются препятствиями и т.п.
                List<Vertex> neighbors = GetValidNeighbors(current);

                foreach (Vertex neighbor in neighbors)
                {
                    double currentWeight = current.Label + Weight(current, neighbor);
                    if (currentWeight < neighbor.Label)
                    {
                        neighbor.Label = currentWeight;
                        neighbor.CameFrom = current.Coordinate;
                        // Добавляем соседа в очередь с приоритетом используя в качестве приоритета значение его метки
                        priorityQueue.Enqueue(neighbor, neighbor.Label);
                    }                    
                }          
            }

            // В конце работы алгоритма в целевой вершине в свойстве Label будет находиться длина искомого пути
            shortestPathLength = goal.Label;
            // А с помощью свойства CameFrom сформируем и вернем сам искомый путь
            return GetShortestPath(goal);
        }      

        /// <summary>
        /// Формирует кратчайший путь, начиная с целевой вершины и заканчивая стартовой
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
        /// Возвращает величину уклона между двумя вершинами в градусах
        /// </summary>
        /// <param name="v1">первая вершина</param>
        /// <param name="v2">вторая вершина</param>
        /// <returns></returns>
        private double Slope(Vertex v1, Vertex v2)
        {
            double hypotenuse = Weight(v1, v2); // Вес ребра - это и есть по факту расстояние между точками
            double zDiffAbs = Math.Abs(v1.Height - v2.Height); // Модуль разности по высоте

            return Math.Asin(zDiffAbs / hypotenuse) * 180.0 / Math.PI; // Переводим радианы в градусы
        }        

        #region Методы для получения соседней вершины в зависимости от направления
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
        /// Возвращает для текущей вершины подходящих (валидных) соседей
        /// </summary>
        /// <param name="current">текущая вершина</param>
        /// <returns></returns>
        private List<Vertex> GetValidNeighbors(Vertex current)
        {
            // Из всех смежных вершин оставляем только те, которые 
            // 1. Еще не посещены
            // 2. Не являются вершинами-препятствиями
            // 3. Наклон к которым меньше заданной величины (например, 30 градусов)
            return GetAllAdjacentVertices(current).Where(v => !v.IsVisited && !v.IsObstacle && Slope(v, current) < MaxSlope).ToList();
        }

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

        /// <summary>
        /// Задает высоты определенных вершин графа. Необходим для создания (имитации) на карте местности сооружений/зданий
        /// </summary>
        /// <param name="bottomLeftCoordinate">левая нижняя координата сооружения. От нее будет начинаться отсчет</param>
        /// <param name="width">ширина сооружения (количество вершин, которое будет занимать сооружение по оси Ox)</param>
        /// <param name="length">длина сооружения (количество вершин, которое будет занимать сооружение по оси Oy)</param>
        /// <param name="height">высота сооружения</param>
        public void CreateBuilding(Point2D bottomLeftCoordinate, int width, int length, double height)
        {
            for (int i = bottomLeftCoordinate.i; i < bottomLeftCoordinate.i + width; i++)
            {
                for (int j = bottomLeftCoordinate.j; j < bottomLeftCoordinate.j + length; j++)
                    Vertices[i, j].Height = height;
            }
        }

        /// <summary>
        /// Записывает поверхность в файл
        /// </summary>
        /// <param name="fileName">имя файла</param>
        public void WriteSurfaceToFile(string fileName)
        {
            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                for (int j = 0; j < M; j++)
                {
                    for (int i = 0; i < N; i++)
                        outputFile.Write(Vertices[i, j].Height.ToString() + ";");

                    outputFile.WriteLine();
                }
            }
        }

        /// <summary>
        /// Инициализирует граф с помощью композиций функций Гаусса
        /// </summary>
        /// <param name="dx">величина шага по оси Ox</param>
        /// <param name="dy">величина шага по оси Oy</param>
        /// <param name="N">количество вершин по оси Ox</param>
        /// <param name="M">количество вершин по оси Oy</param>
        /// <param name="MaxSlope">предельная величина уклона</param>
        /// <param name="gaussianParameters">список параметров для вычисления Гауссианов</param>
        public Graph(double dx, double dy, int N, int M, double MaxSlope, params GaussianParameter[] gaussianParameters)
        {
            this.dx = dx;
            this.dy = dy;
            this.N = N;
            this.M = M;
            this.MaxSlope = MaxSlope;

            Vertices = new Vertex[N, M];

            for (int j = 0; j < M; j++)
                for (int i = 0; i < N; i++)
                {
                    double height = 0.0;
                    foreach (GaussianParameter gp in gaussianParameters)
                        height += Surface.Gaussian(i * dx, j * dy, gp);

                    Vertices[i, j] = new Vertex(i, j, Height: height);
                }
        }

        /// <summary>
        /// Инициализирует граф с помощью матрицы препятствий
        /// </summary>
        /// <param name="obstacleMatrix">матрица с препятствиями</param>
        /// <param name="MaxSlope">предельная величина уклона</param>
        public Graph(int[,] obstacleMatrix, double MaxSlope = 20.0)
        {
            this.N = Convert.ToInt32(obstacleMatrix.GetLongLength(1));
            this.M = Convert.ToInt32(obstacleMatrix.GetLongLength(0));
            this.dx = 1.0;
            this.dy = 1.0;
            this.MaxSlope = MaxSlope;

            Vertices = new Vertex[N, M];

            for (int j = 0; j < M; j++)
                for (int i = 0; i < N; i++)
                {
                    bool isObstacle = Convert.ToBoolean(obstacleMatrix[j, i]);
                    Vertices[i, j] = new Vertex(i, j, IsObstacle: isObstacle);
                }
        }        
    }
}