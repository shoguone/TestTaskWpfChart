using System.Linq;
using WpfChart.Domain;
using WpfChart.Utility;

namespace WpfChart.Services
{
    public class FunctionCsvService
    {
        private static readonly string[] pointsHeaders = new string[] { "X", "Y" };

        public static PiecewiseLinearFunction LoadFromCsv(string filename)
        {
            var (headers, rows) = CsvFileStorage.Load(filename);
            var points = rows.Select(r => (double.Parse(r[0]), double.Parse(r[1]))).ToList();
            var function = new PiecewiseLinearFunction(points);
            return function;
        }

        public static void SaveToCsv(PiecewiseLinearFunction function, string filename)
        {
            var rows = function.Points.Select(p => new string[] { p.x.ToString(), p.y.ToString() }).ToList();
            CsvFileStorage.Store(pointsHeaders, rows, filename);
        }
    }
}
