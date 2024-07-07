using CanvasDragNDrop.APIClases;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

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
            var checkAuth = API.GetUserId(API.Username);
            var response = API.GetAllSchemas();
            if (response.isSuccess == false || checkAuth.isSuccess == false || checkAuth.response == -1)
            {
                MessageBox.Show("Ошибка при загрузки схем с сервера. Проверьте настройки подключения.");
                SetupWindow loginWindow = new();
                loginWindow.ShowDialog();
                GetSchemasList(null, null);
                return;
            }
            AvailableSchemas = new(response.Schemas.OrderBy(x => x.SchemaId));
        }

        private void CreateNewSchema(object sender, RoutedEventArgs e)
        {
            AlternateMainWindow mainWindow = new AlternateMainWindow();
            mainWindow.Show();
            Close();
        }

        private void LeftButtonDownGoSetupWindow(object sender, RoutedEventArgs e)
        {
            SetupWindow setupWindow = new();
            setupWindow.ShowDialog();
            GetSchemasList(null, null);
        }

        private void OpenSchema(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedSchemaIndex < 0)
            {
                return;
            }
            MessageBox.Show(AvailableSchemas[SelectedSchemaIndex].SchemaName);
            var resp = API.GetSchema(AvailableSchemas[SelectedSchemaIndex].SchemaId);
            if (resp.isSuccess == false)
            {
                MessageBox.Show("Ошибка при загрузки схемы с сервера");
                return;
            }
            AlternateMainWindow mainWindow = new(resp.Schemas);
            mainWindow.Show();
            GetSchemasList(null, null);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Image_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}
