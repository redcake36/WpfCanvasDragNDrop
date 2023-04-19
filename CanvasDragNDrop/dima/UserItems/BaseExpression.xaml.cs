using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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

        public string Exp
        {
            get { return (string)GetValue(ExpProperty); }
            set { SetValue(ExpProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Exp.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExpProperty =
            DependencyProperty.Register("Exp", typeof(string), typeof(BaseExpression));

        public string DefinedVariable
        {
            get { return (string)GetValue(DefinedVariableProperty); }
            set { SetValue(DefinedVariableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefinedVariable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefinedVariableProperty =
            DependencyProperty.Register("DefinedVariable", typeof(string), typeof(BaseExpression));

        public string NeededVars
        {
            get { return (string)GetValue(NeededVarsProperty); }
            set { SetValue(NeededVarsProperty, value); }
        }
        // Using a DependencyProperty as the backing store for NeededVars.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NeededVarsProperty =
            DependencyProperty.Register("NeededVars", typeof(string), typeof(BaseExpression));

        public int ExpOrder
        {
            get { return (int)GetValue(ExpOrderProperty); }
            set { SetValue(ExpOrderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ExpOrder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExpOrderProperty =
            DependencyProperty.Register("ExpOrder", typeof(int), typeof(BaseExpression), new PropertyMetadata(0));



    }
}
