using CanvasDragNDrop.APIClases;
using CanvasDragNDrop.Windows.ModelsExplorer.Classes;
using Newtonsoft.Json.Linq;
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

        private bool isDragging = false;
        private Point startPoint;
        public ModelAndCatalogId modelAndCatalogId
        {
            get { return _modelAndCatalogId; }
            set { _modelAndCatalogId = value; }
        }
        private ModelAndCatalogId _modelAndCatalogId = new();

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
                AvailableCatalogModels = new ObservableCollection<APIDirBlockModelClass>(GettingCatalogModelsResult.catalogModels.Children);
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

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int ModelId = (int)((ContentControl)sender).Tag;
            ModelSelectedHandler?.Invoke(ModelId);
        }

        private void MyTreeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void MyTreeView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point mousePos = e.GetPosition(null);
                Vector diff = startPoint - mousePos;

                if (!isDragging && (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
                {
                    TreeViewItem item = FindAncestor<TreeViewItem>((DependencyObject)e.OriginalSource);

                    if (item != null)
                    {
                        // Здесь мы передаем данные о перетаскиваемом элементе в объект DataObject
                        DataObject data = new DataObject(typeof(TreeViewItem), item);
                        DragDrop.DoDragDrop(item, data, DragDropEffects.Move);
                        //isDragging = true;
                    }
                }
            }
        }

        private void MyTreeView_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(TreeViewItem)))
            {
                return;
            }

            TreeViewItem droppedItem = (TreeViewItem)e.Data.GetData(typeof(TreeViewItem));
            var idProperty = droppedItem.DataContext.GetType().GetProperty("ModelId");
            modelAndCatalogId.ModelId = (int)idProperty.GetValue(droppedItem.DataContext);
            TreeViewItem targetItem = FindAncestor<TreeViewItem>((DependencyObject)e.OriginalSource);
            var idProperty1 = targetItem.DataContext.GetType().GetProperty("CatalogId");
            modelAndCatalogId.CatalogId = (int)idProperty1.GetValue(targetItem.DataContext);
            API.MovingBlockModel(modelAndCatalogId);
            //if (droppedItem != null && targetItem != null && !ReferenceEquals(droppedItem, targetItem))
            //{
            //    if (droppedItem.Parent is TreeView)
            //    {
            //        ((TreeView)droppedItem.Parent).Items.Remove(droppedItem);
            //    }
            //    else if (droppedItem.Parent is TreeViewItem parentItem)
            //    {
            //        parentItem.Items.Remove(droppedItem);
            //    }

            //    if (targetItem.Items.Count == 0 || targetItem.IsExpanded)
            //    {
            //        targetItem.Items.Add(droppedItem);
            //    }
            //    else
            //    {
            //        targetItem.IsExpanded = true;
            //        targetItem.Items.Add(droppedItem);
            //        targetItem.IsExpanded = false;
            //    }
            //}
        }

        private static T FindAncestor<T>(DependencyObject current)
            where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }

                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);

            return null;
        }
    }
}
