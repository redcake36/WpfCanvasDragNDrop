using CanvasDragNDrop.UtilityClasses;
using CanvasDragNDrop.Windows.MainWindow.Classes;
using System.Collections.Generic;

namespace CanvasDragNDrop.APIClases
{
    public class APIBlockInstance : NotifyPropertyChangedClass
    {
        public int BlockInstanceId
        {
            get { return _blockInstanceId; }
            set { _blockInstanceId = value; OnPropertyChanged(); }
        }
        private int _blockInstanceId;

        public double OffsetTop
        {
            get { return _offsetTop; }
            set { _offsetTop = value; OnPropertyChanged(); }
        }
        private double _offsetTop = 0;

        public double OffsetLeft
        {
            get { return _offsetLeft; }
            set { _offsetLeft = value; OnPropertyChanged(); }
        }
        private double _offsetLeft = 0;

        public APIBlockModelVersionClass BlockModel
        {
            get { return _blockModel; }
            set { _blockModel = value; OnPropertyChanged(); }
        }
        private APIBlockModelVersionClass _blockModel;

        /// <summary> Массив значений переменных по умолчанию </summary>
        public List<BlockInstanceVariable> DefaultVariables
        {
            get { return _defaultVariables; }
            set { _defaultVariables = value; OnPropertyChanged(); }
        }
        private List<BlockInstanceVariable> _defaultVariables;




    }
}
