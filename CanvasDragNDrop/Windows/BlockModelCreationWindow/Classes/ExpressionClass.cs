using CanvasDragNDrop.UtilityClasses;
using Newtonsoft.Json;
using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace CanvasDragNDrop.Windows.BlockModelCreationWindow.Classes
{
    /// <summary> Класс описания расчётного выражения </summary>
    public class ExpressionClass : NotifyPropertyChangedClass
    {
        /// <summary> Делегат метода обновления дополнительных переменных </summary>
        public delegate void RegenerateCustomParametersHandler(string oldVar, string newVar);

        private RegenerateCustomParametersHandler regenerateCustomParameters;

        [JsonIgnore]
        /// <summary> Отображаемый тип расчётного выражения </summary>
        public string UIExpressionType
        {
            get => _UIExpressionType;
            set { _UIExpressionType = value; OnPropertyChanged(); }
        }
        private string _UIExpressionType;

        public string ExpressionTypeName
        {
            get { return _expressionTypeName; }
            set { _expressionTypeName = value; OnPropertyChanged(); }
        }
        private string _expressionTypeName;

        public GlobalTypes.ExpressionTypes ExpressionType
        {
            get { return _expressionType; }
            set { _expressionType = value; UIExpressionType = GlobalTypes.ExpressionTypesNames[value].UIName; ExpressionTypeName = GlobalTypes.ExpressionTypesNames[value].PropertyName; }
        }
        private GlobalTypes.ExpressionTypes _expressionType;


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
            set { var old = _definedVariable; _definedVariable = Regex.Replace(value, @"[^a-zA-Z0-9]", ""); OnPropertyChanged(); regenerateCustomParameters(old, _definedVariable); }
        }
        private string _definedVariable;
        
        /// <summary> Массив строк необходимых переменных </summary>
        public ObservableCollection<string> NeededVariables
        {
            get { return _neededVariables; }
            set { _neededVariables = value; OnPropertyChanged(); }
        }
        private ObservableCollection<string> _neededVariables = new();


        public ExpressionClass(int order, string expression, string definedVariable, RegenerateCustomParametersHandler handler)
        {
            regenerateCustomParameters = handler;
            Order = order;
            Expression = expression;
            DefinedVariable = definedVariable;
            ExpressionType = GlobalTypes.ExpressionTypes.Expression;
            ExtractNeededVariables();
        }

        /// <summary> Метод получения из выражения необходимых перменных и обновления поля необходимых переменных </summary>
        public void ExtractNeededVariables()
        {
            string ClearedExpression = CalcUtilitiesClass.RemoveWhitespaces(_expression);
            NeededVariables.Clear();
            if (ClearedExpression == "")
            {
                return;
            }

            if (Regex.IsMatch(ClearedExpression, PrecompiledCalcExpressionClass.PropSICheckPattern))
            {
                ExpressionType = GlobalTypes.ExpressionTypes.PropSI;
                List<string> Params = ClearedExpression.Split(';').ToList();
                if (!CalcUtilitiesClass.CheckStringIsFiniteDouble(Params[2]))
                {
                    NeededVariables.Add(Params[2]);
                }
                if (!CalcUtilitiesClass.CheckStringIsFiniteDouble(Params[4]))
                {
                    NeededVariables.Add(Params[4]);
                }
            }
            else
            {
                ExpressionType = GlobalTypes.ExpressionTypes.Expression;
                Expression exp = new Expression(_expression);
                exp.disableImpliedMultiplicationMode();
                List<string> varsInExp = exp.getMissingUserDefinedArguments().ToList();
                foreach (var item in varsInExp)
                {
                    NeededVariables.Add(item);
                }
            }
        }
    }
}
