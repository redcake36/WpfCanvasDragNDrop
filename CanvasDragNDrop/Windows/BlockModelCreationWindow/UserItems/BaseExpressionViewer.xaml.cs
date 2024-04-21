using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CanvasDragNDrop.UserItems
{
    /// <summary>
    /// Interaction logic for BaseExpressionViewer.xaml
    /// </summary>
    public partial class BaseExpressionViewer : UserControl
    {
        public BaseExpressionViewer()
        {
            InitializeComponent();
        }

        public string Exp
        {
            get { return (string)GetValue(ExpProperty); }
            set { SetValue(ExpProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Exp.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExpProperty =
            DependencyProperty.Register(nameof(Exp), typeof(string), typeof(BaseExpressionViewer));

        public string DefinedVariable
        {
            get { return (string)GetValue(DefinedVariableProperty); }
            set { SetValue(DefinedVariableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefinedVariable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefinedVariableProperty =
            DependencyProperty.Register(nameof(DefinedVariable), typeof(string), typeof(BaseExpressionViewer));

        /// <summary> Используемые переменные </summary>
        public ObservableCollection<string> NeededVars
        {
            get { return (ObservableCollection<string>)GetValue(NeededVarsProperty); }
            set { SetValue(NeededVarsProperty, value); }
        }
        public static readonly DependencyProperty NeededVarsProperty =
            DependencyProperty.Register(nameof(NeededVars), typeof(ObservableCollection<string>), typeof(BaseExpressionViewer));

        public int ExpOrder
        {
            get { return (int)GetValue(ExpOrderProperty); }
            set { SetValue(ExpOrderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ExpOrder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExpOrderProperty =
            DependencyProperty.Register(nameof(ExpOrder), typeof(int), typeof(BaseExpressionViewer), new PropertyMetadata(0));



    }
}
