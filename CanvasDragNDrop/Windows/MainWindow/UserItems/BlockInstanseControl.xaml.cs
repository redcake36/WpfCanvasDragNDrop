using CanvasDragNDrop;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CanvasDragNDrop
{
    /// <summary>
    /// Interaction logic for BlockInstanseControl.xaml
    /// </summary>
    public partial class BlockInstanseControl : UserControl
    {
        public BlockInstanseControl()
        {
            InitializeComponent();
        }

        public int BlockHeight
        {
            get { return (int)GetValue(BlockHeightProperty); }
            set { SetValue(BlockHeightProperty, value); }
        }
        public static readonly DependencyProperty BlockHeightProperty =
            DependencyProperty.Register(nameof(BlockHeight), typeof(int), typeof(BlockInstanseControl), new PropertyMetadata(0));

        public int BlockWidth
        {
            get { return (int)GetValue(BlockWidthProperty); }
            set { SetValue(BlockWidthProperty, value); }
        }
        public static readonly DependencyProperty BlockWidthProperty =
            DependencyProperty.Register(nameof(BlockWidth), typeof(int), typeof(BlockInstanseControl), new PropertyMetadata(0));


        public Brush BlockBackgroundColor
        {
            get { return (Brush)GetValue(BlockBackgroundColorProperty); }
            set { SetValue(BlockBackgroundColorProperty, value); }
        }
        public static readonly DependencyProperty BlockBackgroundColorProperty =
            DependencyProperty.Register(nameof(BlockBackgroundColor), typeof(Brush), typeof(BlockInstanseControl));

        public ObservableCollection<FlowConnector> InputConnectors
        {
            get { return (ObservableCollection<FlowConnector>)GetValue(InputConnectorsProperty); }
            set { SetValue(InputConnectorsProperty, value); }
        }
        public static readonly DependencyProperty InputConnectorsProperty =
            DependencyProperty.Register(nameof(InputConnectors), typeof(ObservableCollection<FlowConnector>), typeof(BlockInstanseControl));

        public ObservableCollection<FlowConnector> OutputConnectors
        {
            get { return (ObservableCollection<FlowConnector>)GetValue(OutputConnectorsProperty); }
            set { SetValue(OutputConnectorsProperty, value); }
        }
        public static readonly DependencyProperty OutputConnectorsProperty =
            DependencyProperty.Register(nameof(OutputConnectors), typeof(ObservableCollection<FlowConnector>), typeof(BlockInstanseControl));

        public int ConnectorSize
        {
            get { return (int)GetValue(ConnectorSizeProperty); }
            set { SetValue(ConnectorSizeProperty, value); }
        }
        public static readonly DependencyProperty ConnectorSizeProperty =
            DependencyProperty.Register(nameof(ConnectorSize), typeof(int), typeof(BlockInstanseControl), new PropertyMetadata(0));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(BlockInstanseControl));

    }
}
