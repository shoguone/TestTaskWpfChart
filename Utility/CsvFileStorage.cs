using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestTaskWpfChart.Utility
{
    public class CsvFileStorage
    {
        public static (string[], List<string[]>) Load(string fileName, char separator = '\0')
        {
            if (!File.Exists(fileName))
                return Empty();

            using (var r = new StreamReader(fileName, Encoding.UTF8))
            {
                var headerRow = r.ReadLine();
                if (headerRow == null)
                    return Empty();

                if (separator == '\0')
                {
                    // Auto detect
                    var commaCount = headerRow.Count(c => c == ',');
                    var semicolonCount = headerRow.Count(c => c == ';');
                    separator = commaCount > semicolonCount ? ',' : ';';
                }

                var headers = headerRow.Split(separator);
                var items = new List<string[]>();

                while (!r.EndOfStream)
                {
                    var line = r.ReadLine();
                    if (line == null || line.StartsWith("%") || line.StartsWith("//"))
                        continue;
                    items.Add(line.Split(separator));
                }

                return (headers, items);
            }

            (string[], List<string[]>) Empty() => (Array.Empty<string>(), new List<string[]>());
        }

        public static void Store(string[] headers, List<string[]> items, string fileName, char separator = ';')
        {
            using (var fileWriter = new FileStream(fileName, FileMode.Create))
            using (var writer = new StreamWriter(fileWriter, Encoding.UTF8))
            {
                writer.WriteLine(string.Join(separator, headers));
                foreach (var row in items)
                {
                    writer.WriteLine(string.Join(separator, row));
                }

                writer.Flush();
            }
        }
    }
}
