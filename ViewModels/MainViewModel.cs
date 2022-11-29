using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using TestTaskWpfChart.Infrastructure;
using TestTaskWpfChart.Services;

namespace TestTaskWpfChart.ViewModels
{
    public class MainViewModel
    {
        private readonly string csvFilename = "function.csv";

        private bool _isDirty = false;

        public MainViewModel()
        {
            var function = FunctionCsvService.LoadFromCsv(csvFilename);

            //var function = new PiecewiseLinearFunction(new List<(double x, double y)>
            //{
            //    (0, 4),
            //    (10, 12),
            //    (20, 16),
            //    (30, 25),
            //    (40, 5),
            //});

            FunctionObservableService = new FunctionObservableService(function, UpdateData);
            Model = CreatePlotModel(FunctionObservableService.Points);
        }


        public FunctionObservableService FunctionObservableService { get; set; }

        public PlotModel Model { get; private set; }

        private void UpdateData()
        {
            _isDirty = true;
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
