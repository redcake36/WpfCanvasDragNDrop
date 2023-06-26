﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
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
using System.IO;
using Newtonsoft.Json;
using System.Security.Principal;
using System.Windows.Ink;
using System.Net;
using System.Windows.Markup;
using CanvasDragNDrop.UserItems;

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


        public List<LogicElement> LogicElements = new List<LogicElement>();
        public List<MashaDBClass> mashaDBClasses = new List<MashaDBClass>();

        public List<UserControl> userControls = new List<UserControl>();
        public string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
        void DrowGrid()
        {
            for (int i = 10; i < canvas.Width; i += 10)
            {
                CustomLine customLine = new CustomLine(i, 0, i, canvas.Height);
                //Line l = new Line();
                //l.X1 = i;
                //l.Y1 = 0;
                //l.X2 = i;
                //l.Y2 = canvas.Height;
                //l.StrokeThickness = 1;
                //l.Stroke = Brushes.Gray;
                canvas.Children.Add(customLine.GetLine());
            }
            for (int i = 10; i < canvas.Height; i += 10)
            {
                Line l = new Line();
                l.X1 = 0;
                l.Y1 = i;
                l.X2 = canvas.Width;
                l.Y2 = i;
                l.StrokeThickness = 1;
                l.Stroke = Brushes.Gray;
                canvas.Children.Add(l);
            }
        }
        void GetFromServerElemList(object sender, RoutedEventArgs e)
        {
            UIElementList.Children.Clear();
            mashaDBClasses.Clear();
            LogicElements.Clear();
            Trace.WriteLine(UIElementList.Children.Count);
            var samplelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MashaDBClass>>
                (Get(RootUrl.rootServer + "/get_models"));
            foreach (var item in samplelist)
            {
                MashaDBClass m = new MashaDBClass();
                byte[] bytes = Encoding.Default.GetBytes(item.Description);
                var desc = Encoding.UTF8.GetString(bytes);
                m.Description = desc;
                bytes = Encoding.Default.GetBytes(item.Title);
                var ttl = Encoding.UTF8.GetString(bytes);
                m.Title = ttl;
                m.Id = item.Id;
                m.InputFlows = item.InputFlows;
                m.OutputFlows = item.OutputFlows;
                mashaDBClasses.Add(m);
            }
            foreach (var item in mashaDBClasses)
            {
                LogicElement l = new LogicElement(item);
                LogicElements.Add(l);
                MenuItem menuItem = new MenuItem();
                menuItem.Header = item.Title;
                menuItem.Click += new RoutedEventHandler(Button_Click);
                UIElementList.Children.Add(menuItem);
            }

        }
        void GetFromJsonElemList()
        {
            string json = File.ReadAllText("element.json");
            Trace.WriteLine(json);
            mashaDBClasses = JsonConvert.DeserializeObject<List<MashaDBClass>>(json);

            foreach (var item in mashaDBClasses)
            {
                LogicElement l = new LogicElement(item);
                LogicElements.Add(l);
                Button btn = new Button();
                btn.Style = Resources["Cmbbtn"] as Style;
                btn.Content = item.Title;
                btn.Click += new RoutedEventHandler(Button_Click);
                UIElementList.Children.Add(btn);
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            DrowGrid();

            if (RootUrl.AutomotiveWork)
            {
                GetFromJsonElemList();
            }
            else
            {
                GetFromServerElemList(null, null);
            }

            //// string json = File.ReadAllText(rootFolder + @"element.json");
            // Trace.WriteLine(json);
            // mashaDBClasses = JsonConvert.DeserializeObject<List<MashaDBClass>>(json);
            // foreach (var item in mashaDBClasses)
            // {
            //     Trace.WriteLine(item.title);
            // }

            //elementList.Items.Add((new ListBoxItem()).Content = "Паровой котел");
            //elementList.Items.Add((new ListBoxItem()).Content = "Дымовая труба");
            //elementList.Items.Add((new ListBoxItem()).Content = "Паровая турбина");
            //elementList.Items.Add((new ListBoxItem()).Content = "Конденсатор");
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
                currentLine.X2 = e.GetPosition(canvas).X - 1f;
                currentLine.Y2 = e.GetPosition(canvas).Y - 1f;
                //if (currentLine is null)
                //{

                //    Trace.WriteLine("newLine");
                //    currentLine = new Line();
                //    currentLine.Stroke = System.Windows.Media.Brushes.Red;

                //    if (linePathStarted && prewPoint is not null)
                //    {
                //        currentLine.X1 = prewPoint.Value.X;
                //        currentLine.Y1 = prewPoint.Value.Y;
                //    }
                //    else
                //    {
                //        currentLine.X1 = e.GetPosition(canvas).X;
                //        currentLine.Y1 = e.GetPosition(canvas).Y;
                //    }
                //    currentLine.X2 = e.GetPosition(canvas).X;
                //    currentLine.Y2 = e.GetPosition(canvas).Y;

                //    currentLine.StrokeThickness = 3;
                //    canvas.Children.Add(currentLine);
                //}
                //Trace.WriteLine(e.GetPosition(canvas).X.ToString(), e.GetPosition(canvas).Y.ToString());
                //// для ломанной
                ////if (Math.Abs(currentLine.X1 - e.GetPosition(canvas).X) > Math.Abs(currentLine.Y1 - e.GetPosition(canvas).Y))
                ////{
                ////	currentLine.X2 = e.GetPosition(canvas).X;
                ////	currentLine.Y2 = currentLine.Y1;
                ////}
                ////else
                ////{
                ////	currentLine.X2 = currentLine.X1;
                ////	currentLine.Y2 = e.GetPosition(canvas).Y;
                ////}
                //currentLine.X2 = e.GetPosition(canvas).X - 1f;
                //currentLine.Y2 = e.GetPosition(canvas).Y - 1f;
                //prewPoint = new Point(currentLine.X2, currentLine.Y2);

            }
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dp = LogicalTreeHelper.GetParent(e.OriginalSource as DependencyObject);
            //LogicalTreeHelper.GetParent( e.OriginalSource as DependencyObject)
            Trace.WriteLine(e.Source + " !!! " + e.GetPosition(e.Source as UIElement)
                + e.OriginalSource.ToString() + " !!! " + dp);
            if (canDrowLine && typeof(FlowPoint).IsAssignableFrom(dp.GetType()))
            {
                Trace.WriteLine((dp as FlowPoint).pointName);
                if (startDrow && (dp as FlowPoint).type == "in")
                {
                    linePathStarted = true;
                    Trace.WriteLine("startDrow= false;");
                    currentLine.Stroke = System.Windows.Media.Brushes.Black;
                    (dp as FlowPoint).connectedLines.Add(currentLine);
                    currentLine = null;
                    Trace.WriteLine("currentLine = null; ");
                    startDrow = false;
                }
                else if ((dp as FlowPoint).type == "out")
                {
                    currentLine = new Line();
                    currentLine.Stroke = System.Windows.Media.Brushes.Red;
                    currentLine.X1 = e.GetPosition(canvas).X;
                    currentLine.Y1 = e.GetPosition(canvas).Y;
                    currentLine.X2 = e.GetPosition(canvas).X;
                    currentLine.Y2 = e.GetPosition(canvas).Y;
                    (dp as FlowPoint).connectedLines.Add(currentLine);
                    currentLine.StrokeThickness = 3;
                    canvas.Children.Add(currentLine);
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
            //Random r = new Random();
            //LogicElement le = new LogicElement(
            //    100,
            //    100,
            //    "rect",
            //    "white",
            //    30.0,
            //    3, 3);

            //LogicElement? le = JsonConvert.DeserializeObject<LogicElement>(json);
            DependencyObject dp = LogicalTreeHelper.GetParent(sender as DependencyObject);
            Trace.WriteLine(e.Source + " !!! "
                + e.OriginalSource.ToString() + " !!! " + dp);
            Trace.Write((dp as StackPanel).Children.IndexOf(e.Source as MenuItem));
            AddElement((dp as StackPanel).Children.IndexOf(e.Source as MenuItem));

            //elementList.Items.Add((new ListBoxItem()).Content = le.title);
        }
        private void AddElement(int elemId)
        {
            LogicElement le = new LogicElement(LogicElements[elemId]);
            userControls.Add(le);
            le.Initialize();
            canvas.Children.Add(le);
            Trace.WriteLine(le.title);
            //elementList.Items.Add((new ListBoxItem()).Content = le.title);
        }
        private void AddElement()
        {
            LogicElement le = new LogicElement(LogicElements[0]);
            userControls.Add(le);
            le.Initialize();
            canvas.Children.Add(le);
            Trace.WriteLine(le.title);
            //elementList.Items.Add((new ListBoxItem()).Content = le.title);
        }
        private void AddElement(LogicElement le)
        {
            userControls.Add(le);
            le.Initialize();
            canvas.Children.Add(le);
            Trace.WriteLine(le.title);
            //elementList.Items.Add((new ListBoxItem()).Content = le.title);
        }
        private void UpdateUi()
        {

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
                int localX = ((int)Math.Round(dropPoint.X / 10.0)) * 10;
                int localY = ((int)Math.Round(dropPoint.Y / 10.0)) * 10;
                Canvas.SetLeft(obj, localX);
                Canvas.SetTop(obj, localY);
                obj.ChangePosition(new Point(localX, localY));
                Canvas.SetZIndex(obj, 1);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LogicElement le = new LogicElement(LogicElements[1]);
            AddElement(le);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            LogicElement le = new LogicElement(LogicElements[2]);
            AddElement(le);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

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
        //private void Button_ClickProekt(object sender, RoutedEventArgs e)
        //{
        //    if (Proektgrid.Visibility == Visibility.Hidden)
        //        Proektgrid.Visibility = Visibility.Visible;
        //    else
        //        Proektgrid.Visibility = Visibility.Hidden;
        //}

        //private void Button_ClickInstrument(object sender, RoutedEventArgs e)
        //{
        //    if (Instrumentgrid.Visibility == Visibility.Hidden)
        //        Instrumentgrid.Visibility = Visibility.Visible;
        //    else
        //        Instrumentgrid.Visibility = Visibility.Hidden;
        //}

        //private void Button_ClickPolzovatel(object sender, RoutedEventArgs e)
        //{
        //    if (Polzovatelgrid.Visibility == Visibility.Hidden)
        //        Polzovatelgrid.Visibility = Visibility.Visible;
        //    else
        //        Polzovatelgrid.Visibility = Visibility.Hidden;
        //}

        //private void Button_Click_4(object sender, RoutedEventArgs e)
        //{

        //    if (Elementsgrid.Visibility == Visibility.Hidden)
        //        Elementsgrid.Visibility = Visibility.Visible;
        //    else
        //        Elementsgrid.Visibility = Visibility.Hidden;
        //}

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            MainWindowD mainWindowD = new MainWindowD();
            mainWindowD.ShowDialog();
        }
    }
}
