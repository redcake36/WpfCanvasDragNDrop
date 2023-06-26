using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CanvasDragNDrop
{
    interface ICanvasChild
    {
        void Selected();
        void Deselected();
        void Delete();
        FrameworkElement GetVisualElement();
    }
}
