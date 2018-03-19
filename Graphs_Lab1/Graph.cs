using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


namespace Graphs_Lab1
{

    internal class Edge
    {
        internal Edge(Vertex f, Vertex t, int w, SolidColorBrush color)
        {
            from = f;
            to = t;
            weight = w;
            this.color = color;
        }
        internal Vertex from;
        internal Vertex to;
        internal int weight;
        internal SolidColorBrush color;
    }

    internal class Vertex
    {
        internal Vertex(int num)
        {
            this.number = num;
            neighbors = new List<Edge>();
        }
        internal int number;
        internal List<Edge> neighbors;
        internal double x;
        internal double y;
        internal SolidColorBrush color;
    }

    internal class Graph
    {
        internal Graph()
        {
            vertexes = new List<Vertex>();
        }

        internal List<Vertex> vertexes;

        internal int size()
        {
            return vertexes.Count;
        }
        
        internal void read(string file)
        {
            vertexes = new List<Vertex>();
            Random r = new Random();
            StreamReader sr = new StreamReader(file);
            string line = sr.ReadLine();
            int count_edges;
            bool k = Int32.TryParse(line, out count_edges);
            string ori = sr.ReadLine();
            int orient; 
            bool l = Int32.TryParse(ori, out orient);
            string countv = sr.ReadLine();
            int count; 
            bool m = Int32.TryParse(countv, out count);
            for (int iter = 0; iter < count; iter++)
            {
                vertexes.Add(new Vertex(iter));
                vertexes[iter].x = r.Next(5, 400);
                vertexes[iter].y = r.Next(5, 400);
                vertexes[iter].color = Brushes.Moccasin;
            }


            for (int iter = 0; iter < count_edges; iter++)
            {
                string linetemp = sr.ReadLine();
                string[] edge = linetemp.Split(' ');
                int i;
                int j;
                int w;
                bool b = Int32.TryParse(edge[0], out i);
                bool c = Int32.TryParse(edge[1], out j);
                bool a = Int32.TryParse(edge[2], out w);
                vertexes[i].neighbors.Add(new Edge(vertexes[i], vertexes[j], w, Brushes.Wheat));
                if (orient == 1)
                    vertexes[j].neighbors.Add(new Edge(vertexes[j], vertexes[i], w, Brushes.Wheat));

            }
            sr.Close();
        }

        internal int[,] matrix()
        {
            int[,] matrix = new int[vertexes.Count, vertexes.Count];
            for (int i = 0; i < vertexes.Count; i++)
            {
                if (vertexes[i].neighbors.Count != 0)
                {
                    for (int j = 0; j < vertexes[i].neighbors.Count; j++)
                        matrix[(vertexes[i].number), (vertexes[i].neighbors[j].to.number)] = vertexes[i].neighbors[j].weight;
                }
            }
            return matrix;
        }

        private Tuple<Vertex, Vertex, SolidColorBrush> addVrtx(Vertex my, SolidColorBrush color)
        {
          return new Tuple<Vertex, Vertex, SolidColorBrush>(my, null, color);
        }

        private Tuple<Vertex, Vertex, SolidColorBrush> addEdge(Vertex from, Vertex to, SolidColorBrush color)
        {
            return new Tuple<Vertex, Vertex, SolidColorBrush>(from, to, color);
        }

        internal List<Tuple<Vertex, Vertex, SolidColorBrush>> bfs(int index)
        {
            Queue<Vertex> queue = new Queue<Vertex>();
            List<Tuple<Vertex, Vertex, SolidColorBrush>> result = new List<Tuple<Vertex, Vertex, SolidColorBrush>>();
            bool[] check = new bool[vertexes.Count];
            for (int i = 0; i < vertexes.Count; i++)
                check[i] = true;
            queue.Enqueue(vertexes[index]);
            check[index] = false; 
            result.Add(addVrtx(vertexes[index], Brushes.Yellow));   // vertex in queue
            while (queue.Count != 0)
            {
                Vertex temp = queue.Dequeue();
                result.Add(addVrtx(temp, Brushes.Red)); //stand on vertex
                foreach (Edge edge in temp.neighbors)
                {
                    if (check[edge.to.number] == true)
                    {
                        result.Add(addEdge(temp, edge.to, Brushes.Yellow));
                        result.Add(addVrtx(edge.to, Brushes.Yellow));
                        check[edge.to.number] = false;
                        queue.Enqueue(edge.to);
                    }
                }
                result.Add(addVrtx(temp, Brushes.DarkBlue)); //end of check of vertex
            }
            return result;
        }

        internal List<Tuple<Vertex, Vertex, SolidColorBrush>> dfs(int index)
        {
            Stack<Vertex> stack = new Stack<Vertex>();
            List<Tuple<Vertex, Vertex, SolidColorBrush>> result = new List<Tuple<Vertex, Vertex, SolidColorBrush>>();
            bool[] check = new bool[vertexes.Count];
            for (int i = 0; i < vertexes.Count; i++)
                check[i] = true;
            stack.Push(vertexes[index]);
            check[index] = false;
            result.Add(addVrtx(vertexes[index], Brushes.Red));   // vertex in stack
            while (stack.Count != 0)
            {
                bool del = true;
                Vertex temp = stack.Peek();
                result.Add(addVrtx(temp, Brushes.Orange)); //stand on vertex
                foreach (Edge edge in temp.neighbors)
                {
                    if (check[edge.to.number] == true)
                    {
                        result.Add(addEdge(temp, edge.to, Brushes.Red));
                        result.Add(addVrtx(edge.to, Brushes.Orange));
                        result.Add(addVrtx(edge.from, Brushes.Red));
                        check[edge.to.number] = false;
                        stack.Push(edge.to);
                        del = false;
                        break;
                    }

                }
                if (del == true)
                {
                    stack.Pop();
                    result.Add(addVrtx(temp, Brushes.DarkBlue)); //end of check of vertex
                    if(stack.Count != 0)
                        result.Add(addVrtx(stack.Peek(), Brushes.Orange)); //end of check of vertex
  
                }
            }
            return result;

        }

        internal int[] dijkstra(int index)
        {
            int[] minway = new int[size()];
            for (int i = 0; i < vertexes.Count; i++)
            {
                if (i == index)
                    minway[i] = 0;
                else
                    minway[i] = -1;
            }
            for (int i = 0; i < vertexes.Count - 1; i++)
            {
                int min = 0;
                for (int j = 1; j < vertexes.Count; j++)
                {

                    if (minway[min] > minway[j])
                    {
                        min = j;
                    }
                }
                foreach (Edge edge in vertexes[min].neighbors)
                {
                    int weight = edge.weight;
                    if (minway[edge.to.number] == -1)
                    {
                        minway[edge.to.number] = weight + minway[min];
                    }

                    else
                    {
                        if (minway[edge.to.number] > minway[min] + weight)
                        {
                            minway[edge.to.number] = minway[min] + weight;
                        }
                    }

                }

            }
            return minway;
        }

        internal int[,] floyd_worshell()
        {
            int[,] ways = new int[vertexes.Count, vertexes.Count];
            ways = matrix();
            for (int i = 0; i < vertexes.Count; i++)
                for (int j = 0; j < vertexes.Count; j++)
                    if (ways[i, j] == 0)
                        ways[i, j] = Int32.MaxValue / 2 - 1;
                    for (int k = 0; k < vertexes.Count; k++)
                for (int i = 0; i < vertexes.Count; i++)
                    for (int j = 0; j < vertexes.Count; j++)
                        if (ways[i, k] + ways[k, j] < ways[i, j])
                            ways[i, j] = ways[i, k] + ways[k, j];
            for (int i = 0; i < vertexes.Count; i++)
                for (int j = 0; j < vertexes.Count; j++)
                    if (ways[i, j] == Int32.MaxValue / 2 - 1)
                        ways[i, j] = -1;
            for (int i = 0; i < vertexes.Count; i++)
                ways[i, i] = 0;
            return ways; 
        }

        private int find_color(ref int[] colors, int v)
        {
            if (v == colors[v])
                return v;
            colors[v] = find_color(ref colors, colors[v]);
            return colors[v];
        }

        internal List<Tuple<Vertex, Vertex, SolidColorBrush>> kruskal()
        {
            List<Tuple<Vertex, Vertex, SolidColorBrush>> result = new List<Tuple<Vertex, Vertex, SolidColorBrush>>();
            List<Edge> edges = new List<Edge>();
            foreach (Vertex vertex in vertexes)
                foreach (Edge edge in vertex.neighbors)
                    edges.Add(edge);
            edges.Sort((a, b) => a.weight.CompareTo(b.weight));
            int[] colors = new int[vertexes.Count];
            for (int i = 0; i < vertexes.Count; i++)
                colors[i] = i;
            for(int i = 0; i < edges.Count; i++)
            {
                int c_from = find_color(ref colors, edges[i].from.number);
                int c_to = find_color(ref colors, edges[i].to.number);
                if (c_from != c_to)
                {
                    colors[c_from] = c_to;
                    result.Add(addVrtx(edges[i].from, Brushes.Blue));
                    result.Add(addEdge(edges[i].to, edges[i].from, Brushes.Blue));
                    result.Add(addVrtx(edges[i].to, Brushes.Blue));
                }
            }
            return result;
        }

        internal List<Tuple<Vertex, Vertex, SolidColorBrush>> prima(int index)
        {
            List<Tuple<Vertex, Vertex, SolidColorBrush>> result = new List<Tuple<Vertex, Vertex, SolidColorBrush>>();
            int[] check = new int[vertexes.Count];
            for (int i = 0; i < vertexes.Count; i++)
            {
                if (i == index)
                    check[i] = 0;
                else
                    check[i] = -1;
            }
            List<Vertex> tree = new List<Vertex>();
            tree.Add(vertexes[index]);
            result.Add(addVrtx(vertexes[index], Brushes.Blue));
            int minweight = Int16.MaxValue / 2;
            int numberto = 0;
            int numberfrom = 0; 
            for (int i = 0; i < vertexes.Count; i++)
            {
                foreach (Vertex vrtx in tree)
                {
                    foreach (Edge edge in vrtx.neighbors)
                    {
                        if (minweight > edge.weight && check[edge.to.number] == -1)
                        {
                            minweight = edge.weight;
                            numberfrom = edge.from.number;
                            numberto = edge.to.number;
                        }
                    }
                }
                check[numberto] = 0;
                tree.Add(vertexes[numberto]);
                result.Add(addEdge(vertexes[numberfrom], vertexes[numberto], Brushes.Blue));
                result.Add(addVrtx(vertexes[numberto], Brushes.Blue));
                minweight = Int16.MaxValue / 2; 
            }
            return result; 
        }

        internal int[] bellmanford(int index)
        {
            int[] minway = new int[vertexes.Count];
            for (int i = 0; i < vertexes.Count; i++)
                    minway[i] = Int16.MaxValue/2 - 2;
            minway[index] = 0;
            List<Edge> edges = new List<Edge>();
            foreach (Vertex vertex in vertexes)
                foreach (Edge edge in vertex.neighbors)
                    edges.Add(edge);
            for(int i = 0; i < vertexes.Count - 1; i++)
                for(int j = 0; j < edges.Count; j++)
                {
                    if (minway[edges[j].to.number] > minway[edges[j].from.number] + edges[j].weight)
                        minway[edges[j].to.number] = minway[edges[j].from.number] + edges[j].weight;
                }
            for (int i = 0; i < minway.Length; i++)
                if (minway[i] == Int16.MaxValue / 2 - 2)
                    minway[i] = -1; 
            return minway;
        }


    }
}
