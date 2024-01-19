using CanvasDragNDrop.APIClases;
using CanvasDragNDrop.UtilityClasses;
using CanvasDragNDrop.Windows;
using CanvasDragNDrop.Windows.CalculationResultWindow;
using CanvasDragNDrop.Windows.MainWindow.Classes;
using CanvasDragNDrop.Windows.ModelsExplorer;
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

        public SchemaClass Schema
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


        //public ObservableCollection<BlockInstance> BlockInstances
        //{
        //    get { return _blockInstances; }
        //    set { _blockInstances = value; OnPropertyChanged(); }
        //}
        //private ObservableCollection<BlockInstance> _blockInstances = new();


        /// <summary> Коллекция доступных к соззданию блоков </summary>
        public ObservableCollection<APIBlockModelClass> AvailableBlockModels
        {
            get { return _availableBlockModels; }
            set { _availableBlockModels = value; OnPropertyChanged(); }
        }
        private ObservableCollection<APIBlockModelClass> _availableBlockModels = new();

        //public ObservableCollection<FlowInterconnectLine> BlockInterconnections
        //{
        //    get { return _blockInterconnections; }
        //    set { _blockInterconnections = value; OnPropertyChanged(); }
        //}
        //private ObservableCollection<FlowInterconnectLine> _blockInterconnections = new();

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
                AvailableBlockModels = new ObservableCollection<APIBlockModelClass>(GettingBlockModelsResult.blockModels);
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
                Schema.BlockInstances[_canvasOverseer.SelectedBlockIndex].OffsetLeft += (_canvasOverseer.ViewportMousePositionDelta.X) / matrix.M11;
                Schema.BlockInstances[_canvasOverseer.SelectedBlockIndex].OffsetTop += (_canvasOverseer.ViewportMousePositionDelta.Y) / matrix.M11;
            }
            //Обновляем точку соединительного потока при его создании
            if (_canvasOverseer.CanUpdateLiveInterconnectionPoint)
            {
                Schema.BlockInterconnections[_canvasOverseer.SelectedFlowInterconnectLineIndex].UpdateLiveInterconnectPoint(e.GetPosition(CanvasArea));
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
                            newInterconnectLine.SetFlowConnector(Schema.BlockInstances[BlockIndex].InputConnectors[FlowConnectorIndex], IsInputConnector);
                        }
                        else
                        {
                            newInterconnectLine.SetFlowConnector(Schema.BlockInstances[BlockIndex].OutputConnectors[FlowConnectorIndex], IsInputConnector);
                        }
                        newInterconnectLine.UpdateLiveInterconnectPoint(_canvasOverseer.AreaMousePosition);
                        Schema.BlockInterconnections.Add(newInterconnectLine);
                        _canvasOverseer.ConnectFirstFlowInterconnectPoint(Schema.BlockInterconnections.Count - 1);
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
                                Schema.BlockInterconnections[_canvasOverseer.SelectedFlowInterconnectLineIndex].SetFlowConnector(Schema.BlockInstances[BlockIndex].InputConnectors[FlowConnectorIndex], IsInputConnector);
                            }
                            else
                            {
                                Schema.BlockInterconnections[_canvasOverseer.SelectedFlowInterconnectLineIndex].SetFlowConnector(Schema.BlockInstances[BlockIndex].OutputConnectors[FlowConnectorIndex], IsInputConnector);
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

        private void CreateBlockInstanse(object sender, RoutedEventArgs e)
        {
            var foundedBlock = _availableBlockModels.FirstOrDefault(block => block.ModelId == (int)((MenuItem)sender).Tag);

            Schema.BlockInstances.Add(new(foundedBlock, _instanceIdGenerator.IncrementedIndex));

            ////TEST
            //_blockInterconnections.Add(new FlowInterconnectLine());
            //_blockInterconnections[0].SetFlowConnector(_blockInstances[0].OutputConnectors[0], true);

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
            CalculationResultWindow resultWindow = new(Schema.BlockInstances);
            var results = JsonConvert.SerializeObject(Schema);
            resultWindow.Show();

            //PrecompiledCalcExpressionClass CheckExp = new("2*sin(x)");

            //Dictionary<string, BlockInstanceVariable> args = new();
            //args.Add("x", new("x", 1));
            //args.Add("y", new("y", 2));

            //var watch = System.Diagnostics.Stopwatch.StartNew();
            //for (int i = 0; i < 100000; i++)
            //{
            //    args["x"].Value = i;
            //    CheckExp.Calc(args);
            //}
            //watch.Stop();
            //MessageBox.Show(watch.ElapsedMilliseconds.ToString());

            if (!_canvasOverseer.CanCalcScheme)
            {
                return;
            }
            foreach (var block in Schema.BlockInstances)
            {
                foreach (var item in block.InputConnectors)
                {
                    if (item.InterconnectLine == null)
                    {
                        MessageBox.Show($"Не все входные потоки блока {block.BlockModel.Title} подключены", "Ошибка при расёте");
                    }
                }
            }

            MessageBoxResult Result = MessageBox.Show("Calc?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (Result == MessageBoxResult.No)
            {
                return;
            }

            BlockInstance currentBlock = Schema.BlockInstances.First(x => x.BlockModel.ModelId == 3);

            for (int i = 0; i < Schema.BlockInstances.Count * 500; i++)
            {
                currentBlock.CalculateBlockInstance();
                if (currentBlock.OutputConnectors.Count > 0 && currentBlock.OutputConnectors[0].InterconnectLine != null)
                {
                    currentBlock = Schema.BlockInstances.First(x => x.BlockInstanceId == currentBlock.OutputConnectors[0].InterconnectLine.InputFlowConnector.BlockInstanceID);
                }
            }
        }

        private void OpenBlockProperties()
        {
            if (_canvasOverseer.CanOpenBlockProperties)
            {
                BlockInstancePropertiesWindow PropertiesWindow = new(Schema.BlockInstances[_canvasOverseer.SelectedBlockIndex]);
                PropertiesWindow.Show();
                _canvasOverseer.BlockPropertiesOpened();
            }
        }

        private void OpenModelsBrowser(object sender, RoutedEventArgs e)
        {
            ModelsExplorer ModelsExplorer = new();
            ModelsExplorer.Show();
        }

        private void OpenBlockModelCreationWindow(object sender, RoutedEventArgs e)
        {
            BlockModelCreationWindow BlockModelCreationWindow = new BlockModelCreationWindow();
            BlockModelCreationWindow.ShowDialog();
        }
    }
}
