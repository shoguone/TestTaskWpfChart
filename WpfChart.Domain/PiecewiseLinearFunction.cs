using System;
using System.Collections.Generic;

namespace WpfChart.Domain
{
    public class PiecewiseLinearFunction
    {
        public PiecewiseLinearFunction(List<(double x, double y)> points)
        {
            Points = points;
        }

        public List<(double x, double y)> Points { get; set; }

        public PiecewiseLinearFunction GetInverseFunction()
        {
            if (!HasInverseFunction())
                throw new Exception("Can't create inverse for the specified function!");

            var points = new List<(double x, double y)>();
            for (var i = 0; i < Points.Count; i++)
            {
                points.Add(new(Points[i].y, Points[i].x));
            }

            return new PiecewiseLinearFunction(points);
        }

        public bool HasInverseFunction()
        {
            if (Points.Count < 3)
                return true;

            var firstY = Points[0].y;
            var secondY = Points[1].y;
            var isYGrowing = secondY > firstY;

            var prevY = isYGrowing ? double.MinValue : double.MaxValue;
            for (var i = 0; i < Points.Count; i++)
            {
                var currentY = Points[i].y;
                if (isYGrowing && currentY <= prevY)
                    return false;

                if (!isYGrowing && currentY >= prevY)
                    return false;

                prevY = currentY;
            }

            return true;
        }
    }
}
