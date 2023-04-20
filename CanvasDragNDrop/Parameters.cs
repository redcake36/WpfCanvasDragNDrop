using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop
{
    public class Parameters
    {
        int id;
        string name;
        string title;
        string units;
        float? value;

        public Parameters()
        {
            id = 0;
            name = string.Empty;
            title = string.Empty;
            units = string.Empty;
            value = 0;
        }

        public Parameters(int id, string name, string title, string units, float? value)
        {
            this.id = id;
            this.name = name;
            this.title = title;
            this.units = units;
            this.value = value;
        }
    }
}
