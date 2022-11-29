using OxyPlot;
using System.ComponentModel;

namespace TestTaskWpfChart.Infrastructure
{
    public class EditableDataPoint : IDataPointProvider, INotifyPropertyChanged
    {
        private double x;
        private double y;

        public double X
        {
            get => x;
            set
            {
                x = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(X)));
            }
        }
        public double Y
        {
            get => y;
            set
            {
                y = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Y)));
            }
        }

        public EditableDataPoint()
        {
        }

        public EditableDataPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public DataPoint GetDataPoint()
        {
            return new DataPoint(X, Y);
        }
    }
}
