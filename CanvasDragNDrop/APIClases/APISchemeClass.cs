using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop.APIClases
{
    public class APISchemeClass
    {
		public int SchemaId
		{
			get { return _schemaId; }
			set { _schemaId = value; }
		}
		private int _schemaId;

		public string SchemaName
		{
			get { return _schemaName; }
			set { _schemaName = value; }
		}
		private string _schemaName;

	}
}
