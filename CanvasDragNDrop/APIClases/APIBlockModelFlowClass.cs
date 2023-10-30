using Newtonsoft.Json;
using System.Collections.Generic;

namespace CanvasDragNDrop.APIClases
{
    /// <summary> Класс потока модели блока </summary>
    public class APIBlockModelFlowClass
    {
        /// <summary> Id потока модели блока </summary>
        public int FlowId { get; set; }

        /// <summary> Id типа среды потока </summary>
        public int EnvironmentId { get; set; }

        /// <summary> Индекс переменных потока </summary>
        public string FlowVariablesIndex { get; set; }

        /// <summary> Доступные переменны потока (для входных переменных) </summary>
        public List<APIBlockModelFlowVariableClass> AvailableVariables { get; set; } = new();

        /// <summary> Необходимое число определяемых переменных (для выходных потоков) </summary>
        public int CountOfMustBeDefinedVars { get; set; } = 0;

        /// <summary> Определяемые перемнные потока (для выходных потоков) </summary>
        public List<APIBlockModelFlowVariableClass> RequiredVariables { get; set; } = new();

        [JsonConstructor]
        /// <summary> Конструктор для входных потков </summary>
        public APIBlockModelFlowClass(int flowId, int environmentId, string flowVariablesIndex, List<APIBlockModelFlowVariableClass> availableVariables)
        {
            FlowId = flowId;
            EnvironmentId = environmentId;
            FlowVariablesIndex = flowVariablesIndex;
            AvailableVariables = availableVariables;
        }

        /// <summary> Конструктор для выходных потоков </summary>
        public APIBlockModelFlowClass(int flowId, int environmentId, string flowVariablesIndex, List<APIBlockModelFlowVariableClass> requiredVariables, int countOfMustBeDefinedVars)
        {
            FlowId = flowId;
            EnvironmentId = environmentId;
            FlowVariablesIndex = flowVariablesIndex;
            AvailableVariables = requiredVariables;
            CountOfMustBeDefinedVars = countOfMustBeDefinedVars;
        }
    }
}
