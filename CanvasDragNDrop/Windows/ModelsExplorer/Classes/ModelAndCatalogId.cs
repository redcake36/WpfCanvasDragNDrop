using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop.Windows.ModelsExplorer.Classes
{
    public class ModelAndCatalogId
    {
        public int ModelId
        {
            get { return _modelId; }
            set { _modelId = value; }
        }
        private int _modelId = 0;
        public int CatalogId
        {
            get { return _catalogId; }
            set { _catalogId = value; }
        }
        private int _catalogId = 0;
    }
}
