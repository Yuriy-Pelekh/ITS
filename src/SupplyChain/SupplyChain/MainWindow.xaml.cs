using System.Threading;
using System.Windows;
using SupplyChain.ViewModels;

namespace SupplyChain
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (LoadingSpinner ls = new LoadingSpinner(Simulator))
            {
                ls.Owner = this;
                ls.ShowDialog();
            }
        }

        void Simulator()
        {
            for(int i = 0; i < 1500; i++)
            {
                Thread.Sleep(5);
            }
        }
    }
}
