using CanvasDragNDrop.APIClases;
using CanvasDragNDrop.UtilityClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace CanvasDragNDrop.Windows.ModelsExplorer.Classes
{
    public class BlockModelClass: NotifyPropertyChangedClass
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

        public BlockModelClass(int modelID, string title)
        {
            ModelId = modelID;
            Title = title;
        }
    }
}
