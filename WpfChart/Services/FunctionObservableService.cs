using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using WpfChart.Domain;
using WpfChart.Model;

namespace WpfChart.Services
{
    public class FunctionObservableService : INotifyPropertyChanged
    {
        private ObservableCollection<EditableDataPoint> _points;

        public FunctionObservableService(PiecewiseLinearFunction function, Action onPointsChanged)
        {
            _OnPointsChanged += onPointsChanged;
            _points = CreateEditablePoints(function);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private event Action _OnPointsChanged;

        public ObservableCollection<EditableDataPoint> Points
        {
            get => _points;
            private set
            {
                _points = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Points)));
            }
        }

        public PiecewiseLinearFunction Function => PointsToFunction(Points);

        public void ReInit(PiecewiseLinearFunction function)
        {
            ClearPoints();
            Points = CreateEditablePoints(function);
        }

        private static PiecewiseLinearFunction PointsToFunction(ObservableCollection<EditableDataPoint> points)
        {
            return new PiecewiseLinearFunction(points.Select(p => (p.X, p.Y)).ToList());
        }

        private ObservableCollection<EditableDataPoint> CreateEditablePoints(PiecewiseLinearFunction function)
        {
            var points = new ObservableCollection<EditableDataPoint>();
            points.CollectionChanged += Points_CollectionChanged;
            function.Points.ForEach(p => points.Add(new EditableDataPoint(p.x, p.y)));
            return points;
        }

        private void ClearPoints()
        {
            if (Points != null)
            {
                Points.CollectionChanged -= Points_CollectionChanged;
                foreach (var point in Points)
                    point.PropertyChanged -= Point_PropertyChanged;

                Points.Clear();
            }
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

            _OnPointsChanged?.Invoke();
        }

        private void Point_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            _OnPointsChanged?.Invoke();
        }
    }
}
