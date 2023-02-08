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
using System.Windows.Shapes;

namespace CanvasDragNDrop
{
    /// <summary>
    /// Логика взаимодействия для ElementPropertiesWindow.xaml
    /// </summary>
    public partial class ElementPropertiesWindow : Window
    {
        Canvas canvas;
        public ElementPropertiesWindow()
        {
            InitializeComponent();
            canvas = elementPropertiesWindowCanvas;
        }
        public void AddCanvasText(string s)
        {
            canvas.Children.Add(new TextBlock { Text = s });
        }
        public void AddListText(string s) {
            elementPropertiesWindowList.Items.Add((new ListBoxItem { }).Content = s);
        }
    }
}
