using CanvasDragNDrop.Windows.MainWindow.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using Expression = org.mariuszgromada.math.mxparser.Expression;

namespace CanvasDragNDrop.UtilityClasses
{
    /// <summary> Класс подготовленных выражений для максимально быстрого их использования </summary>
    public class PrecompiledCalcExpressionClass
    {
        // language = regex
        /// <summary> Паттерн проверки строки для вызова функции PropSI </summary>
        public const string PropSICheckPattern = @"^[Pp][Rr][Oo][Pp][Ss][Ii]\([A-Z];[A-Z];(([a-zA-Z][a-zA-Z0-9]*)|([0-9.,]+));[A-Z];(([a-zA-Z][a-zA-Z0-9]*)|([0-9.,]+));\w+\)$";

        public enum ExpressionArgumentTypes : int
        {
            Constant,
            Variable
        }

        public GlobalTypes.ExpressionTypes ExpressionType { get; private set; }

        public string PropSIReturnValueSymbol { get; private set; }

        public ExpressionArgumentTypes PropSIFirstPassedValueType { get; private set; }
        public string PropSIFirstPassedValueSymbol { get; private set; }
        public string PropSIFirstPassedValueVariableName { get; private set; }
        public double PropSIFirstPassedValue { get; private set; }

        public ExpressionArgumentTypes PropSISecondPassedValueType { get; private set; }
        public string PropSISecondPassedValueSymbol { get; private set; }
        public string PropSISecondPassedValueVariableName { get; private set; }
        public double PropSISecondPassedValue { get; private set; }

        public string PropSIFlowType { get; private set; }

        public Expression PrecompiledExpression { get; private set; }

        public PrecompiledCalcExpressionClass(string ExpressionString)
        {
            if (CheckIsPropSICall(ExpressionString))
            {
                PreparePropSIExpression(ExpressionString);
                return;
            }
            ExpressionType = GlobalTypes.ExpressionTypes.Expression;
            PrecompiledExpression = ConstructMathExpression(ExpressionString);
        }

        /// <summary> Метод анализа первого парамтера функции PropSI </summary>
        public void AnalizePropSIFirstParam(string FirstParam)
        {
            if (CalcUtilitiesClass.CheckStringIsFiniteDouble(FirstParam))
            {
                PropSIFirstPassedValueType = ExpressionArgumentTypes.Constant;
                PropSIFirstPassedValue = double.Parse(FirstParam);
                return;
            }
            PropSIFirstPassedValueType = ExpressionArgumentTypes.Variable;
            PropSIFirstPassedValueVariableName = FirstParam;
        }

        public void PreparePropSIExpression(string PropSIString)
        {
            // Устнавливаем тип выражения как PropSI тип
            ExpressionType = GlobalTypes.ExpressionTypes.PropSI;

            string clearedString = CalcUtilitiesClass.RemoveWhitespaces(PropSIString);

            //Разделяем строку на параметры
            List<string> Params = clearedString.Split(")")[0].Split("(")[1].Split(";").ToList();

            PropSIReturnValueSymbol = Params[0];
            PropSIFirstPassedValueSymbol = Params[1];
            PropSISecondPassedValueSymbol = Params[3];
            PropSIFlowType = Params[5];

            AnalizePropSIFirstParam(Params[2]);
            AnalizePropSISecondParam(Params[4]);
        }


        /// <summary> Метод анализа второго парамтера функции PropSI </summary>
        public void AnalizePropSISecondParam(string Second)
        {
            if (CalcUtilitiesClass.CheckStringIsFiniteDouble(Second))
            {
                PropSISecondPassedValueType = ExpressionArgumentTypes.Constant;
                PropSISecondPassedValue = double.Parse(Second);
                return;
            }
            PropSISecondPassedValueType = ExpressionArgumentTypes.Variable;
            PropSISecondPassedValueVariableName = Second;
        }

        public double Calc(Dictionary<string, BlockInstanceVariable> variables)
        {
            if (ExpressionType == GlobalTypes.ExpressionTypes.PropSI)
            {
                if (PropSIFirstPassedValueType == ExpressionArgumentTypes.Variable)
                {
                    PropSIFirstPassedValue = variables[PropSIFirstPassedValueVariableName].Value;
                }

                if (PropSISecondPassedValueType == ExpressionArgumentTypes.Variable)
                {
                    PropSISecondPassedValue = variables[PropSISecondPassedValueVariableName].Value;
                }

                return CoolProp.PropsSI(PropSIReturnValueSymbol, PropSIFirstPassedValueSymbol, PropSIFirstPassedValue, PropSISecondPassedValueSymbol, PropSISecondPassedValue, PropSIFlowType);
            }
            else
            {
                return CalcMathExpression(PrecompiledExpression, variables);
            }
        }

        /// <summary> Проверяет что строка соответствует вызову PropSI </summary>
        public static bool CheckIsPropSICall(string call)
        {
            return Regex.IsMatch(CalcUtilitiesClass.RemoveWhitespaces(call), PropSICheckPattern);
        }

        public static Expression ConstructMathExpression(string expression)
        {
            Expression ex = new(expression);
            ex.disableImpliedMultiplicationMode();
            string[] missedVars = ex.getMissingUserDefinedArguments();
            ex.defineArguments(missedVars);
            if (ex.checkSyntax() == false)
            {
                MessageBox.Show("АХТУНГ, Создание выражения провалилось");
            }
            return ex;
        }

        public static double CalcMathExpression(Expression ex, Dictionary<string, BlockInstanceVariable> vars)
        {
            foreach (var var in vars)
            {
                ex.setArgumentValue(var.Key, var.Value.Value);
            }
            return ex.calculate();
        }

    }
}
