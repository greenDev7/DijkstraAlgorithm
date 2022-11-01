using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraAlgorithm
{
    public class Vertex
    {
        public Point2D Coordinate { get; set; }
        public double Height { get; set; }
        public List<Label> Labels { get; set; }
        public bool IsVisited { get; set; }

        /// <summary>
        /// Возвращает true, если координаты совпадают, иначе false
        /// </summary>
        /// <param name="i">Координата по оси Ox</param>
        /// <param name="j">Координата по оси Oy</param>
        /// <returns></returns>
        public bool HasCoordinates(int i, int j)
        {
            return Coordinate.i == i && Coordinate.j == j;
        }



        public Vertex(int i, int j, double Height = 0.0, List<Label> Labels = null, bool IsVisited = false)
        {
            Coordinate = new Point2D(i, j);
            this.Height = Height;
            this.Labels = Labels;
            this.IsVisited = IsVisited;
        }
    }
}