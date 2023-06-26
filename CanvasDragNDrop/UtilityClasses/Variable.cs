using CanvasDragNDrop.UserItems;
using Newtonsoft.Json.Linq;
using org.mariuszgromada.math.mxparser.mathcollection;
using System.Xml.Linq;

namespace CanvasDragNDrop
{
    public class Variable
    {
        public int FlowId { get; set; }
        public int FlowVariableId { get; set; }
        public string FlowVariableName { get; set; }
        public VarPrototype VariablePrototype { get; set; }

        public Variable(int flowId, int flowVariableId, string flowVariableName, VarPrototype variablePrototype)
        {
            FlowId = flowId;
            FlowVariableId = flowVariableId;
            FlowVariableName = flowVariableName;
            VariablePrototype = variablePrototype;
        }
        public Variable()
        {
            FlowId = 0;
            FlowVariableId = 0;
            FlowVariableName = "";
            VariablePrototype = new VarPrototype();
        }

        public override string ToString()
        {
            return string.Format("Variable:\nFlowId:{0}\nFlowVariableId:{1}\nFlowVariableName:{2}\nVariablePrototype:{3}\n", FlowId, FlowVariableId, FlowVariableName, VariablePrototype.ToString());
        }
    }
}