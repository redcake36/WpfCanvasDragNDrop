using CanvasDragNDrop.APIClases;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CanvasDragNDrop.Windows.BlockModelCreationWindow.Classes
{
    /// <summary> Класс описания потока </summary>
    public class FlowClass
    {
        /// <summary> Делегат для обновления дополнительных параметров </summary>
        public delegate void RegenerateCustomParametersHandler();

        private RegenerateCustomParametersHandler regenerateCustomParameters;

        private int _flowVariableIndex;
        public int FlowVariableIndex
        {
            get { return _flowVariableIndex; }
            set { _flowVariableIndex = value; ChangeFlowType(); }
        }

        private int _flowEnvironment;
        public int FlowEnvironment
        {
            get { return _flowEnvironment; }
            set { _flowEnvironment = value; ChangeFlowType(); }
        }

        private List<APIBaseParameterClass> _baseParameters;
        [JsonIgnore]
        public ObservableCollection<APIFlowTypeClass> FlowTypes { get; set; } = new ObservableCollection<APIFlowTypeClass>();

        [JsonIgnore]
        public ObservableCollection<FlowParametersClass> FlowParameters { get; set; } = new ObservableCollection<FlowParametersClass>();
        public FlowClass(int flowVariableIndex, List<APIBaseParameterClass> baseParameters, List<APIFlowTypeClass> flowTypes, RegenerateCustomParametersHandler handler)
        {
            regenerateCustomParameters = handler;
            _baseParameters = new List<APIBaseParameterClass>(baseParameters);
            FlowTypes = new ObservableCollection<APIFlowTypeClass>(flowTypes);
            _flowVariableIndex = flowVariableIndex;
            FlowEnvironment = FlowTypes[0].FlowEnvironmentId;

        }

        private void ChangeFlowType()
        {
            FlowParameters.Clear();
            foreach (var item in FlowTypes.FirstOrDefault(x => x.FlowEnvironmentId == FlowEnvironment).BaseParameters)
            {
                var param = _baseParameters.Find(x => x.ParameterId == item);
                FlowParameters.Add(new FlowParametersClass($"{param.Title}, {param.Units} | {param.Symbol}{FlowVariableIndex}", 1, $"{param.Symbol}{FlowVariableIndex}"));
            }
            regenerateCustomParameters?.Invoke();
        }
    }
}
