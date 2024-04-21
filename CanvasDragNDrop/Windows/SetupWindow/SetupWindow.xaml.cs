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
            bool result = CheckData(false);
            if (result)
            {
                Close();
            }
        }

        bool CheckData(bool isSilent)
        {
            var tryResponce = API.GetEnvironments();
            if (tryResponce.isSuccess == false)
            {
                //if (isSilent == false)
                //{
                MessageBox.Show("Неверные данные для подключения");
                //}
                return false;
            }
            API.StoreSettings();
            if (isSilent == false)
            {
                MessageBox.Show("Данные успешно обновлены");
            }
            return true;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            API.Password = PasswordField.Password;
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool checkResults = CheckData(true);
            if (checkResults == false)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
