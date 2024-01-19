using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop.APIClases
{
    /// <summary> Класс объекта, возвращаемого запросом к серверу на получение доступных сред </summary>
    public class APIGetEnvsResponseClass
    {
        /// <summary> Массив базовых параметров, используемых в типах сред </summary>
        public List<APIBaseParameterClass> BaseParameters = new List<APIBaseParameterClass>();

        /// <summary> Массив типов сред </summary>
        public List<APIFlowTypeClass> FlowEnvironments = new List<APIFlowTypeClass>();
    }
}
