using CanvasDragNDrop.UtilityClasses;
using CanvasDragNDrop.Windows.BlockModelCreationWindow.Classes;
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

        /// <summary> Расчтное выражение </summary>
        public string Expression { get; set; }

        /// <summary> Id необходимых переменных для выражения </summary>
        public List<int> NeededVariables { get; set; }

        /// <summary> Порядковый номер выражения </summary>
        public int Order { get; set; }

        //FIX list to int
        /// <summary> ID определеяемой выражением переменной </summary>
        public int DefinedVariable {  get; set; }

        /// <summary> Тип выражения (формула, обращение к CoolProp) </summary>
        public GlobalTypes.ExpressionTypes ExpressionType { get; set; }

        public APIBlockModelExpressionClass(int expressionId, string expression, List<int> neededVariables, int order, int definedVariable)
        {
            ExpressionId = expressionId;
            Expression = expression;
            NeededVariables = neededVariables;
            Order = order;
            DefinedVariable = definedVariable;
        }
    }
}
