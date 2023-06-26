using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CanvasDragNDrop
{
    public class WindowEventHandler
    {
        private static WindowEventHandler? instance;
        private FrameworkElement? element;
        private WindowEventHandler()
        {
            instance = this;
        }

        public static WindowEventHandler getInstance()
        {
            if (instance == null)
                instance = new WindowEventHandler();
            return instance;
        }
        public FrameworkElement? GetCanvasElement()
        {
            if(element != null)
                return ((ICanvasChild)this.element).GetVisualElement();
            return null;
        }
        public FrameworkElement? GetElement()
        {
            return element;
        }
        public void SetElement(FrameworkElement element)
        {
            if (this.element != null && this.element != element)
                SelectElement((ICanvasChild)this.element, false);
            this.element = element;
            SelectElement((ICanvasChild)this.element, true);
            Trace.WriteLine("set");
        }
        public void ResetElement()
        {
            if (this.element != null)
            {
                SelectElement((ICanvasChild)this.element, false);
                this.element = null;
            }
        }

        public void DeleteElement()
        {
            if (this.element != null)
            {
                ((ICanvasChild)this.element).Delete();
                this.element = null;
            }
        }
        void SelectElement(ICanvasChild canvasChild, bool b)
        {
            if (b)
            {
                canvasChild.Selected();
            }
            else
            {
                canvasChild.Deselected();
            }
        }
        public void GetSelectedObjectName()
        {
            try
            {
                Trace.WriteLine("elem: " + element.Name);
            }
            catch (Exception)
            {
                Trace.WriteLine("element not set");
                throw;
            }
        }
    }
}
