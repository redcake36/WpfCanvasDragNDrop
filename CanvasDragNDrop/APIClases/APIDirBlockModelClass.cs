using CanvasDragNDrop.UtilityClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CanvasDragNDrop.APIClases
{
    /// <summary> Класс каталога моделей блока </summary>
    public class APIDirBlockModelClass : NotifyPropertyChangedClass
    {
        /// <summary> ID каталога блока </summary>
        public int CatalogId
        {
            get { return _catalogId; }
            set
            { _catalogId = value; OnPropertyChanged(nameof(CatalogId)); }
        }
        private int _catalogId;

        /// <summary> Название каталога блока </summary>
        public string CatalogName
        {
            get { return _catalogName; }
            set
            { _catalogName = value; OnPropertyChanged(nameof(CatalogName)); }
        }
        private string _catalogName;

        /// <summary> Массив моделей блока </summary>
        public List<APIBlockModelClass> Models;

        public ObservableCollection<APIDirBlockModelClass> Childs
        {
            get { return _childs; }
            set { _childs = value; OnPropertyChanged(); }
        }
        private ObservableCollection<APIDirBlockModelClass> _childs = new();


        public APIDirBlockModelClass(int catalogId, string catalogName, List<APIBlockModelClass> models, ObservableCollection<APIDirBlockModelClass> childs)
            {
               CatalogId = catalogId;
               CatalogName = catalogName;
               Models = models;
               Childs = childs;
            }

    }
}
