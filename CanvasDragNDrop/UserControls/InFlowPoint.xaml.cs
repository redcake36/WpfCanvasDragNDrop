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
    /// Логика взаимодействия для InFlowPoint.xaml
    /// </summary>
    public partial class InFlowPoint : FlowPoint
    {
        public InFlowPoint() : base()
        {
            InitializeComponent();
        }
        public InFlowPoint(Brush br, string type, int index, FrameworkElement parentElement) : base(br, type, index, parentElement)
        {
            InitializeComponent();
        }
        public override void ChangePosition(int parentHeight, int parentWidth, Point p, float countPointsOnEdge)
        {
            if (connectedLines.Count == 0)
            {
                return;
            }
            foreach (Line ln in connectedLines)
            {
                ln.X2 = p.X;
                ln.Y2 = parentHeight * (index + 1) / (countPointsOnEdge + 1)  + p.Y;
            }
        }
    }
}
