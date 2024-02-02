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

        public delegate void ModelSelected(int ModelId);

        public ModelSelected ModelSelectedHandler;

        private void ModelDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            int ModelId = (int)((ContentControl)sender).Tag;
            ModelSelectedHandler?.Invoke(ModelId);
        }
    }
}
