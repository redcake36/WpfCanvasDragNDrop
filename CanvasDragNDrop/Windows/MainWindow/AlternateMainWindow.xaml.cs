using CanvasDragNDrop.APIClases;
using CanvasDragNDrop.UtilityClasses;
using CanvasDragNDrop.Windows;
using CanvasDragNDrop.Windows.BlockModelCreationWindow.Classes;
using CanvasDragNDrop.Windows.CalculationResultWindow;
using CanvasDragNDrop.Windows.CycleCalcStartParamsEnterWindow;
using CanvasDragNDrop.Windows.ErrorSavingSchemaWindow;
using CanvasDragNDrop.Windows.MainWindow.Classes;
using CanvasDragNDrop.Windows.ModelsExplorer;
using CanvasDragNDrop.Windows.SchemeSelectionWindow;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CanvasDragNDrop
{
    /// <summary>
    /// Interaction logic for AlternateMainWindow.xaml
    /// </summary>
    public partial class AlternateMainWindow : Window, INotifyPropertyChanged
    {
        public CanvasOverseerClass CanvasOverseer
        {
            get => _canvasOverseer;
            set { _canvasOverseer = value; OnPropertyChanged(); }
        }
        private CanvasOverseerClass _canvasOverseer = new();

        public SchemaClass Scheme
        {
            get => _schema;
            set { _schema = value; OnPropertyChanged(); }
        }
        private SchemaClass _schema = new();

        public Brush TestBrush
        {
            get { return _testBrush; }
            set { _testBrush = value; OnPropertyChanged(); }
        }
        private Brush _testBrush = new SolidColorBrush(Colors.BlueViolet);

        /// <summary> Коллекция доступных к соззданию блоков </summary>
        public ObservableCollection<APIBlockModelVersionClass> AvailableBlockModels
        {
            get { return _availableBlockModels; }
            set { _availableBlockModels = value; OnPropertyChanged(); }
        }
        private ObservableCollection<APIBlockModelVersionClass> _availableBlockModels = new();

        private IncrementingIndexGenerator _instanceIdGenerator = new();

        public AlternateMainWindow()
        {
            InitializeComponent();
            GetFromServerElemList(this, new());
        }

        /// <summary> Метод обновления списка моделей блоков с сервера </summary>
        void GetFromServerElemList(object sender, RoutedEventArgs e)
        {
            var GettingBlockModelsResult = API.GetBlockModels();
            if (GettingBlockModelsResult.isSuccess)
            {
                AvailableBlockModels = new ObservableCollection<APIBlockModelVersionClass>(GettingBlockModelsResult.blockModels);
            }
            else
            {
                MessageBox.Show("Не удалось получить данные с сервера");
                return;
            }
        }

        //ViewportHandlers

        /// <summary> Обработчик изменения размера основного канваса </summary>
        private void ViewportSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize.Width != 0 | e.PreviousSize.Height != 0)
            {
                var matrix = transform.Matrix;
                matrix.Translate((e.NewSize.Width - e.PreviousSize.Width) / 2, (e.NewSize.Height - e.PreviousSize.Height) / 2);
                transform.Matrix = matrix;
            }
        }
        private void ViewportMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle && e.MiddleButton == MouseButtonState.Pressed)
            {
                _canvasOverseer.ViewportMidleMouseDown();
            }
        }

        private void ViewportMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle && e.MiddleButton == MouseButtonState.Released)
            {
                _canvasOverseer.ViewportMidleMouseUp();
            }
        }

        /// <summary> Обработчик движения мыши по основному канвасу </summary>
        private void ViewportMouseMove(object sender, MouseEventArgs e)
        {
            //Обновляем позицию мыши на вьюпорте
            _canvasOverseer.ViewportMouseMoved(e.GetPosition(CanvasViewport), e.GetPosition(CanvasArea));
            //Перемещение холста с помощью средней кнопки мыши
            if (_canvasOverseer.CanDragViewport)
            {
                var matrix = transform.Matrix;
                matrix.Translate(_canvasOverseer.ViewportMousePositionDelta.X, _canvasOverseer.ViewportMousePositionDelta.Y);
                transform.Matrix = matrix;
            }
            //Перемещение блока в режиме премещения блока
            if (_canvasOverseer.CanDragBlock)
            {
                var matrix = transform.Matrix;
                Scheme.BlockInstances[_canvasOverseer.SelectedBlockIndex].OffsetLeft += (_canvasOverseer.ViewportMousePositionDelta.X) / matrix.M11;
                Scheme.BlockInstances[_canvasOverseer.SelectedBlockIndex].OffsetTop += (_canvasOverseer.ViewportMousePositionDelta.Y) / matrix.M11;
            }
            //Обновляем точку соединительного потока при его создании
            if (_canvasOverseer.CanUpdateLiveInterconnectionPoint)
            {
                Scheme.BlockInterconnections[_canvasOverseer.SelectedFlowInterconnectLineIndex].UpdateLiveInterconnectPoint(e.GetPosition(CanvasArea));
            }
        }

        /// <summary> Обработчик прокрутки колёсика мыши на основном канвасе </summary>
        private void ViewportMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var pos = e.GetPosition((UIElement)sender);
            var matrix = transform.Matrix;
            var scale = e.Delta > 0 ? 1.1 : 1 / 1.1;
            matrix.ScaleAt(scale, scale, pos.X, pos.Y);
            transform.Matrix = matrix;
        }
        private void ViewportKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                _canvasOverseer.ExitFlowInterconnectionMode();
            }
        }

        /// <summary> Выбор блока для операции </summary>
        private void BlockLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int BlockIndex = (int)((BlockInstanseControl)sender).Tag;
            _canvasOverseer.BlockLeftMouseDown(BlockIndex, e.ClickCount);
            OpenBlockProperties();
            if (e.OriginalSource is Ellipse)
            {
                int FlowConnectorIndex = (int)((Ellipse)e.OriginalSource).Tag;
                bool IsInputConnector = ((Ellipse)e.OriginalSource).Name == "InputConnector";
                if (_canvasOverseer.CanStartInterconnection)
                {
                    try
                    {
                        FlowInterconnectLine newInterconnectLine = new();
                        if (IsInputConnector)
                        {
                            newInterconnectLine.SetFlowConnector(Scheme.BlockInstances[BlockIndex].InputConnectors[FlowConnectorIndex], IsInputConnector);
                        }
                        else
                        {
                            newInterconnectLine.SetFlowConnector(Scheme.BlockInstances[BlockIndex].OutputConnectors[FlowConnectorIndex], IsInputConnector);
                        }
                        newInterconnectLine.UpdateLiveInterconnectPoint(_canvasOverseer.AreaMousePosition);
                        Scheme.BlockInterconnections.Add(newInterconnectLine);
                        _canvasOverseer.ConnectFirstFlowInterconnectPoint(Scheme.BlockInterconnections.Count - 1);
                    }
                    catch { }
                }
                else
                {
                    if (_canvasOverseer.CanFinishInterconnection)
                    {
                        try
                        {
                            if (IsInputConnector)
                            {
                                Scheme.BlockInterconnections[_canvasOverseer.SelectedFlowInterconnectLineIndex].SetFlowConnector(Scheme.BlockInstances[BlockIndex].InputConnectors[FlowConnectorIndex], IsInputConnector);
                            }
                            else
                            {
                                Scheme.BlockInterconnections[_canvasOverseer.SelectedFlowInterconnectLineIndex].SetFlowConnector(Scheme.BlockInstances[BlockIndex].OutputConnectors[FlowConnectorIndex], IsInputConnector);
                            }
                            _canvasOverseer.ConnectSecondFlowInterconnectPoint();
                        }
                        catch { }
                    }
                }
            }
        }

        /// <summary> Выбор блока для операции </summary>
        private void BlockLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _canvasOverseer.BlockLeftMouseUp();
        }

        private void CreateBlockInstanse(int BlockModelVersionId)
        {
            //var foundedBlock = _availableBlockModels.FirstOrDefault(block => block.ModelId == BlockModelId);

            var requestedModelVersion = API.GetModelVersion(BlockModelVersionId);
            if (requestedModelVersion.isSuccess == false)
            {
                MessageBox.Show("Ошибка на сервере");
                return;
            }

            Scheme.BlockInstances.Add(new(requestedModelVersion.response, _instanceIdGenerator.IncrementedIndex));
        }

        //private Point GetElemenyPositionOnCanvas(object sender)
        //{
        //    return ((UIElement)sender).TranslatePoint(new Point(0, 0), CanvasArea);
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void StartInterconnectCreation(object sender, RoutedEventArgs e)
        {
            _canvasOverseer.EnterFlowInterconnectMode();
        }

        private void CalculateScheme(object sender, RoutedEventArgs e)
        {
            // Проверка схемы на корректность

            if (!_canvasOverseer.CanCalcScheme)
            {
                return;
            }

            if (Scheme.BlockInstances.Count == 0)
            {
                MessageBox.Show("На схеме нет ни одного блока", "Ошибка при расчёте");
                return;
            }

            foreach (var block in Scheme.BlockInstances)
            {
                foreach (var item in block.InputConnectors)
                {
                    if (item.InterconnectLine == null)
                    {
                        MessageBox.Show($"Не все входные потоки блока {block.BlockInstanceId}:{block.BlockModel.Title} подключены", "Ошибка при расчёте");
                        return;
                    }
                }
            }

            MessageBoxResult Result = MessageBox.Show("Calc?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (Result == MessageBoxResult.No)
            {
                return;
            }

            //Сброс статусов схемы
            foreach (var instance in Scheme.BlockInstances)
            {
                instance.BlockInstanceStatus = BlockInstance.BlockInstanceStatuses.UnSeen;
            }
            foreach (var interconnect in Scheme.BlockInterconnections)
            {
                interconnect.FlowInterconnectStatus = FlowInterconnectLine.FlowInterconnectStatuses.UnSeen;
            }

            //Первичный анализ схемы - ищем циклы
            BlockInstance currentBlockInstance;
            List<int> visitedInstances = new List<int>();
            List<List<int>> foundedCycles = new List<List<int>>();

            //Выполняем обход пока не кончатся блоки, до которых не добрался алгоритм
            while (Scheme.BlockInstances.Any(x => x.BlockInstanceStatus == BlockInstance.BlockInstanceStatuses.UnSeen))
            {
                visitedInstances.Clear();
                visitedInstances.Add(Scheme.BlockInstances.First(x => x.BlockInstanceStatus == BlockInstance.BlockInstanceStatuses.UnSeen).BlockInstanceId);
                //Пока обход не вернётся в точку своего начала обрабатываем блоки
                while (visitedInstances.Count > 0)
                {
                    //Подкладываем новый текущий блок когда вышли из какого-то блока
                    currentBlockInstance = Scheme.BlockInstances.First(x => x.BlockInstanceId == visitedInstances.Last());
                    //Пока у текущего блока есть выходные потоки, которые не были обойдены, то обрабатываем его
                    while (currentBlockInstance.OutputConnectors.Any(x => x.InterconnectLine?.FlowInterconnectStatus == FlowInterconnectLine.FlowInterconnectStatuses.UnSeen))
                    {
                        //Выбираем новый поток, по которому ещё не ходили
                        FlowInterconnectLine nextInterconnection = currentBlockInstance.OutputConnectors.First(x => x.InterconnectLine?.FlowInterconnectStatus == FlowInterconnectLine.FlowInterconnectStatuses.UnSeen).InterconnectLine;
                        //Выбираем следующий блок
                        BlockInstance nextBlock = Scheme.BlockInstances[nextInterconnection.InputFlowConnector.BlockInstanceID];
                        //Если блок уже встречался нам, то это цикл - надо промаркировать потоки и блоки в цепи
                        var foundedIndex = visitedInstances.FindIndex(x => x == nextBlock.BlockInstanceId);
                        if (foundedIndex >= 0)
                        {
                            //Цикл найден, помещаем его последовательность в массив циклов
                            List<int> foundedCycle = visitedInstances.Skip(foundedIndex).ToList();
                            foundedCycles.Add(foundedCycle);
                            //Помечаем все блоки и потоки, связывающие их как цикличные
                            for (int i = 0; i < foundedCycle.Count; i++)
                            {
                                //Выбираем индексы источников потоков и их приёмников
                                int sourceInstanceId = foundedCycle[i];
                                int destinationInstanceId = foundedCycle[(i + 1) % foundedCycle.Count];
                                //Меняяем статус блока
                                Scheme.BlockInstances.First(x => x.BlockInstanceId == sourceInstanceId).BlockInstanceStatus = BlockInstance.BlockInstanceStatuses.InCycle;
                                //Меняем статус всех соединяющих потоков
                                Scheme.BlockInterconnections.Where(x => x.OutputFlowConnector.BlockInstanceID == sourceInstanceId && x.InputFlowConnector.BlockInstanceID == destinationInstanceId).ToList().ForEach(x => x.FlowInterconnectStatus = FlowInterconnectLine.FlowInterconnectStatuses.InCycle);
                            }
                            //Завершаем обработку этого потока
                            continue;
                        }
                        //Если блок уже посещён и все его потоки обработаны или он ведёт к блоку цикла и не входит в него, то цикла быть не может - маркируем ведущий к нему поток как просмотернный
                        if (nextBlock.BlockInstanceStatus == BlockInstance.BlockInstanceStatuses.UnCalc || nextBlock.BlockInstanceStatus == BlockInstance.BlockInstanceStatuses.InCycle)
                        {
                            nextInterconnection.FlowInterconnectStatus = FlowInterconnectLine.FlowInterconnectStatuses.UnCalc;
                            continue;
                        }
                        //Если блок ещё не посещался или находится в цикле, то надо пойти в него для обработки
                        currentBlockInstance = nextBlock;
                        visitedInstances.Add(nextBlock.BlockInstanceId);
                    }
                    //Когда в текущем блоке не осталось необработанных выходных потоков, то уходим из него и помечаем как обработанный, если он не в цикле
                    if (currentBlockInstance.BlockInstanceStatus == BlockInstance.BlockInstanceStatuses.UnSeen)
                    {
                        currentBlockInstance.BlockInstanceStatus = BlockInstance.BlockInstanceStatuses.UnCalc;
                    }
                    //Убираем из списка Id блока, из которого вышли
                    visitedInstances.RemoveAt(visitedInstances.Count - 1);
                }
            }

            //Если циклов больше двух, то приплыли - пока не можем
            if (foundedCycles.Count > 1)
            {
                MessageBox.Show("Данная схема содержит более одного цикла - расчёт не возможен", "Ошибка при расчёте");
                return;
            }

            //Подготовка всех остальных блоков, не находящихся в циклах к расчёту
            List<BlockInstance> cycledInstances = new();
            if (foundedCycles.Count > 0)
            {
                foundedCycles[0].ForEach(x => cycledInstances.Add(Scheme.BlockInstances.First(y => y.BlockInstanceId == x)));
                //Обозначаем все выходные потоки этих блоков, не участвующие в цикле как ожидающие результатов цикла
                List<FlowInterconnectLine> waitingCyclesInterconnects = Scheme.BlockInterconnections.Where(x => foundedCycles[0].Contains(x.OutputFlowConnector.BlockInstanceID) && x.FlowInterconnectStatus != FlowInterconnectLine.FlowInterconnectStatuses.InCycle).ToList();
                foreach (var item in waitingCyclesInterconnects)
                {
                    item.FlowInterconnectStatus = FlowInterconnectLine.FlowInterconnectStatuses.WaitingCycle;
                }
            }

            //Расчёт блоков, у которых на входе все потоки готовы к расчёту
            while (Scheme.BlockInstances.Any(x => x.InputConnectors.All(y => y.InterconnectLine.FlowInterconnectStatus == FlowInterconnectLine.FlowInterconnectStatuses.Ready) && x.BlockInstanceStatus == BlockInstance.BlockInstanceStatuses.UnCalc))
            {
                BlockInstance instanceForCalculation = Scheme.BlockInstances.First(x => x.InputConnectors.All(y => y.InterconnectLine.FlowInterconnectStatus == FlowInterconnectLine.FlowInterconnectStatuses.Ready));
                try
                {
                    instanceForCalculation.CalculateBlockInstance();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, $"Ошибка при расчёте блока {instanceForCalculation.BlockInstanceId}: {instanceForCalculation.BlockModel.Title}");
                    return;
                }
                instanceForCalculation.BlockInstanceStatus = BlockInstance.BlockInstanceStatuses.Ready;
                foreach (var outputConnector in instanceForCalculation.OutputConnectors)
                {
                    if (outputConnector.InterconnectLine != null)
                    {
                        outputConnector.InterconnectLine.FlowInterconnectStatus = FlowInterconnectLine.FlowInterconnectStatuses.Ready;
                    }
                }
            }

            if (foundedCycles.Count > 0)
            {
                //MessageBox.Show($"Найден цикл {foundedCycles[0].Aggregate("",(string accum, int id) => $"{accum} {id}")}");
                CycleCalcStartParamsEnterWindow cycleParamsWindow = new(cycledInstances);
                cycleParamsWindow.ShowDialog();
                int startInstanseIndex = cycleParamsWindow.SelectedInstanceIndex;
                //Перекладываем значения на выходы потоков для начального блока цикла
                foreach (var inputConnector in cycledInstances[startInstanseIndex].InputConnectors)
                {
                    if (inputConnector.InterconnectLine.FlowInterconnectStatus == FlowInterconnectLine.FlowInterconnectStatuses.InCycle)
                    {
                        inputConnector.InterconnectLine.TransferValuesToOutputConnector();
                    }
                }
                //Начинаем расчёт цикла
                bool neededPresisionFounded = false;
                for (int i = 0; i < 10000 && !neededPresisionFounded; i++)
                {
                    neededPresisionFounded = true;
                    for (int j = 0; j < cycledInstances.Count; j++)
                    {
                        try
                        {
                            neededPresisionFounded &= cycledInstances[(j + startInstanseIndex) % cycledInstances.Count].CalculateBlockInstance();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, $"Ошибка при расчёте блока {cycledInstances[(j + startInstanseIndex) % cycledInstances.Count].BlockInstanceId}: {cycledInstances[(j + startInstanseIndex) % cycledInstances.Count].BlockModel.Title}");
                            return;
                        }
                    }
                    if (neededPresisionFounded)
                    {
                        MessageBox.Show($"Estimated {i}");
                    }
                }
                //Обновляем все блоки цикла как расчитанные
                foreach (var instance in cycledInstances)
                {
                    instance.BlockInstanceStatus = BlockInstance.BlockInstanceStatuses.Ready;
                    foreach (var outputConnector in instance.OutputConnectors)
                    {
                        if (outputConnector.InterconnectLine != null)
                        {
                            outputConnector.InterconnectLine.FlowInterconnectStatus = FlowInterconnectLine.FlowInterconnectStatuses.Ready;
                        }
                    }
                }

                //Дорасчитываем все блоки, которые не посчитали раньше из за цикла
                while (Scheme.BlockInstances.Any(x => x.InputConnectors.All(y => y.InterconnectLine.FlowInterconnectStatus == FlowInterconnectLine.FlowInterconnectStatuses.Ready) && x.BlockInstanceStatus == BlockInstance.BlockInstanceStatuses.UnCalc))
                {
                    BlockInstance instanceForCalculation = Scheme.BlockInstances.First(x => x.InputConnectors.All(y => y.InterconnectLine.FlowInterconnectStatus == FlowInterconnectLine.FlowInterconnectStatuses.Ready));
                    try
                    {
                        instanceForCalculation.CalculateBlockInstance();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, $"Ошибка при расчёте блока {instanceForCalculation.BlockInstanceId}: {instanceForCalculation.BlockModel.Title}");
                        return;
                    }
                    instanceForCalculation.BlockInstanceStatus = BlockInstance.BlockInstanceStatuses.Ready;
                    foreach (var outputConnector in instanceForCalculation.OutputConnectors)
                    {
                        if (outputConnector.InterconnectLine != null)
                        {
                            outputConnector.InterconnectLine.FlowInterconnectStatus = FlowInterconnectLine.FlowInterconnectStatuses.Ready;
                        }
                    }
                }
            }

            CalculationResultWindow resultWindow = new(Scheme.BlockInstances);
            resultWindow.Owner = this;
            resultWindow.Show();
        }

        private void OpenBlockProperties()
        {
            if (_canvasOverseer.CanOpenBlockProperties)
            {
                BlockInstancePropertiesWindow PropertiesWindow = new(Scheme.BlockInstances[_canvasOverseer.SelectedBlockIndex]);
                PropertiesWindow.Owner = this;
                PropertiesWindow.Show();
                _canvasOverseer.BlockPropertiesOpened();
            }
        }

        private void OpenModelsBrowser(object sender, RoutedEventArgs e)
        {
            ModelsExplorer ModelsExplorer = new();
            ModelsExplorer.ModelVersionSelectedHandler += CreateBlockInstanse;
            ModelsExplorer.Owner = this;
            ModelsExplorer.Show();
        }

        private void OpenBlockModelCreationWindow(object sender, RoutedEventArgs e)
        {
            BlockModelCreationWindow CreationWindow = new BlockModelCreationWindow();
            CreationWindow.ShowDialog();
        }

        private void SaveScheme(object sender, RoutedEventArgs e)
        {
            if (_schema.SchemaId < 0)
            {
                ErrorSaveSchema errorSaveSchema = new ErrorSaveSchema();
                errorSaveSchema.ShowDialog();
                _schema.Title = errorSaveSchema.Title1;
            }
            var Result = API.CreateSchema(_schema);
            if (Result.isSuccess)
            {
                MessageBox.Show("Схема успешно сохранена");
            }
            else
            {
                MessageBox.Show(Result.response, "Не удалось выполнить запрос");
            }
            //var parsed = JsonConvert.SerializeObject(_schema);
            //MessageBox.Show(parsed);
        }
    }
}
