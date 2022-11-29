using System.Collections.Generic;
using WpfChart.Domain;
using WpfChart.Services;
using Xunit;

namespace WpfChart.Test
{
    public class CsvTest
    {
        [Fact]
        public void SavedAndLoadedFunctionsDoMatch()
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
            var filename = "unit-test-file.csv";

            // Act
            FunctionCsvService.SaveToCsv(function, filename);
            var outFunction = FunctionCsvService.LoadFromCsv(filename);

            // Assert
            var fault = false;
            for (var i = 0; i < points.Count; i++)
            {
                if (points[i] != function.Points[i] || points[i] != outFunction.Points[i])
                {
                    fault = true;
                    continue;
                }
            }

            Assert.False(fault);
        }
    }
}