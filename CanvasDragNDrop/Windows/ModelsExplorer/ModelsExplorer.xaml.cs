using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CanvasDragNDrop.Windows.ModelsExplorer
{
    /// <summary>
    /// Interaction logic for ModelsExplorer.xaml
    /// </summary>
    public partial class ModelsExplorer : Window
    {
        public ModelsExplorer()
        {
            InitializeComponent();
            var a = API.GetCatalogs();
        }
    }
}
