using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop
{
    public class BlockExpression
    {
        int DefinedVariableId;
        string Expression;
        int NeededVariables;
        int Order;
        int id;
        public BlockExpression()
        {
            DefinedVariableId = 0;
            Expression = String.Empty;
            NeededVariables = 0;
            Order = 0;
            id = 0;
        }

        public BlockExpression(int definedVariableId, string expression, int neededVariables, int order, int id)
        {
            DefinedVariableId = definedVariableId;
            Expression = expression;
            NeededVariables = neededVariables;
            Order = order;
            this.id = id;
        }
    }
}
