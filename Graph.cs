using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraAlgorithm
{
    public class Graph
    {
        public double dx { get; set; }
        public double dy { get; set; }
        public int N { get; set; }
        public int M { get; set; }
        public Vertex[,] Vertices { get; }
        public Func<double, double, double> SurfaceFunc { get; set; }

        public Graph(double dx, double dy, int N, int M, Func<double, double, double> SurfaceFunc)
        {
            this.N = N;
            this.M = M;
            this.dx = dx;
            this.dy = dy;
            this.SurfaceFunc = SurfaceFunc;

            Vertices = new Vertex[N, M];

            for (int i = 0; i < N; i++)
                for (int j = 0; j < M; j++)
                {
                    double height = SurfaceFunc(i * dx, j * dy);
                    Vertices[i, j] = new Vertex(height);
                }
        }
    }
}