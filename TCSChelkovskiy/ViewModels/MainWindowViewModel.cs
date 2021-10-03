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

            FeedbackModel = new FeedbackModel();
            SearchText = "Поиск";
             
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
                OnPropertyChanged("AboutMall");
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
                OnPropertyChanged("Contacts");
            }
        }

        #endregion

        #region Колллекции сущностей
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
                OnPropertyChanged("SearchText");
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
                OnPropertyChanged("CurrentFloor");
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
                        FeedbackModel.MakeDefaultValues();
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
                        FeedbackModel.MakeDefaultValues();
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
        public RelayCommand GoShops
        {
            get
            {
                return goShops ??
                    (goShops = new RelayCommand(obj =>
                    {
                        ShopsByCategory = Shops;
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
        #region Навигация к объектам из ListView
        private RelayCommand goShopPage;
        public RelayCommand GoShopPage
        {
            get
            {
                return goShopPage ??
                    (goShopPage = new RelayCommand(obj =>
                    {
                        if (CurrentShop != null)
                        {
                            This.frame.Navigate(new ShopPage() { DataContext = this });
                        }                    
                    }));
            }
        }
        private RelayCommand goShopsByCategory;
        public RelayCommand GoShopsByCategory
        {
            get
            {
                return goShopsByCategory ??
                    (goShopsByCategory = new RelayCommand(obj =>
                    {
                        if (CurrentCategory != null)
                        {
                            ShopsByCategory = new ObservableCollection<ShopModel>(Shops.Where(o => o.Category.ID == CurrentCategory.ID).ToList());
                            This.frame.Navigate(new ShopsView() { DataContext = this });
                        }
                    }));
            }
        }
        private RelayCommand goMapShopRoute;
        public RelayCommand GoMapShopRoute
        {
            get
            {
                return goMapShopRoute ??
                    (goMapShopRoute = new RelayCommand(obj =>
                    {
                        if (CurrentShop != null)
                        {
                            This.frame.Navigate(new MapPage(This, CurrentShop));
                        }
                    }));
            }
        }
        #endregion

        #region Обратная связь
        private FeedbackModel feedbackModel;
        public FeedbackModel FeedbackModel
        {
            get
            {
                return feedbackModel;
            }
            set
            {
                feedbackModel = value;
                OnPropertyChanged("FeedbackModel");
            }
        }

        private RelayCommand sendFeedback;
        public RelayCommand SendFeedback
        {
            get
            {
                return sendFeedback ??
                    (sendFeedback = new RelayCommand(obj =>
                    {
                        FeedbackModel.ValidationErrors = new List<string>();
                        var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                        var context = new ValidationContext(FeedbackModel);
                        if (!Validator.TryValidateObject(FeedbackModel, context, results, true))
                        {                           
                            foreach (var error in results)
                            {
                                FeedbackModel.ValidationErrors.Add(error.ErrorMessage);
                            }
                            FeedbackModel.FirstValidationError = FeedbackModel.ValidationErrors.FirstOrDefault();
                        }
                        else
                        {
                            FeedbackModel.FirstValidationError = string.Empty;

                            MailAddress from = new MailAddress("", FeedbackModel.EmailOrPhone);                       
                            MailAddress to = new MailAddress("");
                            MailMessage message = new MailMessage(from, to);
                            message.Subject = FeedbackModel.Topic;
                            message.Body =FeedbackModel.Text;
                            SmtpClient smtp = new SmtpClient("", 587);
                            // логин и пароль
                            smtp.Credentials = new NetworkCredential("", "");
                            smtp.EnableSsl = true;
                            smtp.Send(message);
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
