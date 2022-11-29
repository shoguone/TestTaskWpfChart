using Microsoft.Win32;

namespace TestTaskWpfChart.Services
{
    public class DialogService
    {
        public static (bool, string?) OpenFileDialog()
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                return (true, openFileDialog.FileName);

            return (false, null);
        }

        public static (bool, string?) SaveFileDialog()
        {
            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                return (true, saveFileDialog.FileName);

            return (false, null);
        }
    }
}
