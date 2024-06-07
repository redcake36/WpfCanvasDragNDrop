using CanvasDragNDrop.UtilityClasses;

namespace CanvasDragNDrop.APIClases
{
    public class APIBlockInterconnection : NotifyPropertyChangedClass
    {
        public APIFlowConnector InputFlowConnector
        {
            get { return _inputFlowConnector; }
            set { _inputFlowConnector = value; OnPropertyChanged(); }
        }
        private APIFlowConnector _inputFlowConnector;

        public APIFlowConnector OutputFlowConnector
        {
            get { return _outputFlowConnector; }
            set { _outputFlowConnector = value; OnPropertyChanged(); }
        }
        private APIFlowConnector _outputFlowConnector;

    }
}
