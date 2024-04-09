using CanvasDragNDrop.Windows.LoginWindow.Classes;
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

namespace CanvasDragNDrop.Windows.LoginWindow
{
    /// <summary>
    /// Логика взаимодействия для SetupWindow.xaml
    /// </summary>
    public partial class SetupWindow : Window
    {
        public SetupWindow()
        {
            InitializeComponent();
        }

        private void Button_MouseLeftButtonDownEntry(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("No");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string moc = SetupClass.MockAdress;
            MessageBox.Show("Yes");
        }
    }
}
