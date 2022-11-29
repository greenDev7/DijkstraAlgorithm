# DijkstraAlgorithm

Реализация алгоритма Дейкстры для поиска кратчайшего пути между двумя вершинами и оптимального маршрута на 3D поверхности с использованием очереди с приоритетом (PriorityQueue).

## Пример 1 (Обход простого препятствия)

Исходный файл obstacle1.csv: 

``` csharp
0;0;0;0;0;0;0;0;0;0;0;0;0;0;0
0;0;0;0;0;0;0;0;0;0;0;0;0;0;0
0;0;0;0;0;0;0;1;0;0;0;0;0;0;0
0;0;0;0;0;0;0;1;0;0;0;0;0;0;0
0;0;0;0;0;0;0;1;0;0;0;0;0;0;0
0;0;0;0;0;0;0;1;0;0;0;0;0;0;0
0;0;0;0;0;0;0;1;0;0;0;0;0;0;0
0;0;0;0;0;0;0;1;0;0;0;0;0;0;0
0;0;0;0;0;0;0;0;0;0;0;0;0;0;0
0;0;0;0;0;0;0;0;0;0;0;0;0;0;0

```

Program.cs:

``` csharp
static void Main(string[] args)
        {
            // Создаем матрицу-препятствий из csv-файла
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            int[,] obstacleMatrix = Obstacle.CreateObstacleMatrixFromCSVFile(Path.Combine(docPath, "obstacle1.csv"));

            // Инициализируем граф с помощью этой матрицы
            Graph graph = new Graph(obstacleMatrix);

            // Вычисляем кратчайший путь
            double shortestPathLength = 0.0;           

            Point2D startPoint = new Point2D(3, 4);
            Point2D goalPoint = new Point2D(12, 4);

            List<Point2D> shortestPath = graph.FindShortestPathAndLength(startPoint, goalPoint, out shortestPathLength);

            // Записываем найденный путь в файл
            //WriteShortestPathToFile(shortestPath, Path.Combine(docPath, "shortestPath.txt"));

            Console.WriteLine("Coordinates of shortest path: ");
            foreach (Point2D p in shortestPath)
                Console.WriteLine(string.Format("({0}, {1})", p.i, p.j));

            Console.WriteLine(string.Format("Length of shortest path: {0}", shortestPathLength));

            Console.ReadLine();
        }
```

Вывод в консоль:

``` csharp
Coordinates of shortest path:
(12, 4)
(11, 3)
(10, 2)
(9, 1)
(8, 1)
(7, 1)
(6, 2)
(5, 3)
(4, 4)
(3, 4)
Length of shortest path: 11,4852813742386
```

Результат:

![screenshot1](https://github.com/greenDev7/DijkstraAlgorithm/blob/master/Examples/Obstacle1/shortestPath.png)


## Пример 2 (Поиск оптимального пути на 3D-поверхности)

Program.cs:

``` csharp
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
```

Результат работы программы:
 - surface.txt --- Матрица со значениями 3D поверхности;
 - shortestPath.txt --- файл с трехмерными координатами оптимального пути

Визуализация:

![screenshot1](https://github.com/greenDev7/DijkstraAlgorithm/blob/master/Examples/3D/Surface/3dView1.png)