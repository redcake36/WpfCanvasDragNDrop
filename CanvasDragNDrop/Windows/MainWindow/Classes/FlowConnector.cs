using CanvasDragNDrop.UtilityClasses;
using CanvasDragNDrop.Windows.MainWindow.Classes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CanvasDragNDrop
{
    public class FlowConnector: NotifyPropertyChangedClass
    {
		public int FlowTypeID { get; private set; }
		public int BlockInstanceID { get; private set; }
		public int FlowID { get; private set; }
		[JsonIgnore]
		public FlowInterconnectLine? InterconnectLine { get; set; } = null;

        [JsonIgnore]
        public ObservableCollection<BlockInstanceVariable> CalculationVariables
		{
			get => _calculationVariables;
			set { _calculationVariables = value; OnPropertyChanged(); }
		}
		private ObservableCollection<BlockInstanceVariable> _calculationVariables;

        [JsonIgnore]
        public double ConnectorOffsetTop
		{
			get { return _connectorOffsetTop; }
			set { _connectorOffsetTop = value; OnPropertyChanged(); }
		}
		private double _connectorOffsetTop = 0;

        [JsonIgnore]
        public double ConnectorOffsetLeft
		{
			get { return _connectorOffsetLeft; }
			set { _connectorOffsetLeft = value; OnPropertyChanged(); }
		}
		private double _connectorOffsetLeft = 0;

        [JsonIgnore]
        public bool IsInputConnector
		{
			get { return _isInputConnector; }
			set { _isInputConnector = value; OnPropertyChanged(); }
		}
		private bool _isInputConnector = true;

        [JsonIgnore]
        public double FlowPointOffsetTop
		{
			get { return _flowPointOffsetTop; }
			set { _flowPointOffsetTop = value; OnPropertyChanged(); }
		}
		private double _flowPointOffsetTop;

        [JsonIgnore]
        public double FlowPointOffsetLeft
        {
			get { return _flowPointOffsetLeft; }
			set { _flowPointOffsetLeft = value; OnPropertyChanged(); }
		}
		private double _flowPointOffsetLeft;

		private int _connectorSize;


		public FlowConnector(double offcetTop, double offsetLeft, bool isInput, int connectorSize, int flowTypeID, int blockInstanseID, int flowID, ObservableCollection<BlockInstanceVariable> calculationVariables)
		{
			_connectorSize = connectorSize;
			ConnectorOffsetLeft = offsetLeft;
			ConnectorOffsetTop = offcetTop;
			IsInputConnector = isInput;
			FlowTypeID = flowTypeID;
            BlockInstanceID = blockInstanseID;
			FlowID = flowID;
			CalculationVariables = calculationVariables;
		}

		public void ParentPropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName) 
			{
				case nameof(BlockInstance.OffsetLeft):
                case nameof(BlockInstance.OffsetTop):
                    RecalculateFlowPoint((sender as BlockInstance).OffsetLeft, (sender as BlockInstance).OffsetTop);
					break;
			}
		}

		public void RecalculateFlowPoint(double newParentOffcetLeft, double newParentOffcetTop)
		{
            FlowPointOffsetLeft = newParentOffcetLeft+ ConnectorOffsetLeft + (IsInputConnector?0:_connectorSize);
			FlowPointOffsetTop = newParentOffcetTop+ConnectorOffsetTop + _connectorSize/2.0;
        }

		//public void ConnectInterconnection(FlowConnector anotherConnector)
		//{
		//	string Error = "Can't connect";
		//	if (InterconnectLine != null)
		//	{
		//		throw new Exception(Error);
		//	}

		//}


	}
}
