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
    /// <summary>
    /// Логика взаимодействия для FlowPoint.xaml
    /// </summary>
    public partial class FlowPoint : UserControl
    {
        public FlowPoint()
        {
            InitializeComponent();
            DataContext = this;
        }
        public FlowPoint(Brush br, string type, int index)
        {
            InitializeComponent();
            DataContext = this;
            this.brush = br;
            this.pointName = type + index.ToString();
            this.type = type;
            this.index = index;
        }
        public string? pointName;
        public string? type;
        public int index;
        public int edgeLength = 20;
        public List<Line> connectedLines = new List<Line>();
        public virtual void ChangePosition(int parentHeight, int parentWidth, Point p, float countPointsOnEdge)
        {
            if (connectedLines.Count == 0)
            {
                return;
            }
            foreach (Line ln in connectedLines)
            {
                if (type == "out")
                {
                    ln.X1 = parentWidth + p.X;
                    ln.Y1 = (index + 1) * (parentHeight / countPointsOnEdge - edgeLength / 2) + p.Y;
                }
                else if (true)
                {
                    ln.X2 = p.X;
                    ln.Y2 = (index + 1) * (parentHeight / countPointsOnEdge - edgeLength / 2) + p.Y;
                }
            }
        }

        public Brush brush
        {
            get { return (Brush)GetValue(brushProperty); }
            set { SetValue(brushProperty, value); }

        }
        public static readonly DependencyProperty brushProperty = DependencyProperty.Register("brush", typeof(Brush), typeof(LogicElement), new PropertyMetadata(null));

    }
}
