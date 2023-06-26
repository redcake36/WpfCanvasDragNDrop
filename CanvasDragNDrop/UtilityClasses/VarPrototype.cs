using CanvasDragNDrop.UserItems;
using org.mariuszgromada.math.mxparser.mathcollection;

namespace CanvasDragNDrop
{
    public class VarPrototype
    {
        public int ParametrId { get; set; }
        public string Symbol { get; set; }
        public string Title { get; set; }
        public string Units { get; set; }

        public VarPrototype(int parametrId, string symbol, string title, string units)
        {
            ParametrId = parametrId;
            Symbol = symbol;
            Title = title;
            Units = units;
        }
        public VarPrototype()
        {
            ParametrId = 0;
            Symbol = string.Empty;
            Title = string.Empty;
            Units = string.Empty;
        }

        public override string ToString()
        {
            return string.Format("VarPrototype:\n ParametrId:{0}\n Symbol:{1}\n Title:{2}\n Units:{3}\n", ParametrId, Symbol, Title, Units);
        }
    }
}