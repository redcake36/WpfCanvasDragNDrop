﻿using CanvasDragNDrop.UtilityClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop
{
    public class FlowInterconnectLine : NotifyPropertyChangedClass
    {

        public FlowConnector InputFlowConnector => _inputFlowConnector;
        private FlowConnector _inputFlowConnector = null;

        public FlowConnector OutputFlowConnector => _outputFlowConnector;
        private FlowConnector _outputFlowConnector = null;

        [JsonIgnore]
        public double InputPointX
        {
            get { return _inputPointX; }
            set { _inputPointX = value; OnPropertyChanged(); }
        }
        private double _inputPointX = 0;

        [JsonIgnore]
        public double InputPointY
        {
            get { return _inputPointY; }
            set { _inputPointY = value; OnPropertyChanged(); }
        }
        private double _inputPointY = 0;

        [JsonIgnore]
        public double OutputPointX
        {
            get { return _outputPointX; }
            set { _outputPointX = value; OnPropertyChanged(); }
        }
        private double _outputPointX = 0;

        [JsonIgnore]
        public double OutputPointY
        {
            get { return _outputPointY; }
            set { _outputPointY = value; OnPropertyChanged(); }
        }
        private double _outputPointY = 0;

        public void SetFlowConnector(FlowConnector flowConnector, bool isInput)
        {
            if (isInput)
            {
                bool checkResult = CheckInterconnectionPossibility(flowConnector, _outputFlowConnector);
                if (!checkResult || flowConnector.InterconnectLine != null)
                {
                    throw new Exception("Can't interconnect");
                }
                if (_inputFlowConnector != null)
                {
                    _inputFlowConnector.PropertyChanged -= ParentPropertyChangedEventHandler;
                    _inputFlowConnector.InterconnectLine = null;
                }
                _inputFlowConnector = flowConnector;
                if (_inputFlowConnector != null)
                {
                    _inputFlowConnector.PropertyChanged += ParentPropertyChangedEventHandler;
                    _inputFlowConnector.InterconnectLine = this;
                }
            }
            else
            {
                bool checkResult = CheckInterconnectionPossibility(_inputFlowConnector, flowConnector);
                if (!checkResult || flowConnector.InterconnectLine != null)
                {
                    throw new Exception("Can't interconnect");
                }
                if (_outputFlowConnector != null)
                {
                    _outputFlowConnector.PropertyChanged -= ParentPropertyChangedEventHandler;
                    _outputFlowConnector.InterconnectLine = null;
                }
                _outputFlowConnector = flowConnector;
                if (_outputFlowConnector != null)
                {
                    _outputFlowConnector.PropertyChanged += ParentPropertyChangedEventHandler;
                    _outputFlowConnector.InterconnectLine = this;
                }
            }
            RecalculateFlowPoints();
        }

        public bool CheckInterconnectionPossibility(FlowConnector inputConnector, FlowConnector outputConnector)
        {
            if ( _inputFlowConnector == null && _outputFlowConnector == null)
            {
                return true;
            }

            if(inputConnector == null || outputConnector == null)
            {
                return false;
            }

            bool DifferentInstancesCondition = inputConnector.BlockInstanceID != outputConnector.BlockInstanceID;
            bool FlowTypesCondition = inputConnector.FlowTypeID == outputConnector.FlowTypeID;
            if (DifferentInstancesCondition  && FlowTypesCondition)
            {
                return true;
            }

            return false;
        }

        public void RecalculateFlowPoints()
        {
            if (_inputFlowConnector != null)
            {
                InputPointX = _inputFlowConnector.FlowPointOffsetLeft;
                InputPointY = _inputFlowConnector.FlowPointOffsetTop;
            }
            if (_outputFlowConnector != null)
            {
                OutputPointX = _outputFlowConnector.FlowPointOffsetLeft;
                OutputPointY = _outputFlowConnector.FlowPointOffsetTop;
            }
        }

        internal void UpdateLiveInterconnectPoint(System.Windows.Point point)
        {
            if (_inputFlowConnector == null)
            {
                InputPointX = point.X;
                InputPointY = point.Y;
            }
            else
            {
                if (_outputFlowConnector == null)
                {
                    OutputPointX = point.X;
                    OutputPointY = point.Y;
                }
            }
        }

        public void ParentPropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(FlowConnector.FlowPointOffsetLeft):
                case nameof(FlowConnector.FlowPointOffsetTop):
                    RecalculateFlowPoints();
                    break;
            }
        }

    }
}
