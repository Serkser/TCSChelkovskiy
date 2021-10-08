
using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using TCEvropeyskiy;
using TCSChelkovskiy.Memory;
using TCSChelkovskiy.Views;
using TCSchelkovskiyAPI.Models;

namespace TCSChelkovskiy.ViewModels
{
    public class MapViewModel
    {
        Views.MapPage This { get; set; }
        MainWindow MainWindow { get; set; }
        
        void InitMap()
        {
          
            Floors = KioskObjects.Floors;

            Categories = KioskObjects.Categories;
            Stations = KioskObjects.Stations;
            Shops = KioskObjects.Shops;
            Gallery = KioskObjects.Gallery;
            if (Floors.Count > 0)
            {
                CurrentFloor = Floors.FirstOrDefault();
                This.Map.SelectedStation = CurrentFloor.Stations.FirstOrDefault();
            }
            
        }
        public MapViewModel(Views.MapPage _this, MainWindow main)
        {
            This = _this;
            MainWindow = main;
            InitMap();

        }
        public MapViewModel(Views.MapPage _this, MainWindow main,ShopModel selectedShop)
        {
            bool isShopFound = false;

            This = _this;
            MainWindow = main;
            InitMap();
            CurrentShop = selectedShop;
            foreach (var floor in Floors)
            {
                foreach (var area in floor.Areas)
                {
                    if (selectedShop.ID == area.Id)
                    {
                        CurrentFloorShop = area;
                        CurrentFloor = floor;
                        isShopFound = true;
                        This.Map.Navigate(CurrentFloorShop.Id);
                        break;
                    }
                }

                if (isShopFound) { break; }
            }
        }

        #region Свойства карты и навигации
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
                OnPropertyChanged("CurrentFloor");
            }
        }

        private Area currentFloorShop;
        public Area CurrentFloorShop
        {
            get
            {
                return currentFloorShop;
            }
            set
            {
                currentFloorShop = value;
                OnPropertyChanged("CurrentFloorShop");
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
                OnPropertyChanged("CurrentCategory");
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
                OnPropertyChanged("CurrentShop");
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
                OnPropertyChanged("CurrentGallery");
            }
        }

        private ObservableCollection<Floor> floors;
        public ObservableCollection<Floor> Floors
        {
            get
            {
                return floors;
            }
            set
            {
                floors = value;
                OnPropertyChanged("Floors");
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


        #region Работа с картой
        private RelayCommand zoomIn;
        public RelayCommand ZoomIn
        {
            get
            {
                return zoomIn ??
                    (zoomIn = new RelayCommand(obj =>
                    {
                        This.Map.ZoomIn();
                    }));
            }
        }

        private RelayCommand zoomOut;
        public RelayCommand ZoomOut
        {
            get
            {
                return zoomOut ??
                    (zoomOut = new RelayCommand(obj =>
                    {
                        This.Map.ZoomOut();
                    }));
            }
        }
        private RelayCommand navigateTo;
        public RelayCommand NavigateTo
        {
            get
            {
                return navigateTo ??
                    (navigateTo = new RelayCommand(obj =>
                    {
                        var station = floors.First().Stations.First().Id;
                        var test = floors.First().Areas.First().Id;
                        This.Map.Navigate(test);
                        if (CurrentFloorShop != null)
                        {
                            This.Map.Navigate(CurrentFloorShop.Id);
                        }
                    }));
            }
        }
        private RelayCommand changeFloor;
        public RelayCommand ChangeFloor
        {
            get
            {
                return changeFloor ??
                    (changeFloor = new RelayCommand(obj =>
                    {
                            if (obj != null)
                            {
                                This.Map.SelectedFloor = (Floor)obj;
                            }
                    }));
            }
        }
        private RelayCommand goParking;
        public RelayCommand GoParking
        {
            get
            {
                return goParking ??
                    (goParking = new RelayCommand(obj =>
                    {
                        Parking nextPage = new Parking();
                        MainWindow.frame.Navigate(nextPage);
                    }));
            }
        }
        private RelayCommand goHome;
        public RelayCommand GoHome
        {
            get
            {
                return goHome ??
                    (goHome = new RelayCommand(obj =>
                    {
                        MapPage nextPage = new MapPage(MainWindow); nextPage.DataContext = this;
                        MainWindow.frame.Navigate(nextPage);
                    }));
            }
        }
        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
