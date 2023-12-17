using CanvasDragNDrop.APIClases;
using CanvasDragNDrop.UtilityClasses;
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
        public enum BlockInstanceStatuses : int
        {
            NotCalculated,
            Seen,
            Calculated
        }

        public BlockInstanceStatuses BlockInstanceStatus
        {
            get { return _blockInstanceStatus; }
            set { _blockInstanceStatus = value; OnPropertyChanged(); }
        }
        private BlockInstanceStatuses _blockInstanceStatus = BlockInstanceStatuses.NotCalculated;


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

        public ObservableCollection<BlockInstanceVariable> DefaultVariables
        {
            get { return _defaultVariables; }
            set { _defaultVariables = value; }
        }
        private ObservableCollection<BlockInstanceVariable> _defaultVariables = new();


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
                FlowConnector InputFlowConnector = new(ConnectorOffcetTop, ConnectorOffcetLeft, true, ConnectorSize, FlowTypeId, BlockInstanceId, FlowId, variables);
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
                FlowConnector OutputFlowConnector = new(ConnectorOffcetTop, ConnectorOffcetLeft, false, ConnectorSize, FlowTypeId, BlockInstanceId, FlowId, variables);
                PropertyChanged += OutputFlowConnector.ParentPropertyChangedEventHandler;
                OutputConnectors.Add(OutputFlowConnector);
            }

            foreach (var item in _blockModel.DefaultParameters)
            {
                DefaultVariables.Add(new(BlockInstanceVariable.Types.Default, item.ParameterId, -1, item.VariableName, 0, item.Title, item.Units));
            }

            BlockHeight = Math.Max(_blockModel.InputFlows.Count, _blockModel.OutputFlows.Count) * (ConnectorSize + _flowConnectorsStep) + _flowConnectorsStep;
            OffsetLeft = OffsetTop = 10;
        }

        public void CalculateBlockInstance()
        {
            //Наполнение списка всех переменных
            List<BlockInstanceVariable> AllVariables = new List<BlockInstanceVariable>();
            foreach (var flow in InputConnectors)
            {
                AllVariables.AddRange(flow.CalculationVariables);
            }
            foreach (var flow in OutputConnectors)
            {
                AllVariables.AddRange(flow.CalculationVariables);
            }
            AllVariables.AddRange(DefaultVariables);
            foreach (var variable in BlockModel.CustomParameters) 
            {
                AllVariables.Add(new(BlockInstanceVariable.Types.Custom, variable.ParameterId, -1, variable.VariableName, 0, variable.Title, variable.Units));
            }

            //перенос значений с выходных потоков предыдущих блоков
            foreach(var flow in InputConnectors)
            {
                foreach (var item in flow.InterconnectLine.OutputFlowConnector.CalculationVariables)
                {
                    AllVariables.Find(x => x.VariablePrototypeId == item.VariablePrototypeId).Value = item.Value;
                }
            }

            //расчёт выражений
            foreach (var expression in BlockModel.Expressions)
            {
                var definedVar = AllVariables.Find(x => x.VariableId == expression.DefinedVariable[0]);
                var neededVars = AllVariables.FindAll(x => expression.NeededVariables.Contains(x.VariableId));
                if (expression.ExpressionType == GlobalTypes.ExpressionTypes.PropSI)
                {
                    definedVar.Value = CalcUtilitiesClass.CallPropSIFromString(expression.Expression, neededVars);
                }
                else
                {
                    var expr = CalcUtilitiesClass.ConstructMathExpression(expression.Expression, neededVars);
                    definedVar.Value = CalcUtilitiesClass.calcMathExpression(expr, neededVars);
                }
            }

            //Кладём новые значения в выходные потоки блока
            foreach (var flow in OutputConnectors)
            {
                foreach (var item in flow.CalculationVariables)
                {
                    item.Value = AllVariables.Find(x => x.VariableName == item.VariableName).Value;
                }
            }

            BlockInstanceStatus = BlockInstanceStatuses.Calculated;
        }
    }
}
