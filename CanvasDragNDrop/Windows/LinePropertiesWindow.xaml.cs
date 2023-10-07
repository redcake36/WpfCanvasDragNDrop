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

namespace CanvasDragNDrop.Windows
{
    /// <summary>
    /// Логика взаимодействия для LinePropertiesWindow.xaml
    /// </summary>
    public partial class LinePropertiesWindow : Window
    {
        public LinePropertiesWindow()
        {
            InitializeComponent();
        }
        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
