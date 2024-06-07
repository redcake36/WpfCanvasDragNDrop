using CanvasDragNDrop.UtilityClasses;

namespace CanvasDragNDrop.APIClases
{
    public class APIFlowConnector : NotifyPropertyChangedClass
    {
        public int BlockInstanceID
        {
            get { return _blockInstanceID; }
            set { _blockInstanceID = value; OnPropertyChanged(); }
        }
        private int _blockInstanceID;

        public int FlowID
        {
            get { return _flowID; }
            set { _flowID = value; OnPropertyChanged(); }
        }
        private int _flowID;
    }
}
