using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Printing.IndexedProperties;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
using static CanvasDragNDrop.BlockModelCreation;
using org.mariuszgromada.math.mxparser;
using Expression = org.mariuszgromada.math.mxparser.Expression;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.DirectoryServices.ActiveDirectory;

namespace CanvasDragNDrop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    //Класс объекта, возвращаемого запрсом к серверу на получение доступных сред
    public class FlowEnvironmentsJSONResponseClass
    {
        public List<BaseParametreClass> BaseParametres = new List<BaseParametreClass>();
        public List<FlowTypeClass> FlowEnvironments = new List<FlowTypeClass>();
    }

    //Класс базовых параметров
    public class BaseParametreClass
    {
        public int ParameterId { get; set; } //Id базового параметра
        public string Title { get; set; } //Название параметра
        public string Symbol { get; set; } //Символ обозначение параметра
        public string Units { get; set; } //Единицы измерения
        public BaseParametreClass(int parametreId, string title, string symbol, string units)
        {
            ParameterId = parametreId;
            Title = title;
            Symbol = symbol;
            Units = units;
        }
    }

    //Класс дополнительных параметров
    public class CustomParametreClass : INotifyPropertyChanged
    {
        private BlockModelCreation _context;

        public string Title { get; set; }

        private string symbol;
        public string Symbol
        {
            get => symbol;
            set { symbol = Regex.Replace(value, @"[^a-zA-Z0-9]", ""); OnPropertyChanged(nameof(Symbol)); if (_context != null) _context.RegenerateCustomParametres(); }
        }

        public string Units { get; set; }
        public CustomParametreClass(string title, string symbol, string units, BlockModelCreation context = null)
        {
            _context = context;
            Title = title;
            Symbol = symbol;
            Units = units;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    //Класс типа потока
    public class FlowTypeClass
    {
        public string FlowEnvironmentType { get; set; } //Название типа потока
        public int FlowEnviromentId { get; set; } //Id типа потока
        public List<int> BaseParametres { get; set; } //Массив идентификаторов базовых параметров, доступных в потоке
        public FlowTypeClass(int Ind, string Name, List<int> baseParam)
        {
            BaseParametres = baseParam;
            FlowEnvironmentType = Name;
            FlowEnviromentId = Ind;
        }

    }

    //Класс параметров конкретного потока
    public class FlowParametersClass
    {
        public string Parameter { get; set; } //Полное наименование параметра
        public string Variable { get; set; } //Наименоване переменной
                                             //public int ParameterId { get; set; }
        public FlowParametersClass(string p, int pId, string Var)
        {
            Parameter = p;
            Variable = Var;
            //ParameterId = pId;
        }
    }

    //Класс описания потока
    public class FlowClass
    {
        private BlockModelCreation _context;

        private int _flowVariableIndex;
        public int FlowVariableIndex
        {
            get { return _flowVariableIndex; }
            set { _flowVariableIndex = value; ChangeFlowType(); }
        }

        private int _flowEnviroment;
        public int FlowEnviroment
        {
            get { return _flowEnviroment; }
            set { _flowEnviroment = value; ChangeFlowType(); }
        }

        private List<BaseParametreClass> _baseParameters;
        [JsonIgnore]
        public ObservableCollection<FlowTypeClass> FlowTypes { get; set; } = new ObservableCollection<FlowTypeClass>();

        [JsonIgnore]
        public ObservableCollection<FlowParametersClass> FlowParameters { get; set; } = new ObservableCollection<FlowParametersClass>();
        public FlowClass(int flowVariableIndex, BlockModelCreation context)
        {
            _context = context;
            _baseParameters = context.BaseParameters;
            FlowTypes = new ObservableCollection<FlowTypeClass>(context.FlowTypes);
            _flowVariableIndex = flowVariableIndex;
            FlowEnviroment = FlowTypes[0].FlowEnviromentId;

        }

        private void ChangeFlowType()
        {
            FlowParameters.Clear();
            foreach (var item in FlowTypes.FirstOrDefault(x => x.FlowEnviromentId == FlowEnviroment).BaseParametres)
            {
                var param = _baseParameters.Find(x => x.ParameterId == item);
                FlowParameters.Add(new FlowParametersClass($"{param.Title} | {param.Symbol}{FlowVariableIndex}", 1, $"{param.Symbol}{FlowVariableIndex}"));
            }
            _context.RegenerateCustomParametres();
        }
    }

    //Класс описания выражения
    public class ExpressionClass: INotifyPropertyChanged
    {
        public int Order { get; set; }

        private string _expression;

        public string Expression
        {
            get { return _expression; }
            set
            {
                _expression = value;
                ExtractNeededVariables();
                OnPropertyChanged(nameof(NeededVariables));
            }
        }

        private string _definedVariable;

        public string DefinedVariable
        {
            get { return _definedVariable; }
            set { var old = _definedVariable; _definedVariable = Regex.Replace(value, @"[^a-zA-Z0-9]", ""); _context.RegenerateCustomParametres(old, _definedVariable); }
        }

        private string _neededVariables;

        public string NeededVariables
        {
            get { return _neededVariables; }
            set { _neededVariables = value; }
        }

        private BlockModelCreation _context;

        public ExpressionClass(int order, string expression, string definedVariable, string neededVariables, BlockModelCreation context)
        {
            _context = context;
            Order = order;
            Expression = expression;
            DefinedVariable = definedVariable;
            NeededVariables = neededVariables;

        }

        public void ExtractNeededVariables ()
        {
            if (_expression != "")
            {
                Expression exp = new Expression(_expression);
                exp.disableImpliedMultiplicationMode();
                string needvars = "";
                List<string> varsInExp = exp.getMissingUserDefinedArguments().ToList();
                foreach (var item in varsInExp)
                {
                    needvars += item + " ";
                }
                NeededVariables = needvars.Trim();
            }
            else
            {
                NeededVariables = "";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class BlockModelCreation : INotifyPropertyChanged
    {

        // Массив базовых параметров
        [JsonIgnore]
        public List<BaseParametreClass> BaseParameters { get; set; } = new List<BaseParametreClass>();

        // Массив доступных сред
        [JsonIgnore]
        public List<FlowTypeClass> FlowTypes { get; set; } = new List<FlowTypeClass> { };

        //Название
        private string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }

        //Описание
        private string _description = string.Empty;
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        //
        private ObservableCollection<FlowClass> _inputFlows = new ObservableCollection<FlowClass> { };
        public ObservableCollection<FlowClass> InputFlows
        {
            get { return _inputFlows; }
            set { _inputFlows = value; OnPropertyChanged(nameof(InputFlows)); }
        }


        private ObservableCollection<FlowClass> _outputFlows = new ObservableCollection<FlowClass> { };
        public ObservableCollection<FlowClass> OutputFlows
        {
            get { return _outputFlows; }
            set { _outputFlows = value; OnPropertyChanged(nameof(OutputFlows)); }
        }


        private ObservableCollection<CustomParametreClass> _defaultParameters = new ObservableCollection<CustomParametreClass> { };
        public ObservableCollection<CustomParametreClass> DefaultParameters
        {
            get { return _defaultParameters; }
            set { _defaultParameters = value; OnPropertyChanged(nameof(DefaultParameters)); }
        }

        private ObservableCollection<CustomParametreClass> _extraParameters = new ObservableCollection<CustomParametreClass> { };
        public ObservableCollection<CustomParametreClass> ExtraParameters
        {
            get => _extraParameters;
            set { _extraParameters = value; OnPropertyChanged(nameof(ExtraParameters)); }
        }

        private ObservableCollection<ExpressionClass> _expressions = new ObservableCollection<ExpressionClass> { };
        public ObservableCollection<ExpressionClass> Expressions
        {
            get { return _expressions; }
            set { _expressions = value; OnPropertyChanged("Expressions"); }
        }

        public BlockModelCreation()
        {

        }

        public void CheckFlowIndexes()
        {
            var Indexes = new List<int>();
            foreach (var item in InputFlows)
            {
                if ((Indexes.IndexOf(item.FlowVariableIndex)) == -1)
                {
                    Indexes.Add(item.FlowVariableIndex);
                }
                else
                {
                    throw new Exception("Индексы потоков должны быть уникальными для всех потоков");
                }
            }
            foreach (var item in OutputFlows)
            {
                if ((Indexes.IndexOf(item.FlowVariableIndex)) == -1)
                {
                    Indexes.Add(item.FlowVariableIndex);
                }
                else
                {
                    throw new Exception("Индексы потоков должны быть уникальными для всех потоков");
                }
            }
        }

        public void CheckFieldsNotEpmty()
        {
            if (Title == "") { throw new Exception("Название блока не может быть пустым"); }
            if (Description == "") { throw new Exception("Описание блока не может быть пустым"); }
            foreach (var item in DefaultParameters)
            {
                if (item.Symbol == "" || item.Title == "" || item.Units == "") { throw new Exception("Все поля переменных по умолчанию должны быть заполнены"); }
            }
            foreach (var item in Expressions)
            {
                if (item.Expression == "" || item.DefinedVariable == "") { throw new Exception("Поля расчётное выражеие и определяемая переменная должны быть заполнены"); }
            }
        }

        public void CheckVarsUsingIsCorrect()
        {
            if (InputFlows.Count == 0 && OutputFlows.Count == 0)
            {
                throw new Exception("Вы не добавили ни одного входного или выходного потока");
            }

            List<string> ReadonlyVars = new List<string>();
            foreach (var item in InputFlows)
            {
                foreach (var var in item.FlowParameters)
                {
                    ReadonlyVars.Add(var.Variable);
                }
            }

            List<string> WritableVars = new List<string>();
            foreach (var item in OutputFlows)
            {
                foreach (var var in item.FlowParameters)
                {
                    WritableVars.Add(var.Variable);
                }
            }

            foreach (var item in DefaultParameters) //Проверка параметров по умолчанию
            {
                if (Regex.IsMatch(item.Symbol, @"^[A-Za-z][A-Za-z0-9]*") == false)
                {
                    throw new Exception($"Параметр по умолчанию {item.Symbol} не может начинаться с цифры");
                };
                if (ReadonlyVars.Contains(item.Symbol))
                {
                    throw new Exception($"Параметр по умолчанию {item.Symbol} совпадает с параметром из входного потока");
                }
                if (WritableVars.Contains(item.Symbol))
                {
                    throw new Exception($"Параметр по умолчанию {item.Symbol} совпадает с параметром из выходного потока");
                }
                ReadonlyVars.Add(item.Symbol);
            }

            List<string> DefinedWritableVars = new List<string>();

            foreach(var ExpressionBlock in Expressions)
            {
                foreach (var needed in ExpressionBlock.NeededVariables.Split(" ").ToList())
                {
                    if (ReadonlyVars.Contains(needed) == false && DefinedWritableVars.Contains(needed) == false && needed != "")
                    {
                        throw new Exception($"Параметр {needed}, используемый в выражении №{ExpressionBlock.Order}: {ExpressionBlock.Expression} не определён");
                    }
                }
                if (Regex.IsMatch(ExpressionBlock.DefinedVariable, @"^[A-Za-z][A-Za-z0-9]*") == false)
                {
                    throw new Exception($"Параметр { ExpressionBlock.DefinedVariable }, определяемый выражением №{ ExpressionBlock.Order}: { ExpressionBlock.Expression} не может начинаться с цифры");
                }
                if (ReadonlyVars.Contains(ExpressionBlock.DefinedVariable) == true)
                {
                    throw new Exception($"Параметр {ExpressionBlock.DefinedVariable}, определяемый выражением №{ExpressionBlock.Order}: {ExpressionBlock.Expression} доступен только для чтения");
                }
                DefinedWritableVars.Add(ExpressionBlock.DefinedVariable);
            }

            foreach (var OutFlow in OutputFlows)
            {
                int count = 0;
                foreach (var var in OutFlow.FlowParameters)
                {
                    if (DefinedWritableVars.Contains(var.Variable))
                    {
                        count++;
                    }
                }
                if (count < 2)
                {
                    throw new Exception($"В выходном потоке №{OutFlow.FlowVariableIndex} определено менее двух параметров");
                }
            }

        }

        public void CheckExpressionsCorrectness()
        {
            foreach (var expBlock in Expressions)
            {
                Expression e = new Expression(expBlock.Expression);
                e.disableImpliedMultiplicationMode();
                foreach (var arg in expBlock.NeededVariables.Split(" ").ToList())
                {
                    e.defineArgument(arg, 10);
                }
                if (e.checkSyntax() == false)
                {
                    var error = e.getErrorMessage();
                    throw new Exception($"Ошибка при обработке выражения №{expBlock.Order}: {expBlock.Expression}\nОтчёт работы математического ядра:\n{error}");
                }
            }
        }


        public void RegenerateCustomParametres(string oldVar, string newVar)
        {
            var FoundVar = ExtraParameters.FirstOrDefault(x => x.Symbol == oldVar);
            bool remove = false;
            if (newVar == "") remove = true;
            if (InputFlows.FirstOrDefault(x => x.FlowParameters.FirstOrDefault(y => y.Variable == newVar) != null) != null) remove = true;
            if (OutputFlows.FirstOrDefault(x => x.FlowParameters.FirstOrDefault(y => y.Variable == newVar) != null) != null) remove = true;
            if (DefaultParameters.FirstOrDefault(x => x.Symbol == newVar) != null) remove = true;

            if (remove == true)
            {
                if (FoundVar != null)
                {
                    ExtraParameters.Remove(FoundVar);
                }
            }
            else
            {
                if (FoundVar != null)
                {
                    FoundVar.Symbol = newVar;
                }
                else
                {
                    ExtraParameters.Add(new CustomParametreClass("", newVar, ""));
                }
            }
        }

        public void RegenerateCustomParametres()
        {
            foreach (var item in Expressions)
            {
                string checkingVar = item.DefinedVariable;
                if (checkingVar != "")
                {
                    bool remove = false;
                    if (InputFlows.FirstOrDefault(x => x.FlowParameters.FirstOrDefault(y => y.Variable == checkingVar) != null) != null) remove = true;
                    if (OutputFlows.FirstOrDefault(x => x.FlowParameters.FirstOrDefault(y => y.Variable == checkingVar) != null) != null) remove = true;
                    if (DefaultParameters.FirstOrDefault(x => x.Symbol == checkingVar) != null) remove = true;

                    var FoundCustom = ExtraParameters.FirstOrDefault(x => x.Symbol == checkingVar);
                    if (remove == true)
                    {
                        if (FoundCustom != null)
                        {
                            ExtraParameters.Remove(FoundCustom);
                        }
                    }
                    else
                    {
                        if (FoundCustom == null)
                        {
                            ExtraParameters.Add(new CustomParametreClass("", checkingVar, ""));
                        }
                    }
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public partial class MainWindowD : Window
    {
        public MainWindowD()
        {
            HttpClient httpClient = new HttpClient();
            var Domain = "https://1245-95-220-40-200.ngrok-free.app";
            InitializeComponent();
            // получаем ответ
                BlockModelCreation context = (BlockModelCreation)this.DataContext;
            try
            {
                var response = httpClient.GetStringAsync($"{Domain}/get_envs");
                response.Wait();
                FlowEnvironmentsJSONResponseClass Envs = JsonConvert.DeserializeObject<FlowEnvironmentsJSONResponseClass>(response.Result);
                context.BaseParameters.AddRange(Envs.BaseParametres);
                context.FlowTypes.AddRange(Envs.FlowEnvironments);
            }
            catch (Exception e)
            {
                MessageBox.Show("Не удалось получить данные с сервера");
                var Dev = false;
                if (Dev)
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
            BlockModelCreation context = (BlockModelCreation)this.DataContext;
            ExpressionClass Str = new ExpressionClass(context.Expressions.Count + 1, "", "", "", context);
            context.Expressions.Add(Str);
        }

        private void AddDefaultParametres(object sender, RoutedEventArgs e)
        {
            BlockModelCreation context = (BlockModelCreation)this.DataContext;
            CustomParametreClass Str = new CustomParametreClass("", "", "", context);
            context.DefaultParameters.Add(Str);
        }

        private void AddInputFlow(object sender, RoutedEventArgs e)
        {
            BlockModelCreation context = (BlockModelCreation)this.DataContext;
            FlowClass Str = new FlowClass(context.InputFlows.Count + context.OutputFlows.Count + 1, context);
            context.InputFlows.Add(Str);
            context.RegenerateCustomParametres();
        }

        private void AddOutputFlow(object sender, RoutedEventArgs e)
        {
            BlockModelCreation context = (BlockModelCreation)this.DataContext;
            FlowClass Str = new FlowClass(context.InputFlows.Count + context.OutputFlows.Count + 1, context);
            context.OutputFlows.Add(Str);
            context.RegenerateCustomParametres();
        }

        private void SaveBlock(object sender, RoutedEventArgs e)
        {
            BlockModelCreation context = (BlockModelCreation)this.DataContext;
            try
            {
                context.RegenerateCustomParametres();
                context.CheckFieldsNotEpmty();
                context.CheckFlowIndexes();
                context.CheckVarsUsingIsCorrect();
                context.CheckExpressionsCorrectness();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка описания блока"); return; }

            JObject rss = JObject.FromObject(context);
            JArray expressions = (JArray)rss["Expressions"];
            foreach (JObject expression in expressions)
            {
                string needed = (string)expression["NeededVariables"];
                expression["NeededVariables"] = new JArray(needed.Split(" ").ToArray());
            }

            HttpClient httpClient = new HttpClient();
            var Domain = "https://1245-95-220-40-200.ngrok-free.app";
            try
            {
                var JSON = JsonConvert.SerializeObject(rss);
                var request = new StringContent(JSON, Encoding.Unicode, "application/json");
                var response = httpClient.PostAsync($"{Domain}/create_model",request);
                response.Wait();
                this.Close();
            }
            catch (Exception err)
            {
            MessageBox.Show(err.Message,"Не удалось выполнить запрос");

            }
        }
    }
}
