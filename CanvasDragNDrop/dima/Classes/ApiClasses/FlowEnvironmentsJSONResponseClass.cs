using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop
{
    //Класс объекта, возвращаемого запрсом к серверу на получение доступных сред
    public class FlowEnvironmentsJSONResponseClass
    {
        public List<BaseParametreClass> BaseParametres = new List<BaseParametreClass>();
        public List<FlowTypeClass> FlowEnvironments = new List<FlowTypeClass>();
    }
}
