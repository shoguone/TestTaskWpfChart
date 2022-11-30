using System.ComponentModel;
using System.Windows;
using WpfChart.ViewModels;

namespace WpfChart
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Need to call this after InitializeComponent, to have PlotView initialized
            AdjustPlotMouseBehaviour();
        }

        private void AdjustPlotMouseBehaviour()
        {
            if (DataContext is not MainViewModel vm)
                return;

            vm.AdjustPlotMouseBehaviour();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (DataContext is not MainViewModel vm)
                return;

            if (vm.TrySaveChanges() == MessageBoxResult.Cancel)
                e.Cancel = true;
        }
    }

}
