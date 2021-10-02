
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
using TCEvropeyskiy;
using TCSChelkovskiy.Memory;
using TCSchelkovskiyAPI.Models;

namespace TCSChelkovskiy.ViewModels
{
    public class MapViewModel
    {
        Views.MapPage This { get; set; }
        MainWindow MainWindow { get; set; }
        public MapViewModel(Views.MapPage _this, MainWindow main)
        {
            This = _this;
            MainWindow = main;

            Floors = KioskObjects.Floors;
            Categories = KioskObjects.Categories;
            Stations = KioskObjects.Stations;
            Shops = KioskObjects.Shops;
            Gallery = KioskObjects.Gallery;
            if (Floors.Count > 0)
            {
                CurrentFloor = Floors.FirstOrDefault();
            }
            This.Map.SelectedStation = CurrentFloor.Stations.FirstOrDefault();


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
                        var area = Floors[0].Areas[0];
                        This.Map.Navigate(area.Id);
                            //if (obj != null)
                            //{
                            //    This.Map.Navigate(Convert.ToInt32(obj));
                            //}
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
     
        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
