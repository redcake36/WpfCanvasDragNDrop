using CanvasDragNDrop.APIClases;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CanvasDragNDrop.Windows.ModelsExplorer.UserItems
{
    /// <summary>
    /// Interaction logic for ModelsTreeControl.xaml
    /// </summary>
    public partial class ModelsTreeControl : UserControl
    {
        public ModelsTreeControl()
        {
            InitializeComponent();
        }

        public ObservableCollection<APIDirBlockModelClass> AvailableCatalogModels
        {
            get { return (ObservableCollection<APIDirBlockModelClass>)GetValue(AvailableCatalogModelsProperty); }
            set { SetValue(AvailableCatalogModelsProperty, value); }
        }
        public static readonly DependencyProperty AvailableCatalogModelsProperty =
            DependencyProperty.Register(nameof(AvailableCatalogModels), typeof(ObservableCollection<APIDirBlockModelClass>), typeof(ModelsTreeControl));


        public event MouseButtonEventHandler FolderSelected;
        public event MouseButtonEventHandler ModelSelected;
        public event MouseButtonEventHandler ModelDelete;
        public event MouseButtonEventHandler CatalogDelete;
        public event MouseButtonEventHandler CatalogAdd;

        private void FolderDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            FolderSelected?.Invoke(sender, e);
            e.Handled = true;
        }

        private void VersionDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            ModelSelected?.Invoke(sender, e);
            e.Handled = true;
        }

        private void Image_MouseLeftButtonDownDeleteModel(object sender, MouseButtonEventArgs e)
        {
            ModelDelete?.Invoke(sender, e);
            e.Handled = true;
        }

        private void Image_MouseLeftButtonDownDeleteCatalog(object sender, MouseButtonEventArgs e)
        {
            CatalogDelete?.Invoke(sender, e);
            e.Handled = true;
        }

        private void Image_MouseLeftButtonDownAddCatalog(object sender, MouseButtonEventArgs e)
        {
            CatalogAdd?.Invoke(sender, e);
            e.Handled = true;
        }
    }
}
