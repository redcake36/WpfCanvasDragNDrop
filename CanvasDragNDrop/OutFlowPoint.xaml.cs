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
    /// Логика взаимодействия для OutFlowPoint.xaml
    /// </summary>
    public partial class OutFlowPoint : FlowPoint
    {
        public OutFlowPoint() : base()
        {
        }
        public OutFlowPoint(Brush br, string type, int index) : base(br, type, index)
        {
        }

        public override void ChangePosition(int parentHeight, int parentWidth, Point p, float countPointsOnEdge)
        {
            if (connectedLines.Count == 0)
            {
                return;
            }
            foreach (Line ln in connectedLines)
            {
                ln.X1 = parentWidth + p.X;
                ln.Y1 = (index + 1) * (parentHeight / countPointsOnEdge - edgeLength / 2) + p.Y;
            }
        }
    }
}
