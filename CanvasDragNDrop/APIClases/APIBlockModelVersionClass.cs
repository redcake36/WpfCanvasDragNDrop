using CanvasDragNDrop.UtilityClasses;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CanvasDragNDrop.APIClases
{
    /// <summary> Класс модели блока </summary>
    public class APIBlockModelVersionClass : NotifyPropertyChangedClass
    {
        /// <summary> ID модели блока </summary>
        public int ModelId
        {
            get { return _modelId; }
            set { _modelId = value; OnPropertyChanged(); }
        }
        private int _modelId;

        /// <summary> ID версии модели блока </summary>
        public int VersionId
        {
            get { return _versionId; }
            set { _versionId = value; OnPropertyChanged(); }
        }
        private int _versionId;

        /// <summary> Название модели блока </summary>
        public string NoteText
        {
            get { return _noteText; }
            set { _noteText = value; OnPropertyChanged(); }
        }
        private string _noteText;

        /// <summary> Название модели блока </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }
        private string _title;

        [JsonIgnore]
        /// <summary> Описание модели блока </summary>
        public string Description;

        [JsonIgnore]
        /// <summary> Массив входных потоков </summary>
        public List<APIBlockModelFlowClass> InputFlows;

        [JsonIgnore]
        /// <summary> Массив выходных потоков </summary>
        public List<APIBlockModelFlowClass> OutputFlows;

        [JsonIgnore]
        /// <summary> Массив параметров по умолчанию </summary>
        public List<APIBlockModelParameterClass> DefaultParameters;

        [JsonIgnore]
        /// <summary> Массив дополнительных параметров </summary>
        public List<APIBlockModelParameterClass> CustomParameters;

        [JsonIgnore]
        /// <summary> Массив расчётных выражений </summary>
        public List<APIBlockModelExpressionClass> Expressions;

        public APIBlockModelVersionClass(int modelID, int versionID, string title, string noteText, string description, List<APIBlockModelFlowClass> inputFlows, List<APIBlockModelFlowClass> outputFlows, List<APIBlockModelParameterClass> customParameters, List<APIBlockModelParameterClass> defaultParameters, List<APIBlockModelExpressionClass> expressions)
        {
            ModelId = modelID;
            VersionId = versionID;
            NoteText = noteText;
            Title = title;
            Description = description;
            InputFlows = inputFlows;
            OutputFlows = outputFlows;
            CustomParameters = customParameters;
            DefaultParameters = defaultParameters;
            Expressions = expressions;
        }
    }
}
