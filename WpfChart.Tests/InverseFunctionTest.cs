using System;
using System.Collections.Generic;
using System.Linq;
using WpfChart.Domain;
using Xunit;

namespace WpfChart.Tests
{
    public class InverseFunctionTest
    {
        [Fact]
        public void InvertedFunctionMatchesExpected()
        {
            // Arrange
            var points = new List<(double x, double y)>
            {
                (0,4),
                (10,8),
                (20,16),
                (30,32),
                (40,64),
            };
            var expectedInvertedPoints = new List<(double x, double y)>
            {
                (4,0),
                (8,10),
                (16,20),
                (32,30),
                (64,40),
            };
            var function = new PiecewiseLinearFunction(points);

            // Act
            var inverseFunction = function.GetInverseFunction();

            // Assert
            Assert.False(inverseFunction.Points.Where((p, i) => p != expectedInvertedPoints[i]).Any());
        }

        [Fact]
        public void InverseTwiceFunctionMatchesInitial()
        {
            // Arrange
            var points = new List<(double x, double y)>
            {
                (0,4),
                (10,8),
                (20,16),
                (30,32),
                (40,64),
            };
            var function = new PiecewiseLinearFunction(points);

            // Act
            var twiceInverseFunction = function.GetInverseFunction().GetInverseFunction();

            // Assert
            Assert.False(twiceInverseFunction.Points.Where((p, i) => p != points[i]).Any());
        }

        [Fact]
        public void CantCreateInversedFunction()
        {
            // Arrange
            var points = new List<(double x, double y)>
            {
                (0,4),
                (10,8),
                (20,16),
                (30,8),
                (40,64),
            };
            var function = new PiecewiseLinearFunction(points);

            Assert.Throws<Exception>(() => function.GetInverseFunction());
        }
    }
}
