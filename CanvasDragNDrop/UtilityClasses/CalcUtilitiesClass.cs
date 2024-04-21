using System.Text.RegularExpressions;

namespace CanvasDragNDrop.UtilityClasses
{
    internal static class CalcUtilitiesClass
    {
        /// <summary> Метод удаления всех пробельных символов из строки </summary>
        public static string RemoveWhitespaces(string rawString)
        {
            return Regex.Replace(rawString, @"\s", "");
        }

        public static bool CheckStringIsFiniteDouble(string variable)
        {
            return double.TryParse(variable, out double value) && double.IsFinite(value);
        }
    }
}
