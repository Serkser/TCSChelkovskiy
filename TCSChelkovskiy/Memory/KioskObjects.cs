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
        public static ObservableCollection<Floor> Floors { get; set; } = new ObservableCollection<Floor>();
        public static ObservableCollection<CategoryModel> Categories { get; set; } = new ObservableCollection<CategoryModel>();
        public static ObservableCollection<Station> Stations { get; set; } = new ObservableCollection<Station>();
        public static ObservableCollection<ShopModel> Shops { get; set; } = new ObservableCollection<ShopModel>();
        public static ObservableCollection<ShopGalleryModel> Gallery { get; set; } = new ObservableCollection<ShopGalleryModel>();
        public static ObservableCollection<VacancyModel> Vacancies { get; set; } = new ObservableCollection<VacancyModel>();

        public static ContactsModel Contacts { get; set; } = new ContactsModel();
        public static AboutMallModel AboutMall { get; set; } = new AboutMallModel();
        public static async Task LoadAllObjects()
        {

            await Task.Run(() =>
            {
                Categories = new ObservableCollection<CategoryModel>(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetCategories());
              //  Floors = ConvertToFloors(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetFloors());
                Shops = new ObservableCollection<ShopModel>(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetShops());
                Gallery = new ObservableCollection<ShopGalleryModel>(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetShopsGallery());
                Vacancies = new ObservableCollection<VacancyModel>(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetVacancies());

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

        private static ObservableCollection<Floor> ConvertToFloors(List<FloorModel> floors)
        {
            List<Floor> floorList = new List<Floor>();
            foreach (var fl in floors)
            {
                Floor floor = new Floor
                {
                    Name = fl.Name,
                    Id = fl.ID,
                    Width = 9000,
                    Height = 9000,
                };
                ObservableCollection<Area> areas = new ObservableCollection<Area>();
                foreach (var shop in fl.Shops)
                {
                    Area area = new Area
                    {
                        Description = shop.Description,
                        Id = shop.ID,
                        FloorId = shop.Floor.ID,
                        Image = shop.IconURI,
                        Name = shop.Name,       
                        
                    };
                    area.AreaCategories.Add(new AreaCategory
                    {
                        Id = shop.Category.ID,
                        Name = shop.Category.Name,
                        Image = shop.IconURI
                    });

                    foreach (var photo in shop.Photos)
                    {
                        area.AreaImages.Add(new AreaImage { Image = photo.ImageURI });
                    }
                  
                }
                floorList.Add(floor);
            }

            ObservableCollection<Floor> coll = new ObservableCollection<Floor>(floorList);
            return coll;

        }
    }
}
