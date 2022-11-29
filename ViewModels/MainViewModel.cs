using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using TestTaskWpfChart.Domain;
using TestTaskWpfChart.Infrastructure;

namespace TestTaskWpfChart.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            var function = new PiecewiseLinearFunction(new List<(double x, double y)>
            {
                (0, 4),
                (10, 12),
                (20, 16),
                (30, 25),
                (40, 5),
            });

            Points = CreateEditablePoints(function);

            Model = CreatePlotModel(Points);
        }

        public PlotModel Model { get; private set; }

        public ObservableCollection<EditableDataPoint> Points { get; set; }

        private ObservableCollection<EditableDataPoint> CreateEditablePoints(PiecewiseLinearFunction function)
        {
            var points = new ObservableCollection<EditableDataPoint>();
            points.CollectionChanged += Points_CollectionChanged;
            function.Points.ForEach(p => points.Add(new EditableDataPoint(p.x, p.y)));
            return points;
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

            Model?.InvalidatePlot(true);
        }

        private void Point_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Model?.InvalidatePlot(true);
        }

        private static PlotModel CreatePlotModel(IEnumerable<EditableDataPoint> points)
        {
            var tmp = new PlotModel();

            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            var series = new LineSeries { Title = "Chart", MarkerType = MarkerType.Square };
            series.ItemsSource = points;

            tmp.Series.Add(series);
            return tmp;
        }

    }
}
