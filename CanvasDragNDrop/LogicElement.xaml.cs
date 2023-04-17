using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Unicode;
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
    /// Логика взаимодействия для LogicElement.xaml
    /// </summary>
    public partial class LogicElement : UserControl
    {
        public LogicElement()
        {
            InitializeComponent();
            DataContext = this;
        }
        public LogicElement(int h, int w, string t, Brush b, double tmp, int inFPC, int oFPC)
        {
            thisElementNumber = elementNumber++;
            height = h;
            width = w;
            title = t + this.thisElementNumber.ToString();
            color = b;
            temperature = tmp;
            inputFlowPointsCount = inFPC;
            outputFlowPointsCount = oFPC;
        }
        public LogicElement(int h, int w, string t, string b, double tmp, int inFPC, int oFPC)
        {
            thisElementNumber = elementNumber++;
            height = h;
            width = w;
            title = t + this.thisElementNumber.ToString();
            color = new BrushConverter().ConvertFromString(b) as Brush;
            temperature = tmp;
            inputFlowPointsCount = inFPC;
            outputFlowPointsCount = oFPC;
        }
        public LogicElement(LogicElement le)
        {
            thisElementNumber = elementNumber++;
            height = le.height;
            width = le.width;
            title = le.title;
            color = le.color;
            temperature = le.temperature;
            inputFlowPointsCount = le.inputFlowPointsCount;
            outputFlowPointsCount = le.outputFlowPointsCount;
        }
        public LogicElement(MashaDBClass c)
        {
            thisElementNumber = elementNumber++;
            height = 100;
            width = 100;
            title = c.title;
            color = Brushes.White;
            temperature = 10.0;
            inputFlowPointsCount = 1;
            outputFlowPointsCount = 1;
        }
        public void Initialize()
        {
            InitializeComponent();
            DataContext = this;
            SpawnFlowPoints();
        }
        void SpawnFlowPoints()
        {
            for (int i = 0; i < inputFlowPointsCount; i++)
            {
                InFlowPoint infp = new InFlowPoint(Brushes.Magenta, "in", i);
                LogicElementCanvas.Children.Add(infp);
                Canvas.SetLeft(infp, -infp.edgeLength / 2);
                Canvas.SetTop(infp, (this.height * (i + 1)) / (inputFlowPointsCount + 1)
                    - infp.edgeLength / 2);
                inFlowPoints.Add(infp);
            }

            for (int i = 0; i < outputFlowPointsCount; i++)
            {
                OutFlowPoint outfp = new OutFlowPoint(Brushes.Cyan, "out", i);
                LogicElementCanvas.Children.Add(outfp);
                Canvas.SetLeft(outfp, this.width - outfp.edgeLength / 2);
                Canvas.SetTop(outfp, (this.height * (i + 1)) / (outputFlowPointsCount + 1)
                    - outfp.edgeLength / 2);
                outFlowPoints.Add(outfp);
            }

            Trace.WriteLine(this.height / 2);
        }
        public List<FlowPoint> inFlowPoints = new List<FlowPoint>();
        public List<FlowPoint> outFlowPoints = new List<FlowPoint>();
        public static int elementNumber = 0;
        public int thisElementNumber;
        public int inputFlowPointsCount = 3;
        public int outputFlowPointsCount = 4;
        public int height
        {
            get { return (int)GetValue(heightProperty); }
            set { SetValue(heightProperty, value); }
        }
        public static readonly DependencyProperty heightProperty = DependencyProperty.Register("height", typeof(int), typeof(LogicElement), new PropertyMetadata(null));
        public string title
        {
            get { return (string)GetValue(titleProperty); }
            set { SetValue(titleProperty, value); }
        }
        public static readonly DependencyProperty titleProperty = DependencyProperty.Register("title", typeof(string), typeof(LogicElement), new PropertyMetadata(null));

        public int width
        {
            get { return (int)GetValue(widthProperty); }
            set { SetValue(widthProperty, value); }
        }
        public static readonly DependencyProperty widthProperty = DependencyProperty.Register("width", typeof(int), typeof(LogicElement), new PropertyMetadata(null));

        public Brush color
        {
            get { return (Brush)GetValue(colorProperty); }
            set { SetValue(colorProperty, value); }

        }
        public static readonly DependencyProperty colorProperty = DependencyProperty.Register("color", typeof(Brush), typeof(LogicElement), new PropertyMetadata(null));
        public double temperature
        {
            get { return (double)GetValue(temperatureProperty); }
            set { SetValue(temperatureProperty, value); }
        }
        public static readonly DependencyProperty temperatureProperty = DependencyProperty.Register("temperature", typeof(double), typeof(LogicElement), new PropertyMetadata(null));

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed && MainWindow.state)
            {
                DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
                Trace.WriteLine(color.ToString());
            }
        }

        
        public void ChangePosition(Point p)
        {
            foreach (FlowPoint flowPoint in inFlowPoints)
            {
                flowPoint.ChangePosition(this.height, this.width, p, inputFlowPointsCount);
            }
            foreach (FlowPoint flowPoint in outFlowPoints)
            {
                flowPoint.ChangePosition(this.height, this.width, p, outputFlowPointsCount);
            }
        }
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Trace.WriteLine(e.Source + e.OriginalSource.ToString());
            if (e.ClickCount == 2)
            {
                ElementPropertiesWindow propertiesWindow = new ElementPropertiesWindow();
                //propertiesWindow.AddListText("color: " + this.color.ToString());
                //propertiesWindow.AddListText("text: " + this.text);
                //propertiesWindow.AddListText("width: " + this.width.ToString());
                //propertiesWindow.AddListText("height: " + this.height.ToString());
                propertiesWindow.brush = this.color;
                propertiesWindow.width = this.width;
                propertiesWindow.height = this.height;
                propertiesWindow.title = this.title;
                propertiesWindow.temperature = this.temperature;
                if (propertiesWindow.ShowDialog() == true)
                {
                    this.color = propertiesWindow.brush;
                    this.width = propertiesWindow.width;
                    this.height = propertiesWindow.height;
                    this.title = propertiesWindow.title;
                    this.temperature = propertiesWindow.temperature;
                }
            }
        }
    }
}
