namespace DijkstraAlgorithm
{
    /// <summary>
    /// Вспомогательный класс для хранения параметров для вычисления двумерного Гауссиана
    /// </summary>
    public class GaussianParameter
    {
        /// <summary>
        /// Высота колокола
        /// </summary>
        public double A { get; }
        /// <summary>
        /// Размах колокола по оси Ox
        /// </summary>
        public double sigmaX { get; }
        /// <summary>
        /// Размах колокола по оси Oy
        /// </summary>
        public double sigmaY{ get; }
        /// <summary>
        /// Сдвиг пика по оси Ox
        /// </summary>
        public double x0 { get; }
        /// <summary>
        /// Сдвиг пика по оси Oy
        /// </summary>
        public double y0 { get; }

        public GaussianParameter(double A, double sigmaX, double sigmaY, double x0, double y0)
        {
            this.A = A;
            this.sigmaX = sigmaX;
            this.sigmaY = sigmaY;
            this.x0 = x0;
            this.y0 = y0;
        }
    }
}
