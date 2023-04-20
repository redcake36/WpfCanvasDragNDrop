using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop
{
    public class MashaDBClass
    {
        public List<Parameters> AllParameters;
        public List<Parameters> DefaultParameters;
        public string Description;
        public List<BlockExpression> Expressions;
        public int Id;
        public List<Flow> InputFlows = new List<Flow>();
        public List<Flow> OutputFlows = new List<Flow>();
        public string Title;
        public MashaDBClass(string d, int i, string t)
        {
            Description = d;
            Id = i;
            Title = t;
        }
        public MashaDBClass()
        {
            AllParameters = new List<Parameters>();
            DefaultParameters = new List<Parameters>();
            Description = String.Empty;
            Expressions = new List<BlockExpression>();
            Id = 0;
            InputFlows = new List<Flow>();
            OutputFlows = new List<Flow>();
            Title = String.Empty;
        }
    }
}
