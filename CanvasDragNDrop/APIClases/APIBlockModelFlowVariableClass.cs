namespace CanvasDragNDrop.APIClases
{
    /// <summary> Класс переменной потока модели блока </summary>
    public class APIBlockModelFlowVariableClass
    {
        /// <summary> Id потока </summary>
        public int FlowId { get; set; }

        /// <summary> Id переменной потока </summary>
        public int FlowVariableId { get; set; }

        /// <summary> Имя переменной потока с индексом </summary>
        public string FlowVariableName { get; set; }

        /// <summary> Базовый параметр переменной </summary>
        public APIBaseParameterClass VariablePrototype { get; set; }

        public APIBlockModelFlowVariableClass(int flowId, int flowVariableId, string flowVariableName, APIBaseParameterClass variablePrototype)
        {
            FlowId = flowId;
            FlowVariableId = flowVariableId;
            FlowVariableName = flowVariableName;
            VariablePrototype = variablePrototype;
        }
    }
}