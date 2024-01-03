using CanvasDragNDrop.UtilityClasses;
using System;
using System.Collections.Generic;
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

        //public APIDirBlockModelClass()
        //{
        //    Models = new List<APIBlockModelClass>();
        //}

        public List<APIDirBlockModelClass> DirModel;

        public APIDirBlockModelClass(int catalogId, string catalogName, List<APIBlockModelClass> models, List<APIDirBlockModelClass> dirModel)
            {
               CatalogId = catalogId;
               CatalogName = catalogName;
               Models = models;
               DirModel = dirModel;
            }

    }
}
