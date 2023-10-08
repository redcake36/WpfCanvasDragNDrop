using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop
{
    public class DBBlockModelClass: NotifyPropertyChangedClass
    {

        public List<Parameter> CustomParameters;
        public List<Parameter> DefaultParameters;
        public string Description;
        public List<BlockExpression> Expressions;
        public List<Flow> InputFlows;
        public int ModelId;
        public List<Flow> OutputFlows;
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }
        private string _title;
        public DBBlockModelClass(string d, int i, string t)
        {
            Description = d;
            ModelId = i;
            Title = t;
            CustomParameters = new List<Parameter>();
            DefaultParameters = new List<Parameter>();
            Expressions = new List<BlockExpression>();
            InputFlows = new List<Flow>();
            OutputFlows = new List<Flow>();
        }
        public DBBlockModelClass()
        {
            CustomParameters = new List<Parameter>();
            DefaultParameters = new List<Parameter>();
            Description = string.Empty;
            Expressions = new List<BlockExpression>();
            ModelId = 0;
            InputFlows = new List<Flow>();
            OutputFlows = new List<Flow>();
            Title = string.Empty;
        }
        public override string ToString()
        {
            return (string.Join("\n",CustomParameters) + "\n" +
                string.Join("\n", DefaultParameters) + "\n" +
                Description + "\n" +
                string.Join("\n", Expressions) + "\n" +
                string.Join("\n", InputFlows) + "\n" +
                ModelId + "\n" +
                string.Join("\n", OutputFlows) + "\n" +
                Title);
        }
    }
}
