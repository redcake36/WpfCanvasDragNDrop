using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop.APIClases
{
    //Класс объекта, возвращаемого запросом к серверу на получение доступных сред
    public class APIGetEnvsResponseClass
    {
        public List<APIBaseParametreClass> BaseParametres = new List<APIBaseParametreClass>();
        public List<APIFlowTypeClass> FlowEnvironments = new List<APIFlowTypeClass>();
    }
}
