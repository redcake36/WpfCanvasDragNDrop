using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using CanvasDragNDrop.UtilityClasses;

namespace CanvasDragNDrop
{
    /// <summary>
    /// Логика взаимодействия для VisualBlockComponent.xaml
    /// </summary>
    public partial class VisualBlockComponent : UserControl, ICanvasChild
    {

        public int height
        {
            get { return (int)GetValue(heightProperty); }
            set { SetValue(heightProperty, value); }
        }
        public static readonly DependencyProperty heightProperty = DependencyProperty.Register("height", typeof(int), typeof(VisualBlockComponent), new PropertyMetadata(null));
        //public string title
        //{
        //    get { return (string)GetValue(titleProperty); }
        //    set { SetValue(titleProperty, value); }
        //}
        //public static readonly DependencyProperty titleProperty = DependencyProperty.Register("title", typeof(string), typeof(LogicElement), new PropertyMetadata(null));
        [DefaultValue(100)]
        public int width
        {
            get { return (int)GetValue(widthProperty); }
            set { SetValue(widthProperty, value); }
        }
        public static readonly DependencyProperty widthProperty = DependencyProperty.Register("width", typeof(int), typeof(VisualBlockComponent), new PropertyMetadata(null));

        public Brush bgColor
        {
            get { return (Brush)GetValue(colorProperty); }
            set { SetValue(colorProperty, value); }

        }
        public static readonly DependencyProperty colorProperty = DependencyProperty.Register("bgColor", typeof(Brush), typeof(VisualBlockComponent), new PropertyMetadata(null));
        public string title
        {
            get { return (string)GetValue(titleProperty); }
            set { SetValue(titleProperty, value); }
        }
        public static readonly DependencyProperty titleProperty = DependencyProperty.Register("title", typeof(string), typeof(VisualBlockComponent), new PropertyMetadata(null));

        public Brush highlightBorderColor = Brushes.Red;
        public Brush defaultBorderColor = Brushes.Black;

        BlockModelHolder? parentElement;
        DBBlockModelClass? dBBlockModelClass;

        public List<InFlowPoint> inFlowPoints = new List<InFlowPoint>();
        public List<InFlowPoint> outFlowPoints = new List<InFlowPoint>();

        void VisualInit()
        {
            width = 100;
            height = 100;
            bgColor = Brushes.White;

            highlightBorderColor = Brushes.Red;
            defaultBorderColor = Brushes.Black;
        }

        public void Initialize()
        {
            InitializeComponent();
            SpawnFlowPoints();
            DataContext = this;

        }
        public VisualBlockComponent()
        {
            VisualInit();
            parentElement = null;
            dBBlockModelClass = null;
        }

        public VisualBlockComponent(VisualBlockComponent visualBlock)
        {
            width = visualBlock.width; height = visualBlock.height;
            bgColor = visualBlock.bgColor; highlightBorderColor = visualBlock.highlightBorderColor;
            defaultBorderColor = visualBlock.defaultBorderColor;
            title = visualBlock.title;
            parentElement = visualBlock.parentElement;
            dBBlockModelClass = visualBlock.dBBlockModelClass;
        }

        public VisualBlockComponent(BlockModelHolder parentElement, DBBlockModelClass dBBlockModelClass)
        {
            VisualInit();
            title = dBBlockModelClass.Title;

            this.parentElement = parentElement;
            this.dBBlockModelClass = dBBlockModelClass;
        }
        public void SpawnFlowPoints()
        {
            if (dBBlockModelClass is not null)
            {
                for (int i = 1; i <= dBBlockModelClass.InputFlows.Count; i++)
                {
                    InFlowPoint infp = new InFlowPoint(Brushes.Green, FlowPointType.InFlowPoint, i, this);
                    InPoints.Children.Add(infp);
                    inFlowPoints.Add(infp);
                }

                for (int i = 1; i <= dBBlockModelClass.OutputFlows.Count; i++)
                {
                    InFlowPoint infp = new InFlowPoint(Brushes.Red, FlowPointType.OutFlowPoint, i, this);
                    OutPoints.Children.Add(infp);
                    outFlowPoints.Add(infp);
                }
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed && MainWindow.state)
            {
                DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
                //Trace.WriteLine(color.ToString());
            }
        }

        public void ChangePosition(Point e)
        {
            foreach (InFlowPoint flowPoint in inFlowPoints)
            {
                flowPoint.ChangePosition(e);
            }
            foreach (InFlowPoint flowPoint in outFlowPoints)
            {
                flowPoint.ChangePosition(e);
            }
        }
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Trace.WriteLine(e.Source + e.OriginalSource.ToString());
            if (e.ClickCount == 1)
            {
                WindowEventHandler.getInstance().SetElement(this);
                WindowEventHandler.getInstance().GetSelectedObjectName();
            }
            if (e.ClickCount == 2)
            {
                ElementPropertiesWindow propertiesWindow = new ElementPropertiesWindow();
                Trace.WriteLine("defParameters:");
                if (propertiesWindow.ShowDialog() == true)
                {

                }
                //propertiesWindow.AddListText("color: " + this.color.ToString());
                //propertiesWindow.AddListText("text: " + this.text);
                //propertiesWindow.AddListText("width: " + this.width.ToString());
                //propertiesWindow.AddListText("height: " + this.height.ToString());

                //propertiesWindow.brush = this.color;
                //propertiesWindow.width = this.width;
                //propertiesWindow.height = this.height;
                //propertiesWindow.title = this.title;
                //propertiesWindow.temperature = this.temperature;
                //if (propertiesWindow.ShowDialog() == true)
                //{
                //    this.color = propertiesWindow.brush;
                //    this.width = propertiesWindow.width;
                //    this.height = propertiesWindow.height;
                //    this.title = propertiesWindow.title;
                //    this.temperature = propertiesWindow.temperature;
                //}
            }
        }

        public void Delete()
        {
            Trace.WriteLine("Visual elem Delete()");
            Trace.TraceWarning("when deleted =>delete parent obj");
            foreach (InFlowPoint flowPoint in inFlowPoints)
            {
                flowPoint.Delete();
            }
            foreach (InFlowPoint flowPoint in outFlowPoints)
            {
                flowPoint.Delete();
            }
        }

        public FrameworkElement GetVisualElement()
        {
            return this;
        }

        public void Selected(bool isSelected)
        {
            if (isSelected)
            {
                visualBlockBorder.BorderBrush = highlightBorderColor;
            }
            else
            {
                visualBlockBorder.BorderBrush = defaultBorderColor;
            }
        }
    }
}
