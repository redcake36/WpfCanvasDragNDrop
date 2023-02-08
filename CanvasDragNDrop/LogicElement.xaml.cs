using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
    /// Логика взаимодействия для LogicElement.xaml
    /// </summary>
    public partial class LogicElement : UserControl
    {
        public LogicElement(int h, int w, string t, Brush b)
        {
            InitializeComponent();
            DataContext = this;
            height = h;
            width = w;
            text = t;
            color = b;
        }
        public int height
        {
            get { return (int)GetValue(heightProperty); }
            set { SetValue(heightProperty, value); }
        }
        public static readonly DependencyProperty heightProperty = DependencyProperty.Register("height", typeof(int), typeof(LogicElement), new PropertyMetadata(null));
        public string text
        {
            get { return (string)GetValue(textProperty); }
            set { SetValue(textProperty, value); }
        }
        public static readonly DependencyProperty textProperty = DependencyProperty.Register("text", typeof(string), typeof(LogicElement), new PropertyMetadata(null));

        public int width
        {
            get { return (int)GetValue(widthProperty); }
            set { SetValue(widthProperty, value); }
        }
        public static readonly DependencyProperty widthProperty = DependencyProperty.Register("width", typeof(int), typeof(LogicElement), new PropertyMetadata(null));

        public Brush color
        {
            get { return (Brush)GetValue(colorProperty); }
            set { SetValue(colorProperty, value); }

        }
        public static readonly DependencyProperty colorProperty = DependencyProperty.Register("color", typeof(Brush), typeof(LogicElement), new PropertyMetadata(null));

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed && MainWindow.state)
            {
                DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
                Trace.WriteLine(color.ToString());
            }
        }

        public LogicElement()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                ElementPropertiesWindow propertiesWindow = new ElementPropertiesWindow();
                propertiesWindow.AddListText("color: " + this.color.ToString());
                propertiesWindow.AddListText("text: " + this.text);
                propertiesWindow.AddListText("width: " + this.width.ToString());
                propertiesWindow.AddListText("height: " + this.height.ToString());
                propertiesWindow.Show();

            }
        }
    }
}
