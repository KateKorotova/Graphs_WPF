using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graphs_Lab1
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        Graph mygraph = new Graph();
        int radius = 15;
        int checkPaintVrt = -1;
        Vertex tempVrt = null;
        int orient = 0;
        int numbercurrvrtx; 

        private void paintVer(Vertex ver, SolidColorBrush color)
        {
            TextBlock txt = new TextBlock();
            txt.Text = ver.number.ToString();
            txt.FontSize = 10;
            txt.Width = 40;
            txt.Height = 30;
            txt.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            txt.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            Ellipse vertex = new Ellipse();
            vertex.Width = radius * 2;
            vertex.Height = radius * 2;
            vertex.Fill = color;
            vertex.Stroke = Brushes.Wheat;
            vertex.StrokeThickness = 1;
            Canvas.SetLeft(vertex, ver.x - radius);
            Canvas.SetTop(vertex, ver.y - radius);
            Canvas.SetLeft(txt, ver.x - radius/3 + 2);
            Canvas.SetTop(txt, ver.y - radius/3 - 2);

            graph.Children.Add(vertex);
            graph.Children.Add(txt);
        }

        private List<double> paintArrow(double x1, double y1, double x2, double y2)
        {
            List<double> result = new List<double>();
            int l = 10;
            double alfa = 180 - 35;
            double d = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
            double x = ((d - radius) * x2 + radius * x1) / d;
            double y = ((d - radius) * y2 + radius * y1) / d;
            result.Add(x);
            result.Add(y);
            double tempx = ((d - radius - l) * x2 + (radius + l) * x1)/d;
            double tempy = ((d - radius - l) * y2 + (radius + l) * y1)/d;
            double x0 = Math.Cos(alfa) * (tempx - x) - Math.Sin(alfa) * (tempy - y) + x;
            double y0 = Math.Sin(alfa) * (tempx - x) + Math.Cos(alfa) * (tempy - y) + y;
            result.Add(x0);
            result.Add(y0);
            double x3 = Math.Cos(alfa) * (tempx - x) + Math.Sin(alfa) * (tempy - y) + x;
            double y3 = Math.Sin(alfa) * (-1) * (tempx - x) + Math.Cos(alfa) * (tempy - y) + y;
            result.Add(x3);
            result.Add(y3);
            return result;
        }

        private void paintEdge(Vertex vrtx1, Vertex vrtx2, SolidColorBrush color)
        {

            Line edge = new Line();
            edge.X1 = vrtx1.x;
            edge.Y1 = vrtx1.y;
            edge.X2 = vrtx2.x;
            edge.Y2 = vrtx2.y;

            edge.Stroke = color;
            edge.StrokeThickness = 1;
            edge.HorizontalAlignment = HorizontalAlignment.Left;
            edge.VerticalAlignment = VerticalAlignment.Center;
            graph.Children.Add(edge);
            if (orient == 2)
            {
                Polygon arrow = new Polygon();
                List<double> points = paintArrow(edge.X1, edge.Y1, edge.X2, edge.Y2);
                arrow.Points.Add(new Point(points[0], points[1]));
                arrow.Points.Add(new Point(points[2], points[3]));
                arrow.Points.Add(new Point(points[4], points[5]));
                arrow.Stroke = color;
                arrow.Fill = color;
                arrow.StrokeThickness = 1;
                graph.Children.Add(arrow);
            }
        }

        private void paintEdgeWeight(Vertex vrtx1, Vertex vrtx2, SolidColorBrush color, int weight)
        {
            paintEdge(vrtx1, vrtx2, color);
            TextBlock txt = new TextBlock();
            if (weight != 0)
                txt.Text = weight.ToString();
            else
                txt.Text = "1";
            txt.FontSize = 10;
            txt.Width = 40;
            txt.Height = 30;
            txt.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            txt.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            Point w = new Point();
            w.X = (vrtx1.x + vrtx2.x) / 2;
            w.Y = (vrtx1.y + vrtx2.y) / 2;
            Canvas.SetLeft(txt, w.X);
            Canvas.SetTop(txt, w.Y);
            graph.Children.Add(txt);
            textBox.Clear();
        }

        private void createEdge(Vertex vrtx1, Vertex vrtx2, SolidColorBrush color)
        {
            string text = textBox.Text;
            int weight = 1;
            bool t = Int32.TryParse(text, out weight);
            if (t == false)
                weight = 1;
            if (orient == 1)
            {
                mygraph.vertexes[vrtx1.number].neighbors.Add(new Edge(vrtx1, vrtx2, weight, color));
                mygraph.vertexes[vrtx2.number].neighbors.Add(new Edge(vrtx2, vrtx1, weight, color));
            }
            else
                mygraph.vertexes[vrtx1.number].neighbors.Add(new Edge(vrtx1, vrtx2, weight, color));
            paintEdgeWeight(vrtx1, vrtx2, color, weight);
        }

        private void repaintGraph()
        {
            foreach (Vertex vrtx in mygraph.vertexes)
                foreach (Edge edge in vrtx.neighbors)
                    paintEdgeWeight(edge.from, edge.to, edge.color, edge.weight);
            foreach (Vertex vrtx in mygraph.vertexes)
                paintVer(vrtx, vrtx.color);
        }

        private void paintAlg(List<Tuple<Vertex, Vertex, SolidColorBrush>> steps) 
        {
            SolidColorBrush[] colors = new SolidColorBrush[mygraph.vertexes.Count];
            for(int i = 0; i< colors.Length; i++)
                colors[i] = mygraph.vertexes[i].color;
            foreach(Tuple<Vertex, Vertex, SolidColorBrush> step in steps)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    if (step.Item2 == null) // vertex
                    {
                        paintVer(step.Item1, step.Item3);
                        colors[step.Item1.number] = step.Item3;
                    }
                    else
                    {
                        paintEdge(step.Item1, step.Item2, step.Item3);
                        paintVer(step.Item1, colors[step.Item1.number]);
                        paintVer(step.Item2, colors[step.Item2.number]);
                    }

                }));
                System.Threading.Thread.Sleep(700);
            }
        }

        private Vertex check(double x, double y, double radius)
        {
            foreach (Vertex ver in mygraph.vertexes)
            {
                double del = (x - ver.x) * (x - ver.x) + (y - ver.y) * (y - ver.y);
                if (del <= radius * radius)
                    return ver;
            }
            return null;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int index = comboBox.SelectedIndex;
            int indexvrtx = 0;
            if (checkPaintVrt == 1)
            {
                indexvrtx = numbercurrvrtx;
                checkPaintVrt = -1;
                mygraph.vertexes[numbercurrvrtx].color = Brushes.Moccasin;
            }
            Task.Run(() =>
            {

                switch (index)
                {
                    case 0:
                        paintAlg(mygraph.bfs(indexvrtx));
                        break;
                    case 1:
                        paintAlg(mygraph.dfs(indexvrtx));
                        break;
                    case 2:
                        paintAlg(mygraph.kruskal());
                        break;
                    case 3:
                        paintAlg(mygraph.prima(indexvrtx));
                        break;
                    case 4:
                        int[] result = mygraph.bellmanford(indexvrtx);
                        string message = "";
                        for (int i = 0; i < mygraph.vertexes.Count; i++)
                        {
                            if (result[i] == 0)
                                message += " Start Vertex: " + i.ToString() + "\n";
                            if (result[i] == -1)
                                message += ("vertex: " + i.ToString() + " - minimum distance: " + "INF" + "\n");
 
                            else
                                message += ("vertex: " + i.ToString() + " - minimum distance: " + result[i].ToString() + "\n");
                        }
                        MessageBox.Show("INF - you don't have common edge");
                        MessageBox.Show(message);
                        break;
                    case 5:
                        int[] result1 = mygraph.dijkstra(indexvrtx);
                        string message1 = "";
                        for (int i = 0; i < mygraph.vertexes.Count; i++)
                        {
                            if (result1[i] == 0)
                                message1 += " Start Vertex: " + i.ToString() + "\n";
                            if (result1[i] == -1)
                                message1 += ("vertex: " + i.ToString() + " - minimum distance: " + "INF" + "\n");
                            else
                                message1 += ("vertex: " + i.ToString() + " - minimum distance: " + result1[i].ToString() + "\n");
                        }
                        MessageBox.Show("INF - you don't have common edge");
                        MessageBox.Show(message1);
                        break;
                    case 6:
                        int[,] res = mygraph.floyd_worshell();
                        string mess = "";
                        for (int i = 0; i < mygraph.vertexes.Count; i++)
                        {
                            for (int j = 0; j < mygraph.vertexes.Count; j++)
                            {
                                if (res[i, j] == -1)
                                    mess += "INF"+ "\t";
                                else
                                    mess += (res[i, j].ToString() + "\t");
                            }
                            mess += "\n";
                        }
                        MessageBox.Show("INF - you don't have common edge");
                        MessageBox.Show(mess);

                        break;

                }
            });
            int a = comboBox.SelectedIndex;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            repaintGraph();
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void comboBox_Initialized(object sender, EventArgs e)
        {
            List<string> algorithms = new List<string>();
            algorithms.Add("Bfs");
            algorithms.Add("Dfs");
            algorithms.Add("Kruskal algorithm");
            algorithms.Add("Prima algorithm");
            algorithms.Add("Bellman-Ford algorithm");
            algorithms.Add("Dijkstra algorithm");
            algorithms.Add("Floyd-Worshell algoritm");
            algorithms.Add("Johnson's algorithm");
            algorithms.Add("Ford–Fulkerson algorithm");
            algorithms.Add("Edmonds–Karp algorithm");
            foreach (string algorithm in algorithms)
                comboBox.Items.Add(algorithm);
            comboBox.SelectedIndex = 0;
        }

        private void graph_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            Point position = e.GetPosition(graph);
            Vertex myvertex = check(position.X, position.Y, radius);
            if (myvertex == null)
            {
                Vertex ver = new Vertex(mygraph.size());
                ver.x = position.X;
                ver.y = position.Y;
                ver.color = Brushes.Moccasin;
                paintVer(ver, ver.color);
                mygraph.vertexes.Add(ver);
            }
            else
            {
                myvertex.color = Brushes.LightGreen;
                paintVer(myvertex, myvertex.color);
                if (checkPaintVrt == -1)
                {
                    checkPaintVrt = 1;
                    numbercurrvrtx = myvertex.number;
                    tempVrt = myvertex;
                }
                else
                {
                    tempVrt.color = Brushes.Moccasin;
                    myvertex.color = Brushes.Moccasin;
                    createEdge(tempVrt, myvertex, Brushes.Wheat);
                    paintVer(tempVrt, tempVrt.color);
                    paintVer(myvertex, myvertex.color);
                    checkPaintVrt = -1;
                }
            }
        }

        private void graph_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(graph);
            Vertex myvertex = check(position.X, position.Y, radius);
            int num = myvertex.number;
            for (int i = 0; i < mygraph.size(); i++)
                for (int j = 0; j < mygraph.vertexes[i].neighbors.Count; j++)
                    if (mygraph.vertexes[i].neighbors[j].to.number == num)
                    {
                        mygraph.vertexes[i].neighbors.RemoveAt(j);
                        break;
                    }

            mygraph.vertexes.RemoveAt(num);
            for (int i = num; i < mygraph.size(); i++)
            {
                mygraph.vertexes[i].number = i;
            }
            graph.Children.Clear();
            repaintGraph();
        }

        private void textBox_Initialized(object sender, EventArgs e)
        {
            textBox.Text = "Weight";
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            int check = 0;
            foreach (Vertex vrtx in mygraph.vertexes)
                if (vrtx.neighbors.Count != 0)
                {
                    check = 1;
                    break;
                }
            if (check != 0)
            {
                graph.Children.Clear();
                mygraph = new Graph();
            }

            orient = 1;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            int check = 0;
            foreach (Vertex vrtx in mygraph.vertexes)
                if (vrtx.neighbors.Count != 0)
                {
                    check = 1;
                    break;
                }
            if (check != 0)
            {
                graph.Children.Clear();
                mygraph = new Graph();
            }
            orient = 2;
        }

        private void download(object sender, RoutedEventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        graph.Children.Clear();
                        mygraph.read(openFileDialog1.FileName);
                        repaintGraph();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void delete(object sender, RoutedEventArgs e)
        {
            graph.Children.Clear();
            mygraph = new Graph();
        }
    }
}
