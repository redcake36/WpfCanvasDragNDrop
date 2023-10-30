using CanvasDragNDrop.APIClases;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CanvasDragNDrop
{
    /// <summary> Класс описания потока </summary>
    public class FlowClass
    {
        /// <summary> Делегат для обновления дополнительных параметров </summary>
        public delegate void RegenerateCustomParametresHandler();

        private RegenerateCustomParametresHandler regenerateCustomParametres;

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

        private List<APIBaseParametreClass> _baseParameters;
        [JsonIgnore]
        public ObservableCollection<APIFlowTypeClass> FlowTypes { get; set; } = new ObservableCollection<APIFlowTypeClass>();

        [JsonIgnore]
        public ObservableCollection<FlowParametersClass> FlowParameters { get; set; } = new ObservableCollection<FlowParametersClass>();
        public FlowClass(int flowVariableIndex,List<APIBaseParametreClass> baseParametres,List<APIFlowTypeClass> flowTypes, RegenerateCustomParametresHandler handler)
        {
            regenerateCustomParametres = handler;
            _baseParameters = new List<APIBaseParametreClass>(baseParametres);
            FlowTypes = new ObservableCollection<APIFlowTypeClass>(flowTypes);
            _flowVariableIndex = flowVariableIndex;
            FlowEnvironment = FlowTypes[0].FlowEnvironmentId;

        }

        private void ChangeFlowType()
        {
            FlowParameters.Clear();
            foreach (var item in FlowTypes.FirstOrDefault(x => x.FlowEnvironmentId == FlowEnvironment).BaseParametres)
            {
                var param = _baseParameters.Find(x => x.ParameterId == item);
                FlowParameters.Add(new FlowParametersClass($"{param.Title} | {param.Symbol}{FlowVariableIndex}", 1, $"{param.Symbol}{FlowVariableIndex}"));
            }
            regenerateCustomParametres?.Invoke();
        }
    }
}
