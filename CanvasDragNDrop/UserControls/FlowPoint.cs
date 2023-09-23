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

namespace CanvasDragNDrop
{
    public enum FlowPointType
    {
        InFlowPoint, 
        OutFlowPoint
    }
    public partial class FlowPoint : UserControl
    {
        public FlowPoint()
        {
            DataContext = this;
            parentElement = null;
        }
        public FlowPoint(Brush br, FlowPointType type, int index, FrameworkElement parentElement)
        {
            DataContext = this;
            brush = br;
            pointName = type + index.ToString();
            this.type = type;
            this.index = index;
            this.parentElement = parentElement;
        }
        public string? pointName;
        public FrameworkElement? parentElement;
        public FlowPointType type;
        public int index;
        public int edgeLength = 20;
        public Pipe connectedPipe;

        public Brush brush
        {
            get { return (Brush)GetValue(brushProperty); }
            set { SetValue(brushProperty, value); }
        }
        public static readonly DependencyProperty brushProperty = DependencyProperty.Register("brush", typeof(Brush), typeof(FlowPoint), new PropertyMetadata(null));
    }
}