using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CanvasDragNDrop.UserItems
{
    /// <summary>
    /// Interaction logic for BaseExpression.xaml
    /// </summary>
    public partial class BaseExpression : UserControl
    {
        public BaseExpression()
        {
            InitializeComponent();
        }

        /// <summary> Расчётное выражение </summary>
        public string Exp
        {
            get { return (string)GetValue(ExpProperty); }
            set { SetValue(ExpProperty, value); }
        }
        public static readonly DependencyProperty ExpProperty =
            DependencyProperty.Register(nameof(Exp), typeof(string), typeof(BaseExpression));

        /// <summary> Определяемая переменная </summary>
        public string DefinedVariable
        {
            get { return (string)GetValue(DefinedVariableProperty); }
            set { SetValue(DefinedVariableProperty, value); }
        }
        public static readonly DependencyProperty DefinedVariableProperty =
            DependencyProperty.Register(nameof(DefinedVariable), typeof(string), typeof(BaseExpression));

        /// <summary> Используемые переменные </summary>
        public ObservableCollection<string> NeededVars
        {
            get { return (ObservableCollection<string>)GetValue(NeededVarsProperty); }
            set { SetValue(NeededVarsProperty, value); }
        }
        public static readonly DependencyProperty NeededVarsProperty =
            DependencyProperty.Register(nameof(NeededVars), typeof(ObservableCollection<string>), typeof(BaseExpression));

        /// <summary> Порядковый номер расчётного выражения </summary>
        public int ExpOrder
        {
            get { return (int)GetValue(ExpOrderProperty); }
            set { SetValue(ExpOrderProperty, value); }
        }
        public static readonly DependencyProperty ExpOrderProperty =
            DependencyProperty.Register(nameof(ExpOrder), typeof(int), typeof(BaseExpression), new PropertyMetadata(0));

        public string ExpressionType
        {
            get { return (string)GetValue(ExpressionTypeProperty); }
            set { SetValue(ExpressionTypeProperty, value); }
        }
        public static readonly DependencyProperty ExpressionTypeProperty =
            DependencyProperty.Register(nameof(ExpressionType), typeof(string), typeof(BaseExpression));


        /// <summary> Делегат операции над выражением </summary>
        public delegate void ExpressionOperationHandler(int expressionOrder);

        public event ExpressionOperationHandler DeleteExpressionEvent;
        /// <summary> Метод для удаления расчётного выражения </summary>
        private void DeleteExpression(object sender, MouseButtonEventArgs e)
        {
            DeleteExpressionEvent?.Invoke(ExpOrder);
        }

        public event ExpressionOperationHandler MoveUpExpressionEvent;
        /// <summary> Метод для подъёма выражения на один вверх </summary>
        private void MoveUpExpression(object sender, MouseButtonEventArgs e)
        {
            MoveUpExpressionEvent?.Invoke(ExpOrder);
        }

        public event ExpressionOperationHandler MoveDownExpressionEvent;
        /// <summary> Метод для опускания выражения на один вниз </summary>
        private void MoveDownExpression(object sender, MouseButtonEventArgs e)
        {
            MoveDownExpressionEvent?.Invoke(ExpOrder);
        }

    }
}
