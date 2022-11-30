using OxyPlot;
using OxyPlot.Series;
using WpfChart.Model;

namespace WpfChart.Services
{
    public class DragDropTrackerManipulator : MouseManipulator
    {
        public DragDropTrackerManipulator(IPlotView plotView)
            : base(plotView)
        {
        }

        private EditableDataPoint? _currentEditablePoint;
        private LineSeries? _currentLineSeries;

        public override void Started(OxyMouseEventArgs e)
        {
            base.Started(e);
            var mouseDownArgs = e as OxyMouseDownEventArgs;
            if (mouseDownArgs == null)
                return;

            if (mouseDownArgs.HitTestResult == null)
                return;

            if (mouseDownArgs.HitTestResult.Element is not LineSeries lineSeries
                || mouseDownArgs.HitTestResult.Item is not EditableDataPoint editablePoint)
                return;

            _currentLineSeries = lineSeries;
            _currentEditablePoint = editablePoint;
        }

        public override void Delta(OxyMouseEventArgs e)
        {
            base.Delta(e);

            if (_currentLineSeries == null || _currentEditablePoint == null)
                return;

            var newPosition = _currentLineSeries.InverseTransform(e.Position);
            _currentEditablePoint.X = newPosition.X;
            _currentEditablePoint.Y = newPosition.Y;
        }

        public override void Completed(OxyMouseEventArgs e)
        {
            base.Completed(e);

            _currentLineSeries = null;
            _currentEditablePoint = null;
        }
    }
}
