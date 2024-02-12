using CanvasDragNDrop.APIClases;
using CanvasDragNDrop.Windows.MainWindow.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using CanvasDragNDrop;

namespace CanvasDragNDrop.Windows.ErrorSavingSchemaWindow
{
    /// <summary>
    /// Логика взаимодействия для ErrorSaveSchema.xaml
    /// </summary>
    public partial class ErrorSaveSchema : Window, INotifyPropertyChanged
    {
        public ErrorSaveSchema()
        {
            InitializeComponent();
            DataContext = this;
        }
        public string Title1 {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }
        private string _title = "";
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void SaveTitleSchemaButton_Click(object sender, RoutedEventArgs e)
        {
            //_schema.Title = TitleSchema.Text;
            if (string.IsNullOrWhiteSpace(TitleSchema.Text))
            {
                MessageBox.Show("Поле не может быть пустым!");
                return;
            }
            Close();
        }
    }
}
