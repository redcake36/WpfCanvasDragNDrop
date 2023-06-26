using System;
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
        WindowEventHandler windowEventHandler;
        //string rootFolder = @"C:/Users/User/Desktop/RABoTA/ПНИ/WPF/testSamples/CanvasDragNDrop/CanvasDragNDrop/";
        bool canDrowLine = false;
        bool startDrow = false;
        bool linePathStarted = false;
        CustomLine? currentCustLine = null;
        Point? prewPoint = null;
        public static bool state = true;


        public List<LogicElement> LogicElements = new List<LogicElement>();
        public List<DBBlockModelClass> blockModelClasses = new List<DBBlockModelClass>();

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
            Line line;
            for (int i = 10; i < canvas.Width; i += 10)
            {
                line = new Line();
                line.X1 = i;
                line.Y1 = 0;
                line.X2 = i;
                line.Y2 = canvas.Height;
                line.StrokeThickness = 1;
                line.Stroke = Brushes.Gray;
                canvas.Children.Add(line);
            }
            for (int i = 10; i < canvas.Height; i += 10)
            {
                line = new Line();
                line.X1 = 0;
                line.Y1 = i;
                line.X2 = canvas.Width;
                line.Y2 = i;
                line.StrokeThickness = 1;
                line.Stroke = Brushes.Gray;
                canvas.Children.Add(line);
            }
        }
        void GetFromServerElemList(object sender, RoutedEventArgs e)
        {
            UIElementList.Children.Clear();
            blockModelClasses.Clear();
            LogicElements.Clear();
            List<DBBlockModelClass>? samplelist;
            //Trace.WriteLine(UIElementList.Children.Count);
            if (!RootUrl.debug)
            {
                samplelist = JsonConvert.DeserializeObject<List<DBBlockModelClass>>(
                    Get(RootUrl.rootServer + "/get_models"));
            }
            else
            {
                samplelist = JsonConvert.DeserializeObject<List<DBBlockModelClass>>(
                    File.ReadAllText("element.json"));
            }
            //Trace.WriteLine(string.Join("\n", samplelist));
            foreach (var item in samplelist)
            {
                DBBlockModelClass m = new DBBlockModelClass();
                byte[] bytes = Encoding.Default.GetBytes(item.Description);
                var desc = Encoding.UTF8.GetString(bytes);
                m.Description = desc;
                bytes = Encoding.Default.GetBytes(item.Title);
                var ttl = Encoding.UTF8.GetString(bytes);
                m.Title = ttl;
                m.ModelId = item.ModelId;
                m.InputFlows = item.InputFlows;
                m.OutputFlows = item.OutputFlows;
                m.DefaultParameters = item.DefaultParameters;
                m.Description = desc;
                m.Expressions = item.Expressions;
                blockModelClasses.Add(m);
            }
            foreach (var item in blockModelClasses)
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
        void GetFromJsonElemList()
        {
            string json = File.ReadAllText("element.json");
            Trace.WriteLine(json);
            blockModelClasses = JsonConvert.DeserializeObject<List<DBBlockModelClass>>(json);

            foreach (var item in blockModelClasses)
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
            windowEventHandler = WindowEventHandler.getInstance();
            //Trace.WriteLine(Encoding.Unicode.GetString(Get("https://1245-95-220-40-200.ngrok-free.app/get_models")));

            GetFromServerElemList(null, null);
            //GetFromJsonElemList();
            // string json = File.ReadAllText(Get("https://1245-95-220-40-200.ngrok-free.app/get_models"));
            //// string json = File.ReadAllText(rootFolder + @"element.json");
            // Trace.WriteLine(json);
            // blockModelClasses = JsonConvert.DeserializeObject<List<MashaDBClass>>(json);
            // foreach (var item in blockModelClasses)
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
                currentCustLine.GetLine().X2 = e.GetPosition(canvas).X - 1f;
                currentCustLine.GetLine().Y2 = e.GetPosition(canvas).Y - 1f;
                //if (currentCustLine is null)
                //{

                //    Trace.WriteLine("newLine");
                //    currentCustLine = new Line();
                //    currentCustLine.Stroke = System.Windows.Media.Brushes.Red;

                //    if (linePathStarted && prewPoint is not null)
                //    {
                //        currentCustLine.X1 = prewPoint.Value.X;
                //        currentCustLine.Y1 = prewPoint.Value.Y;
                //    }
                //    else
                //    {
                //        currentCustLine.X1 = e.GetPosition(canvas).X;
                //        currentCustLine.Y1 = e.GetPosition(canvas).Y;
                //    }
                //    currentCustLine.X2 = e.GetPosition(canvas).X;
                //    currentCustLine.Y2 = e.GetPosition(canvas).Y;

                //    currentCustLine.StrokeThickness = 3;
                //    canvas.Children.Add(currentCustLine);
                //}
                //Trace.WriteLine(e.GetPosition(canvas).X.ToString(), e.GetPosition(canvas).Y.ToString());
                //// для ломанной
                ////if (Math.Abs(currentCustLine.X1 - e.GetPosition(canvas).X) > Math.Abs(currentCustLine.Y1 - e.GetPosition(canvas).Y))
                ////{
                ////	currentCustLine.X2 = e.GetPosition(canvas).X;
                ////	currentCustLine.Y2 = currentCustLine.Y1;
                ////}
                ////else
                ////{
                ////	currentCustLine.X2 = currentCustLine.X1;
                ////	currentCustLine.Y2 = e.GetPosition(canvas).Y;
                ////}
                //currentCustLine.X2 = e.GetPosition(canvas).X - 1f;
                //currentCustLine.Y2 = e.GetPosition(canvas).Y - 1f;
                //prewPoint = new Point(currentCustLine.X2, currentCustLine.Y2);

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
                    currentCustLine.GetLine().Stroke = System.Windows.Media.Brushes.Black;
                    (dp as FlowPoint).connectedLines.Add(currentCustLine.GetLine());
                    currentCustLine = null;
                    Trace.WriteLine("currentCustLine = null; ");
                    startDrow = false;
                }
                else if ((dp as FlowPoint).type == "out")
                {
                    currentCustLine = new CustomLine();
                    currentCustLine.GetLine().Stroke = System.Windows.Media.Brushes.Red;
                    currentCustLine.GetLine().X1 = e.GetPosition(canvas).X;
                    currentCustLine.GetLine().Y1 = e.GetPosition(canvas).Y;
                    currentCustLine.GetLine().X2 = e.GetPosition(canvas).X;
                    currentCustLine.GetLine().Y2 = e.GetPosition(canvas).Y;
                    (dp as FlowPoint).connectedLines.Add(currentCustLine.GetLine());
                    currentCustLine.GetLine().StrokeThickness = 3;
                    canvas.Children.Add(currentCustLine.GetLine());
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
            if (currentCustLine != null)
            {
                canvas.Children.Remove(currentCustLine.GetLine());
                currentCustLine.SetLine(null);
            }


            canDrowLine = false;
            state = true;
            Mouse.OverrideCursor = null;
        }
        private void canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            EndLineDrawMode();
            windowEventHandler.ResetElement();
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
            Trace.Write((dp as StackPanel).Children.IndexOf(e.Source as Button));
            AddElement((dp as StackPanel).Children.IndexOf(e.Source as Button));

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


        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            MainWindowD mainWindowD = new MainWindowD();
            mainWindowD.ShowDialog();
        }

        private void canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && windowEventHandler.GetElement != null)
            {
                canvas.Children.Remove(windowEventHandler.GetCanvasElement());
                windowEventHandler.DeleteElement();
                Trace.WriteLine("deleted");
            }
            if (e.Key == Key.Escape)
            {
                windowEventHandler.ResetElement();
            }
        }
    }
}
