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
using CanvasDragNDrop.Windows;
using CanvasDragNDrop.UtilityClasses;
using System.Windows.Media.Media3D;

namespace CanvasDragNDrop
{
    public partial class MainWindow : Window
    {
        WindowEventHandler windowEventHandler;
        public Canvas mainCanvas;
        bool canDrowLine = false;
        bool startDrow = false;
        bool linePathStarted = false;
        Pipe? currentPipe = null;
        Point? prewPoint = null;
        public static bool state = true;

        public List<BlockModelHolder> LogicElements = new List<BlockModelHolder>();
        public List<DBBlockModelClass> blockModelClasses = new List<DBBlockModelClass>();

        public List<UserControl> userControls = new List<UserControl>();

        private static MainWindow? instance;

        public static MainWindow Instance()
        {
            if (instance == null)
                instance = new MainWindow();
            return instance;
        }

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
            CustomLine customLine;
            customLine = new CustomLine(0, 0, 0, canvas.Height);
            canvas.Children.Add(customLine.GetLine());

            customLine = new CustomLine(canvas.Width, 0, canvas.Width, canvas.Height);
            canvas.Children.Add(customLine.GetLine());

            customLine = new CustomLine(0, 0, canvas.Width, 0);
            canvas.Children.Add(customLine.GetLine());

            customLine = new CustomLine(0, canvas.Height, canvas.Width, canvas.Height);
            canvas.Children.Add(customLine.GetLine());
            for (int i = 10; i < canvas.Width; i += 10)
            {
                customLine = new CustomLine(i, 0, i, canvas.Height, DefBrush: Brushes.Gray);
                canvas.Children.Add(customLine.GetLine());
            }
            for (int i = 10; i < canvas.Height; i += 10)
            {
                customLine = new CustomLine(0, i, canvas.Width, i, DefBrush: Brushes.Gray);
                canvas.Children.Add(customLine.GetLine());
            }
        }
        void GetFromServerElemList(object sender, RoutedEventArgs e)
        {
            UIElementList.Children.Clear();
            blockModelClasses.Clear();
            LogicElements.Clear();
            List<DBBlockModelClass>? samplelist;
            //Trace.WriteLine(UIElementList.Children.Count);
            if (!RootUrl.AutomotiveWork)
            {
                samplelist = JsonConvert.DeserializeObject<List<DBBlockModelClass>>(
                    Get(RootUrl.rootServer + "/get_models"));
            }
            else
            {
                samplelist = JsonConvert.DeserializeObject<List<DBBlockModelClass>>(
                    File.ReadAllText("element.json"));
            }

            foreach (DBBlockModelClass item in samplelist)
            {
                BlockModelHolder l = new BlockModelHolder(item);
                LogicElements.Add(l);
                Button btn = new Button();
                btn.Style = Resources["Cmbbtn"] as Style;
                btn.Content = l.dBBlockModel.Title;
                btn.Click += new RoutedEventHandler(Button_Click);
                UIElementList.Children.Add(btn);
            }
        }

        public MainWindow()
        {

            InitializeComponent();
            DrowGrid();
            windowEventHandler = WindowEventHandler.getInstance();
            GetFromServerElemList(null, null);
            mainCanvas = canvas;
            instance = this;
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


        }


        protected void canvas_MouseMove(object sender, MouseEventArgs e)
        {

            if (startDrow)
            {
                currentPipe.SetEndPoint(e.GetPosition(canvas));
                //currentPipe.GetLine().X2 = e.GetPosition(canvas).X - 1f;
                //currentPipe.GetLine().Y2 = e.GetPosition(canvas).Y - 1f;
            }
        }
        List<DependencyObject> hitResultsList = new List<DependencyObject>();
        // Return the result of the hit test to the callback.
        public HitTestResultBehavior MyHitTestResult(HitTestResult result)
        {
            // Add the hit test result to the list that will be processed after the enumeration.
            hitResultsList.Add(result.VisualHit);

            // Set the behavior to return visuals at all z-order levels.
            return HitTestResultBehavior.Continue;
        }
        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //DependencyObject dp = LogicalTreeHelper.GetParent(e.OriginalSource as DependencyObject);

            //======================================
            // Retrieve the coordinate of the mouse position.
            Point pt = e.GetPosition((UIElement)sender);

            // Clear the contents of the list used for hit test results.
            hitResultsList.Clear();

            // Set up a callback to receive the hit test result enumeration.
            VisualTreeHelper.HitTest(mainCanvas, null,
                new HitTestResultCallback(MyHitTestResult),
                new PointHitTestParameters(pt));
            DependencyObject dp = null;
            foreach (DependencyObject item in hitResultsList)
            {
                DependencyObject itemParent = LogicalTreeHelper.GetParent(item);
                if (itemParent is FlowPoint)
                {
                    Trace.WriteLine("PIPEEEEEEEEEEEEEEEEEEEEE");
                    dp = (FlowPoint)itemParent;
                    break;
                }
            }
            //======================================
            if (dp != null)
            {
                if (startDrow && (dp as FlowPoint).type == FlowPointType.InFlowPoint)
                {
                    if (currentPipe.fromFlowPoint.parentElement != (dp as FlowPoint).parentElement)
                    {
                        linePathStarted = true;
                        currentPipe.toFlowPoint = (dp as FlowPoint);
                        currentPipe.SetDefltColor();
                        (dp as FlowPoint).connectedPipe = currentPipe;
                        currentPipe = null;
                        startDrow = false;
                    }
                }
                else if ((dp as FlowPoint).type == FlowPointType.OutFlowPoint)
                {
                    if ((dp as FlowPoint).connectedPipe == null)
                    {
                        currentPipe = new Pipe(e, canvas);
                        currentPipe.HighLight();

                        (dp as FlowPoint).connectedPipe = currentPipe;
                        currentPipe.fromFlowPoint = (dp as FlowPoint);

                        canvas.Children.Add(currentPipe.GetLine());
                        startDrow = true;
                    }
                }
            }
        }
        void EndLineDrawMode()
        {
            linePathStarted = false;
            prewPoint = null;
            startDrow = false;
            if (currentPipe != null)
            {
                canvas.Children.Remove(currentPipe.GetLine());
                currentPipe.Delete();
                currentPipe = null;
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
            //Trace.WriteLine("activeLine = true;");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //LogicElement? le = JsonConvert.DeserializeObject<LogicElement>(json);
            DependencyObject dp = LogicalTreeHelper.GetParent(sender as DependencyObject);
            //Trace.WriteLine(e.Source + " !!! "
            //    + e.OriginalSource.ToString() + " !!! " + dp);
            //Trace.Write((dp as StackPanel).Children.IndexOf(e.Source as Button));
            AddElement((dp as StackPanel).Children.IndexOf(e.Source as Button));

            //elementList.Items.Add((new ListBoxItem()).Content = le.title);
        }
        private void AddElement(int elemId)
        {
            BlockModelHolder le = new BlockModelHolder(LogicElements[elemId]);
            userControls.Add(le.visualBlockComponent);
            le.visualBlockComponent.Initialize();
            canvas.Children.Add(le.visualBlockComponent);

        }
        private void AddElement()
        {
            BlockModelHolder le = new BlockModelHolder(LogicElements[0]);
            userControls.Add(le.visualBlockComponent);
            le.visualBlockComponent.Initialize();
            canvas.Children.Add(le.visualBlockComponent);
        }

        //private void AddElement(LogicElement le)
        //{
        //    userControls.Add(le);
        //    le.Initialize();
        //    canvas.Children.Add(le);
        //    //Trace.WriteLine(le.title);
        //    //elementList.Items.Add((new ListBoxItem()).Content = le.title);
        //}
        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            var obj = e.Data.GetData(typeof(VisualBlockComponent)) as VisualBlockComponent;
            if (obj != null)
            {
                Canvas.SetZIndex(obj, 0);
            }
            elementDragPoint = new Point();
        }
        Point elementDragPoint = new Point();
        private void canvas_DragOver(object sender, DragEventArgs e)
        {
            Point dropPoint = e.GetPosition(canvas);

            var obj = e.Data.GetData(typeof(VisualBlockComponent)) as VisualBlockComponent;
            //Trace.WriteLine("main win 268 i:" + obj.inFlowPoints[1]
            //    .TransformToAncestor(mainCanvas)
            //              .Transform(new Point(0, 0)));
            //Trace.WriteLine("main win 268 o:" + obj.outFlowPoints[0]
            //    .TransformToAncestor(mainCanvas)
            //              .Transform(new Point(0, 0)));

            if (elementDragPoint == new Point())
            {
                elementDragPoint = e.GetPosition(obj);
            }
            if (obj != null)
            {
                dropPoint.X = dropPoint.X - elementDragPoint.X;
                dropPoint.Y = dropPoint.Y - elementDragPoint.Y;
                int localX = ((int)Math.Round(dropPoint.X / 10.0)) * 10;
                int localY = ((int)Math.Round(dropPoint.Y / 10.0)) * 10;
                if (localX < 0)
                {
                    localX = 0;
                }
                if (localY < 0)
                {
                    localY = 0;
                }
                if (localX > mainCanvas.Width - obj.width)
                {
                    localX = (int)(mainCanvas.Width - obj.width);
                }
                if (localY > mainCanvas.Height - obj.height)
                {
                    localY = (int)(mainCanvas.Height - obj.height);
                }
                obj.ChangePosition(new Point(localX, localY));
                Canvas.SetLeft(obj, localX);
                Canvas.SetTop(obj, localY);
                Canvas.SetZIndex(obj, 1);
            }
        }


        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            BlockModelCreationWindow mainWindowD = new BlockModelCreationWindow();
            mainWindowD.ShowDialog();
        }

        private void canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && windowEventHandler.GetElement != null)
            {
                canvas.Children.Remove(windowEventHandler.GetCanvasElement());
                windowEventHandler.DeleteElement();
                //Trace.WriteLine("deleted");
            }
            if (e.Key == Key.Escape)
            {
                windowEventHandler.ResetElement();
            }
        }

        private void CreateCompBlock(object sender, RoutedEventArgs e)
        {
            FakeComposBlockWindow fakeW = new FakeComposBlockWindow();
            fakeW.ShowDialog();
        }
    }
}
