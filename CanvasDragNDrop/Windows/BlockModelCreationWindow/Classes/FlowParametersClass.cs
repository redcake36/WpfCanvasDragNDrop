using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop.Windows.BlockModelCreationWindow.Classes
{
    /// <summary> Класс параметров конкретного потока </summary>
    public class FlowParametersClass
    {
        /// <summary>Полное наименование параметра</summary>
        public string Parameter { get; set; }

        /// <summary> Наименоване переменной </summary>
        public string Variable { get; set; }

        public FlowParametersClass(string p, int pId, string Var)
        {
            Parameter = p;
            Variable = Var;
        }
    }

}
