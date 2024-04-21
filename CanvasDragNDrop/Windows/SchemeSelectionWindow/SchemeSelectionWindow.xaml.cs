using CanvasDragNDrop.APIClases;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            var response = API.GetAllSchemas();
            if (response.isSuccess == false)
            {
                MessageBox.Show("Ошибка при загрузки схем с сервера. Проверьте настройки подключения.");
                SetupWindow loginWindow = new();
                loginWindow.ShowDialog();
                return;
            }
            AvailableSchemas = new(response.Schemas);
            //AvailableSchemas.Add(new(1, "Цикл ренкина"));
            //AvailableSchemas.Add(new(2, "Цикл ренкина 2"));
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
        }

        private void OpenSchema(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show(AvailableSchemas[SelectedSchemaIndex].SchemaName);
            //var resp = API.GetSchema(AvailableSchemas[SelectedSchemaIndex].SchemaId);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
