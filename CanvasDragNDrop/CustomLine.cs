using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CanvasDragNDrop
{
    public class CustomLine
    {
        public CustomLine()
        {
            line = new Line();
        }
        public CustomLine(int x1, int y1, int x2, int y2)
        {
            line = new Line();
            line.X1= x1;
            line.Y1= y1;
            line.X2= x2;
            line.Y2= y2;
            line.StrokeThickness = 1;
            line.Stroke = Brushes.Gray;
        }
        public CustomLine(double x1, double y1, double x2, double y2)
        {
            line = new Line();
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            line.StrokeThickness = 1;
            line.Stroke = Brushes.Gray;
        }
        public Line GetLine()
        {
            return line;
        }
        Line line;

    }
}
