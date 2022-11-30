using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using WpfChart.Commands;
using WpfChart.Domain;
using WpfChart.Model;
using WpfChart.Services;

namespace WpfChart.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly string csvFilename = "function.csv";
        private PlotModel _model;

        public MainViewModel()
        {
            var function = FunctionCsvService.LoadFromCsv(csvFilename);
            FunctionObservableService = new FunctionObservableService(function, UpdateData);
            _model = CreatePlotModel(FunctionObservableService.Points);
            IsDirty = false;

            ImportCmd = new ActionCommand(_ => Import());
            ExportCmd = new ActionCommand(_ => Export());
            InvertCmd = new ActionCommand(_ => Invert());
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public FunctionObservableService FunctionObservableService { get; set; }

        public PlotModel Model
        {
            get => _model;
            private set
            {
                _model = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Model)));
            }
        }

        private bool IsDirty { get; set; }

        public ICommand ImportCmd { get; private set; }
        public ICommand ExportCmd { get; private set; }
        public ICommand InvertCmd { get; private set; }

        public MessageBoxResult TrySaveChanges()
        {
            if (!IsDirty)
                return MessageBoxResult.None;

            var result = MessageBox.Show(
                "You are about to exit. Do you like to save your changes?",
                "Save?",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Cancel)
                return result;

            if (result == MessageBoxResult.OK || result == MessageBoxResult.Yes)
            {
                Export();
            }

            return result;
        }

        public void AdjustPlotMouseBehaviour()
        {
            if (Model == null || Model.PlotView == null)
                return;

            var plotView = Model.PlotView;
            plotView.ActualController.BindMouseDown(
                OxyMouseButton.Left,
                OxyModifierKeys.Shift,
                new DelegatePlotCommand<OxyMouseDownEventArgs>((view, controller, args) =>
                    controller.AddMouseManipulator(view, new DragDropTrackerManipulator(view), args)));
        }

        private static PlotModel CreatePlotModel(IEnumerable<EditableDataPoint> points)
        {
            var plotModel = new PlotModel();

            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            var series = new LineSeries { Title = "Chart", MarkerType = MarkerType.Square };
            series.ItemsSource = points;

            plotModel.Series.Add(series);
            return plotModel;
        }

        private void UpdateData()
        {
            IsDirty = true;
            Model?.InvalidatePlot(true);
        }

        private void Import()
        {
            var (ok, filename) = DialogService.OpenFileDialog();
            if (!ok)
                return;

            if (string.IsNullOrEmpty(filename))
            {
                MessageBox.Show("File name was not specified!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var function = FunctionCsvService.LoadFromCsv(filename);
            SetNewFunction(function);
        }

        private void Export()
        {
            var (ok, filename) = DialogService.SaveFileDialog();
            if (!ok)
                return;

            if (string.IsNullOrEmpty(filename))
            {
                MessageBox.Show("File name was not specified!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            FunctionCsvService.SaveToCsv(FunctionObservableService.Function, filename);
        }

        private void Invert()
        {
            try
            {
                var inverseFunction = FunctionObservableService.Function.GetInverseFunction();
                SetNewFunction(inverseFunction);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sorry", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SetNewFunction(PiecewiseLinearFunction function)
        {
            FunctionObservableService.ReInit(function);
            Model = CreatePlotModel(FunctionObservableService.Points);
            IsDirty = false;
        }
    }
}
