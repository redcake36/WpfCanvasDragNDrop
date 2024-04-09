using CanvasDragNDrop.APIClases;
using CanvasDragNDrop.Windows;
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

namespace CanvasDragNDrop.Windows.SchemeSelectionWindow
{
    /// <summary>
    /// Логика взаимодействия для SchemeSelectionWindow.xaml
    /// </summary>
    public partial class SchemeSelectionWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<APISchemeClass> AvailableSchemas
        {
            get { return _availableSchemas; }
            set { _availableSchemas = value; OnPropertyChanged(); }
        }
        private ObservableCollection<APISchemeClass> _availableSchemas;

        public int SelectedSchemaIndex
        {
            get { return _selectedSchemaIndex; }
            set { _selectedSchemaIndex = value; OnPropertyChanged(); }
        }
        private int _selectedSchemaIndex = -1;


        public SchemeSelectionWindow()
        {
            InitializeComponent();
        }

        private void GetSchemasList(object sender, RoutedEventArgs e)
        {
            var response = API.GetAllSchemas();
            if (response.isSuccess == false)
            {
                MessageBox.Show("Ошибка при загрузки схем с сервера");
                Application.Current.Shutdown();
                return;
            }
            AvailableSchemas = new(response.Schemas);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void CreateNewSchema(object sender, RoutedEventArgs e)
        {
            AlternateMainWindow mainWindow = new AlternateMainWindow();
            mainWindow.Show();
            Close();
        }

        private void LeftButtonDownGoSetupWindow(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new Windows.LoginWindow();
            LoginWindow loginWindow = new LoginWindow();
            mainWindow.Show();
            Close();
        }

        private void OpenSchema(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show(AvailableSchemas[SelectedSchemaIndex].SchemaName);
        }
    }
}
