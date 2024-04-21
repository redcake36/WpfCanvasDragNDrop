using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CanvasDragNDrop.UtilityClasses;
using Newtonsoft.Json;

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

        /// <summary> Массив версий модели </summary>
        public ObservableCollection<APIBlockModelVersionClass> Versions
        {
            get { return _versions; }
            set { _versions = value; OnPropertyChanged(); }
        }
        private ObservableCollection<APIBlockModelVersionClass> _versions;

        public APIBlockModelClass(int modelID, string title, string description, List<APIBlockModelFlowClass> inputFlows, List<APIBlockModelFlowClass> outputFlows, List<APIBlockModelParameterClass> customParameters, List<APIBlockModelParameterClass> defaultParameters, List<APIBlockModelExpressionClass> expressions)
        {
            ModelId = modelID;
            Title = title;

        }
    }
}
