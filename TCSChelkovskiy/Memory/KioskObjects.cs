using NavigationMap.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TCSChelkovskiy.Models;
using TCSchelkovskiyAPI.Models;

namespace TCSChelkovskiy.Memory
{
    public static class KioskObjects
    {
        public static ObservableCollection<FloorModel> FilterFloors { get; set; } = new ObservableCollection<FloorModel>();
        public static ObservableCollection<Floor> Floors { get; set; } = new ObservableCollection<Floor>();
        public static ObservableCollection<CategoryModel> Categories { get; set; } = new ObservableCollection<CategoryModel>();
        public static ObservableCollection<Station> Stations { get; set; } = new ObservableCollection<Station>();
        public static ObservableCollection<ShopModel> Shops { get; set; } = new ObservableCollection<ShopModel>();
        public static ObservableCollection<ShopGalleryModel> Gallery { get; set; } = new ObservableCollection<ShopGalleryModel>();
        public static ObservableCollection<VacancyModel> Vacancies { get; set; } = new ObservableCollection<VacancyModel>();
        public static ObservableCollection<PromoModel> Promos { get; set; } = new ObservableCollection<PromoModel>();
        public static ObservableCollection<BannerContainer> Banners { get; set; } = new ObservableCollection<BannerContainer>();
        public static ObservableCollection<ParkingModel> ParkingFloors { get; set; } = new ObservableCollection<ParkingModel>();
        public static ObservableCollection<RuleModel> Rules { get; set; } = new ObservableCollection<RuleModel>();

        public static ContactsModel Contacts { get; set; } = new ContactsModel();
        public static AboutMallModel AboutMall { get; set; } = new AboutMallModel();
        public static async Task LoadAllObjects()
        {

            await Task.Run(() =>
            {
                Categories = new ObservableCollection<CategoryModel>(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetCategories());


                var pagesCount = TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetShopPagesCount();
                List<ShopModel> shops = new List<ShopModel>();
                for (int i = 0; i < pagesCount + 1; i++)
                {
                    shops.AddRange(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetShops(i));
                }
                Shops = new ObservableCollection<ShopModel>(shops);


                Gallery = new ObservableCollection<ShopGalleryModel>(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetShopsGallery());
                Vacancies = new ObservableCollection<VacancyModel>(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetVacancies());
                Promos = new ObservableCollection<PromoModel>(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetPromos());


                Rules = new ObservableCollection<RuleModel>(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetRules());

                foreach (var model in TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetBanners())
                {
                    Banners.Add(new BannerContainer { BannerModel = model });
                }
                

                //MessageBox.Show(Banners.Count.ToString());

                var filterFloors = TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetFloors();
                filterFloors.Sort();
                FilterFloors = new ObservableCollection<FloorModel>(filterFloors);

                var parkingFloors = TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetParking();
                parkingFloors.Sort();
                ParkingFloors = new ObservableCollection<ParkingModel>(parkingFloors);


                Contacts = TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetContacts();
                AboutMall = TCSchelkovskiyAPI.TCSchelkovskiyAPI.AboutMall();


                RestoreSettings();
            });

            
        }
        static JsonSerializer serializer = new JsonSerializer();
        public static void RestoreSettings()
        {
            Floors = ConvertToFloors(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetFloors());
            ConvertToFloorsFromJson(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetFloors());
            Floors = ConvertToFloors(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetFloors());

        }
        public static string FilePath = @"settings.json";
        private static ObservableCollection<Floor> ConvertToFloors(List<FloorModel> floors)
        {

            List<Floor> floorList = new List<Floor>();
            foreach (var fl in floors)
            {
                if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "AllImages", fl.Image)))
                {
                    Services.ImageDownloader.DownloadImage(fl.ImagesPrefix + fl.Image, fl.Image).Wait();
                }
                Floor floor = new Floor
                {
                    Name = fl.Name,
                    FloorNumber = fl.Floor.ToString(),
                    Id = fl.ID,
                    Image = Path.Combine(Environment.CurrentDirectory, "AllImages", fl.Image),
                    Width = 9000,
                    Height = 9000,
                };
                floorList.Add(floor);
            }
            return new ObservableCollection<Floor>(floorList);
        }

        static int floorCount = 0;
        static int floorCounter = 0;
        private static void ConvertToFloorsFromJson(List<FloorModel> floors)
        {
            floorCount = floors.Count;
            List<Floor> floorList = new List<Floor>();

            foreach (var fl in floors)
            {
                try
                {
                    floorCounter++;
                    Floor floorFromJson = new Floor();
                    string url = "https://navigator.useful.su/" + fl.FilePrefix + fl.File;
                    var jsonFile = Path.Combine(Environment.CurrentDirectory, "JSON", fl.File);
                    if (!Directory.Exists("JSON"))
                        Directory.CreateDirectory("JSON");

                    WebClient client = new WebClient();
                    client.DownloadFile(url, jsonFile);

                    if (File.Exists(jsonFile))
                    {

                        using (StreamReader file = File.OpenText(jsonFile))
                        {
                            floorFromJson = (Floor)serializer.Deserialize(file, typeof(Floor));
                        }
                    }
                    var selected = Floors.Where(o => o.Id == floorFromJson.Id).FirstOrDefault();
                    if (selected != null)
                    {

                        int itemIndex = Floors.IndexOf(selected);
                        string img = Floors[itemIndex].Image;
                        Floors[itemIndex] = floorFromJson;
                        Floors[itemIndex].Image = img;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message); MessageBox.Show(ex.StackTrace);
                    Debug.WriteLine("Не удалось загрузить файл");
                }
            }
        }

    }
}
