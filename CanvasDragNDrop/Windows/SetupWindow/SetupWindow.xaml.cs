using System.Windows;

namespace CanvasDragNDrop.Windows
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

        private void Login(object sender, RoutedEventArgs e)
        {
            bool result = CheckData();
            if (result)
            {
                Close();
            }
        }

        bool CheckData()
        {
            var tryResponce = API.GetEnvironments();
            if (tryResponce.isSuccess == false)
            {
                MessageBox.Show("Неверные данные для подключения");
                return false;
            }
            API.StoreSettings();
            MessageBox.Show("Данные успешно обновлены");
            return true;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            API.Password = PasswordField.Password;
        }
    }
}
