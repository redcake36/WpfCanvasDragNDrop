using org.mariuszgromada.math.mxparser.mathcollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop
{
    public class Parameter
    {
        public int ParametrId { get; set; }
        public string Title { get; set; }
        public string Units { get; set; }
        public string VariableName { get; set; }

        public Parameter()
        {
            ParametrId = 0;

            Title = string.Empty;
            Units = string.Empty;
            VariableName = string.Empty;
        }

        public Parameter(int parametrId, string variableName, string title, string units)
        {
            this.ParametrId = parametrId;
            this.VariableName = variableName;
            this.Title = title;
            this.Units = units;
        }

        public override string ToString()
        {
            return string.Format("Parameter:\n " +
                "ParametrId:{0}\n " +
                "VariableName:{1}\n " +
                "Title:{2}\n " +
                "Units:{3}\n", ParametrId,VariableName,Title,Units);
        }
    }
}
