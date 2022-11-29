using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using TestTaskWpfChart.Domain;
using TestTaskWpfChart.Infrastructure;

namespace TestTaskWpfChart.Services
{
    public class FunctionObservableService
    {
        private event Action OnPointsChanged;

        public FunctionObservableService(PiecewiseLinearFunction function, Action onPointsChanged)
        {
            OnPointsChanged += onPointsChanged;
            Points = CreateEditablePoints(function);
        }

        public ObservableCollection<EditableDataPoint> Points { get; private set; }

        private ObservableCollection<EditableDataPoint> CreateEditablePoints(PiecewiseLinearFunction function)
        {
            var points = new ObservableCollection<EditableDataPoint>();
            points.CollectionChanged += Points_CollectionChanged;
            function.Points.ForEach(p => points.Add(new EditableDataPoint(p.x, p.y)));
            return points;
        }

        private void ClearPoints(ObservableCollection<EditableDataPoint> points)
        {
            if (points != null)
            {
                points.CollectionChanged -= Points_CollectionChanged;
                foreach (var point in points)
                    point.PropertyChanged -= Point_PropertyChanged;

                points.Clear();
            }
        }

        private PiecewiseLinearFunction PointsToFunction(ObservableCollection<EditableDataPoint> points)
        {
            return new PiecewiseLinearFunction(points.Select(p => (p.X, p.Y)).ToList());
        }

        private void Points_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                foreach (EditableDataPoint item in e.NewItems)
                {
                    item.PropertyChanged += Point_PropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
            {
                foreach (EditableDataPoint item in e.OldItems)
                {
                    item.PropertyChanged -= Point_PropertyChanged;
                }
            }

            OnPointsChanged?.Invoke();
        }

        private void Point_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPointsChanged?.Invoke();
        }
    }
}
