using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop.APIClases
{
    /// <summary> Класс базовых параметров </summary>
    public class APIBaseParameterClass
    {
        /// <summary> Id базового параметра </summary>
        public int ParameterId { get; set; }

        /// <summary> Название параметра </summary>
        public string Title { get; set; }

        /// <summary> Символ обозначение параметра </summary>
        public string Symbol { get; set; }

        /// <summary> Единицы измерения </summary>
        public string Units { get; set; }

        public APIBaseParameterClass(int parametreId, string title, string symbol, string units)
        {
            ParameterId = parametreId;
            Title = title;
            Symbol = symbol;
            Units = units;
        }
    }
}
