using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs_Lab1
{
    internal class Vertex
    {
        internal Vertex(int num)
        {
            this.number = num;
            neighbors = new List<Tuple<Vertex, int>>();
        }
        internal int number;
        internal List<Tuple<Vertex, int>> neighbors;
        internal double x;
        internal double y; 
    }

    internal class Edge
    {
        internal Edge(Vertex f, Vertex t, int w)
        {
            from = f;
            to = t;
            weight = w;
        }
        internal Vertex from;
        internal Vertex to;
        internal int weight;
    }
    internal class Graph
    {
        internal Graph(int count)
        {
            this.count = count;
            vertexes = new List<Vertex>;
        }
        internal int count;
        internal List<Vertex> vertexes;

        internal void read()
        {
            int count_edges;
            Console.WriteLine("Write numbers of edges: ");
            bool k = Int32.TryParse(Console.ReadLine(), out count_edges);
            for (int iter = 0; iter < count; iter++)
            {
                vertexes[iter] = new Vertex(iter);
            }
            for (int iter = 0; iter < count_edges; iter++)
            {
                string putin = Console.ReadLine();
                string[] edge = putin.Split(' ');
                int i;
                int j;
                int w;
                bool b = Int32.TryParse(edge[0], out i);
                bool c = Int32.TryParse(edge[1], out j);
                bool a = Int32.TryParse(edge[2], out w);
                vertexes[i].neighbors.Add(new Tuple<Vertex, int>(vertexes[j], w));
            }
        }

        internal int[,] matrix()
        {
            int[,] matrix = new int[count, count];
            for (int i = 0; i < count; i++)
            {
                if (vertexes[i].neighbors.Count != 0)
                {
                    for (int j = 0; j < vertexes[i].neighbors.Count; j++)
                        matrix[(vertexes[i].number), (vertexes[i].neighbors[j].Item1.number)] = vertexes[i].neighbors[j].Item2;
                }
            }
            return matrix;
        }

        internal List<Vertex> bfs(int index)
        {
            Queue<Vertex> queue = new Queue<Vertex>();
            List<Vertex> result = new List<Vertex>();
            bool[] check = new bool[count];
            for (int i = 0; i < count; i++)
                check[i] = true;
            queue.Enqueue(vertexes[index]);
            while (queue.Count != 0)
            {
                Vertex temp = queue.Dequeue();
                result.Add(temp);
                foreach (Tuple<Vertex, int> ver in temp.neighbors)
                {
                    if (check[ver.Item1.number] == true)
                    {
                        check[ver.Item1.number] = false;
                        queue.Enqueue(ver.Item1);
                    }
                }
            }
            return result;
        }

        internal void dfs(int index)
        {
            Stack<Vertex> stack = new Stack<Vertex>();
            bool[] check = new bool[count];
            for (int i = 0; i < count; i++)
                check[i] = true;
            stack.Push(vertexes[index]);
            while (stack.Count != 0)
            {
                bool del = true;
                Vertex temp = stack.Peek();

                foreach (Tuple<Vertex, int> ver in temp.neighbors)
                {
                    if (check[ver.Item1.number] == true)
                    {
                        check[ver.Item1.number] = false;
                        stack.Push(ver.Item1);
                        del = false;
                        break;
                    }

                }
                if (del == true)
                    stack.Pop();
            }

        }

        internal void dijkstra(int index)
        {
            int[] minway = new int[count];
            for (int i = 0; i < count; i++)
            {
                if (i == index)
                    minway[i] = 0;
                else
                    minway[i] = -1;
            }
            for (int i = 0; i < count - 1; i++)
            {
                int min = 0;
                for (int j = 1; j < count; j++)
                {
                    if (minway[min] > minway[j])
                        min = j;
                }
                foreach (Tuple<Vertex, int> edge in vertexes[min].neighbors)
                {
                    int weight = edge.Item2;
                    if (minway[edge.Item1.number] == -1)
                        minway[edge.Item1.number] = weight + minway[min];
                    else
                    {
                        if (minway[edge.Item1.number] > minway[min] + weight)
                            minway[edge.Item1.number] = minway[min] + weight;
                    }

                }

            }
        }

        internal void floyd_worshell()
        {
            int[,] ways = new int[count, count];
            ways = matrix();
            for (int k = 0; k < count; k++)
                for (int i = 0; i < count; i++)
                    for (int j = 0; j < count; j++)
                        if (ways[i, k] + ways[k, j] < ways[i, j])
                            ways[i, j] = ways[i, k] + ways[k, j];

        }

        internal int find_color(ref int[] colors, int v)
        {
            if (v == colors[v])
                return v;
            colors[v] = find_color(ref colors, colors[v]);
            return colors[v];
        }


        internal void Kruskal()
        {
            List<Edge> edges = new List<Edge>();
            foreach (Vertex vertex in vertexes)
                foreach (Tuple<Vertex, int> neighbour in vertex.neighbors)
                    edges.Add(new Edge(vertex, neighbour.Item1, neighbour.Item2));
            edges.Sort((a, b) => a.weight.CompareTo(b.weight));

            int[] colors = new int[count];
            for (int i = 0; i < count; i++)
                colors[i] = i;

        }

    }
}
