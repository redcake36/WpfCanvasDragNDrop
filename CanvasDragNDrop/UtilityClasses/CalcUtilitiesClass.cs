using System.Collections.Generic;
using System.Linq;
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

        public static void NormalizeCycle(List<int> cycle)
        {
            if (cycle == null || cycle.Count == 0)
                return;

            int min = cycle.Min();
            int shift = cycle.FindIndex((x) => x == min);

            if (shift <= 0)
                return;

            // Сохраняем элементы, которые будут перенесены в конец
            List<int> temp = cycle.GetRange(0, shift);

            // Сдвигаем оставшиеся элементы влево
            for (int i = 0; i < cycle.Count - shift; i++)
            {
                cycle[i] = cycle[i + shift];
            }

            // Помещаем сохраненные элементы в конец
            for (int i = 0; i < shift; i++)
            {
                cycle[cycle.Count - shift + i] = temp[i];
            }
        }

        public static bool IsCycleNew(List<List<int>> cyclesList, List<int> cycle)
        {
            foreach (var existingCycle in cyclesList)
            {
                if (existingCycle.Count == cycle.Count)
                {
                    if (existingCycle.SequenceEqual(cycle))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
