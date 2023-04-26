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
using System.Net.WebSockets;

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
    public class ExpressionClass : INotifyPropertyChanged
    {
        public int Order { get; set; }

        private string _expression;

        public string Expression
        {
            get { return _expression; }
            set
            {
                _expression = value;
                OnPropertyChanged(nameof(Expression));
                ExtractNeededVariables();
            }
        }

        private string _definedVariable;

        public string DefinedVariable
        {
            get { return _definedVariable; }
            set { var old = _definedVariable; _definedVariable = Regex.Replace(value, @"[^a-zA-Z0-9]", ""); OnPropertyChanged(nameof(DefinedVariable)); _context.RegenerateCustomParametres(old, _definedVariable); }
        }

        private string _neededVariables;

        public string NeededVariables
        {
            get { return _neededVariables; }
            set { _neededVariables = value; OnPropertyChanged(nameof(NeededVariables)); }
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

        public void ExtractNeededVariables()
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

    //Класс описания переменных, используемых в вычислениях
    public class CalcVariable : INotifyPropertyChanged
    {
        private string _variable = "";
        public string Variable
        {
            get { return _variable; }
            set { _variable = value; OnPropertyChanged(nameof(Variable)); }
        }

        private double _value = 0;
        public double Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(nameof(Value)); }
        }

        public CalcVariable(string var, double val)
        {
            Variable = var;
            Value = val;
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

        public (string CheckError, string CalcAvailable) StaticStrings = ("В моделе имеются ошибки", "Рассчитать");

        private string _calcButtonText = "";

        public string CalcButtonText
        {
            get { return _calcButtonText; }
            set { _calcButtonText = value; OnPropertyChanged(nameof(CalcButtonText)); }
        }

        private bool _calcButtonAvailable = false;

        public bool CalcButtonAvailable
        {
            get { return _calcButtonAvailable; }
            set { _calcButtonAvailable = value; OnPropertyChanged(nameof(CalcButtonAvailable)); }
        }

        private ObservableCollection<CalcVariable> _filledCalcVariables = new ObservableCollection<CalcVariable>();
        
        [JsonIgnore]
        public ObservableCollection<CalcVariable> FilledCalcVariables
        {
            get { return _filledCalcVariables; }
            set { _filledCalcVariables = value; OnPropertyChanged(nameof(FilledCalcVariables)); }
        }

        private ObservableCollection<CalcVariable> _calcedCalcVariables = new ObservableCollection<CalcVariable>();

        [JsonIgnore]
        public ObservableCollection<CalcVariable> CalcedCalcVariables
        {
            get { return _calcedCalcVariables; }
            set { _calcedCalcVariables = value; OnPropertyChanged(nameof(CalcedCalcVariables)); }
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

            foreach (var ExpressionBlock in Expressions)
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
                    throw new Exception($"Параметр {ExpressionBlock.DefinedVariable}, определяемый выражением №{ExpressionBlock.Order}: {ExpressionBlock.Expression} не может начинаться с цифры");
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
            //if (oldVar == newVar) return;
            var OldFoundVar = ExtraParameters.FirstOrDefault(x => x.Symbol == oldVar);
            var NewFoundVar = ExtraParameters.FirstOrDefault(y => y.Symbol == newVar);
            if (OldFoundVar != null && NewFoundVar == null && newVar!="")
            {
                ExtraParameters.Add(new CustomParametreClass(OldFoundVar.Title,newVar, OldFoundVar.Units));
            }
            RegenerateCustomParametres();
        }

        public void RegenerateCustomParametres()
        {
            List<string> RawVars = Expressions.Select(x => x.DefinedVariable).ToList();
            RawVars = RawVars.Where(rawVar => (InputFlows.FirstOrDefault(flow => flow.FlowParameters.FirstOrDefault(flowParam => flowParam.Variable == rawVar) != null) == null)).ToList();
            RawVars = RawVars.Where(rawVar => (OutputFlows.FirstOrDefault(flow => flow.FlowParameters.FirstOrDefault(flowParam => flowParam.Variable == rawVar) != null) == null)).ToList();
            RawVars = RawVars.Where(rawVar => (DefaultParameters.FirstOrDefault(DefParam => DefParam.Symbol == rawVar) == null)).ToList();

            ExtraParameters = new ObservableCollection<CustomParametreClass>(ExtraParameters.Where(extraParam => RawVars.Contains(extraParam.Symbol)));

            foreach (var var in RawVars)
            {
                if (ExtraParameters.FirstOrDefault(x => x.Symbol == var) == null && var != "")
                {
                    ExtraParameters.Add(new CustomParametreClass("", var, ""));
                }
            }
        }

        public bool CheckBlock(bool silent = false)
        {
            try
            {
                RegenerateCustomParametres();
                CheckFieldsNotEpmty();
                CheckFlowIndexes();
                CheckVarsUsingIsCorrect();
                CheckExpressionsCorrectness();
            }
            catch (Exception ex)
            {
                if (silent == false)
                {
                    MessageBox.Show(ex.Message, "Ошибка описания блока");
                }
                return false;
            }
            return true;
        }

        public bool RegenerateCalcVariables()
        {
            if (CheckBlock(true) == false)
            {
                CalcButtonText = StaticStrings.CheckError;
                CalcButtonAvailable = false;
                return false;
            }
            else
            {
                var ProcessedCalcedVars = new List<string>(Expressions.Select(x => x.DefinedVariable).ToList());
                CalcedCalcVariables = new ObservableCollection<CalcVariable>(CalcedCalcVariables.Where(x => ProcessedCalcedVars.Any(y => x.Variable == y)));

                var ProcessedFilledVars = new List<string>();
                foreach (var expressionBlock in Expressions)
                {
                    var NeededVars = expressionBlock.NeededVariables.Split(" ").ToList();
                    foreach (var variable in NeededVars)
                    {
                        if (variable != "")
                        {
                            if (CalcedCalcVariables.Any(x => x.Variable == variable) == false)
                            {
                                if (FilledCalcVariables.Any(x => x.Variable == variable) == false)
                                {
                                    FilledCalcVariables.Add(new CalcVariable(variable, 0));
                                }
                                ProcessedFilledVars.Add(variable);

                            }
                        }
                    }

                    var defVar = expressionBlock.DefinedVariable;
                    if (CalcedCalcVariables.Any(x => x.Variable == defVar) == false)
                    {
                        CalcedCalcVariables.Add(new CalcVariable(defVar, 0));
                    }
                }

                FilledCalcVariables = new ObservableCollection<CalcVariable>(FilledCalcVariables.Where(x => ProcessedFilledVars.Any(y => x.Variable == y)));

                CalcButtonText = StaticStrings.CalcAvailable;
                CalcButtonAvailable = true;
                return true;
            }
        }

        public void CalculateModel()
        {
            foreach (var expBlock in Expressions)
            {
                Expression e = new Expression(expBlock.Expression);
                e.disableImpliedMultiplicationMode();
                foreach (var arg in expBlock.NeededVariables.Split(" ").ToList())
                {
                    var Var = FilledCalcVariables.FirstOrDefault(x => x.Variable == arg);
                    if (Var != null)
                    {
                    e.defineArgument(Var.Variable, Var.Value);
                    }
                    else
                    {
                        Var = CalcedCalcVariables.FirstOrDefault(x => x.Variable == arg);
                        e.defineArgument(Var.Variable, Var.Value);
                    }
                }
                if (e.checkSyntax() == false)
                {
                    var error = e.getErrorMessage();
                    throw new Exception($"Ошибка при обработке выражения №{expBlock.Order}: {expBlock.Expression}\nОтчёт работы математического ядра:\n{error}");
                }
                else
                {
                    var result = e.calculate();
                    CalcedCalcVariables[CalcedCalcVariables.IndexOf(CalcedCalcVariables.FirstOrDefault(x => x.Variable == expBlock.DefinedVariable))].Value = result;
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
            InitializeComponent();
            // получаем ответ
            BlockModelCreation context = (BlockModelCreation)this.DataContext;
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
            //context.RegenerateCustomParametres();
        }

        private void AddOutputFlow(object sender, RoutedEventArgs e)
        {
            BlockModelCreation context = (BlockModelCreation)this.DataContext;
            FlowClass Str = new FlowClass(context.InputFlows.Count + context.OutputFlows.Count + 1, context);
            context.OutputFlows.Add(Str);
            //context.RegenerateCustomParametres();
        }

        private void SaveBlock(object sender, RoutedEventArgs e)
        {
            BlockModelCreation context = (BlockModelCreation)this.DataContext;
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
            BlockModelCreation context = (BlockModelCreation)this.DataContext;
            if (context.CheckBlock() == true)
            {
                MessageBox.Show("Модель блока корректна", "Проверка блока");
            }
        }

        private void PrepareCalculationsVars(object sender, RoutedEventArgs e)
        {
            BlockModelCreation context = (BlockModelCreation)this.DataContext;
            context.RegenerateCalcVariables();
        }

        private void CalculateModel(object sender, RoutedEventArgs e)
        {
            BlockModelCreation context = (BlockModelCreation)this.DataContext;
            //if (context.RegenerateCalcVariables() == false)
            //{
            //    return;
            //}
            context.CalculateModel();
        }
    }
}
