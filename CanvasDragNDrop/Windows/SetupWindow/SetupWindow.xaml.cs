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
            PasswordField.Password = API.Password;
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
            OnPasswordChanged(null, null);
            var tryResponce = API.GetUserId(API.Username);
            if (tryResponce.isSuccess == false || tryResponce.response == -1)
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

        private void WindowClosed(object sender, System.EventArgs e)
        {
            API.LoadSettings();
        }
    }
}
