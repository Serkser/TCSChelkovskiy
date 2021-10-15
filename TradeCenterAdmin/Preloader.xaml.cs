using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TradeCenterAdmin
{
    /// <summary>
    /// Логика взаимодействия для Preloader.xaml
    /// </summary>
    public partial class Preloader : Window, INotifyPropertyChanged
    {
        public Preloader()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(
            "Progress", typeof(int), typeof(Preloader), new PropertyMetadata(default(int)));

        public int Progress
        {
            get => (int)GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }

        private int floors = 0;
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }


        async Task LoadData()
        {
            await Task.Run(async() =>
            {
                Storage.KioskObjects.LoadAllObjects();
                while (true)
                {
                    Progress = Storage.KioskObjects.LoadingPercent;
                    await Task.Delay(1000);

                    if (Progress == 100)
                    {
                        this.Close();
                        MainWindow f = new MainWindow(); f.Show();
                    }
                }
            });
           
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
