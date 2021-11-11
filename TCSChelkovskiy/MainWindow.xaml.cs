using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TCSChelkovskiy.Views;

namespace TCEvropeyskiy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.MainWindowViewModel(this);
            
        }

        private DispatcherTimer timer = new DispatcherTimer();
        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += TimerOnTick;
            await Task.Delay(120000);
            timer.Start();
        }

        private int _index = 0;
        private void TimerOnTick(object? sender, EventArgs e)
        {
            bannersListBox.ScrollIntoView(bannersListBox.Items[_index]);
            _index++;
            if (_index + 1 == bannersListBox.Items.Count)
                _index = 0;
        }
    }
}
