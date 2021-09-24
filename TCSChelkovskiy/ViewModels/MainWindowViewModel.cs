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
            Gallery = KioskObjects.Gallery;
        
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
                        Search page = new Search();
                        page.DataContext = this;
                        This.frame.Navigate(page);
                    }));
            }
        }
        #endregion


        #region Свойства сущностей

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
                            //This.frame.GoBack();
                        }
                        else
                        {
                            This.frame.Navigate(new MapPage(This));
                        }
                    }));
            }
        }

        private RelayCommand goShops;
        public RelayCommand GoShops
        {
            get
            {
                return goShops ??
                    (goShops = new RelayCommand(obj =>
                    {
                        This.frame.Navigate(new ShopsView() { DataContext = this });
                    }));
            }
        }

        private RelayCommand goCategories;
        public RelayCommand GoCategories
        {
            get
            {
                return goCategories ??
                    (goCategories = new RelayCommand(obj =>
                    {
                        This.frame.Navigate(new Categories() { DataContext = this });
                        
                    }));
            }
        }

        private RelayCommand goInfo;
        public RelayCommand GoInfo
        {
            get
            {
                return goInfo ??
                    (goInfo = new RelayCommand(obj =>
                    {
                        This.frame.Navigate(new AboutTradeCenter() { DataContext = this });
                    }));
            }
        }

        private RelayCommand goNews;
        public RelayCommand GoNews
        {
            get
            {
                return goNews ??
                    (goNews = new RelayCommand(obj =>
                    {
                        This.frame.Navigate(new News() { DataContext = this });
                    }));
            }
        }

        private RelayCommand goContacts;
        public RelayCommand GoContacts
        {
            get
            {
                return goContacts ??
                    (goContacts = new RelayCommand(obj =>
                    {
                        This.frame.Navigate(new Contacts() { DataContext = this });
                    }));
            }
        }

        private RelayCommand goFeedback;
        public RelayCommand GoFeedback
        {
            get
            {
                return goFeedback ??
                    (goFeedback = new RelayCommand(obj =>
                    {
                        This.frame.Navigate(new Feedback() { DataContext = this });
                    }));
            }
        }

        private RelayCommand goRules;
        public RelayCommand GoRules
        {
            get
            {
                return goRules ??
                    (goRules = new RelayCommand(obj =>
                    {
                        This.frame.Navigate(new Rules() { DataContext = this });
                    }));
            }
        }

        private RelayCommand goVacancies;
        public RelayCommand GoVacancies
        {
            get
            {
                return goVacancies ??
                    (goVacancies = new RelayCommand(obj =>
                    {
                        This.frame.Navigate(new Vacancies() {DataContext = this });
                    }));
            }
        }

        private RelayCommand goWC;
        public RelayCommand GoWC
        {
            get
            {
                return goWC ??
                    (goWC = new RelayCommand(obj =>
                    {
                        This.frame.Navigate(new WCPage() { DataContext = this });
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
