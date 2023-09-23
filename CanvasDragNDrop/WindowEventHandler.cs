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
        private FrameworkElement? currentElement;
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
            if(currentElement != null)
                return ((ICanvasChild)currentElement).GetVisualElement();
            return null;
        }
        public FrameworkElement? GetElement()
        {
            return currentElement;
        }
        public void SetElement(FrameworkElement element)
        {
            if (currentElement != null && currentElement != element)
                SelectElement((ICanvasChild)currentElement, false);
            currentElement = element;
            SelectElement((ICanvasChild)currentElement, true);
            Trace.WriteLine("set");
        }
        public void ResetElement()
        {
            if (currentElement != null)
            {
                SelectElement((ICanvasChild)currentElement, false);
                currentElement = null;
            }
        }

        public void DeleteElement()
        {
            if (currentElement != null)
            {
                ((ICanvasChild)currentElement).Delete();
                currentElement = null;
            }
        }
        void SelectElement(ICanvasChild canvasChild, bool isSelected)
        {
            canvasChild.Selected(isSelected);
        }
        public void GetSelectedObjectName()
        {
            try
            {
                Trace.WriteLine("elem: " + currentElement.Name);
            }
            catch (Exception)
            {
                Trace.WriteLine("element not set");
                throw;
            }
        }
    }
}
