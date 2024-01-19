using CanvasDragNDrop.UtilityClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop.Windows.MainWindow.Classes
{
    public class SchemaClass:NotifyPropertyChangedClass
    {
        public string Title
        {
            get => _title;
            set { Title = value; OnPropertyChanged(); }
        }
        private string _title = "";

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
