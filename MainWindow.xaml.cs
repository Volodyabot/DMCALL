using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;


namespace DMCALL
{
    public partial class MainWindow : Window
    {
        private Graph graph = new Graph();
        private GraphMatrix graphMatrix = new GraphMatrix();
        public MainWindow()
        {
            InitializeComponent();
            GridRowTextBlock.Height = new GridLength(100);
        }

        public void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        public void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";
            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    string[] text = File.ReadAllLines(openFileDialog.FileName);
                    int[,] matrix = ToInt2d(text);

                    graphMatrix.matrix = matrix;

                    graph = GraphFromArray2d(matrix);
                    string message = string.Format("Opened file ({0})\\{1}", System.IO.Path.GetDirectoryName(openFileDialog.FileName), openFileDialog.FileName);
                    ShowMessage(message, MessageType.Info);
                    Draw(graph);
                }
            }
            catch
            {
                ShowMessage("An error occurred. Please try again.", MessageType.Error);
            }
        }

        private int[,] OpenFileToArray2D()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    string[] text = File.ReadAllLines(openFileDialog.FileName);
                    int[,] matrix = ToInt2d(text);

                    string message = string.Format("Opened file ({0})\\{1}", System.IO.Path.GetDirectoryName(openFileDialog.FileName), openFileDialog.FileName);
                    ShowMessage(message, MessageType.Info);

                    return matrix;
                }
            }
            catch
            {
                ShowMessage("An error occurred. Please try again.", MessageType.Error);
            }
            return null;
        }

        public enum MessageType
        {
            Info = 0,
            Warning = 1,
            Sucsess = 2,
            Error = 3
        }

        private void ShowMessage(string message, MessageType messageType)
        {
            if (TextOutput.Inlines.Count >= 40)
            {
                TextOutput.Inlines.Clear();
            }
            switch (messageType)
            {
                case MessageType.Info:
                    var run0 = new Run(message + "\n");
                    //run0.Foreground = Brushes.Black;
                    TextOutput.Inlines.Add(run0);
                    break;
                case MessageType.Warning:
                    var run1 = new Run(message + "\n");
                    run1.Foreground = Brushes.DarkOrange;
                    TextOutput.Inlines.Add(run1);
                    break;
                case MessageType.Sucsess:
                    var run2 = new Run(message + "\n");
                    run2.Foreground = Brushes.LightGreen;
                    TextOutput.Inlines.Add(run2);
                    break;
                case MessageType.Error:
                    var run3 = new Run(message + "\n");
                    run3.Foreground = Brushes.Red;
                    TextOutput.Inlines.Add(run3);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(messageType), "Invalid message type");
            }
            Scroll.ScrollToBottom();
        }

        private static int[,] ToInt2d(string[] array)
        {
            int[,] int_array = new int[array.Length - 1, array.Length - 1];

            for (int i = 0; i < int_array.GetLength(0); i++)
            {
                for (int k = 0; k < int_array.GetLength(1); k++)
                {
                    int_array[i, k] = int.Parse(array[i + 1].Split(' ')[k]);
                }
            }
            return int_array;
        }
        private Graph GraphFromArray2d(int[,] array2d)
        {
            Graph graph = new Graph();

            int arraySize = array2d.GetLength(0);

            List<Vertex> vertices = new List<Vertex>();
            for (int i = 0; i < arraySize; i++)
            {
                vertices.Add(new Vertex(50, i.ToString()));
            }

            List<Edge> edges = new List<Edge>();

            Random random = new Random();
            bool overlap = true;
            double minDistance = 50;
            double Size = 50;

            for (int i = 0; i < arraySize; i++)
            {
                overlap = true;
                while (overlap)
                {
                    overlap = false;
                    vertices[i].X = random.Next((int)GraphCanvas.ActualWidth - (int)Size);
                    vertices[i].Y = random.Next((int)GraphCanvas.ActualHeight - (int)Size);
                    foreach (Vertex element in vertices)
                    {
                        if (element != vertices[vertices.IndexOf(element)])
                        {
                            double x1 = element.X + Size / 2;
                            double y1 = element.Y + Size / 2;
                            double x2 = vertices[i].X + Size / 2;
                            double y2 = vertices[i].Y + Size / 2;
                            double distance = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
                            if (distance < minDistance)
                            {
                                overlap = true;
                                break;
                            }
                        }
                    }
                }
                for (int j = 0; j < arraySize; j++)
                {
                    if (array2d[i, j] > 0)
                    {
                        Edge edge = new Edge();
                        edge.Value = array2d[i, j];
                        edge.textBlock.Text = edge.Value.ToString();
                        edge.Start = vertices[i];
                        edge.End = vertices[j];
                        edges.Add(edge);
                    }
                }
            }
            graph.Vertices = vertices;
            graph.Edges = edges;

            return graph;
        }

        public void Draw(Graph graph)
        {
            GraphCanvas.Children.Clear();

            foreach (Vertex vertex in graph.Vertices)
            {
                Canvas.SetLeft(vertex.ellipse, vertex.X);
                Canvas.SetTop(vertex.ellipse, vertex.Y);

                GraphCanvas.Children.Add(vertex.ellipse);

                Canvas.SetLeft(vertex.textBlock, vertex.X + (vertex.ellipse.Width / 2) - 4);
                Canvas.SetTop(vertex.textBlock, vertex.Y + (vertex.ellipse.Height / 2) - 11);
                GraphCanvas.Children.Add(vertex.textBlock);

                vertex.ellipse.MouseLeftButtonDown += Ellipse_MouseLeftButtonDown;
                vertex.ellipse.MouseMove += Ellipse_MouseMove;
                vertex.ellipse.MouseLeftButtonUp += Ellipse_MouseLeftButtonUp;
                vertex.ellipse.MouseRightButtonDown += Ellipse_MouseRightButtonDown;
            }
            foreach (Edge edge in graph.Edges)
            {
                edge.line.X1 = edge.Start.X + edge.Start.ellipse.Width / 2;
                edge.line.Y1 = edge.Start.Y + edge.Start.ellipse.Height / 2;

                edge.line.X2 = edge.End.X + edge.End.ellipse.Width / 2;
                edge.line.Y2 = edge.End.Y + edge.End.ellipse.Height / 2;

                GraphCanvas.Children.Insert(0, edge.line);

                Canvas.SetLeft(edge.textBlock, (edge.Start.X + edge.End.X) / 2);
                Canvas.SetTop(edge.textBlock, (edge.Start.Y + edge.End.Y) / 2);
                GraphCanvas.Children.Add(edge.textBlock);

            }
        }

        private Vertex selectedVertex = null;
        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = (Ellipse)sender;
            if (!isDrawable)
            {
                ellipse.CaptureMouse();
            }
            selectedVertex = graph.Vertices.Find(n => n.ellipse == sender);
            string message = string.Format("Index: {0}; Vertex cords: [{1}], [{2}];", selectedVertex.Name, selectedVertex.X, selectedVertex.Y);
            //ShowMessage(message,MessageType.Info);
        }
        private void Ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            Ellipse ellipse = (Ellipse)sender;
            if (ellipse.IsMouseCaptured)
            {
                if (selectedVertex != null)
                {
                    selectedVertex.X = e.GetPosition(GraphCanvas).X - ellipse.ActualWidth / 2;
                    selectedVertex.Y = e.GetPosition(GraphCanvas).Y - ellipse.ActualHeight / 2;

                    if (selectedVertex.X > GraphCanvas.ActualWidth - ellipse.ActualWidth)
                    {
                        selectedVertex.X = GraphCanvas.ActualWidth - ellipse.ActualWidth;
                    }
                    else if (selectedVertex.X < 0)
                    {
                        selectedVertex.X = 0;
                    }
                    if (selectedVertex.Y > GraphCanvas.ActualHeight - ellipse.ActualHeight)
                    {
                        selectedVertex.Y = GraphCanvas.ActualHeight - ellipse.ActualHeight;
                    }
                    else if (selectedVertex.Y < 0)
                    {
                        selectedVertex.Y = 0;
                    }
                    /*Rect newRect = new Rect(selectedVertex.X, selectedVertex.Y, selectedVertex.ellipse.Width + 1, selectedVertex.ellipse.Height + 1);
                    foreach (UIElement element in GraphCanvas.Children)
                    {
                        if (element is Ellipse && element != selectedVertex.ellipse)
                        {
                            Rect elementRect = new Rect(Canvas.GetLeft(element), Canvas.GetTop(element), element.RenderSize.Width, element.RenderSize.Height);
                            if (newRect.IntersectsWith(elementRect))
                            {
                                ShowMessage("Intersects", MessageType.Warning);
                            }
                        }
                    }*/

                    Canvas.SetLeft(ellipse, selectedVertex.X);
                    Canvas.SetTop(ellipse, selectedVertex.Y);

                    foreach (Vertex vertex in graph.Vertices)
                    {
                        Canvas.SetLeft(vertex.textBlock, vertex.X + (vertex.ellipse.Width / 2) - 4);
                        Canvas.SetTop(vertex.textBlock, vertex.Y + (vertex.ellipse.Height / 2) - 11);
                    }
                    foreach (Edge edge in graph.Edges)
                    {
                        edge.line.X1 = edge.Start.X + edge.Start.ellipse.Width / 2;
                        edge.line.Y1 = edge.Start.Y + edge.Start.ellipse.Height / 2;

                        edge.line.X2 = edge.End.X + edge.End.ellipse.Width / 2;
                        edge.line.Y2 = edge.End.Y + edge.End.ellipse.Height / 2;

                        Canvas.SetLeft(edge.textBlock, (edge.Start.X + edge.End.X) / 2);
                        Canvas.SetTop(edge.textBlock, (edge.Start.Y + edge.End.Y) / 2);
                    }
                }
            }
        }
        private void Ellipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = (Ellipse)sender;
            ellipse.ReleaseMouseCapture();
        }
        private void Ellipse_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectedVertex = graph.Vertices.Find(n => n.ellipse == sender);
            if (selectedVertex.ellipse.Stroke == Brushes.Black)
                selectedVertex.ellipse.Stroke = null;
            else
                selectedVertex.ellipse.Stroke = Brushes.Black;

            selectedVertex.ellipse.StrokeThickness = 2;

        }

        private bool isDrawable;
        private void DrawableChecked(object sender, RoutedEventArgs e)
        {
            ShowMessage("isDrawable Checked", MessageType.Info);
            isDrawable = true;
            GraphCanvas.Cursor = Cursors.Pen;
        }

        private void DrawableUnchecked(object sender, RoutedEventArgs e)
        {
            ShowMessage("isDrawable Unchecked", MessageType.Info);
            isDrawable = false;
            GraphCanvas.Cursor = Cursors.Arrow;
        }

        private void IsomorphismButton(object sender, RoutedEventArgs e)
        {
            if (graphMatrix.matrix != null)
            {
                try { Isomorphism(); }
                catch { ShowMessage("Error occured!", MessageType.Error); }
            }
            else
            {
                OpenFile();
                if (graphMatrix.matrix != null)
                {
                    try { Isomorphism(); }
                    catch { ShowMessage("Error occured!", MessageType.Error); }
                }
            }
        }

        private void Isomorphism()
        {


            int[,] array2d = OpenFileToArray2D();
            bool isISomorphic = VF2.Main(graphMatrix.matrix, array2d);
            string message = "Graphs are isomorphic: ";
            message += string.Format("{0}", isISomorphic);
            ShowMessage(message, MessageType.Info);
        }

        private void ComivoyagerButton(object sender, RoutedEventArgs e)
        {
            if (graphMatrix.matrix != null)
            {
                try { Comivoyager(); }
                catch { ShowMessage("Error occured!", MessageType.Error); }
            }
            else
            {
                OpenFile();
                if (graphMatrix.matrix != null)
                {
                    try { Comivoyager(); }
                    catch { ShowMessage("Error occured!", MessageType.Error); }
                }
            }
        }

        private void Comivoyager()
        {

            int[] tour = TSP.NearestNeighbor(graphMatrix.matrix, 0);

            List<int> tourList = tour.ToList();
            string message = string.Format("Path of vertices: ");
            foreach (int i in tourList)
            {
                message += string.Format("   {0}", i);
            }
            ShowMessage(message, MessageType.Info);
        }

        private void MaxFlowButton(object sender, RoutedEventArgs e)
        {
            if (graphMatrix.matrix != null)
            {
                try { MaxFlow(); }
                catch { ShowMessage("Error occured!", MessageType.Error); }
            }
            else
            {
                OpenFile();
                if (graphMatrix.matrix != null)
                {
                    try { MaxFlow(); }
                    catch { ShowMessage("Error occured!", MessageType.Error); }
                }
            }
        }

        private void MaxFlow()
        {
            bool RetuntList = true;
            List<int> flowPath = MaxFlowAlgorithm.FordFulkersonAlgo(graphMatrix.matrix, 0, graphMatrix.matrix.GetLength(0) - 1, RetuntList);
            string message = string.Format("Path of vertices: ");
            foreach (int i in flowPath)
            {
                message += string.Format("   {0}", i);
            }
            message += string.Format("\tFlow is: {0}", MaxFlowAlgorithm.FordFulkersonAlgo(graphMatrix.matrix, 0, graphMatrix.matrix.GetLength(0) - 1));
            ShowMessage(message, MessageType.Info);
        }

        private void PostManButton(object sender, RoutedEventArgs e)
        {
            if (graphMatrix.matrix != null)
            {
                PostMan();
            }
            else
            {
                OpenFile();
                if (graphMatrix.matrix != null)
                {
                    PostMan();
                }
            }
        }

        private void PostMan()
        {
            List<int> list = PostManAlgorithm.Main(graphMatrix.matrix);
            string message = string.Format("Vertices path: ");
            foreach (int i in list)
            {
                message += string.Format("   {0}", i);
            }
            message += string.Format("   {0}", list[0]);
            ShowMessage(message, MessageType.Info);
        }

        private void DijkstraButton(object sender, RoutedEventArgs e)
        {
            if (graphMatrix.matrix != null)
            {
                Dijkstra();
            }
            else
            {
                OpenFile();
                if (graphMatrix.matrix != null)
                {
                    Dijkstra();
                }
            }
        }

        public void Dijkstra()
        {
            ShowMessage("Dijkstra() //", MessageType.Warning);

            int[] array = DijkstraAlgorithm.Dijkstra(graphMatrix.matrix, 0);
            string message = "";
            for (int i = 0; i < array.Length; i++)
            {
                message += string.Format("{0} \t {1}\n", i, array[i]);
            }
            ShowMessage("Vertex \t Distance from Start" + " \t Start point is 0 \t Endpoint is 3", MessageType.Info);
            ShowMessage(message, MessageType.Info);

            List<int> list = DijkstraAlgorithm.Dijkstra(graphMatrix.matrix, 0, 3);
            message = string.Format("Vertices path: ");
            foreach (int e in list)
            {
                message += string.Format("\t{0}", e);
            }
            ShowMessage(message, MessageType.Info);

            ShowMessage("//", MessageType.Warning);
        }

        private bool isDrawing = false;
        private Point startPoint;
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isDrawable)
            {
                isDrawing = true;
            }

            startPoint = e.GetPosition((Canvas)sender);
            startPoint.Y += 18;
        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawable && isDrawing)
            {
                Line line = new Line();
                line.Stroke = Brushes.Gray;
                line.X1 = startPoint.X;
                line.Y1 = startPoint.Y - 18;
                line.X2 = e.GetPosition(this).X;
                line.Y2 = e.GetPosition(this).Y - 18;

                startPoint = e.GetPosition(this);

                GraphCanvas.Children.Insert(0, line);
            }
        }
        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDrawing = false;
        }
        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {
            var list = new List<Line>();
            foreach (var child in GraphCanvas.Children)
            {
                if (child is Line line && line.Stroke == Brushes.Gray)
                {
                    list.Add(line);
                    // GraphCanvas.Children.Remove((UIElement)child);
                }
            }
            foreach (var line in list)
            {
                GraphCanvas.Children.Remove(line);
            }
        }

        private void SwitchTheme_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary newDictionary = new ResourceDictionary();
            MenuItem menuItem = (MenuItem)sender;

            // Check which theme is currently applied
            if (Application.Current.Resources.MergedDictionaries[0].Source.OriginalString.EndsWith("LightTheme.xaml"))
            {
                newDictionary.Source = new Uri("DarkTheme.xaml", UriKind.Relative);
                menuItem.Header = "Light";
            }
            else
            {
                newDictionary.Source = new Uri("LightTheme.xaml", UriKind.Relative);
                menuItem.Header = "Dark";
            }

            // Replace the current resource dictionary with the new one
            Application.Current.Resources.MergedDictionaries.RemoveAt(0);
            Application.Current.Resources.MergedDictionaries.Insert(0, newDictionary);

            foreach (var child in GraphCanvas.Children)
            {
                if (child is Line)
                {
                    Line line = child as Line;
                    if (line.Stroke != Brushes.Gray)
                    { line.Stroke = (Brush)FindResource("ForegroundColor"); }
                }
                else if (child is TextBlock)
                {
                    TextBlock textBlock = child as TextBlock;
                    textBlock.Foreground = (Brush)FindResource("ForegroundColor");
                }
                else if (child is Ellipse)
                {
                    Ellipse ellipse = child as Ellipse;
                    ellipse.Fill = (Brush)FindResource("EllipseColor");
                }
            }

        }

    }

    public class Vertex
    {
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public Ellipse ellipse { get; set; }
        public TextBlock textBlock { get; set; }

        public Vertex(double ellipseSize, string name)
        {
            ellipse = new Ellipse();
            ellipse.Width = ellipseSize;
            ellipse.Height = ellipseSize;
            ellipse.Fill = (Brush)ellipse.FindResource("EllipseColor");

            textBlock = new TextBlock();
            textBlock.Text = name;
            textBlock.FontSize = 16;
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.Foreground = (Brush)textBlock.FindResource("ForegroundColor");

            Name = name;
        }
    }

    public class Edge
    {
        public Vertex Start { get; set; }
        public Vertex End { get; set; }
        public double Value { get; set; }

        public Line line { get; set; }
        public TextBlock textBlock { get; set; }

        public Edge()
        {
            line = new Line();
            line.StrokeThickness = 2;
            line.Stroke = (Brush)line.FindResource("ForegroundColor");

            textBlock = new TextBlock();
            textBlock.Text = Value.ToString();
            textBlock.FontSize = 16;
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.Foreground = (Brush)textBlock.FindResource("ForegroundColor");
        }
    }

    public class Graph
    {
        public List<Vertex> Vertices { get; set; }
        public List<Edge> Edges { get; set; }

        public Graph()
        {
            Vertices = new List<Vertex>();
            Edges = new List<Edge>();
        }
    }

    public class GraphMatrix
    {
        public int[,] matrix { get; set; }
    }
}
