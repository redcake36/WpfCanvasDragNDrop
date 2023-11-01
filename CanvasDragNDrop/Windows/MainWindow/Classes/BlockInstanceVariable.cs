using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CanvasDragNDrop.Windows.MainWindow.Classes
{
    public class BlockInstanceVariable
    {
		public enum Types : int
		{
			NotSet,
			Input,
			Output,
			Default,
			Custom
		}

		public Types Type
		{
			get { return _type; }
			set { _type = value; }
		}
		private Types _type = 0;

		public int VariableId
		{
			get { return _variableId; }
			set { _variableId = value; }
		}
		private int _variableId = 0;

		public int VariablePrototypeId
		{
			get { return _variablePrototypeId; }
			set { _variablePrototypeId = value; }
		}
		private int _variablePrototypeId = 0;

		public string VariableName
		{
			get { return _variableName; }
			set { _variableName = value; }
		}
		private string _variableName;

		public double Value
		{
			get { return _value; }
			set { _value = value; }
		}
		private double _value;

		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}
		private string _title = "";

		public string Units
		{
			get { return _units; }
			set { _units = value; }
		}
		private string _units = "";

		public BlockInstanceVariable(Types type, int variableId, int varaiblePrototypeId, string variableName, double value, string title, string units)
        {
            Type = type;
            VariableId = variableId;
            VariablePrototypeId = varaiblePrototypeId;
            VariableName = variableName;
            Value = value;
            Title = title;
            Units = units;
        }


		public BlockInstanceVariable(string variableName, double value)
		{
			VariableName = variableName;
			Value = value;
		}
    }
}
