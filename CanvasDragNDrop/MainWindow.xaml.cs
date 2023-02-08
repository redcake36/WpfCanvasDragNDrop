using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace CanvasDragNDrop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool canDrowLine = false;
        bool startDrow = false;
        bool linePathStarted = false;
        Line? currentLine = null;
        Point? prewPoint = null;
        public static bool state = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Pen_Click(object sender, RoutedEventArgs e)
        {
            
            canDrowLine = !canDrowLine;
            state = !state;
            if (canDrowLine)
            {
                Mouse.OverrideCursor = Cursors.Pen;
            }
            else { EndLineDrawMode(); }

            // для проверки пересечения
            //Rectangle r = new Rectangle();
            //var g = br.RenderedGeometry.Clone();
            //g.Transform = GetFullTransform(br);
            //var q = redr.RenderedGeometry.Clone();
            //q.Transform = GetFullTransform(redr);
            //Trace.WriteLine("HasIntersection " + HasIntersection(g, q).ToString());
            //Canvas.SetLeft(br, 99);
        }
        private static bool HasIntersection(Geometry g1, Geometry g2) =>
    g1.FillContainsWithDetail(g2) != IntersectionDetail.Empty;
        private static Transform GetFullTransform(UIElement e)
        {
            // The order in which transforms are applied is important!
            var transforms = new TransformGroup();

            if (e.RenderTransform != null)
                transforms.Children.Add(e.RenderTransform);

            var xTranslate = (double)e.GetValue(Canvas.LeftProperty);
            if (double.IsNaN(xTranslate))
                xTranslate = 0D;

            var yTranslate = (double)e.GetValue(Canvas.TopProperty);
            if (double.IsNaN(yTranslate))
                yTranslate = 0D;

            var translateTransform = new TranslateTransform(xTranslate, yTranslate);
            transforms.Children.Add(translateTransform);

            return transforms;
        }

        protected void canvas_MouseMove(object sender, MouseEventArgs e)
        {

            if (startDrow)
            {

                if (currentLine is null)
                {

                    Trace.WriteLine("newLine");
                    currentLine = new Line();
                    currentLine.Stroke = System.Windows.Media.Brushes.Red;

                    if (linePathStarted && prewPoint is not null)
                    {
                        currentLine.X1 = prewPoint.Value.X;
                        currentLine.Y1 = prewPoint.Value.Y;
                    }
                    else
                    {
                        currentLine.X1 = e.GetPosition(canvas).X;
                        currentLine.Y1 = e.GetPosition(canvas).Y;
                    }
                    currentLine.X2 = e.GetPosition(canvas).X;
                    currentLine.Y2 = e.GetPosition(canvas).Y;

                    currentLine.StrokeThickness = 3;
                    canvas.Children.Add(currentLine);
                }
                Trace.WriteLine(e.GetPosition(canvas).X.ToString(), e.GetPosition(canvas).Y.ToString());
                if (Math.Abs(currentLine.X1 - e.GetPosition(canvas).X) > Math.Abs(currentLine.Y1 - e.GetPosition(canvas).Y))
                {
                    currentLine.X2 = e.GetPosition(canvas).X;
                    currentLine.Y2 = currentLine.Y1;
                }
                else
                {
                    currentLine.X2 = currentLine.X1;
                    currentLine.Y2 = e.GetPosition(canvas).Y;
                }
                prewPoint = new Point(currentLine.X2, currentLine.Y2);

            }
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (canDrowLine)
            {
                if (startDrow)
                {
                    linePathStarted = true;
                    Trace.WriteLine("startDrow= false;");
                    currentLine.Stroke = System.Windows.Media.Brushes.Black;
                    currentLine = null;
                    Trace.WriteLine("currentLine = null; ");
                }
                else
                {
                    startDrow = true;
                    Trace.WriteLine("startDrow = true");
                }
            }

        }
        void EndLineDrawMode()
        {
            linePathStarted = false;
            prewPoint = null;
            startDrow = false;
            canvas.Children.Remove(currentLine);
            currentLine = null;

            canDrowLine = false;
            state = true;
            Mouse.OverrideCursor = null;
        }
        private void canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            EndLineDrawMode();
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            // activeLine = true;
            Trace.WriteLine("activeLine = true;");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            canvas.Children.Add(new LogicElement(
                100,
                100,
                "rect",
                new SolidColorBrush(Color.FromRgb((byte)r.Next(0, 255), (byte)r.Next(0, 255), (byte)r.Next(0, 255)))));
            elementList.Items.Add((new ListBoxItem()).Content="rect");
        }
        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            var obj = e.Data.GetData(typeof(LogicElement)) as LogicElement;
            if (obj != null)
                Canvas.SetZIndex(obj, 0);
        }

        private void canvas_DragOver(object sender, DragEventArgs e)
        {
            Point dropPoint = e.GetPosition(canvas);

            var obj = e.Data.GetData(typeof(LogicElement)) as LogicElement;
            if (obj != null)
            {
                Canvas.SetLeft(obj, dropPoint.X);
                Canvas.SetTop(obj, dropPoint.Y);
                Canvas.SetZIndex(obj,1);
            }
        }

        //private void RectObj_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        DragDrop.DoDragDrop(redRectangle, redRectangle, DragDropEffects.Move);
        //    }
        //    if (e.LeftButton == MouseButtonState.Released)
        //    {
        //        Trace.WriteLine(sender);
        //    }
        //}

        //private void Canvas_Drop(object sender, DragEventArgs e)
        //{
        //    Point p = e.GetPosition(canvas);

        //    Trace.WriteLine(sender);
        //}
    }
}
