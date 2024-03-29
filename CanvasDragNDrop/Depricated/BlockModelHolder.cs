﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CanvasDragNDrop.APIClases;

namespace CanvasDragNDrop.UtilityClasses
{
    public class BlockModelHolder
    {
        public APIBlockModelClass dBBlockModel;
        public VisualBlockComponent visualBlockComponent;

        public BlockModelHolder(BlockModelHolder bhm)
        {
            //visualBlockComponent = new VisualBlockComponent {DataContext = bhm.visualBlockComponent.DataContext };
            visualBlockComponent = new VisualBlockComponent(bhm.visualBlockComponent);
            dBBlockModel = bhm.dBBlockModel;
        }

        public BlockModelHolder(APIBlockModelClass blockModel)
        {
            visualBlockComponent = new VisualBlockComponent(this, blockModel);
            dBBlockModel = blockModel;
        }
    }
}
