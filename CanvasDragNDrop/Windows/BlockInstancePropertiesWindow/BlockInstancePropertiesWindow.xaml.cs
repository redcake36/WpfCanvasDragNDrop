using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CanvasDragNDrop.Windows
{
    /// <summary>
    /// Interaction logic for BlockInstancePropertiesWindow.xaml
    /// </summary>
    public partial class BlockInstancePropertiesWindow : Window, INotifyPropertyChanged
    {
        public BlockInstance BlockInstance
        {
            get { return _blockInstance; }
            set { _blockInstance = value; OnPropertyChanged(); }
        }
        private BlockInstance _blockInstance;

        public BlockInstancePropertiesWindow(BlockInstance overseedBlockInctance)
        {
            InitializeComponent();
            BlockInstance = overseedBlockInctance;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
