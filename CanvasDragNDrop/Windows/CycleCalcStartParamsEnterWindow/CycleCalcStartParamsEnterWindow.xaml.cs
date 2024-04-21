using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CanvasDragNDrop.Windows.CycleCalcStartParamsEnterWindow
{
    /// <summary>
    /// Interaction logic for CycleCalcStartParamsEnterWindow.xaml
    /// </summary>
    public partial class CycleCalcStartParamsEnterWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<BlockInstance> Instances
        {
            get => _instances;
            set { _instances = value; OnPropertyChanged(); }
        }
        private ObservableCollection<BlockInstance> _instances;

        public ObservableCollection<FlowConnector> InputFlowConnectors
        {
            get { return _inputFlowConnectors; }
            set { _inputFlowConnectors = value; OnPropertyChanged(); }
        }
        private ObservableCollection<FlowConnector> _inputFlowConnectors;

        public int SelectedInstanceIndex
        {
            get { return _selectedInstanceIndex; }
            set { _selectedInstanceIndex = value; OnPropertyChanged(); InputFlowConnectors = new(Instances[value].InputConnectors.Where(x => x.InterconnectLine.FlowInterconnectStatus == FlowInterconnectLine.FlowInterconnectStatuses.InCycle)); }
        }
        private int _selectedInstanceIndex = -1;



        public CycleCalcStartParamsEnterWindow(List<BlockInstance> blockInstances)
        {
            InitializeComponent();
            Instances = new ObservableCollection<BlockInstance>(blockInstances);
        }
        private void StartCycleCalculation(object sender, RoutedEventArgs e)
        {
            Close();
        }



        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
