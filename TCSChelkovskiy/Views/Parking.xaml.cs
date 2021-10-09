
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TCSChelkovskiy.Services;
using TCSChelkovskiy.Utilities;
using TCSchelkovskiyAPI.Models;

namespace TCSChelkovskiy.Views
{
    /// <summary>
    /// Логика взаимодействия для Parking.xaml
    /// </summary>
    public partial class Parking : Page
    {
        public Parking()
        {
            InitializeComponent();
            ParkingFloors = Memory.KioskObjects.ParkingFloors;
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            CurrentImage?.Dispose();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (ParkingFloors.Count > 0)
            {
                var first = ParkingFloors.FirstOrDefault();
                CurrentImage = await ImageDownloader.DownloadImage(first.ImagesPrefix + first.Image, System.IO.Path.GetFileName(first.Image));
            }         
        }

        public static readonly DependencyProperty CurrentImageProperty = DependencyProperty.Register(
           "CurrentImage", typeof(DisposableImage), typeof(Parking), new PropertyMetadata(default(DisposableImage)));
        public DisposableImage CurrentImage
        {
            get => (DisposableImage)GetValue(CurrentImageProperty);
            set => SetValue(CurrentImageProperty, value);
        }
        public static readonly DependencyProperty ParkingFloorsProperty = DependencyProperty.Register(
      "ParkingFloors", typeof(ObservableCollection<ParkingModel>), typeof(Parking), new PropertyMetadata(default(ObservableCollection<ParkingModel>)));

        public ObservableCollection<ParkingModel> ParkingFloors
        {
            get => (ObservableCollection<ParkingModel>)GetValue(ParkingFloorsProperty);
            set => SetValue(ParkingFloorsProperty, value);
        }

        private ICommand _changeFloor;
        public ICommand ChangeFloor => _changeFloor ??= new RelayCommand(async f => 
        {
            var floor = f as ParkingModel;
            CurrentImage?.Dispose();
            CurrentImage 
            = await ImageDownloader.DownloadImage(floor.ImagesPrefix+floor.Image, System.IO.Path.GetFileName(floor.Image));
        });

    }
}
