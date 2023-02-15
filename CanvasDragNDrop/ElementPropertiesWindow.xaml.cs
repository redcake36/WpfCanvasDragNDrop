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

        public Brush brush
        {
            get { return new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorBox.Text)); }
            set { colorBox.Text = value.ToString(); }
        }
        public String name
        {
            get { return nameBox.Text; }
            set { nameBox.Text = value; }
        }
        public int height
        {
            get { return Int32.Parse( heightBox.Text); }
            set { heightBox.Text = value.ToString();}
        }
        public int width
        {
            get { return Int32.Parse(widthBox.Text); }
            set { widthBox.Text = value.ToString();}
        }
        public ElementPropertiesWindow()
        {
            InitializeComponent();
            //canvas = elementPropertiesWindowCanvas;
        }
        public void AddCanvasText(string s)
        {
            canvas.Children.Add(new TextBlock { Text = s });
        }
        public void AddListText(string s) {
            //elementPropertiesWindowList.Items.Add((new ListBoxItem { }).Content = s);
        }

        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
