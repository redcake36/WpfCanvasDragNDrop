using org.mariuszgromada.math.mxparser;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CanvasDragNDrop
{
    /// <summary> Класс описания расчётного выражения </summary>
    public class ExpressionClass : NotifyPropertyChangedClass
    {
        /// <summary> Делегат метода обновления дополнительных переменных </summary>
        public delegate void RegenerateCustomParametresHandler(string oldVar, string newVar);

        private RegenerateCustomParametresHandler regenerateCustomParametres;

        /// <summary> Порядковый номер расчётного выражения в общем списке </summary>
        public int Order
        {
            get { return _order; } 
            set
            {
                _order = value; 
                OnPropertyChanged();
            }
        }
        private int _order;

        /// <summary> Расчётное выражение </summary>
        public string Expression
        {
            get { return _expression; }
            set
            {
                _expression = value;
                OnPropertyChanged();
                ExtractNeededVariables();
            }
        }
        private string _expression;

        /// <summary> Определяемая выражением переменная </summary>
        public string DefinedVariable
        {
            get { return _definedVariable; }
            set { var old = _definedVariable; _definedVariable = Regex.Replace(value, @"[^a-zA-Z0-9]", ""); OnPropertyChanged(); regenerateCustomParametres(old, _definedVariable); }
        }
        private string _definedVariable;

        /// <summary> Строка с необходимыми переменными </summary>
        public string NeededVariables
        {
            get { return _neededVariables; }
            set { _neededVariables = value; OnPropertyChanged(); }
        }
        private string _neededVariables;


        public ExpressionClass(int order, string expression, string definedVariable, string neededVariables, RegenerateCustomParametresHandler handler)
        {
            regenerateCustomParametres = handler;
            Order = order;
            Expression = expression;
            DefinedVariable = definedVariable;
            NeededVariables = neededVariables;

        }

        /// <summary> Метод получения из выражения необходимых перменных и обновления поля необходимых переменных </summary>
        public void ExtractNeededVariables()
        {
            if (_expression != "")
            {
                Expression exp = new Expression(_expression);
                exp.disableImpliedMultiplicationMode();
                string needvars = "";
                List<string> varsInExp = exp.getMissingUserDefinedArguments().ToList();
                foreach (var item in varsInExp)
                {
                    needvars += item + " ";
                }
                NeededVariables = needvars.Trim();
            }
            else
            {
                NeededVariables = "";
            }
        }
    }
}
