namespace CanvasDragNDrop.APIClases
{
    /// <summary> Класс для дополнительных и дефолтных параметров модели блока </summary>
    public class APIBlockModelParameterClass
    {
        /// <summary> Id параметра </summary>
        public int ParameterId { get; set; }

        /// <summary> Название параметра </summary>
        public string Title { get; set; }

        /// <summary> Единицы измерения параметра </summary>
        public string Units { get; set; }

        /// <summary> Название параметра </summary>
        public string VariableName { get; set; }

        public APIBlockModelParameterClass(int parameterId, string variableName, string title, string units)
        {
            ParameterId = parameterId;
            VariableName = variableName;
            Title = title;
            Units = units;
        }
    }
}
