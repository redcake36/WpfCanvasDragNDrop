using System.Collections.Generic;

namespace CanvasDragNDrop.APIClases
{
    /// <summary> Класс типа потока </summary>
    public class APIFlowTypeClass
    {
        /// <summary> Название типа потока </summary>
        public string FlowEnvironmentType { get; set; }

        /// <summary> Id типа потока </summary>
        public int FlowEnvironmentId { get; set; }

        /// <summary> Массив идентификаторов базовых параметров, доступных в потоке </summary>
        public List<int> BaseParameters { get; set; }

        public APIFlowTypeClass(int ind, string name, List<int> baseParam)
        {
            BaseParameters = baseParam;
            FlowEnvironmentType = name;
            FlowEnvironmentId = ind;
        }

    }
}
