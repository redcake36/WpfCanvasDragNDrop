using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop
{
    //Класс типа потока
    public class FlowTypeClass
    {
        public string FlowEnvironmentType { get; set; } //Название типа потока
        public int FlowEnviromentId { get; set; } //Id типа потока
        public List<int> BaseParametres { get; set; } //Массив идентификаторов базовых параметров, доступных в потоке
        public FlowTypeClass(int Ind, string Name, List<int> baseParam)
        {
            BaseParametres = baseParam;
            FlowEnvironmentType = Name;
            FlowEnviromentId = Ind;
        }

    }
}
