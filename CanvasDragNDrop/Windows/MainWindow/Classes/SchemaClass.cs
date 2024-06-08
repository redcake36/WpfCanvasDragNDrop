using CanvasDragNDrop.APIClases;
using CanvasDragNDrop.UtilityClasses;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

        public SchemaClass(APISchemeClass schema, List<APIBlockModelVersionClass> modelsVersions)
        {
            SchemaName = schema.SchemaName;
            SchemaId = schema.SchemaId;
            foreach (var item in schema.BlockInstanсes)
            {
                BlockInstances.Add(new(item, modelsVersions.Find(x => x.VersionId == item.BlockModel.VersionId)));
            }
            foreach (var interconnect in schema.BlockInterconnections)
            {
                //FIX исправить подгрузку потоков
                FlowConnector inputConnector = BlockInstances.Single(x => x.BlockInstanceId == interconnect.InputFlowConnector.BlockInstanceID).OutputConnectors.Single(y => y.FlowID == interconnect.InputFlowConnector.FlowID);
                FlowConnector outputConnector = BlockInstances.Single(x => x.BlockInstanceId == interconnect.OutputFlowConnector.BlockInstanceID).InputConnectors.Single(y => y.FlowID == interconnect.OutputFlowConnector.FlowID);
                BlockInterconnections.Add(new(inputConnector, outputConnector));
            }
        }
    }
}
