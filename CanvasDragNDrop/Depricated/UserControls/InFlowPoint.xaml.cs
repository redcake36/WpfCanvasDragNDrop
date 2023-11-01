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
    /// Логика взаимодействия для InFlowPoint.xaml
    /// </summary>
    public partial class InFlowPoint : FlowPoint
    {
        public InFlowPoint() : base()
        {
            InitializeComponent();
        }
        public InFlowPoint(Brush br, FlowPointType type, int index, FrameworkElement parentElement) 
            : base(br, type, index, parentElement)
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Trace.WriteLine(e.GetPosition(MainWindow.Instance().canvas));
        }
        public void ChangePosition(Point parentPos)
        {
            if (connectedPipe is null || connectedPipe.GetLine() is null)
            {
                return;
            }
            if (type == FlowPointType.InFlowPoint)
            {
                connectedPipe.SetEndPoint(parentPos + new Vector(0, (5 + edgeLength / 2) * index));
                //connectedPipe.SetEndPoint(this.TransformToAncestor(MainWindow.getInstance().mainCanvas)
                //          .Transform(new Point(edgeLength/2, edgeLength / 2)));
            }
            if (type == FlowPointType.OutFlowPoint)
            {
                connectedPipe.SetStartPoint(parentPos + new Vector(100, (5 + edgeLength / 2) * index));

                // WORKS but very slow, can freeze when mouse moves fast
                //connectedPipe.SetStartPoint(this.TransformToAncestor(MainWindow.getInstance().mainCanvas)
                //          .Transform(new Point(edgeLength / 2, edgeLength / 2)));
            }
        }
        public void Delete()
        {
            if (connectedPipe != null)
            {
                MainWindow.Instance().canvas.Children.Remove(connectedPipe.GetLine());
                connectedPipe.Delete();
                
            }
        }
    }
}
