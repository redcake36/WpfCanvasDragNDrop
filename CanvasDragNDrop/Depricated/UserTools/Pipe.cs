using CanvasDragNDrop.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace CanvasDragNDrop
{
    public class Pipe : CustomLine, ICanvasChild
    {
        public FlowPoint? fromFlowPoint { get; set; }
        public FlowPoint? toFlowPoint { get; set; }

        public Pipe() : base()
        {
            line.MouseDown += new MouseButtonEventHandler(LineClick);
        }

        public Pipe(MouseButtonEventArgs e, Canvas canvas) : base(e, canvas)
        {
            line.MouseDown += new MouseButtonEventHandler(LineClick);
        }
        public Pipe(double x1, double y1, double x2, double y2,
            double strokeThickness = 2f,
            Brush? HLBrush = null, Brush? DefBrush = null)
            : base(x1, y1, x2, y2, strokeThickness, HLBrush, DefBrush)
        {
            line.MouseDown += new MouseButtonEventHandler(LineClick);
        }
        void LineClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1 && e.ChangedButton == MouseButton.Left)
            {
                WindowEventHandler.getInstance().SetElement(this);
                WindowEventHandler.getInstance().GetSelectedObjectName();
            }
            if (e.ClickCount == 2 && e.ChangedButton == MouseButton.Left)
            {
                LinePropertiesWindow propertiesWindow = new LinePropertiesWindow();
                if (propertiesWindow.ShowDialog() == true)
                {

                }
            }
        }

        public void Selected(bool isSelected)
        {
            if (isSelected)
            {
                HighLight();
            }
            else
            {
                SetDefltColor();
            }
        }

        public void Delete()
        {
            line = null;
            if (fromFlowPoint != null)
                fromFlowPoint.connectedPipe = null;
            if (toFlowPoint != null)
                toFlowPoint.connectedPipe = null;
        }

        public FrameworkElement GetVisualElement()
        {
            return line;
        }
    }
}
