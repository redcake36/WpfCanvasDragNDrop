using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop.Windows.ModelsExplorer.Classes
{
    public class DropAndTargetId
    {
        public int DropId
        {
            get { return _dropId; }
            set { _dropId = value; }
        }
        private int _dropId = 0;
        public int TargetId
        {
            get { return _targetId; }
            set { _targetId = value; }
        }
        private int _targetId = 0;
    }
}
