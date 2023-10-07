using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop.UtilityClasses
{
    public class BlockModelHolder
    {
        public DBBlockModelClass dBBlockModel;
        public VisualBlockComponent visualBlockComponent;
        public CalcResults calcResults;

        public BlockModelHolder(BlockModelHolder bhm)
        {
            //visualBlockComponent = new VisualBlockComponent {DataContext = bhm.visualBlockComponent.DataContext };
            visualBlockComponent = new VisualBlockComponent(bhm.visualBlockComponent);
            dBBlockModel = bhm.dBBlockModel;
        }

        public BlockModelHolder(DBBlockModelClass blockModel)
        {
            visualBlockComponent = new VisualBlockComponent(this, blockModel);
            dBBlockModel = blockModel;
        }
    }
}
