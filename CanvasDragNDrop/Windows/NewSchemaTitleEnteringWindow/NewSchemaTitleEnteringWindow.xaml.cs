using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CanvasDragNDrop.Windows
{
    /// <summary>
    /// Логика взаимодействия для NewSchemaTitleEnteringWindow.xaml
    /// </summary>
    public partial class NewSchemaTitleEnteringWindow : Window, INotifyPropertyChanged
    {
        public NewSchemaTitleEnteringWindow()
        {
            InitializeComponent();
        }
        public string SchemaName
        {
            get => _schemaName;
            set { _schemaName = value; OnPropertyChanged(); }
        }
        private string _schemaName = "";

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void SaveTitleSchemaButton_Click(object sender, RoutedEventArgs e)
        {
            //_schema.SchemaName = TitleSchema.Text;
            if (string.IsNullOrWhiteSpace(SchemaName))
            {
                MessageBox.Show("Поле не может быть пустым!");
                return;
            }
            Close();
        }
    }
}
