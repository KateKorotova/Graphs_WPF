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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        int count_click = 0;
        Graph mygraph; 

        private void graph_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Vertex ver = new Vertex(count_click);
            TextBlock txt = new TextBlock();
            txt.Text = ver.number.ToString();
            txt.FontSize = 10;
            txt.Width = 40;
            txt.Height = 30;
            txt.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            txt.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            Ellipse vertex = new Ellipse();
            vertex.Width = 40;
            vertex.Height = 40;
            vertex.Fill = Brushes.Aqua;
            vertex.Stroke = Brushes.Black;
            vertex.StrokeThickness = 2;
            Point position = e.GetPosition(graph);
            Canvas.SetLeft(vertex, position.X - vertex.Width / 2);
            Canvas.SetTop(vertex, position.Y - vertex.Height / 2);
            Canvas.SetLeft(txt, position.X);
            Canvas.SetTop(txt, position.Y);
            graph.Children.Add(vertex);
            graph.Children.Add(txt);
            ver.x = position.X - vertex.Width / 2;
            ver.y = position.Y - vertex.Height / 2;
            mygraph.vertexes.Add(ver); 
            count_click++;
        }




    }
}
