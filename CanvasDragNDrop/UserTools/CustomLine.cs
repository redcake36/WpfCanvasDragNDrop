using CanvasDragNDrop.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CanvasDragNDrop
{
    public class CustomLine : Shape, ICanvasChild
    {
        Line? line;
        Brush color;
        public FlowPoint? fromFlowPoint { get; set; }
        public FlowPoint? toFlowPoint { get; set; }

        protected override Geometry DefiningGeometry => 
            throw new NotImplementedException();

        public CustomLine()
        {
            line = new Line();
            color = Brushes.Black;
            line.MouseDown += new MouseButtonEventHandler(LineClick);
        }

        public CustomLine(MouseButtonEventArgs e, Canvas canvas)
        {
            line = new Line();
            line.Stroke = System.Windows.Media.Brushes.Red;
            line.X1 = e.GetPosition(canvas).X;
            line.Y1 = e.GetPosition(canvas).Y;
            line.X2 = e.GetPosition(canvas).X;
            line.Y2 = e.GetPosition(canvas).Y;
            line.StrokeThickness = 3;
            color = Brushes.Black;
            line.MouseDown += new MouseButtonEventHandler(LineClick);
        }
        
        void LineClick(object sender, MouseButtonEventArgs e)
        {
            Trace.WriteLine("im line");
            if (e.ClickCount == 1)
            {
                WindowEventHandler.getInstance().SetElement(this);
                WindowEventHandler.getInstance().GetSelectedObjectName();
            }
            if (e.ClickCount == 2)
            {
                LinePropertiesWindow propertiesWindow = new LinePropertiesWindow();
                if (propertiesWindow.ShowDialog() == true)
                {

                }
            }
        }
        public CustomLine(double x1, double y1, double x2, double y2)
        {
            line = new Line();
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            line.MouseDown += new MouseButtonEventHandler(LineClick);
            color = Brushes.Black;
        }
        public CustomLine(double x1, double y1, double x2, double y2, double strokeThickness, Brush brush)
        {
            line = new Line();
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            line.StrokeThickness = strokeThickness;
            line.Stroke = brush;
            color = brush;
            line.MouseDown += new MouseButtonEventHandler(LineClick);
        }
        public Line? GetLine()
        {
            return line;
        }
        public void SetLine(Line? line)
        {
            this.line = line;
        }

        public void Selected()
        {
            line.Stroke = Brushes.Red;
        }

        public void Deselected()
        {
            line.Stroke = color;
        }

        public void Delete()
        {
            line = null;
        }

        public FrameworkElement GetVisualElement()
        {
            return line;
        }
    }
}
