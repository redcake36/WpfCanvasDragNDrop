using System;
using System.Windows;
using System.Windows.Input;
using CanvasDragNDrop.UtilityClasses;

namespace CanvasDragNDrop.Windows.MainWindow.Classes
{
    public class CanvasOverseerClass : NotifyPropertyChangedClass
    {
        public Point ViewportMousePosition { get; private set; } = new Point(0, 0);
        public Point ViewportMousePositionDelta { get; private set; } = new Point(0, 0);
        public Point AreaMousePosition { get; private set; } = new Point(0, 0);
        public ViewportStates ViewportState { get; private set; } = ViewportStates.Idle;
        public OperationStates OperationState { get; private set; } = OperationStates.Idle;
        public int SelectedBlockIndex { get; private set; } = -1;
        public int SelectedFlowInterconnectLineIndex { get; private set; } = -1;
        public Cursor CursorType
        {
            get { return _cursorType; }
            set { _cursorType = value; OnPropertyChanged(); }
        }
        private Cursor _cursorType = Cursors.Cross;


        public enum ViewportStates : int
        {
            Idle,
            ViewportDragging
        }

        public enum OperationStates : int
        {
            Idle,
            BlockDragging,
            FlowInterconnectingStarted,
            FlowInterconnectionFinishing,
            OpeningBlockProperties
        }

        public bool CanDragViewport
        {
            get => ViewportState == ViewportStates.ViewportDragging;
        }

        public bool CanDragBlock
        {
            get => ViewportState == ViewportStates.Idle && OperationState == OperationStates.BlockDragging;
        }

        public bool CanStartInterconnection
        {
            get => OperationState == OperationStates.FlowInterconnectingStarted;
        }

        public bool CanFinishInterconnection
        {
            get => OperationState == OperationStates.FlowInterconnectionFinishing;
        }

        public bool CanUpdateLiveInterconnectionPoint
        {
            get => OperationState == OperationStates.FlowInterconnectionFinishing && ViewportState == ViewportStates.Idle;
        }

        public bool CanCalculateScheme
        {
            get => OperationState == OperationStates.Idle;
        }

        public bool CanOpenBlockProperties
        {
            get => OperationState == OperationStates.OpeningBlockProperties && ViewportState == ViewportStates.Idle;
        }
        public bool CanCalcScheme
        {
            get => OperationState == OperationStates.Idle && ViewportState == ViewportStates.Idle;
        }

        public void ViewportMouseMoved(Point newMouseViewportPosition, Point newAreaMouseposition)
        {
            ViewportMousePositionDelta = new Point(newMouseViewportPosition.X - ViewportMousePosition.X, newMouseViewportPosition.Y - ViewportMousePosition.Y);
            ViewportMousePosition = newMouseViewportPosition;
            AreaMousePosition = newAreaMouseposition;
        }

        public void ViewportMidleMouseDown()
        {
            ViewportState = ViewportStates.ViewportDragging;
        }

        public void ViewportMidleMouseUp()
        {
            ViewportState = ViewportStates.Idle;
        }

        public void BlockLeftMouseDown(int blockIndex, int clicsCount)
        {
            if (OperationState == OperationStates.Idle)
            {
                if (clicsCount == 1)
                {
                    OperationState = OperationStates.BlockDragging;
                }
                else
                {
                    OperationState = OperationStates.OpeningBlockProperties;
                }
                SelectedBlockIndex = blockIndex;
            }
        }

        public void BlockLeftMouseUp()
        {
            if (OperationState == OperationStates.BlockDragging)
            {
                OperationState = OperationStates.Idle;
                SelectedBlockIndex = -1;
            }
        }

        public void EnterFlowInterconnectMode()
        {
            if (OperationState == OperationStates.Idle)
            {
                OperationState = OperationStates.FlowInterconnectingStarted;
                CursorType = Cursors.Pen;
            }
        }

        public void ExitFlowInterconnectionMode()
        {
            if (OperationState == OperationStates.FlowInterconnectingStarted)
            {
                OperationState = OperationStates.Idle;
                CursorType = Cursors.Cross;
            }
        }

        public void ConnectFirstFlowInterconnectPoint(int flowInterconnectIndex)
        {
            if (CanStartInterconnection)
            {
                SelectedFlowInterconnectLineIndex = flowInterconnectIndex;
                OperationState = OperationStates.FlowInterconnectionFinishing;
            }
        }

        public void ConnectSecondFlowInterconnectPoint()
        {
            if (CanFinishInterconnection)
            {
                SelectedFlowInterconnectLineIndex = -1;
                OperationState = OperationStates.Idle;
                CursorType = Cursors.Cross;
            }
        }

        //public void BlockLeftMouseDouble(int blockIndex)
        //{
        //    if ((OperationState == OperationStates.Idle || OperationState == OperationStates.BlockDragging) && ViewportState == ViewportStates.Idle)
        //    {
        //        SelectedBlockIndex = blockIndex;
        //        OperationState = OperationStates.OpeningBlockProperties;
        //    }
        //}

        internal void BlockPropertiesOpened()
        {
            if (OperationState == OperationStates.OpeningBlockProperties)
            {
                OperationState = OperationStates.Idle;
                SelectedBlockIndex = -1;
            }
        }

        //internal void ViewportKeyDown(Key key)
        //{
        //    if ()
        //}
    }
}
