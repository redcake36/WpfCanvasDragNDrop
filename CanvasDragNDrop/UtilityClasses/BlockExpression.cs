using org.mariuszgromada.math.mxparser.mathcollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop
{
    public class BlockExpression
    {
        public int DefinedVariableId { get; set; }
        public string Expression { get; set; }
        public int ExpressionId { get; set; }
        public int NeededVariables { get; set; }
        public int Order { get; set; }

        public BlockExpression()
        {
            DefinedVariableId = 0;
            Expression = string.Empty;
            ExpressionId = 0;
            NeededVariables = 0;
            Order = 0;
        }

        public BlockExpression(int definedVariableId, string expression, int ExpressionId, int neededVariables, int order)
        {
            DefinedVariableId = definedVariableId;
            Expression = expression;
            this.ExpressionId = ExpressionId;
            NeededVariables = neededVariables;
            Order = order;
        }

        public override string ToString()
        {
            return string.Format("BlockExpression:\n " +
                "DefinedVariableId:{0}\n " +
                "Expression:{1}\n " +
                "ExpressionId:{2}\n " +
                "NeededVariables:{3}\n " +
                "Order:{4}\n", DefinedVariableId, Expression, ExpressionId, NeededVariables, Order);
        }
    }
}
