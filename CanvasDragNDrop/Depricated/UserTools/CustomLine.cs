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
    public class CustomLine : Shape
    {
        protected Line line;
        protected Brush defaultColor;
        protected Brush highlightColor;

        protected override Geometry DefiningGeometry =>
            throw new NotImplementedException();

        public CustomLine()
        {
            line = new Line();
            line.StrokeThickness = 2;
            defaultColor = Brushes.Black;
            highlightColor = Brushes.Red;
            line.Stroke = defaultColor;
        }
        public CustomLine(MouseButtonEventArgs e, IInputElement parent) : this()
        {
            line.X1 = e.GetPosition(parent).X;
            line.Y1 = e.GetPosition(parent).Y;
            line.X2 = e.GetPosition(parent).X;
            line.Y2 = e.GetPosition(parent).Y;
        }
        public CustomLine(double x1, double y1, double x2, double y2,
            double strokeThickness = 1f,
            Brush? HLBrush = null, Brush? DefBrush = null)
        {
            line = new Line();
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            line.StrokeThickness = strokeThickness;

            if (HLBrush is null)
                highlightColor = Brushes.Red;
            else
                highlightColor = HLBrush;
            if (DefBrush is null)
                defaultColor = Brushes.Black;
            else
                defaultColor = DefBrush;
            line.Stroke = defaultColor;
        }

        public void SetCoords(float x1, float y1, float x2, float y2)
        {
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
        }
        public void SetStartPoint(float x1, float y1)
        {
            line.X1 = x1;
            line.Y1 = y1;
        }
        public void SetStartPoint(Point p)
        {
            line.X1 = p.X;
            line.Y1 = p.Y;
        }
        public void SetEndPoint(float x2, float y2)
        {
            line.X2 = x2;
            line.Y2 = y2;
        }
        public void SetEndPoint(Point p)
        {
            line.X2 = p.X;
            line.Y2 = p.Y;
        }
        public void HighLight()
        {
            line.Stroke = highlightColor;
        }

        public void SetDefltColor()
        {
            line.Stroke = defaultColor;
        }

        public Line GetLine()
        {
            return line;
        }
        public void SetLine(Line line)
        {
            this.line = line;
        }
    }
}
