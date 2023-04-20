using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop
{
    public class MashaDBClass
    {
        public string description;
        public int id;
        public string title;
        public List<int> input_flows = new List<int>();
        public List<int> output_flows = new List<int>();
        public MashaDBClass(string d, int i, string t) {
            description= d;
            id= i;
            title= t;
        }
        public MashaDBClass()
        {
            description = "";
            id = 0;
            title = "";
        }
    }
}
