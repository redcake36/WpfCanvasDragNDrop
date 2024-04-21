using CanvasDragNDrop.UtilityClasses;
using System.Collections.ObjectModel;

namespace CanvasDragNDrop.Windows.MainWindow.Classes
{
    public class SchemaClass : NotifyPropertyChangedClass
    {
        public string SchemaName
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }
        private string _title = "";

        public int SchemaId
        {
            get { return _schemaId; }
            set { _schemaId = value; OnPropertyChanged(); }
        }
        private int _schemaId = -1;


        public ObservableCollection<BlockInstance> BlockInstances
        {
            get { return _blockInstances; }
            set { _blockInstances = value; OnPropertyChanged(); }
        }
        private ObservableCollection<BlockInstance> _blockInstances = new();

        public ObservableCollection<FlowInterconnectLine> BlockInterconnections
        {
            get { return _blockInterconnections; }
            set { _blockInterconnections = value; OnPropertyChanged(); }
        }
        private ObservableCollection<FlowInterconnectLine> _blockInterconnections = new();

        public SchemaClass()
        {

        }
    }
}
