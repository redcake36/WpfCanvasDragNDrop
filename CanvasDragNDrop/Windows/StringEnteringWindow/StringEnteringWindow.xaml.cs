using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CanvasDragNDrop.Windows.StringEnteringWindowNS
{
    /// <summary>
    /// Логика взаимодействия для StringEnteringWindow.xaml
    /// </summary>
    public partial class StringEnteringWindow : Window, INotifyPropertyChanged
    {
        public StringEnteringWindow()
        {
            InitializeComponent();
        }
        public StringEnteringWindow(string requestString) : this()
        {
            RequestString = requestString;
        }

        public StringEnteringWindow(string requestString, bool isAllowEmpty) : this(requestString)
        {
            _isAllowEmpty = isAllowEmpty;
        }

        public StringEnteringWindow(string requestString, string windowTitle, bool isAllowEmpty) : this(requestString, isAllowEmpty)
        {
            WindowTitle = windowTitle;
        }

        public string EnteredString
        {
            get => _enteredString;
            set { _enteredString = value; OnPropertyChanged(); }
        }
        private string _enteredString = String.Empty;

        public string WindowTitle
        {
            get { return _windowTitle; }
            set { _windowTitle = value; OnPropertyChanged(); }
        }
        private string _windowTitle = "Запрос значения";

        public string RequestString
        {
            get { return _requestString; }
            set { _requestString = value; OnPropertyChanged(); }
        }
        private string _requestString = String.Empty;

        private bool _isAllowEmpty = true;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Save(object sender = null, RoutedEventArgs e = null)
        {
            if (string.IsNullOrWhiteSpace(EnteredString) && _isAllowEmpty == false)
            {
                MessageBox.Show("Поле не может быть пустым!");
                return;
            }
            Close();
        }

        private void Close(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EnteredString) && _isAllowEmpty == false)
            {
                MessageBox.Show("Поле не может быть пустым!");
                e.Cancel = true;
            }
        }
    }
}
