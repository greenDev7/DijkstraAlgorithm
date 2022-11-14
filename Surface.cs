using System;

namespace DijkstraAlgorithm
{
    public static class Surface
    {
        public static double Plane(double x, double y) => 0.0;

        /// <summary>
        /// Возвращает значение функции Гаусса (двумерный Гауссиан).
        /// Описание параметров (раздел "Многомерные обобщения"): https://ru.wikipedia.org/wiki/%D0%93%D0%B0%D1%83%D1%81%D1%81%D0%BE%D0%B2%D0%B0_%D1%84%D1%83%D0%BD%D0%BA%D1%86%D0%B8%D1%8F
        /// </summary>
        /// <param name="x">абсцисса точки, в которой необходимо вычислить двумерный Гауссиан</param>
        /// <param name="y">ордината точки, в которой необходимо вычислить двумерный Гауссиан</param>
        /// <param name="A">высота колокола</param>
        /// <param name="sigmaX">размах колокола по оси Ox</param>
        /// <param name="sigmaY">размах колокола по оси Oy</param>
        /// <param name="x0">сдвиг пика по оси Ox</param>
        /// <param name="y0">сдвиг пика по оси Oy</param>
        /// <returns></returns>
        public static double Gaussian(double x, double y, double A, double sigmaX, double sigmaY, double x0, double y0)
        {
            double xx = Math.Pow(x - x0, 2.0) / (2.0 * Math.Pow(sigmaX, 2.0));
            double yy = Math.Pow(y - y0, 2.0) / (2.0 * Math.Pow(sigmaY, 2.0));

            return A * Math.Exp(-(xx + yy));
        }

        /// <summary>
        /// Возвращает значение функции Гаусса (двумерный Гауссиан).
        /// </summary>
        /// <param name="x">абсцисса точки, в которой необходимо вычислить двумерный Гауссиан</param>
        /// <param name="y">ордината точки, в которой необходимо вычислить двумерный Гауссиан</param>
        /// <param name="gaussianParameters">набор параметров</param>
        /// <returns></returns>
        public static double Gaussian(double x, double y, GaussianParameter gaussianParameters)
        {
            double xx = Math.Pow(x - gaussianParameters.x0, 2.0) / (2.0 * Math.Pow(gaussianParameters.sigmaX, 2.0));
            double yy = Math.Pow(y - gaussianParameters.y0, 2.0) / (2.0 * Math.Pow(gaussianParameters.sigmaY, 2.0));

            return gaussianParameters.A * Math.Exp(-(xx + yy));
        }
    }
}
