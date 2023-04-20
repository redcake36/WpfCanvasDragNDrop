using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop
{

    public class Flow
    {
        int FlowEnviroment;
        string FlowVariableIndex;
        int id;

        public Flow(int flowEnviroment, string flowVariableIndex, int id)
        {
            FlowEnviroment = flowEnviroment;
            FlowVariableIndex = flowVariableIndex;
            this.id = id;
        }
        public Flow()
        {
            FlowEnviroment = 0;
            FlowVariableIndex = string.Empty;
            this.id = 0;
        }
    }
}
