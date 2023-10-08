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

        private int _flowEnviroment;
        public int FlowEnviroment
        {
            get { return _flowEnviroment; }
            set { _flowEnviroment = value; ChangeFlowType(); }
        }

        private List<BaseParametreClass> _baseParameters;
        [JsonIgnore]
        public ObservableCollection<FlowTypeClass> FlowTypes { get; set; } = new ObservableCollection<FlowTypeClass>();

        [JsonIgnore]
        public ObservableCollection<FlowParametersClass> FlowParameters { get; set; } = new ObservableCollection<FlowParametersClass>();
        public FlowClass(int flowVariableIndex,List<BaseParametreClass> baseParametres,List<FlowTypeClass> flowTypes, RegenerateCustomParametresHandler handler)
        {
            regenerateCustomParametres = handler;
            _baseParameters = new List<BaseParametreClass>(baseParametres);
            FlowTypes = new ObservableCollection<FlowTypeClass>(flowTypes);
            _flowVariableIndex = flowVariableIndex;
            FlowEnviroment = FlowTypes[0].FlowEnviromentId;

        }

        private void ChangeFlowType()
        {
            FlowParameters.Clear();
            foreach (var item in FlowTypes.FirstOrDefault(x => x.FlowEnviromentId == FlowEnviroment).BaseParametres)
            {
                var param = _baseParameters.Find(x => x.ParameterId == item);
                FlowParameters.Add(new FlowParametersClass($"{param.Title} | {param.Symbol}{FlowVariableIndex}", 1, $"{param.Symbol}{FlowVariableIndex}"));
            }
            regenerateCustomParametres?.Invoke();
        }
    }
}
