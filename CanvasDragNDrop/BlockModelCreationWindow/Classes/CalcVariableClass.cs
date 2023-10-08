using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop
{
    //Класс описания переменных, используемых в вычислениях
    public class CalcVariableClass : NotifyPropertyChangedClass
    {
        private string _variable = "";
        public string Variable
        {
            get { return _variable; }
            set { _variable = value; OnPropertyChanged(); }
        }

        private double _value = 0;
        public double Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(); }
        }

        public CalcVariableClass(string var, double val)
        {
            Variable = var;
            Value = val;
        }
    }
}
