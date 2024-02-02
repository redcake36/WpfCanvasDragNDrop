using CanvasDragNDrop.APIClases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private void FolderDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            FolderSelected?.Invoke(sender, e);
            e.Handled = true;
        }

        private void ModelDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            ModelSelected?.Invoke(sender, e);
            e.Handled = true;
        }
    }
}
