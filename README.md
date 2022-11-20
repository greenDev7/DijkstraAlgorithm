# DijkstraAlgorithm

Реализация алгоритма Дейкстры для поиска кратчайшего пути между двумя вершинами и оптимального маршрута на 3D поверхности с использованием очереди с приоритетом (PriorityQueue).

## Использование

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
