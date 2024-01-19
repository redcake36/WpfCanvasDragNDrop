﻿using CanvasDragNDrop.APIClases;
using CanvasDragNDrop.UserItems;
using CanvasDragNDrop.UtilityClasses;
using CanvasDragNDrop.Windows.MainWindow.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

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

        [JsonIgnore]
        public BlockInstanceStatuses BlockInstanceStatus
        {
            get { return _blockInstancestatus; }
            set { _blockInstancestatus = value; OnPropertyChanged(); }
        }
        private BlockInstanceStatuses _blockInstancestatus = BlockInstanceStatuses.NotCalculated;


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

        [JsonIgnore]
        public int BlockHeight
        {
            get { return _blockHeight; }
            set { _blockHeight = value; OnPropertyChanged(); }
        }
        private int _blockHeight = 200;

        [JsonIgnore]
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

        [JsonIgnore]
        public Brush BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; OnPropertyChanged(); }
        }
        private Brush _backgroundColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#61789C"));

        [JsonIgnore]
        public ObservableCollection<FlowConnector> InputConnectors
        {
            get { return _inputConnectors; }
            set { _inputConnectors = value; OnPropertyChanged(); }
        }
        private ObservableCollection<FlowConnector> _inputConnectors = new();

        [JsonIgnore]
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

        [JsonIgnore]
        public int ConnectorSize { get; set; } = 30;

        private const int _flowConnectorsStep = 10;

        private List<(PrecompiledCalcExpressionClass PreparedExpression, APIBlockModelExpressionClass OriginalExpression)> _preparedCalcExpressions = new();

        public BlockInstance(APIBlockModelClass BlockModel, int blockInstanceId)
        {
            _blockModel = BlockModel;
            BlockInstanceId = blockInstanceId;
            PrepareInputFlowConnectors();
            PrepareOutputFlowConnectors();

            PrepareDefaultParameters();
            PrepareCalcExpressions();

            BlockHeight = Math.Max(_blockModel.InputFlows.Count, _blockModel.OutputFlows.Count) * (ConnectorSize + _flowConnectorsStep) + _flowConnectorsStep;
            OffsetLeft = OffsetTop = 10;
        }

        /// <summary> Метод подготовки входных коннекторов потоков </summary>
        private void PrepareInputFlowConnectors()
        {
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
        }

        /// <summary> Метод подготовки выходных коннекторов потоков </summary>
        private void PrepareOutputFlowConnectors()
        {
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
        }

        /// <summary> Метод подоговки параметров по умолчанию для отображения </summary>
        private void PrepareDefaultParameters()
        {
            foreach (var item in _blockModel.DefaultParameters)
            {
                DefaultVariables.Add(new(BlockInstanceVariable.Types.Default, item.ParameterId, -1, item.VariableName, 0, item.Title, item.Units));
            }
        }

        private void PrepareCalcExpressions()
        {
            List<APIBlockModelExpressionClass> SortedExpressions = BlockModel.Expressions.OrderBy(x => x.Order).ToList();
            foreach (var expression in SortedExpressions)
            {
                _preparedCalcExpressions.Add((new(expression.Expression), expression));
            }
        }

        public void CalculateBlockInstance()
        {
            //Наполнение списка всех переменных с ключём по Id переменной
            Dictionary<int,BlockInstanceVariable> AllVariables = new();
            foreach (var flow in InputConnectors)
            {
                AllVariables = AllVariables.Concat(flow.CalculationVariables.ToDictionary(x => x.VariableId)).ToDictionary(x => x.Key, x => x.Value);
            }
            foreach (var flow in OutputConnectors)
            {
                AllVariables = AllVariables.Concat(flow.CalculationVariables.ToDictionary(x => x.VariableId)).ToDictionary(x => x.Key, x => x.Value);
            }
            AllVariables = AllVariables.Concat(DefaultVariables.ToDictionary(x => x.VariableId)).ToDictionary(x => x.Key, x => x.Value);
            foreach (var variable in BlockModel.CustomParameters)
            {
                AllVariables.Add(variable.ParameterId,new(BlockInstanceVariable.Types.Custom, variable.ParameterId, -1, variable.VariableName, 0, variable.Title, variable.Units));
            }

            //FIX перенос не работае для нескольких потоков
            //перенос значений с выходных потоков предыдущих блоков
            foreach (var inputFlow in InputConnectors)
            {
                foreach (var outputVar in inputFlow.InterconnectLine.OutputFlowConnector.CalculationVariables)
                {
                    AllVariables[inputFlow.CalculationVariables.First(inputVar => inputVar.VariablePrototypeId == outputVar.VariablePrototypeId).VariableId].Value = outputVar.Value;
                }
            }

            //расчёт выражений
            foreach (var expression in _preparedCalcExpressions)
            {
                BlockInstanceVariable definedVar = AllVariables[expression.OriginalExpression.DefinedVariable];
                Dictionary<string,BlockInstanceVariable> neededVars = AllVariables.Where(x => expression.OriginalExpression.NeededVariables.Contains(x.Key)).ToDictionary(x => x.Value.VariableName, y => y.Value);
                   
                definedVar.Value = expression.PreparedExpression.Calc(neededVars);
            }

            //FIX ? 
            //Кладём новые значения в выходные потоки блока
            //foreach (var flow in OutputConnectors)
            //{
            //    foreach (var item in flow.CalculationVariables)
            //    {
            //        item.Value = AllVariables.Find(x => x.VariableName == item.VariableName).Value;
            //    }
            //}

            BlockInstanceStatus = BlockInstanceStatuses.Calculated;
        }
    }
}
