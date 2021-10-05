using System;
using System.Collections.Generic;
using System.IO;
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
using TCEvropeyskiy.ViewModels;
using TCSChelkovskiy.Utilities;
using TCSchelkovskiyAPI.Models;

namespace TCSChelkovskiy.Views
{
    /// <summary>
    /// Логика взаимодействия для AboutTradeCenter.xaml
    /// </summary>
    public partial class AboutTradeCenter : Page
    {

        public AboutTradeCenter(AboutMallModel aboutMallModel)
        {
            InitializeComponent();         
            Unloaded+= OnUnloaded;
            Model = aboutMallModel;
        }

        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(
            "Model", typeof(AboutMallModel), typeof(AboutTradeCenter), new PropertyMetadata(default(AboutMallModel)));

        public AboutMallModel Model
        {
            get => (AboutMallModel) GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Image1.Dispose();
            Image2?.Dispose();
        }

        public static readonly DependencyProperty Image1Property = DependencyProperty.Register(
            "Image1", typeof(DisposableImage), typeof(AboutTradeCenter), new PropertyMetadata(default(DisposableImage)));

        public DisposableImage Image1
        {
            get { return (DisposableImage) GetValue(Image1Property); }
            set { SetValue(Image1Property, value); }
        }

        public static readonly DependencyProperty Image2Property = DependencyProperty.Register(
            "Image2", typeof(DisposableImage), typeof(AboutTradeCenter), new PropertyMetadata(default(DisposableImage)));

        public DisposableImage Image2
        {
            get { return (DisposableImage) GetValue(Image2Property); }
            set { SetValue(Image2Property, value); }
        }

        private async void loaded(object sender, RoutedEventArgs e)
        {
            string prefix = Model.ImagesPrefix;
            var images = Model.Images;
            foreach (var i in images)
            {
                var disposableImage = await Services.ImageDownloader.DownloadImage(Path.Combine(prefix,i), Path.GetFileName(i));
                allImage.Add(disposableImage);
            }
            try
            {
                Image1 = allImage[0];
                Image2 = allImage[1];
            }
            catch { }
          
        }

        private List<DisposableImage> allImage = new List<DisposableImage>();


        private RelayCommand goContacts;
        public RelayCommand GoContacts => goContacts ??= new RelayCommand(obj =>
        {
            NavigationService?.Navigate(new Contacts(Memory.KioskObjects.Contacts));
        });

        private RelayCommand goFeedback;
        public RelayCommand GoFeedback => goFeedback ??= new RelayCommand(obj =>
        {
            NavigationService?.Navigate(new Feedback());
        });

        private RelayCommand goRules;
        public RelayCommand GoRules => goRules ??= new RelayCommand(obj =>
        {
            NavigationService?.Navigate(new Rules());
        });

        private RelayCommand goVacancies;
        public RelayCommand GoVacancies => goVacancies ??= new RelayCommand(obj =>
        {
            NavigationService?.Navigate(new Vacancies(Memory.KioskObjects.Vacancies));
        });

        private RelayCommand goWC;
        public RelayCommand GoWC => goWC ??= new RelayCommand(obj =>
        {
            NavigationService?.Navigate(new WCPage());
        });
        private RelayCommand goParking;
        public RelayCommand GoParking => goParking ??= new RelayCommand(obj =>
        {
            NavigationService?.Navigate(new Parking());
        });
    }
}
