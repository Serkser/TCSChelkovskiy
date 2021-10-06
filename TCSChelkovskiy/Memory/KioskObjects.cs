using NavigationMap.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        public static ObservableCollection<string> Banners { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<ParkingModel> ParkingFloors { get; set; } = new ObservableCollection<ParkingModel>();
        public static ObservableCollection<RuleModel> Rules { get; set; } = new ObservableCollection<RuleModel>();

        public static ContactsModel Contacts { get; set; } = new ContactsModel();
        public static AboutMallModel AboutMall { get; set; } = new AboutMallModel();
        public static async Task LoadAllObjects()
        {

            await Task.Run(() =>
            {
                Categories = new ObservableCollection<CategoryModel>(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetCategories());
                Shops = new ObservableCollection<ShopModel>(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetShops());
                Gallery = new ObservableCollection<ShopGalleryModel>(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetShopsGallery());
                Vacancies = new ObservableCollection<VacancyModel>(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetVacancies());
                Promos = new ObservableCollection<PromoModel>(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetPromos());


                Rules = new ObservableCollection<RuleModel>(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetRules());
                Banners = new ObservableCollection<string>(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetBanners());


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
            if (File.Exists(FilePath))
            {
                using (StreamReader file = File.OpenText(FilePath))
                {
                    Floors = (ObservableCollection<Floor>)serializer.Deserialize(file, typeof(ObservableCollection<Floor>));
                    //MessageBox.Show(Floors.Count.ToString());dssd
                }
            }

            //string json = "";
            //    if (!string.IsNullOrEmpty(json))
            //    {
            //        using (JsonTextReader stream = new JsonTextReader(new StringReader(json)))
            //        {
            //            var settings = serializer.Deserialize(stream);
            //            JObject jObj = (JObject)settings;
            //            Floors = jObj.ToObject<ObservableCollection<Floor>>();
            //        }
            //    }


        }
        public static string FilePath = @"settings.json";

      
    }
}
