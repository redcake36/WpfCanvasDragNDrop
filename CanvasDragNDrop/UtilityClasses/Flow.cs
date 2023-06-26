using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CanvasDragNDrop
{

    public class Flow
    {
        public List<Variable> AvailableVariables { get; set; }
        public string FlowVariablesIndex { get; set; }
        public int FlowId { get; set; }



        public Flow(List<Variable> availableVariables, string flowVariablesIndex, int flowId)
        {
            AvailableVariables = availableVariables;
            FlowVariablesIndex = flowVariablesIndex;
            FlowId = flowId;
        }
        public Flow()
        {
            AvailableVariables = new List<Variable>();
            FlowVariablesIndex = string.Empty;
            FlowId = 0;
        }

        public override string ToString()
        {
            return string.Format("Flow:\n " +
                "AvailableVariables:{0}\n " +
                "FlowId:{1}\n " +
                "FlowVariablesIndex:{2}\n ", string.Join("\n", AvailableVariables), FlowId, FlowVariablesIndex);
        }
    }
}
