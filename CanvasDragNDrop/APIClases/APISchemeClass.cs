using CanvasDragNDrop.UtilityClasses;
using System.Collections.Generic;

namespace CanvasDragNDrop.APIClases
{
    public class APISchemeClass : NotifyPropertyChangedClass
    {
        /// <summary> ID схемы </summary>
        public int SchemaId
        {
            get { return _schemaId; }
            set { _schemaId = value; OnPropertyChanged(); }
        }
        private int _schemaId;

        /// <summary> Название схемы </summary>
        public string SchemaName
        {
            get { return _schemaName; }
            set { _schemaName = value; OnPropertyChanged(); }
        }
        private string _schemaName;

        /// <summary> Массив экземпляров блоков </summary>
        public List<APIBlockInstance> BlockInstanсes
        {
            get { return _blockInstanсes; }
            set { _blockInstanсes = value; OnPropertyChanged(); }
        }
        private List<APIBlockInstance> _blockInstanсes;

        /// <summary> Массив соединений экземпляров блоков </summary>
        public List<APIBlockInterconnection> BlockInterconnections
        {
            get { return _blockInterconnections; }
            set { _blockInterconnections = value; OnPropertyChanged(); }
        }
        private List<APIBlockInterconnection> _blockInterconnections;

        public APISchemeClass(int schemaId, string schemaName, List<APIBlockInstance> instances, List<APIBlockInterconnection> interconnections)
        {
            _schemaId = schemaId;
            _schemaName = schemaName;
            _blockInstanсes = instances;
            _blockInterconnections = interconnections;
        }

    }
}
