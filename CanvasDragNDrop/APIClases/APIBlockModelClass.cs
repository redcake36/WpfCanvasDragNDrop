using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CanvasDragNDrop.UtilityClasses;

namespace CanvasDragNDrop.APIClases
{
    /// <summary> Класс модели блока </summary>
    public class APIBlockModelClass : NotifyPropertyChangedClass
    {
        /// <summary> ID модели блока </summary>
        public int ModelId
        {
            get { return _modelId; }
            set { _modelId = value; OnPropertyChanged(); }
        }
        private int _modelId;

        /// <summary> Название модели блока </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }
        private string _title;

        /// <summary> Описание модели блока </summary>
        public string Description;

        /// <summary> Массив входных потоков </summary>
        public List<APIBlockModelFlowClass> InputFlows;

        /// <summary> Массив выходных потоков </summary>
        public List<APIBlockModelFlowClass> OutputFlows;

        /// <summary> Массив параметров по умолчанию </summary>
        public List<APIBlockModelParameterClass> DefaultParameters;

        /// <summary> Массив дополнительных параметров </summary>
        public List<APIBlockModelParameterClass> CustomParameters;

        /// <summary> Массив расчётных выражений </summary>
        public List<APIBlockModelExpressionClass> Expressions;

        public APIBlockModelClass(int modelID, string title, string description,List<APIBlockModelFlowClass> inputFlows, List<APIBlockModelFlowClass> outputFlows, List<APIBlockModelParameterClass> customParameters, List<APIBlockModelParameterClass> defaultParameters, List<APIBlockModelExpressionClass> expressions)
        {
            ModelId = modelID;
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
