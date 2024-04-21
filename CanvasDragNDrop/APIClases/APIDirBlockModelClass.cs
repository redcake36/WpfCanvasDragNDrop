using CanvasDragNDrop.UtilityClasses;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
        public ObservableCollection<APIBlockModelClass> Models
        {
            get { return _models; }
            set { _models = value; OnPropertyChanged(); OnPropertyChanged(nameof(Items)); }
        }
        private ObservableCollection<APIBlockModelClass> _models;

        public ObservableCollection<APIDirBlockModelClass> Children
        {
            get { return _childs; }
            set { _childs = value; OnPropertyChanged(); OnPropertyChanged(nameof(Items)); }
        }
        private ObservableCollection<APIDirBlockModelClass> _childs = new();

        public bool IsModelsVisible
        {
            get { return _isModelsVisible; }
            set { _isModelsVisible = value; setModelsVisibility(value); OnPropertyChanged(); OnPropertyChanged(nameof(Items)); }
        }
        private bool _isModelsVisible = true;


        public IEnumerable<object> Items
        {
            get
            {
                foreach (var group in Children)
                    yield return group;
                if (IsModelsVisible == true)
                {
                    foreach (var entry in Models)
                        yield return entry;
                }
            }
        }


        public APIDirBlockModelClass(int catalogId, string catalogName, ObservableCollection<APIBlockModelClass> models, ObservableCollection<APIDirBlockModelClass> childs)
        {
            CatalogId = catalogId;
            CatalogName = catalogName;
            Models = models;
            Children = childs;
        }

        private void setModelsVisibility(bool visibility)
        {
            foreach (var child in Children)
            {
                child.IsModelsVisible = visibility;
            }
        }

    }
}
