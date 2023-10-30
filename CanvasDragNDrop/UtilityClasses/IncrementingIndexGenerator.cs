using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop.UtilityClasses
{
    public class IncrementingIndexGenerator
    {
		public int IncrementedIndex
        {
			get { return _incrementedIndex++; }
		}
		private int _incrementedIndex = 0;
	}
}
