using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CanvasDragNDrop
{
    public partial class BlockModelCreationWindow : Window
    {
        public BlockModelCreationWindow()
        {
            HttpClient httpClient = new HttpClient();
            InitializeComponent();
            // получаем ответ
            BlockModelCreationClass context = (BlockModelCreationClass)this.DataContext;
            try
            {
                var response = httpClient.GetStringAsync(RootUrl.rootServer + "/get_envs");
                response.Wait();
                FlowEnvironmentsJSONResponseClass Envs = JsonConvert.DeserializeObject<FlowEnvironmentsJSONResponseClass>(response.Result);
                context.BaseParameters.AddRange(Envs.BaseParametres);
                context.FlowTypes.AddRange(Envs.FlowEnvironments);
            }
            catch (Exception e)
            {
                MessageBox.Show("Не удалось получить данные с сервера");
                if (RootUrl.AutomotiveWork)
                {
                    context.BaseParameters.Add(new BaseParametreClass(1, "Массовая энтальпия", "h", "-"));
                    context.BaseParameters.Add(new BaseParametreClass(2, "Температура", "T", "-"));
                    context.BaseParameters.Add(new BaseParametreClass(3, "Давление", "p", "-"));
                    context.BaseParameters.Add(new BaseParametreClass(4, "Тепловая мощность", "Q", "-"));

                    context.FlowTypes.Add(new FlowTypeClass(1, "Нормальная", new List<int>() { 1, 2, 3 }));
                    context.FlowTypes.Add(new FlowTypeClass(2, "Влажный пар", new List<int>() { 1, 3, 4 }));
                }
                else
                {
                    this.Close();
                }
            }
        }
        private void AddExpression(object sender, RoutedEventArgs e)
        {
            BlockModelCreationClass context = (BlockModelCreationClass)this.DataContext;
            ExpressionClass Str = new ExpressionClass(context.Expressions.Count + 1, "", "", "", context.RegenerateCustomParametres);
            context.Expressions.Add(Str);
        }

        private void AddDefaultParametres(object sender, RoutedEventArgs e)
        {
            BlockModelCreationClass context = (BlockModelCreationClass)this.DataContext;
            CustomParametreClass Str = new CustomParametreClass("", "", "", context.RegenerateCustomParametres);
            context.DefaultParameters.Add(Str);
        }

        private void AddInputFlow(object sender, RoutedEventArgs e)
        {
            BlockModelCreationClass context = (BlockModelCreationClass)this.DataContext;
            FlowClass Str = new FlowClass(context.GetLastFlowsIndex() + 1, context.BaseParameters,context.FlowTypes,context.RegenerateCustomParametres);
            context.InputFlows.Add(Str);
        }

        private void AddOutputFlow(object sender, RoutedEventArgs e)
        {
            BlockModelCreationClass context = (BlockModelCreationClass)this.DataContext;
            FlowClass Str = new FlowClass(context.GetLastFlowsIndex() + 1, context.BaseParameters, context.FlowTypes, context.RegenerateCustomParametres);
            context.OutputFlows.Add(Str);
        }

        private void SaveBlock(object sender, RoutedEventArgs e)
        {
            BlockModelCreationClass context = (BlockModelCreationClass)this.DataContext;
            if (context.CheckBlock() == false) { return; }

            JObject rss = JObject.FromObject(context);
            JArray expressions = (JArray)rss["Expressions"];
            foreach (JObject expression in expressions)
            {
                string needed = (string)expression["NeededVariables"];
                expression["NeededVariables"] = new JArray(needed.Split(" ").ToArray());
            }

            HttpClient httpClient = new HttpClient();
            try
            {
                var JSON = JsonConvert.SerializeObject(rss);
                var request = new StringContent(JSON, Encoding.Unicode, "application/json");
                //MessageBox.Show(JSON);
                var response = httpClient.PostAsync($"{RootUrl.rootServer}/create_model", request);
                response.Wait();
                if (!response.Result.IsSuccessStatusCode)
                {
                    throw new Exception("Не удалось установить соединение с сервером");
                }
                this.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Не удалось выполнить запрос");

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
            context.CalculateModel();
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
    }
}
