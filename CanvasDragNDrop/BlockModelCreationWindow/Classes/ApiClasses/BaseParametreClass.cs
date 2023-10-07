using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop
{
    //Класс базовых параметров
    public class BaseParametreClass
    {
        public int ParameterId { get; set; } //Id базового параметра
        public string Title { get; set; } //Название параметра
        public string Symbol { get; set; } //Символ обозначение параметра
        public string Units { get; set; } //Единицы измерения
        public BaseParametreClass(int parametreId, string title, string symbol, string units)
        {
            ParameterId = parametreId;
            Title = title;
            Symbol = symbol;
            Units = units;
        }
    }
}
