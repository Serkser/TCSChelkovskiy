using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TCSChelkovskiy.Views;
using System.Windows;
using NavigationMap.Models;
using System.Collections.ObjectModel;
using TCSchelkovskiyAPI.Models;
using TCSChelkovskiy.Memory;
using NavigationMap.Core;
using TCSChelkovskiy.Models;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;
using System.Windows.Controls;
using TCSChelkovskiy.Utilities;
using TCSChelkovskiy.Services;
using System.IO;

namespace TCEvropeyskiy.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        MainWindow This;
        public MainWindowViewModel(MainWindow _this)
        {
            This = _this;
            This.frame.Navigate(new MapPage(This));

            Categories = KioskObjects.Categories;
            Stations = KioskObjects.Stations;
            Shops = KioskObjects.Shops;
            ShopsByCategory = KioskObjects.Shops;
            Gallery = KioskObjects.Gallery;
            Vacancies = KioskObjects.Vacancies;

            Contacts = KioskObjects.Contacts;
            AboutMall = KioskObjects.AboutMall;

            
            SearchText = "Поиск";
            BannersUrls = new ObservableCollection<BannerContainer>(
                 KioskObjects.Banners.Where(o => o.BannerModel.IsVisible && o.BannerModel.Ended > DateTime.Now).ToList());
            FilterFloors = KioskObjects.FilterFloors;
            Promos = KioskObjects.Promos;


            ParkingUrls = new ObservableCollection<ParkingModel>
            {

            };
            if (ParkingUrls.Count > 0)
            {
                CurrentParkingFloor = ParkingUrls.FirstOrDefault();
            }

            MessageBox.Show(BannersUrls.Count.ToString());
            LoadBanners();  /// ПРОВЕРИТЬ !!!


        }

        async Task LoadBanners()
        {
            try
            {
                foreach (var img in BannersUrls)
                {
                    img.Image = await ImageDownloader.DownloadImage(Path.Combine("uploads/banners/" + img.BannerModel.Image), Path.GetFileName(img.BannerModel.Image));
                }
            }
            catch { }
        }
        int currentSlideIndex = 0;
        async Task SlideBanners()
        {
         //  This.bannersListBox.ScrollIntoView //
        }


        #region Методы и команды для хедера
        private RelayCommand tapSearch;
        public RelayCommand TapSearch
        {
            get
            {
                return tapSearch ??
                    (tapSearch = new RelayCommand(obj =>
                    {
                        ShopsByCategory = Shops;
                        Search page = new Search();
                        page.DataContext = this;
                        This.frame.Navigate(page);
                    }));
            }
        }
        #endregion

        #region Прочие свойства
        private AboutMallModel aboutMall;
        public AboutMallModel AboutMall
        {
            get
            {
                return aboutMall;
            }
            set
            {
                aboutMall = value;
                OnPropertyChanged();
            }
        }
        private ContactsModel contacts;
        public ContactsModel Contacts
        {
            get
            {
                return contacts;
            }
            set
            {
                contacts = value;
                OnPropertyChanged();
            }
        }

        #endregion
        #region Колллекции сущностей
        //Эти этажи не для карты, а для фильтрации магазинов по этажам

        private ObservableCollection<FloorModel> filterFloors;
        public ObservableCollection<FloorModel> FilterFloors
        {
            get
            {
                return filterFloors;
            }
            set
            {
                filterFloors = value;
                OnPropertyChanged("FilterFloors");
            }
        }
        //Изображения парковочных этажей
        private ObservableCollection<ParkingModel> parkingUrls;
        public ObservableCollection<ParkingModel> ParkingUrls
        {
            get
            {
                return parkingUrls;
            }
            set
            {
                parkingUrls = value;
                OnPropertyChanged("ParkingUrls");
            }
        }
        //Баннеры в левой стороне главного окна
        private ObservableCollection<BannerContainer> bannersUrls;
        public ObservableCollection<BannerContainer> BannersUrls
        {
            get
            {
                return bannersUrls;
            }
            set
            {
                bannersUrls = value;
                OnPropertyChanged("BannersUrls");
            }
        }
        private ObservableCollection<PromoModel> promos;
        public ObservableCollection<PromoModel> Promos
        {
            get
            {
                return promos;
            }
            set
            {
                promos = value;
                OnPropertyChanged("Promos");
            }
        }
       
        private ObservableCollection<VacancyModel> vacancies;
        public ObservableCollection<VacancyModel> Vacancies
        {
            get
            {
                return vacancies;
            }
            set
            {
                vacancies = value;
                OnPropertyChanged("Vacancies");
            }
        }
        private ObservableCollection<CategoryModel> categories;
        public ObservableCollection<CategoryModel> Categories
        {
            get
            {
                return categories;
            }
            set
            {
                categories = value;
                OnPropertyChanged("Categories");
            }
        }
        private ObservableCollection<Station> stations;
        public ObservableCollection<Station> Stations
        {
            get
            {
                return stations;
            }
            set
            {
                stations = value;
                OnPropertyChanged("Stations");
            }
        }

        private ObservableCollection<ShopModel> shops;
        public ObservableCollection<ShopModel> Shops
        {
            get
            {
                return shops;
            }
            set
            {
                shops = value;
                OnPropertyChanged("Shops");
            }
        }

        //По умолчанию здесь находятся все магазины, свойства выборки по поиску или категории
        private ObservableCollection<ShopModel> shopsByCategory;
        public ObservableCollection<ShopModel> ShopsByCategory
        {
            get
            {
                return shopsByCategory;
            }
            set
            {
                shopsByCategory = value;
                OnPropertyChanged("ShopsByCategory");
            }
        }
        private ObservableCollection<ShopGalleryModel> gallery;
        public ObservableCollection<ShopGalleryModel> Gallery
        {
            get
            {
                return gallery;
            }
            set
            {
                gallery = value;
                OnPropertyChanged("Gallery");
            }
        }
        #endregion
        #region Выбранные объекты
        //Выбранный парковочный этаж
        private ParkingModel currentParkingFloor;
        public ParkingModel CurrentParkingFloor
        {
            get
            {
                return currentParkingFloor;
            }
            set
            {
                currentParkingFloor = value;
                OnPropertyChanged("CurrentParkingFloor");
            }
        }
        //Текст для поиска, отображается в кнопке поиск. По умолчанию - Поиск
        private string searchText;
        public string SearchText
        {
            get
            {
                return searchText;
            }
            set
            {
                searchText = value;
                OnPropertyChanged();
            }
        }

        private Floor currentFloor;
        public Floor CurrentFloor
        {
            get
            {
                return currentFloor;
            }
            set
            {
                currentFloor = value;
                OnPropertyChanged();
            }
        }

        private CategoryModel currentCategory;
        public CategoryModel CurrentCategory
        {
            get
            {
                return currentCategory;
            }
            set
            {
                currentCategory = value;
                OnPropertyChanged();
            }
        }

        private ShopModel currentShop;
        public ShopModel CurrentShop
        {
            get
            {
                return currentShop;
            }
            set
            {
                currentShop = value;
                OnPropertyChanged();
            }
        }
        private ShopGalleryModel currentGallery;
        public ShopGalleryModel CurrentGallery
        {
            get
            {
                return currentGallery;
            }
            set
            {
                currentGallery = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Навигация
        private RelayCommand goHome;
        public RelayCommand GoHome
        {
            get
            {
                return goHome ??
                    (goHome = new RelayCommand(obj =>
                    {
   
                        This.frame.Navigate(new MapPage(This));
                    }));
            }
        }

        private RelayCommand goBack;
        public RelayCommand GoBack
        {
            get
            {
                return goBack ??
                    (goBack = new RelayCommand(obj =>
                    {
                      
                        if (This.frame.CanGoBack)
                        {                          
                            This.frame.GoBack();
                        }
                        else
                        {
                            This.frame.Navigate(new MapPage(This));
                        }
                    }));
            }
        }

        private RelayCommand goShops;
        public RelayCommand GoShops => goShops ??= new RelayCommand(obj =>
                {
                    ShopsByCategory = Shops;
                    This.frame.Navigate(new ShopsView(){AllShops = shops});
                });

        private RelayCommand goCategories;
        public RelayCommand GoCategories=> goCategories ??= new RelayCommand(obj =>
                {
                    This.frame.Navigate(new Categories() { DataContext = this });
                        
                });

        private RelayCommand goInfo;
        public RelayCommand GoInfo => goInfo ??= new RelayCommand(obj =>
                {
                    This.frame.Navigate(new AboutTradeCenter(AboutMall));
                });

        private RelayCommand goNews;
        public RelayCommand GoNews => goNews ??= new RelayCommand(obj =>
        {
            This.frame.Navigate(new News() { AllNews = promos, DataContext = this }) ;
        });



        private RelayCommand goWC;
        public RelayCommand GoWC => goWC ??= new RelayCommand(obj =>
         {
            This.frame.Navigate(new WCPage() { DataContext = this });
         });
        private RelayCommand goParking;
        public RelayCommand GoParking => goParking ??= new RelayCommand(obj =>
        {
            This.frame.Navigate(new Parking() { DataContext = this});
        });
        private RelayCommand goBannersShop;
        public RelayCommand GoBannersShop => goBannersShop ??= new RelayCommand(obj =>
        {
            BannerContainer banner = obj as BannerContainer;
            var shop = KioskObjects.Shops.Where(o => o.ID == banner.BannerModel.ShopID).FirstOrDefault();
            This.frame.Navigate(new ShopPage(shop));
        });
        #endregion
        #region Навигация к объектам из ListView
        //private RelayCommand goShopPage;
        //public RelayCommand GoShopPage => goShopPage ??= new RelayCommand(obj =>
        //        {
        //            if (CurrentShop != null)
        //            {
        //                This.frame.Navigate(new ShopPage() );
        //            }                    
        //        });
        //private RelayCommand goShopsByCategory;
        //public RelayCommand GoShopsByCategory => goShopsByCategory ??= new RelayCommand(obj =>
        //        {
        //            if (CurrentCategory != null)
        //            {
        //                ShopsByCategory = new ObservableCollection<ShopModel>(Shops.Where(o => o.Category.ID == CurrentCategory.ID).ToList());
        //                This.frame.Navigate(new ShopsView() { DataContext = this });
        //            }
        //        });


        private RelayCommand goMapShopRoute;
        public RelayCommand GoMapShopRoute => goMapShopRoute ??= new RelayCommand(obj =>
                {
                    if (CurrentShop != null)
                    {
                        This.frame.Navigate(new MapPage(This, CurrentShop));
                    }
                });
        #endregion

      
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
