using System.Collections.Generic;

namespace DijkstraAlgorithm
{
    /// <summary>
    /// Реализует интерфейс сравнения двух вещественных чисел
    /// </summary>
    class MyDoubleComparer : IComparer<double>
    {
        public int Compare(double x, double y)
        {
            if (x < y)
                return 1;
            if (x > y)
                return -1;
            else
                return 0;
        }
    }
}