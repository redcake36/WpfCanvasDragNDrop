using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CanvasDragNDrop.Windows.CalculationResultWindow
{
    /// <summary>
    /// Interaction logic for CalculationResultWindow.xaml
    /// </summary>
    public partial class CalculationResultWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<BlockInstance> Instances
        {
            get { return _instances; }
            set { _instances = value; }
        }
        private ObservableCollection<BlockInstance> _instances;

        public CalculationResultWindow(IEnumerable<BlockInstance> instances)
        {
            Instances = new ObservableCollection<BlockInstance>(instances);
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
