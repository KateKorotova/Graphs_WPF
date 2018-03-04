using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Graphs_Lab1
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        int count_click = 0;
        Graph mygraph = new Graph();
        int radius = 15;
        int checkPaintVrt = -1;
        Vertex tempVrt = null;

        internal void paintVer(Vertex ver, SolidColorBrush color)
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
            vertex.Stroke = Brushes.DarkGreen;
            vertex.StrokeThickness = 1;
            Canvas.SetLeft(vertex, ver.x - radius);
            Canvas.SetTop(vertex, ver.y - radius);
            Canvas.SetLeft(txt, ver.x - radius/3);
            Canvas.SetTop(txt, ver.y - radius/3);

            graph.Children.Add(vertex);
            graph.Children.Add(txt);
        }
        internal List<double> paintArrow(double x1, double y1, double x2, double y2)
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

        internal void paintEdge(Vertex vrtx1, Vertex vrtx2, SolidColorBrush color)
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

        internal void createEdge(Vertex vrtx1, Vertex vrtx2, SolidColorBrush color)
        {
            paintEdge(vrtx1, vrtx2, color);
            mygraph.vertexes[vrtx1.number].neighbors.Add(new Edge(vrtx1, vrtx2, 1, color));

        }

        //internal void paintGraph(Graph mygraph)
        //{
        //    foreach(Vertex vrtx in mygraph.vertexes)
        //        foreach(Edge edge in vrtx.neighbors)
        //            paintEdge(edge.from, edge.to, edge.color);
        //    foreach (Vertex vrtx in mygraph.vertexes)
        //        paintVer(vrtx);
        //}

        internal void paintAlg(List<Tuple<Vertex, Vertex, SolidColorBrush>> steps) 
        {
            SolidColorBrush[] colors = new SolidColorBrush[mygraph.vertexes.Count];
            for(int i = 0; i< colors.Length; i++)
                colors[i] = mygraph.vertexes[i].color;
            foreach(Tuple<Vertex, Vertex, SolidColorBrush> step in steps)
            {
                if(step.Item2 == null) // vertex
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
                System.Threading.Thread.Sleep(500);
            }
        }
        internal Vertex check(double x, double y, double radius)
        {
            foreach (Vertex ver in mygraph.vertexes)
            {
                double del = (x - ver.x) * (x - ver.x) + (y - ver.y) * (y - ver.y);
                if (del <= radius * radius)
                    return ver;
            }
            return null;
        }

        private void graph_MouseUp(object sender, MouseButtonEventArgs e)
        {

            Point position = e.GetPosition(graph);
            Vertex myvertex = check(position.X, position.Y, radius);
            if (myvertex == null)
            {
                Vertex ver = new Vertex(count_click);
                ver.x = position.X ;
                ver.y = position.Y ;
                ver.color = Brushes.MediumAquamarine; 
                paintVer(ver, ver.color);
                mygraph.vertexes.Add(ver);
                count_click++;
            }
            else
            {
                myvertex.color = Brushes.LightGreen; 
                paintVer( myvertex, myvertex.color);
                if (checkPaintVrt == -1)
                { 
                    checkPaintVrt = 1;
                    tempVrt = myvertex;
                }
                else
                {
                    tempVrt.color = Brushes.MediumAquamarine;
                    myvertex.color = Brushes.MediumAquamarine;
                    createEdge(tempVrt, myvertex, Brushes.DarkGreen); 
                    paintVer(tempVrt, tempVrt.color);
                    paintVer(myvertex, myvertex.color);
                    checkPaintVrt = -1;
                }  
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
              paintAlg(mygraph.bfs(0));
        }
    }
}
