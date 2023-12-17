using CanvasDragNDrop.Windows.MainWindow.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using org.mariuszgromada.math.mxparser;

namespace CanvasDragNDrop.UtilityClasses
{
    internal static class CalcUtilitiesClass
    {
        public static string RemoveWhitespaces(string rawString)
        {
            return Regex.Replace(rawString, @"\s", "");
        }

        // language = regex
        /// <summary> Паттерн проверки строки для вызова функции PropSI </summary>
        public const string PropSICheckPattern = @"^[Pp][Rr][Oo][Pp][Ss][Ii]\([A-Z];[A-Z];(([a-zA-Z][a-zA-Z0-9]*)|([0-9.,]+));[A-Z];(([a-zA-Z][a-zA-Z0-9]*)|([0-9.,]+));\w+\)$";

        /// <summary> Метод вызова функции PropSI по строке пользователя </summary>
        public static double CallPropSIFromString(string request, List<BlockInstanceVariable> values)
        {
            string clearedString = RemoveWhitespaces(request);
            if (!CheckIsPropSICall(clearedString))
            {
                throw new Exception("Invalid PropSI Call");
            }
            List<string> Params = clearedString.Split(")")[0].Split("(")[1].Split(";").ToList();
            string ReturnValueType = Params[0];
            string FirstPassedValueType = Params[1];
            double FirstPassedValue = 1;
            string SecondPassesValueType = Params[3];
            double SecondPassesValue = 1;
            string FlowType = Params[5];

            if (CheckStringIsFiniteDouble(Params[2]))
            {
                FirstPassedValue = double.Parse(Params[2]);
            }
            else
            {
                BlockInstanceVariable FoundVariable = values.First(x => x.VariableName == Params[2]);
                if (FoundVariable == null)
                {
                    throw new Exception("Variable value not found");
                }
                FirstPassedValue = FoundVariable.Value;
            }

            if (CheckStringIsFiniteDouble(Params[4]))
            {
                SecondPassesValue = double.Parse(Params[4]);
            }
            else
            {
                BlockInstanceVariable FoundVariable = values.First(x => x.VariableName == Params[4]);
                if (FoundVariable == null)
                {
                    throw new Exception("Variable value not found");
                }
                SecondPassesValue = FoundVariable.Value;
            }

            try
            {
                return CoolProp.PropsSI(ReturnValueType, FirstPassedValueType, FirstPassedValue, SecondPassesValueType, SecondPassesValue, FlowType);
            }
            catch (Exception ex)
            {
                MessageBox.Show("АХТУНГ", $"PropSI вызов не удался: {ex.Message}");
                return 0;
            }
        }


        public static bool CheckIsPropSICall(string call)
        {
            return Regex.IsMatch(RemoveWhitespaces(call), PropSICheckPattern);
        }

        public static bool CheckStringIsFiniteDouble(string variable)
        {
            return double.TryParse(variable, out double value) && double.IsFinite(value);
        }

        public static org.mariuszgromada.math.mxparser.Expression ConstructMathExpression(string expression, List<BlockInstanceVariable> vars)
        {
            org.mariuszgromada.math.mxparser.Expression ex = new(expression);
            ex.disableImpliedMultiplicationMode();
            ex.defineArguments(vars.Select(x => x.VariableName).ToArray());
            if (ex.checkSyntax() == false)
            {
                MessageBox.Show("АХТУНГ, Создание выражения провалилось");
            }
            return ex;
        }

        public static double calcMathExpression(org.mariuszgromada.math.mxparser.Expression ex, List<BlockInstanceVariable> vars)
        {
            foreach (var var in vars)
            {
                ex.setArgumentValue(var.VariableName, var.Value);
            }
            return ex.calculate();
        }
    }
}
