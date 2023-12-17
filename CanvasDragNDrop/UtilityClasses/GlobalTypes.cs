using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop.UtilityClasses
{
    public static class GlobalTypes
    {
        public enum ExpressionTypes : int
        {
            Expression,
            PropSI
        }

        /// <summary> Словарь возможных типов выражений </summary>
        public static Dictionary<ExpressionTypes, (string UIName, string PropertyName)> ExpressionTypesNames { get; private set; } = new()
        {
            { ExpressionTypes.Expression,("Формула","Expression") },
            { ExpressionTypes.PropSI, ("Функция PropSI", "PropSI") }
        };
    }
}
