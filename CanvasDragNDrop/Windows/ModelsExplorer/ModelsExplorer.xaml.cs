using CanvasDragNDrop.APIClases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CanvasDragNDrop.Windows.ModelsExplorer
{
    /// <summary>
    /// Interaction logic for ModelsExplorer.xaml
    /// </summary>
    public partial class ModelsExplorer : Window, INotifyPropertyChanged
    {
        public ObservableCollection<APIDirBlockModelClass> AvailableCatalogModels
        {
            get { return _availableCatalogModels; }
            set { _availableCatalogModels = value; OnPropertyChanged(); }
        }
        private ObservableCollection<APIDirBlockModelClass> _availableCatalogModels = new();

        public ModelsExplorer()
        {
            InitializeComponent();

            //AvailableCatalogModels = new ObservableCollection<APIDirBlockModelClass>
            //{
            //    new APIDirBlockModelClass(1, "Node 1", new List<APIBlockModelClass>{ }, new List<APIDirBlockModelClass>{ })
            //    {
            //        CatalogId = 1,
            //        CatalogName = "Node 1",
            //        Models = new List<APIBlockModelClass>{ },
            //        Childs = new List<APIDirBlockModelClass>
            //        {
            //            new APIDirBlockModelClass(2, "Node 1.1", new List<APIBlockModelClass>{ }, new List<APIDirBlockModelClass>{ }){ CatalogId = 2, CatalogName = "Node1.1"},
            //            new APIDirBlockModelClass(3, "Node 1.2", new List<APIBlockModelClass>{ }, new List<APIDirBlockModelClass>{ }){ CatalogId = 3, CatalogName = "Node1.2"},
            //        }
            //    },

            //    new APIDirBlockModelClass(4, "Node 2", new List<APIBlockModelClass>{ }, new List<APIDirBlockModelClass>{ })
            //    {
            //        CatalogId = 4,
            //        CatalogName = "Node 2",
            //        Models = new List<APIBlockModelClass>{ },
            //        Childs = new List<APIDirBlockModelClass>
            //        {
            //            new APIDirBlockModelClass(5, "Node 2.1", new List<APIBlockModelClass>{ }, new List<APIDirBlockModelClass>{ }){ CatalogId = 5, CatalogName = "Node2.1"},
            //            new APIDirBlockModelClass(6, "Node 2.2", new List<APIBlockModelClass>{ }, new List<APIDirBlockModelClass>{ }){ CatalogId = 6, CatalogName = "Node2.2"},
            //        }
            //    }
            //};

            //DataContext = this;
            GetFromServerCatalogList();
        }

       

        void GetFromServerCatalogList()
        {
            var GettingCatalogModelsResult = API.GetCatalogs();
            if (GettingCatalogModelsResult.isSuccess)
            {
                AvailableCatalogModels = new ObservableCollection<APIDirBlockModelClass>(GettingCatalogModelsResult.catalogModels);
            }
            else
            {
                MessageBox.Show("Не удалось получить данные с сервера");
                return;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void FolderDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Folder selected" + ((ContentControl)sender).Tag.ToString());
            e.Handled = true;
        }

        private void ModelDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Folder selected" + ((ContentControl)sender).Tag.ToString());
            e.Handled = true;
        }
    }
}
