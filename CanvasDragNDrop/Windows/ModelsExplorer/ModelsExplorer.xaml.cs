﻿using CanvasDragNDrop.APIClases;
using CanvasDragNDrop.Windows.ModelsExplorer.Classes;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;



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
        public DropAndTargetId dropAndTargetId
        {
            get { return _modelAndCatalogId; }
            set { _modelAndCatalogId = value; }
        }
        private DropAndTargetId _modelAndCatalogId = new();

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

        public delegate void ModelVersionSelected(int ModelId);

        public ModelVersionSelected ModelVersionSelectedHandler;

        private void ModelVersionDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            int ModelId = (int)((ContentControl)sender).Tag;
            ModelVersionSelectedHandler?.Invoke(ModelId);
        }

        private void Image_MouseLeftButtonDownDeleteModel(object sender, MouseButtonEventArgs e)
        {
            var idProperty = ((Image)sender).DataContext.GetType().GetProperty("ModelId");
            dropAndTargetId.DropId = (int)idProperty.GetValue(((Image)sender).DataContext);
            dropAndTargetId.TargetId = 0;
            API.DeleteBlockModel(dropAndTargetId);
        }
        private void Image_MouseLeftButtonDownDeleteCatalog(object sender, MouseButtonEventArgs e)
        {
            var idProperty = ((Image)sender).DataContext.GetType().GetProperty("CatalogId");
            dropAndTargetId.TargetId = (int)idProperty.GetValue(((Image)sender).DataContext);
            dropAndTargetId.DropId = 0;
            API.DeleteBlockModel(dropAndTargetId);
        }

        private void Image_MouseLeftButtonDownAddCatalog(object sender, MouseButtonEventArgs e)
        {
            var idProperty = ((Image)sender).DataContext.GetType().GetProperty("CatalogId");
            dropAndTargetId.TargetId = (int)idProperty.GetValue(((Image)sender).DataContext);
            dropAndTargetId.DropId = 0;
            API.AddNewCatalog(dropAndTargetId);
        }

        private void MyTreeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }


        //FIX переписать нормально
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


        //FIX переписать нормально
        private void MyTreeView_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(TreeViewItem)))
            {
                return;
            }
            TreeViewItem droppedItem = (TreeViewItem)e.Data.GetData(typeof(TreeViewItem));
            var idProperty = droppedItem.DataContext.GetType().GetProperty("ModelId");
            if (idProperty == null)
            {
                idProperty = droppedItem.DataContext.GetType().GetProperty("CatalogId");
            }
            dropAndTargetId.DropId = (int)idProperty.GetValue(droppedItem.DataContext);
            TreeViewItem targetItem = FindAncestor<TreeViewItem>((DependencyObject)e.OriginalSource);
            var idProperty1 = targetItem.DataContext.GetType().GetProperty("CatalogId");
            dropAndTargetId.TargetId = (int)idProperty1.GetValue(targetItem.DataContext);
            API.MovingBlockModel(dropAndTargetId);
        }

        //FIX дропнуть
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

        private void MyTreeView_EditModelVersion(object sender, MouseButtonEventArgs e)
        {
            int VersionId = (int)(sender as Image).Tag;
            MessageBox.Show(VersionId.ToString());
            var requestedModelVersion = API.GetModelVersion(VersionId);
            if (requestedModelVersion.isSuccess == false)
            {
                MessageBox.Show("Ошибка на сервере");
                return;
            }
            BlockModelCreationWindow.BlockModelCreationWindow editigWindow = new(requestedModelVersion.blockModelVersion);
            editigWindow.ShowDialog();
            GetFromServerCatalogList();
        }
    }
}
