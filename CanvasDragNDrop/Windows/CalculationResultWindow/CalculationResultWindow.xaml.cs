using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void DataGridMouseWheel(object sender, MouseWheelEventArgs e)
        {
            //e.Handled = false;
            var args = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);

            //then we need to set the event that we're invoking.
            //the ScrollViewer control internally does the scrolling on MouseWheelEvent, so that's what we're going to use:
            args.RoutedEvent = ScrollViewer.MouseWheelEvent;

            //and finally, we raise the event on the parent ScrollViewer.
            scroll_viewer.RaiseEvent(args);
        }
    }
}
