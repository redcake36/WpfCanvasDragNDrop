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
using CanvasDragNDrop.UtilityClasses;

namespace CanvasDragNDrop.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class FakeComposBlockWindow : Window
    {
    //    WindowEventHandler windowEventHandler;
    //    //string rootFolder = @"C:/Users/User/Desktop/RABoTA/ПНИ/WPF/testSamples/CanvasDragNDrop/CanvasDragNDrop/";
    //    bool canDrowLine = false;
    //    bool startDrow = false;
    //    bool linePathStarted = false;
    //    CustomLine? currentCustLine = null;
    //    Point? prewPoint = null;
    //    public static bool state = true;


    //    public List<BlockModelHolder> LogicElements = new List<BlockModelHolder>();
    //    public List<APIBlockModelClass> blockModelClasses = new List<APIBlockModelClass>();

    //    public List<BlockModelHolder> userControls = new List<BlockModelHolder>();
    //    public string Get(string uri)
    //    {
    //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
    //        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

    //        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
    //        using (Stream stream = response.GetResponseStream())
    //        using (StreamReader reader = new StreamReader(stream))
    //        {
    //            return reader.ReadToEnd();
    //        }
    //    }
    //    void DrowGrid()
    //    {
    //        Line line;
    //        for (int i = 10; i < canvas.Width; i += 10)
    //        {
    //            line = new Line();
    //            line.X1 = i;
    //            line.Y1 = 0;
    //            line.X2 = i;
    //            line.Y2 = canvas.Height;
    //            line.StrokeThickness = 1;
    //            line.Stroke = Brushes.Gray;
    //            canvas.Children.Add(line);
    //        }
    //        for (int i = 10; i < canvas.Height; i += 10)
    //        {
    //            line = new Line();
    //            line.X1 = 0;
    //            line.Y1 = i;
    //            line.X2 = canvas.Width;
    //            line.Y2 = i;
    //            line.StrokeThickness = 1;
    //            line.Stroke = Brushes.Gray;
    //            canvas.Children.Add(line);
    //        }
    //    }
    //    void GetFromServerElemList(object sender, RoutedEventArgs e)
    //    {
    //        UIElementList.Children.Clear();
    //        blockModelClasses.Clear();
    //        LogicElements.Clear();
    //        List<APIBlockModelClass>? samplelist;
    //        //Trace.WriteLine(UIElementList.Children.Count);
    //        if (!API.AutomotiveWork)
    //        {
    //            samplelist = JsonConvert.DeserializeObject<List<APIBlockModelClass>>(
    //                Get(API.rootServer + "/get_models"));
    //        }
    //        else
    //        {
    //            samplelist = JsonConvert.DeserializeObject<List<APIBlockModelClass>>(
    //                File.ReadAllText("element.json"));
    //        }

    //        foreach (var item in samplelist)
    //        {
    //            BlockModelHolder l = new BlockModelHolder(item);
    //            LogicElements.Add(l);
    //            Button btn = new Button();
    //            btn.Style = Resources["Cmbbtn"] as Style;
    //            btn.Content = item.Title;
    //            btn.Click += new RoutedEventHandler(Button_Click);
    //            UIElementList.Children.Add(btn);
    //        }
    //    }

    //    public FakeComposBlockWindow()
    //    {
    //        InitializeComponent();
    //        DrowGrid();
    //        windowEventHandler = WindowEventHandler.getInstance();
    //        //Trace.WriteLine(Encoding.Unicode.GetString(Get("https://1245-95-220-40-200.ngrok-free.app/get_models")));

    //        GetFromServerElemList(null, null);
    //        //AddElement(new BlockModelHolder("Вход", Brushes.Green, 0, 1));
    //        //AddElement(new BlockModelHolder("Выход", Brushes.Red, 1, 0));
    //    }

    //    private void Button_Pen_Click(object sender, RoutedEventArgs e)
    //    {

    //        canDrowLine = !canDrowLine;
    //        state = !state;
    //        if (canDrowLine)
    //        {
    //            Mouse.OverrideCursor = Cursors.Pen;
    //        }
    //        else { EndLineDrawMode(); }

    //        // для проверки пересечения
    //        //Rectangle r = new Rectangle();
    //        //var g = br.RenderedGeometry.Clone();
    //        //g.Transform = GetFullTransform(br);
    //        //var q = redr.RenderedGeometry.Clone();
    //        //q.Transform = GetFullTransform(redr);
    //        //Trace.WriteLine("HasIntersection " + HasIntersection(g, q).ToString());
    //        //Canvas.SetLeft(br, 99);
    //    }
    //    private static bool HasIntersection(Geometry g1, Geometry g2) =>
    //g1.FillContainsWithDetail(g2) != IntersectionDetail.Empty;
    //    private static Transform GetFullTransform(UIElement e)
    //    {
    //        // The order in which transforms are applied is important!
    //        var transforms = new TransformGroup();

    //        if (e.RenderTransform != null)
    //            transforms.Children.Add(e.RenderTransform);

    //        var xTranslate = (double)e.GetValue(Canvas.LeftProperty);
    //        if (double.IsNaN(xTranslate))
    //            xTranslate = 0D;

    //        var yTranslate = (double)e.GetValue(Canvas.TopProperty);
    //        if (double.IsNaN(yTranslate))
    //            yTranslate = 0D;

    //        var translateTransform = new TranslateTransform(xTranslate, yTranslate);
    //        transforms.Children.Add(translateTransform);

    //        return transforms;
    //    }

    //    protected void canvas_MouseMove(object sender, MouseEventArgs e)
    //    {

    //        if (startDrow)
    //        {
    //            currentCustLine.GetLine().X2 = e.GetPosition(canvas).X - 1f;
    //            currentCustLine.GetLine().Y2 = e.GetPosition(canvas).Y - 1f;

    //        }
    //    }
    //    private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    //    {
    //        DependencyObject dp = LogicalTreeHelper.GetParent(e.OriginalSource as DependencyObject);
    //        //LogicalTreeHelper.GetParent( e.OriginalSource as DependencyObject)
    //        Trace.WriteLine(e.Source + " !!! " + e.GetPosition(e.Source as UIElement)
    //            + e.OriginalSource.ToString() + " !!! " + dp);
    //        if (canDrowLine && typeof(FlowPoint).IsAssignableFrom(dp.GetType()))
    //        {
    //            Trace.WriteLine((dp as FlowPoint).pointName);
    //            if (startDrow && (dp as FlowPoint).type == "in")
    //            {
    //                linePathStarted = true;
    //                Trace.WriteLine("startDrow= false;");
    //                currentCustLine.toFlowPoint = (dp as FlowPoint);
    //                currentCustLine.GetLine().Stroke = System.Windows.Media.Brushes.Black;
    //                (dp as FlowPoint).connectedLine = currentCustLine.GetLine();
    //                currentCustLine = null;
    //                Trace.WriteLine("currentCustLine = null; ");
    //                startDrow = false;
    //            }
    //            else if ((dp as FlowPoint).type == "out")
    //            {
    //                currentCustLine = new CustomLine(e, canvas);

    //                (dp as FlowPoint).connectedLine = currentCustLine.GetLine();
    //                currentCustLine.fromFlowPoint = (dp as FlowPoint);

    //                canvas.Children.Add(currentCustLine.GetLine());
    //                startDrow = true;
    //                Trace.WriteLine("startDrow = true");
    //            }
    //        }
    //    }
    //    void EndLineDrawMode()
    //    {
    //        linePathStarted = false;
    //        prewPoint = null;
    //        startDrow = false;
    //        if (currentCustLine != null)
    //        {
    //            canvas.Children.Remove(currentCustLine.GetLine());
    //            currentCustLine.SetLine(null);
    //        }

    //        canDrowLine = false;
    //        state = true;
    //        Mouse.OverrideCursor = null;
    //    }
    //    private void canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    //    {
    //        EndLineDrawMode();
    //        windowEventHandler.ResetElement();
    //    }

    //    private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    //    {

    //        // activeLine = true;
    //        Trace.WriteLine("activeLine = true;");
    //    }
    //    private void Button_Click(object sender, RoutedEventArgs e)
    //    {
    //        //BlockModelHolder? le = JsonConvert.DeserializeObject<BlockModelHolder>(json);
    //        DependencyObject dp = LogicalTreeHelper.GetParent(sender as DependencyObject);
    //        Trace.WriteLine(e.Source + " !!! "
    //            + e.OriginalSource.ToString() + " !!! " + dp);
    //        Trace.Write((dp as StackPanel).Children.IndexOf(e.Source as Button));
    //        AddElement((dp as StackPanel).Children.IndexOf(e.Source as Button));

    //        //elementList.Items.Add((new ListBoxItem()).Content = le.title);
    //    }
    //    private void AddElement(int elemId)
    //    {
    //        BlockModelHolder le = new BlockModelHolder(LogicElements[elemId]);
    //        userControls.Add(le);
    //        le.visualBlockComponent.Initialize();
    //        canvas.Children.Add(le.visualBlockComponent);
    //        //elementList.Items.Add((new ListBoxItem()).Content = le.title);
    //    }
    //    private void AddElement()
    //    {
    //        BlockModelHolder le = new BlockModelHolder(LogicElements[0]);
    //        userControls.Add(le);
    //        le.visualBlockComponent.Initialize();
    //        canvas.Children.Add(le.visualBlockComponent);
    //        //elementList.Items.Add((new ListBoxItem()).Content = le.title);
    //    }

    //    private void AddElement(BlockModelHolder le)
    //    {
    //        userControls.Add(le);
    //        le.visualBlockComponent.Initialize();
    //        canvas.Children.Add(le.visualBlockComponent);
    //        //elementList.Items.Add((new ListBoxItem()).Content = le.title);
    //    }
    //    private void Canvas_Drop(object sender, DragEventArgs e)
    //    {
    //        var obj = e.Data.GetData(typeof(VisualBlockComponent)) as VisualBlockComponent;
    //        if (obj != null)
    //            Canvas.SetZIndex(obj, 0);
    //    }

    //    private void canvas_DragOver(object sender, DragEventArgs e)
    //    {
    //        Point dropPoint = e.GetPosition(canvas);

    //        var obj = e.Data.GetData(typeof(VisualBlockComponent)) as VisualBlockComponent;
    //        if (obj != null)
    //        {
    //            int localX = ((int)Math.Round(dropPoint.X / 10.0)) * 10;
    //            int localY = ((int)Math.Round(dropPoint.Y / 10.0)) * 10;
    //            Canvas.SetLeft(obj, localX);
    //            Canvas.SetTop(obj, localY);
    //            //obj.ChangePosition(new Point(localX, localY));
    //            Canvas.SetZIndex(obj, 1);
    //        }
    //    }


    //    private void Button_Click_5(object sender, RoutedEventArgs e)
    //    {
    //        MainWindowD mainWindowD = new MainWindowD();
    //        mainWindowD.ShowDialog();
    //    }

    //    private void canvas_KeyDown(object sender, KeyEventArgs e)
    //    {
    //        if (e.Key == Key.Delete && windowEventHandler.GetElement != null)
    //        {
    //            canvas.Children.Remove(windowEventHandler.GetCanvasElement());
    //            windowEventHandler.DeleteElement();
    //            Trace.WriteLine("deleted");
    //        }
    //        if (e.Key == Key.Escape)
    //        {
    //            windowEventHandler.ResetElement();
    //        }
    //    }
    }
}
