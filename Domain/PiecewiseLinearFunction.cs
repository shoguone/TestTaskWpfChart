using System.Collections.Generic;

namespace TestTaskWpfChart.Domain
{
    public class PiecewiseLinearFunction
    {
        public PiecewiseLinearFunction(List<(double x, double y)> points)
        {
            Points = points;
        }

        public List<(double x, double y)> Points { get; set; }
    }
}
