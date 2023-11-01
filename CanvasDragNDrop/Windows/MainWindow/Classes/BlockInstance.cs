using CanvasDragNDrop.APIClases;
using CanvasDragNDrop.UtilityClasses;
using CanvasDragNDrop.Windows.BlockModelCreationWindow.Classes;
using CanvasDragNDrop.Windows.MainWindow.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CanvasDragNDrop
{
    public class BlockInstance : NotifyPropertyChangedClass
    {
        public int BlockInstanceId
        {
            get { return _blockInstanceId; }
            set { _blockInstanceId = value; }
        }
        private int _blockInstanceId;

        public APIBlockModelClass BlockModel
        {
            get { return _blockModel; }
            set { _blockModel = value; OnPropertyChanged(); }
        }
        private APIBlockModelClass _blockModel;

        public int BlockHeight
        {
            get { return _blockHeight; }
            set { _blockHeight = value; OnPropertyChanged(); }
        }
        private int _blockHeight = 200;

        public int BlockWidth
        {
            get { return _blockWidth; }
            set { _blockWidth = value; OnPropertyChanged(); }
        }
        private int _blockWidth = 100;

        public double OffsetTop
        {
            get { return _offsetTop; }
            set { _offsetTop = value; OnPropertyChanged(); }
        }
        private double _offsetTop = 0;

        public double OffsetLeft
        {
            get { return _offsetLeft; }
            set { _offsetLeft = value; OnPropertyChanged(); }
        }
        private double _offsetLeft = 0;

        public Brush BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; OnPropertyChanged(); }
        }
        private Brush _backgroundColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#61789C"));

        public ObservableCollection<FlowConnector> InputConnectors
        {
            get { return _inputConnectors; }
            set { _inputConnectors = value; OnPropertyChanged(); }
        }
        private ObservableCollection<FlowConnector> _inputConnectors = new();

        public ObservableCollection<FlowConnector> OutputConnectors
        {
            get { return _outputConnectors; }
            set { _outputConnectors = value; OnPropertyChanged(); }
        }
        private ObservableCollection<FlowConnector> _outputConnectors = new();

        public int ConnectorSize { get; set; } = 30;

        private const int _flowConnectorsStep = 10;

        public BlockInstance(APIBlockModelClass BlockModel, int blockInstanceId)
        {
            _blockModel = BlockModel;
            BlockInstanceId = blockInstanceId;
            for (int i = 0; i < _blockModel.InputFlows.Count; i++)
            {
                ObservableCollection<BlockInstanceVariable> variables = new();
                foreach (var item in _blockModel.InputFlows[i].AvailableVariables)
                {
                    variables.Add(new(BlockInstanceVariable.Types.Input, item.FlowVariableId, item.VariablePrototype.ParameterId, item.FlowVariableName, 0, item.VariablePrototype.Title, item.VariablePrototype.Units));
                }
                double ConnectorOffcetTop = i * (ConnectorSize + _flowConnectorsStep) + _flowConnectorsStep;
                double ConnectorOffcetLeft = -1 * (ConnectorSize / 2.0);
                int FlowTypeId = _blockModel.InputFlows[i].EnvironmentId;
                int FlowId = _blockModel.InputFlows[i].FlowId;
                FlowConnector InputFlowConnector = new(ConnectorOffcetTop, ConnectorOffcetLeft,true,ConnectorSize,FlowTypeId,BlockInstanceId,FlowId,variables);
                PropertyChanged += InputFlowConnector.ParentPropertyChangedEventHandler;
                InputConnectors.Add(InputFlowConnector);
            }
            for (int i = 0; i < _blockModel.OutputFlows.Count; i++)
            {
                ObservableCollection<BlockInstanceVariable> variables = new();
                foreach (var item in _blockModel.OutputFlows[i].RequiredVariables)
                {
                    variables.Add(new(BlockInstanceVariable.Types.Output, item.FlowVariableId, item.VariablePrototype.ParameterId, item.FlowVariableName, 0, item.VariablePrototype.Title, item.VariablePrototype.Units));
                }
                double ConnectorOffcetTop = i * (ConnectorSize + _flowConnectorsStep) + _flowConnectorsStep;
                double ConnectorOffcetLeft = BlockWidth - ConnectorSize / 2.0;
                int FlowTypeId = _blockModel.OutputFlows[i].EnvironmentId;
                int FlowId = _blockModel.OutputFlows[i].FlowId;
                FlowConnector OutputFlowConnector = new(ConnectorOffcetTop, ConnectorOffcetLeft, false, ConnectorSize, FlowTypeId, BlockInstanceId, FlowId,variables);
                PropertyChanged += OutputFlowConnector.ParentPropertyChangedEventHandler;
                OutputConnectors.Add(OutputFlowConnector);
            }

            BlockHeight = Math.Max(_blockModel.InputFlows.Count, _blockModel.OutputFlows.Count) * (ConnectorSize + _flowConnectorsStep) + _flowConnectorsStep;
            OffsetLeft = OffsetTop = 10;
        }
    }
}
