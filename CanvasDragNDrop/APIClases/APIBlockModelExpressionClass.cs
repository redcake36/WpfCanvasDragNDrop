using org.mariuszgromada.math.mxparser.mathcollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop.APIClases
{
    /// <summary> Класс расчётного выражения модели блока </summary>
    public class APIBlockModelExpressionClass
    {
        /// <summary> Id расчётного выражения </summary>
        public int ExpressionId { get; set; }

        /// <summary> Id определяемой переменной </summary>
        public int DefinedVariableId { get; set; }

        /// <summary> Расчтное выражение </summary>
        public string Expression { get; set; }

        /// <summary> Id необходимых переменных для выражения </summary>
        public List<int> NeededVariables { get; set; }

        /// <summary> Порядковый номер выражения </summary>
        public int Order { get; set; }

        public APIBlockModelExpressionClass(int expressionId, int definedVariableId, string expression, List<int> neededVariables, int order)
        {
            ExpressionId = expressionId;
            DefinedVariableId = definedVariableId;
            Expression = expression;
            NeededVariables = neededVariables;
            Order = order;
        }
    }
}
