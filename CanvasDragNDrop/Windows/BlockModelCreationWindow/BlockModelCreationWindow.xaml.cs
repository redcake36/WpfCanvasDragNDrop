using CanvasDragNDrop.APIClases;
using CanvasDragNDrop.Windows.BlockModelCreationWindow.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CanvasDragNDrop
{
    public partial class BlockModelCreationWindow : Window
    {
        public BlockModelCreationWindow()
        {
            InitializeComponent();
            // получаем типы сред и массив базовых параметров
            BlockModelCreationClass context = (BlockModelCreationClass)this.DataContext;
            var RequestResult = API.GetEnvironments();
            if (RequestResult.isSuccess)
            {
                context.BaseParameters.AddRange(RequestResult.environments.BaseParameters);
                context.FlowTypes.AddRange(RequestResult.environments.FlowEnvironments);
            }
            else
            {
                MessageBox.Show("Не удалось получить данные с сервера");
                this.Close();
            }
            //Получаем массив дирректорий для сохранения новой модели
            var ModelsDirs = API.GetCatalogs();
            if (ModelsDirs.isSuccess)
            {
                context.AvailableDirs = new(ModelsDirs.catalogModels);
                foreach (var dir in context.AvailableDirs)
                {
                    dir.IsModelsVisible = false;
                }
            }
            else
            {
                MessageBox.Show("Не удалось получить данные с сервера");
                this.Close();
            }
        }
        private void AddExpression(object sender, RoutedEventArgs e)
        {
            BlockModelCreationClass context = (BlockModelCreationClass)this.DataContext;
            ExpressionClass Str = new ExpressionClass(context.Expressions.Count + 1, "", "", context.RegenerateCustomParameters);
            context.Expressions.Add(Str);
        }

        private void AddDefaultParameters(object sender, RoutedEventArgs e)
        {
            BlockModelCreationClass context = (BlockModelCreationClass)this.DataContext;
            CustomParametreClass Str = new CustomParametreClass("", "", "", context.RegenerateCustomParameters);
            context.DefaultParameters.Add(Str);
        }

        private void AddInputFlow(object sender, RoutedEventArgs e)
        {
            BlockModelCreationClass context = (BlockModelCreationClass)this.DataContext;
            FlowClass Str = new FlowClass(context.GetLastFlowsIndex() + 1, context.BaseParameters, context.FlowTypes, context.RegenerateCustomParameters);
            context.InputFlows.Add(Str);
        }

        private void AddOutputFlow(object sender, RoutedEventArgs e)
        {
            BlockModelCreationClass context = (BlockModelCreationClass)this.DataContext;
            FlowClass Str = new FlowClass(context.GetLastFlowsIndex() + 1, context.BaseParameters, context.FlowTypes, context.RegenerateCustomParameters);
            context.OutputFlows.Add(Str);
        }

        private void SaveBlock(object sender, RoutedEventArgs e)
        {
            BlockModelCreationClass context = (BlockModelCreationClass)this.DataContext;
            if (context.CheckBlock() == false) { return; }
            if (context.SaveDirId < 0)
            {
                MessageBox.Show("Вы не указали папку для сохранения новой модели (выбор по двойному клику)");
                return;
            }

            var Result = API.CreateBlockModel(context);
            if (Result.isSuccess)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show(Result.response, "Не удалось выполнить запрос");
            }
        }

        private void CheckBlock(object sender, RoutedEventArgs e)
        {
            BlockModelCreationClass context = (BlockModelCreationClass)this.DataContext;
            if (context.CheckBlock() == true)
            {
                MessageBox.Show("Модель блока корректна", "Проверка блока");
            }
        }

        private void PrepareCalculationsVars(object sender, RoutedEventArgs e)
        {
            BlockModelCreationClass context = (BlockModelCreationClass)this.DataContext;
            context.RegenerateCalcVariables();
        }

        private void CalculateModel(object sender, RoutedEventArgs e)
        {
            BlockModelCreationClass context = (BlockModelCreationClass)this.DataContext;
            try
            {
                context.CalculateModel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при расчёте блока");
            }
        }

        private void DeleteExpression(int Order)
        {
            BlockModelCreationClass context = (BlockModelCreationClass)this.DataContext;
            context.Expressions.RemoveAt(Order - 1);
            context.ResortExpressions();
        }

        private void MoveUpExpression(int Order)
        {
            BlockModelCreationClass context = (BlockModelCreationClass)this.DataContext;
            if (Order > 1)
            {
                context.Expressions.Move(Order - 1, Order - 2);
                context.ResortExpressions();
            }
        }

        private void MoveDownExpression(int Order)
        {
            BlockModelCreationClass context = (BlockModelCreationClass)this.DataContext;
            if (Order < context.Expressions.Count)
            {
                context.Expressions.Move(Order - 1, Order);
                context.ResortExpressions();
            }
        }

        private void DeleteImputFlow(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            BlockModelCreationClass context = (BlockModelCreationClass)this.DataContext;
            context.InputFlows.RemoveAt((int)((sender as Image).Tag));
        }

        private void DeleteOutputFlow(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            BlockModelCreationClass context = (BlockModelCreationClass)this.DataContext;
            context.OutputFlows.RemoveAt((int)((sender as Image).Tag));
        }

        private void DeleteDefaultParametre(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            BlockModelCreationClass context = (BlockModelCreationClass)this.DataContext;
            context.DefaultParameters.RemoveAt((int)((sender as Image).Tag));
        }

        private void ChangeSaveDir(object sender, MouseButtonEventArgs e)
        {
            int DirId = (int)((ContentControl)sender).Tag;
            BlockModelCreationClass context = (BlockModelCreationClass)this.DataContext;
            context.SaveDirId = DirId;
        }
    }
}
