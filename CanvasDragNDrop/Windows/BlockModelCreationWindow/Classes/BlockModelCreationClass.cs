using CanvasDragNDrop.APIClases;
using CanvasDragNDrop.UtilityClasses;
using CanvasDragNDrop.Windows.MainWindow.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using Expression = org.mariuszgromada.math.mxparser.Expression;

namespace CanvasDragNDrop.Windows.BlockModelCreationWindow.Classes
{
    public class BlockModelCreationClass : NotifyPropertyChangedClass
    {
        private bool _isReadOnly;
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value; OnPropertyChanged(nameof(IsReadOnly)); }
        }
        /// <summary>Массив базовых параметров</summary>
        [JsonIgnore]
        public List<APIBaseParameterClass> BaseParameters { get; set; } = new List<APIBaseParameterClass>();

        /// <summary>Массив доступных сред</summary>
        [JsonIgnore]
        public List<APIFlowTypeClass> FlowTypes { get; set; } = new List<APIFlowTypeClass> { };

        /// <summary> Название модели </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }
        private string _title = string.Empty;

        /// <summary> Описание модели </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }
        private string _description = string.Empty;

        /// <summary> Массив входных потоков </summary>
        public ObservableCollection<FlowClass> InputFlows
        {
            get { return _inputFlows; }
            set { _inputFlows = value; OnPropertyChanged(); }
        }
        private ObservableCollection<FlowClass> _inputFlows = new ObservableCollection<FlowClass> { };

        /// <summary> Массив выходных потоков </summary>
        public ObservableCollection<FlowClass> OutputFlows
        {
            get { return _outputFlows; }
            set { _outputFlows = value; OnPropertyChanged(); }
        }
        private ObservableCollection<FlowClass> _outputFlows = new ObservableCollection<FlowClass> { };

        /// <summary> Массив параметров по умолчанию </summary>
        public ObservableCollection<CustomParametreClass> DefaultParameters
        {
            get { return _defaultParameters; }
            set { _defaultParameters = value; OnPropertyChanged(); }
        }
        private ObservableCollection<CustomParametreClass> _defaultParameters = new ObservableCollection<CustomParametreClass> { };

        /// <summary> Массив дополнительных параметров </summary>
        public ObservableCollection<CustomParametreClass> ExtraParameters
        {
            get => _extraParameters;
            set { _extraParameters = value; OnPropertyChanged(); }
        }
        private ObservableCollection<CustomParametreClass> _extraParameters = new ObservableCollection<CustomParametreClass> { };

        /// <summary> Массив выражений </summary>
        public ObservableCollection<ExpressionClass> Expressions
        {
            get { return _expressions; }
            set { _expressions = value; OnPropertyChanged(); }
        }
        private ObservableCollection<ExpressionClass> _expressions = new ObservableCollection<ExpressionClass> { };

        /// <summary> Кортеж надписей на кнопке для расчёта </summary>
        [JsonIgnore]
        public (string CheckError, string CalcAvailable) StaticStrings = ("В моделе имеются ошибки", "Рассчитать");

        /// <summary> Текст на кнопке расчёта </summary>
        [JsonIgnore]
        public string CalcButtonText
        {
            get { return _calcButtonText; }
            set { _calcButtonText = value; OnPropertyChanged(); }
        }
        private string _calcButtonText = "";


        /// <summary> Свойство возможности расчёта </summary>
        [JsonIgnore]
        public bool CalcButtonAvailable
        {
            get { return _calcButtonAvailable; }
            set { _calcButtonAvailable = value; OnPropertyChanged(); }
        }
        private bool _calcButtonAvailable = false;

        /// <summary> Массив вводимых переменных для расчёта </summary>
        [JsonIgnore]
        public ObservableCollection<CalcVariableClass> FilledCalcVariables
        {
            get { return _filledCalcVariables; }
            set { _filledCalcVariables = value; OnPropertyChanged(); }
        }
        private ObservableCollection<CalcVariableClass> _filledCalcVariables = new ObservableCollection<CalcVariableClass>();

        /// <summary> Массив рассчитанных переменных </summary>
        [JsonIgnore]
        public ObservableCollection<CalcVariableClass> CalcedCalcVariables
        {
            get { return _calcedCalcVariables; }
            set { _calcedCalcVariables = value; OnPropertyChanged(); }
        }
        private ObservableCollection<CalcVariableClass> _calcedCalcVariables = new ObservableCollection<CalcVariableClass>();

        /// <summary> Массив дирректорий для выбора места сохранения новой модели </summary>
        [JsonIgnore]
        public ObservableCollection<APIDirBlockModelClass> AvailableDirs
        {
            get { return _availableDirs; }
            set { _availableDirs = value; OnPropertyChanged(); }
        }
        private ObservableCollection<APIDirBlockModelClass> _availableDirs;

        /// <summary> Id директории для сохранения новой модели </summary>
        public int SaveDirId
        {
            get { return _saveDirId; }
            set { _saveDirId = value; }
        }
        private int _saveDirId = -1;



        public BlockModelCreationClass()
        {
            _inputFlows.CollectionChanged += new NotifyCollectionChangedEventHandler(RegenerateCustomParameters);
            _outputFlows.CollectionChanged += new NotifyCollectionChangedEventHandler(RegenerateCustomParameters);
            _expressions.CollectionChanged += new NotifyCollectionChangedEventHandler(RegenerateCustomParameters);
        }

        /// <summary> Метод проверки индексов потоков на уникальность </summary>
        public void CheckFlowIndexes()
        {
            var Indexes = new List<int>();
            foreach (var item in InputFlows)
            {
                if (Indexes.IndexOf(item.FlowVariableIndex) == -1)
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
                if (Indexes.IndexOf(item.FlowVariableIndex) == -1)
                {
                    Indexes.Add(item.FlowVariableIndex);
                }
                else
                {
                    throw new Exception("Индексы потоков должны быть уникальными для всех потоков");
                }
            }
        }

        /// <summary> Метод проверки заполненности полей блока </summary>
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

        /// <summary> Метод проверки корректности использования переменных в расчёте </summary>
        public void CheckVarsUsingIsCorrect()
        {
            //Проверка, есть ли хоть один входной или выходной поток
            if (InputFlows.Count == 0 && OutputFlows.Count == 0)
            {
                throw new Exception("Вы не добавили ни одного входного или выходного потока");
            }

            //Получаем из вхожных потоков массив переменных только для чтения
            List<string> ReadonlyVars = new List<string>();
            foreach (var item in InputFlows)
            {
                foreach (var var in item.FlowParameters)
                {
                    ReadonlyVars.Add(var.Variable);
                }
            }

            //Получаем из выходных потоков массив переменных доступных для записи
            List<string> WritableVars = new List<string>();
            foreach (var item in OutputFlows)
            {
                foreach (var var in item.FlowParameters)
                {
                    WritableVars.Add(var.Variable);
                }
            }

            //Проверка параметров по умолчанию на совпадение с другими параметрами
            foreach (var item in DefaultParameters)
            {
                //Проверка на соответствие формату
                if (Regex.IsMatch(item.Symbol, @"^[A-Za-z][A-Za-z0-9]*") == false)
                {
                    throw new Exception($"Параметр по умолчанию {item.Symbol} не может начинаться с цифры");
                };
                //Проверка на совпадение с парметрами входных потоков
                if (ReadonlyVars.Contains(item.Symbol))
                {
                    throw new Exception($"Параметр по умолчанию {item.Symbol} совпадает с параметром из входного потока");
                }
                //Проверка на совпадение с параметрами из выходных потоков
                if (WritableVars.Contains(item.Symbol))
                {
                    throw new Exception($"Параметр по умолчанию {item.Symbol} совпадает с параметром из выходного потока");
                }
                ReadonlyVars.Add(item.Symbol);
            }

            //Создаём и наполняем список переменных, определяемых расчётными выражениями
            List<string> DefinedWritableVars = new List<string>();
            foreach (var ExpressionBlock in Expressions)
            {
                //Проверяем, что значение переменной, используемое в расчётном выражении уже было определено
                foreach (var needed in ExpressionBlock.NeededVariables)
                {
                    if (ReadonlyVars.Contains(needed) == false && DefinedWritableVars.Contains(needed) == false && needed != "")
                    {
                        throw new Exception($"Параметр {needed}, используемый в выражении №{ExpressionBlock.Order}: {ExpressionBlock.Expression} не определён");
                    }
                }
                //Проверяем, что определяемая переменная выражением удовлетворяет шаблону
                if (Regex.IsMatch(ExpressionBlock.DefinedVariable, @"^[A-Za-z][A-Za-z0-9]*") == false)
                {
                    throw new Exception($"Параметр {ExpressionBlock.DefinedVariable}, определяемый выражением №{ExpressionBlock.Order}: {ExpressionBlock.Expression} не может начинаться с цифры");
                }
                //Проверяем, что выражение не переопределяет переменную из входных потоков
                if (ReadonlyVars.Contains(ExpressionBlock.DefinedVariable) == true)
                {
                    throw new Exception($"Параметр {ExpressionBlock.DefinedVariable}, определяемый выражением №{ExpressionBlock.Order}: {ExpressionBlock.Expression} доступен только для чтения");
                }
                //Добавляем перменную в массив перменных с опредлённым значением
                DefinedWritableVars.Add(ExpressionBlock.DefinedVariable);
            }

            //Проверяем, что в каждом выходном потоке определено необходимое число переменных
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
                //FIX BACK
                if (count != OutFlow.FlowParameters.Count)
                {
                    throw new Exception($"В выходном потоке №{OutFlow.FlowVariableIndex} определено менее двух параметров");
                }
            }

        }

        /// <summary> Метод проверки корректности расчётных выражений </summary>
        public void CheckExpressionsCorrectness()
        {
            foreach (var expBlock in Expressions)
            {
                if (expBlock.ExpressionType == GlobalTypes.ExpressionTypes.Expression)
                {
                    Expression e = new Expression(expBlock.Expression);
                    e.disableImpliedMultiplicationMode();
                    foreach (var arg in expBlock.NeededVariables)
                    {
                        e.defineArgument(arg, 10);
                    }
                    if (e.checkSyntax() == false)
                    {
                        var error = e.getErrorMessage();
                        throw new Exception($"Ошибка при обработке выражения №{expBlock.Order}: {expBlock.Expression}\nОтчёт работы математического ядра:\n{error}");
                    }
                }
                if (expBlock.ExpressionType == GlobalTypes.ExpressionTypes.PropSI)
                {
                    //List<BlockInstanceVariable> Variables = new();
                    //foreach (var var in expBlock.NeededVariables)
                    //{
                    //    Variables.Add(new(var, 10));
                    //}
                    //try
                    //{
                    //    CalcUtilitiesClass.CallPropSIFromString(expBlock.Expression, Variables);
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw new Exception($"Ошибка при обработке выражения №{expBlock.Order}: {expBlock.Expression}\nОтчёт функции PropSI:\n{ex.Message}");
                    //}
                }
            }
        }

        /// <summary> Хендлер события для регенерации параметров по умолчанию </summary>
        private void RegenerateCustomParameters(object sender, NotifyCollectionChangedEventArgs e)
        {
            RegenerateCustomParameters();
        }

        /// <summary> Метод регененрации дополнительных параметров в случе изменения их имени </summary>
        public void RegenerateCustomParameters(string oldVar, string newVar)
        {
            var OldFoundVar = ExtraParameters.FirstOrDefault(x => x.Symbol == oldVar);
            var NewFoundVar = ExtraParameters.FirstOrDefault(y => y.Symbol == newVar);
            //Если дополнительный параметр был переименован, то сохраним старое описание и в новом параметре
            if (OldFoundVar != null && NewFoundVar == null && newVar != "")
            {
                ExtraParameters.Add(new CustomParametreClass(OldFoundVar.Title, newVar, OldFoundVar.Units));
            }
            RegenerateCustomParameters();
        }

        /// <summary> Метод регенерации дополнительных параметров в остальных случаях </summary>
        public void RegenerateCustomParameters()
        {
            //Берём все определяемые расчётными выражениями переменные
            List<string> RawVars = Expressions.Select(x => x.DefinedVariable).ToList();
            //Убираем переменные из входных потоков
            RawVars = RawVars.Where(rawVar => InputFlows.FirstOrDefault(flow => flow.FlowParameters.FirstOrDefault(flowParam => flowParam.Variable == rawVar) != null) == null).ToList();
            //... из выходных потоков
            RawVars = RawVars.Where(rawVar => OutputFlows.FirstOrDefault(flow => flow.FlowParameters.FirstOrDefault(flowParam => flowParam.Variable == rawVar) != null) == null).ToList();
            //Убираем переменные по умолчанию
            RawVars = RawVars.Where(rawVar => DefaultParameters.FirstOrDefault(DefParam => DefParam.Symbol == rawVar) == null).ToList();

            // Убираем страрые, не используемые более дополнительные парамтеры
            ExtraParameters = new ObservableCollection<CustomParametreClass>(ExtraParameters.Where(extraParam => RawVars.Contains(extraParam.Symbol)));

            //Добавляем новые, которых ранее не было в списке
            foreach (var var in RawVars)
            {
                if (ExtraParameters.FirstOrDefault(x => x.Symbol == var) == null && var != "")
                {
                    ExtraParameters.Add(new CustomParametreClass("", var, ""));
                }
            }
        }

        /// <summary> Метод проверки всего блока на корректность </summary>
        public bool CheckBlock(bool silent = false)
        {
            try
            {
                RegenerateCustomParameters();
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

        /// <summary> Метод обновления списка необходимых и рассчитываемых переменных для расчёта модели </summary>
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
                CalcedCalcVariables = new ObservableCollection<CalcVariableClass>(CalcedCalcVariables.Where(x => ProcessedCalcedVars.Any(y => x.Variable == y)));

                var ProcessedFilledVars = new List<string>();
                foreach (var expressionBlock in Expressions)
                {
                    foreach (var variable in expressionBlock.NeededVariables)
                    {
                        if (CalcedCalcVariables.Any(x => x.Variable == variable) == false)
                        {
                            if (FilledCalcVariables.Any(x => x.Variable == variable) == false)
                            {
                                FilledCalcVariables.Add(new CalcVariableClass(variable, 0));
                            }
                            ProcessedFilledVars.Add(variable);
                        }
                    }

                    var defVar = expressionBlock.DefinedVariable;
                    if (CalcedCalcVariables.Any(x => x.Variable == defVar) == false)
                    {
                        CalcedCalcVariables.Add(new CalcVariableClass(defVar, 0));
                    }
                }

                FilledCalcVariables = new ObservableCollection<CalcVariableClass>(FilledCalcVariables.Where(x => ProcessedFilledVars.Any(y => x.Variable == y)));

                CalcButtonText = StaticStrings.CalcAvailable;
                CalcButtonAvailable = true;
                return true;
            }
        }

        /// <summary> Метод расчёта модели на заданных значениях </summary>
        public void CalculateModel()
        {
            foreach (var expBlock in Expressions)
            {
                List<BlockInstanceVariable> PreparedNeededVars = new List<BlockInstanceVariable>();
                foreach (var arg in expBlock.NeededVariables)
                {
                    var Var = FilledCalcVariables.FirstOrDefault(x => x.Variable == arg);
                    if (Var == null)
                    {
                        Var = CalcedCalcVariables.FirstOrDefault(x => x.Variable == arg);
                    }
                    PreparedNeededVars.Add(new(Var.Variable, Var.Value));
                }

                PrecompiledCalcExpressionClass Expression = new(expBlock.Expression);
                double result = 0;

                try
                {
                    result = Expression.Calc(PreparedNeededVars.ToDictionary(x => x.VariableName));
                }
                catch (Exception e)
                {
                    if (Expression.ExpressionType == GlobalTypes.ExpressionTypes.Expression)
                    {
                        throw new Exception($"Ошибка при обработке выражения №{expBlock.Order}: {expBlock.Expression}\nОтчёт работы математического ядра:\n{Expression.PrecompiledExpression.getErrorMessage()}");
                    }

                    if (Expression.ExpressionType == GlobalTypes.ExpressionTypes.PropSI)
                    {
                        throw new Exception($"Ошибка при обработке выражения №{expBlock.Order}: {expBlock.Expression}\nОтчёт функции PropSI:\n{e.Message}");
                    }
                }

                //if (expBlock.ExpressionType == GlobalTypes.ExpressionTypes.Expression)
                //{
                //    Expression e = new Expression(expBlock.Expression);
                //    e.disableImpliedMultiplicationMode();
                //    PreparedNeededVars.ForEach(x => e.defineArgument(x.VariableName, x.Value));
                //    if (e.checkSyntax() == false)
                //    {
                //        var error = e.getErrorMessage();
                //        throw new Exception($"Ошибка при обработке выражения №{expBlock.Order}: {expBlock.Expression}\nОтчёт работы математического ядра:\n{error}");
                //    }
                //    else
                //    {
                //        result = e.calculate();
                //    }
                //}

                //if (expBlock.ExpressionType == GlobalTypes.ExpressionTypes.PropSI)
                //{
                //    try
                //    {
                //        result = CalcUtilitiesClass.CallPropSIFromString(expBlock.Expression, PreparedNeededVars);
                //    }
                //    catch (Exception ex)
                //    {
                //        throw new Exception($"Ошибка при обработке выражения №{expBlock.Order}: {expBlock.Expression}\nОтчёт функции PropSI:\n{ex.Message}");
                //    }
                //}
                CalcedCalcVariables[CalcedCalcVariables.IndexOf(CalcedCalcVariables.FirstOrDefault(x => x.Variable == expBlock.DefinedVariable))].Value = result;
            }
        }

        /// <summary> Обновляет порядковые номера расчётных выражений </summary>
        public void ResortExpressions()
        {
            for (int i = 0; i < Expressions.Count; i++)
            {
                Expressions[i].Order = i + 1;
            }
        }

        /// <summary> Метод получения максимального индекса с входных и выходных потоков </summary>
        public int GetLastFlowsIndex()
        {
            int MaxInputFlowIndex = InputFlows.Count > 0 ? InputFlows.Max(flow => flow.FlowVariableIndex) : 0;
            int MaxOutputFlowIndex = OutputFlows.Count > 0 ? OutputFlows.Max(flow => flow.FlowVariableIndex) : 0;
            return Math.Max(MaxInputFlowIndex, MaxOutputFlowIndex);

        }
    }
}
