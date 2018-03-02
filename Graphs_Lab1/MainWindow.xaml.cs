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

        internal void paintVer(Vertex ver)
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
            vertex.Fill = ver.color;
            vertex.Stroke = Brushes.DarkGreen;
            vertex.StrokeThickness = 1;
            Canvas.SetLeft(vertex, ver.x - radius);
            Canvas.SetTop(vertex, ver.y - radius);
            Canvas.SetLeft(txt, ver.x - radius/3);
            Canvas.SetTop(txt, ver.y - radius/3);
            graph.Children.Add(vertex);
            graph.Children.Add(txt);
        }


        internal void paintEdge(Vertex vrtx1, Vertex vrtx2)
        {
            Line edge = new Line();
            edge.X1 = vrtx1.x;
            edge.Y1 = vrtx1.y;
            edge.X2 = vrtx2.x;
            edge.Y2 = vrtx2.y;
            edge.Stroke = Brushes.DarkGreen;
            edge.StrokeThickness = 1;
            edge.HorizontalAlignment = HorizontalAlignment.Left;
            edge.VerticalAlignment = VerticalAlignment.Center;
            graph.Children.Add(edge);
            mygraph.vertexes[vrtx1.number].neighbors.Add(new Tuple<Vertex, int>(vrtx2, 1));
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
                paintVer(ver);
                mygraph.vertexes.Add(ver);
                count_click++;
            }
            else
            {
                myvertex.color = Brushes.LightGreen; 
                paintVer( myvertex);
                if (checkPaintVrt == -1)
                { 
                    checkPaintVrt = 1;
                    tempVrt = myvertex;
                }
                else
                {
                    tempVrt.color = Brushes.MediumAquamarine;
                    myvertex.color = Brushes.MediumAquamarine;
                    paintEdge(tempVrt, myvertex); 
                    paintVer(tempVrt);
                    paintVer(myvertex);
                    checkPaintVrt = -1;
                }  
            }
        }

  

    }
}
