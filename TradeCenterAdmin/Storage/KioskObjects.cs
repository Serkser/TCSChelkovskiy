using NavigationMap.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using TCSchelkovskiyAPI.Enums;
using TCSchelkovskiyAPI.Models;

namespace TradeCenterAdmin.Storage
{
    public static class KioskObjects
    {

        public static ChangesPool.ChangesPool ChangesPool = new ChangesPool.ChangesPool();
        public static ObservableCollection<Floor> Floors { get; set; } = new ObservableCollection<Floor>();
        public static ObservableCollection<FloorModel> FloorModels { get; set; } = new ObservableCollection<FloorModel>();
        //public static ObservableCollection<CategoryModel> Categories { get; set; } = new ObservableCollection<CategoryModel>();
        //public static ObservableCollection<Station> Stations { get; set; } = new ObservableCollection<Station>();
        public static ObservableCollection<ShopModel> Shops { get; set; } = new ObservableCollection<ShopModel>();
        public static ObservableCollection<TerminalModel> Terminals { get; set; } = new ObservableCollection<TerminalModel>();
        public static ObservableCollection<TerminalModel> ATMs { get; set; } = new ObservableCollection<TerminalModel>();
        public static ObservableCollection<TerminalModel> Escolators { get; set; } = new ObservableCollection<TerminalModel>();
        public static ObservableCollection<TerminalModel> Lifts { get; set; } = new ObservableCollection<TerminalModel>();
        public static ObservableCollection<TerminalModel> Stairs { get; set; } = new ObservableCollection<TerminalModel>();
        public static ObservableCollection<TerminalModel> WCs { get; set; } = new ObservableCollection<TerminalModel>();
        //public static ObservableCollection<ShopGalleryModel> Gallery { get; set; } = new ObservableCollection<ShopGalleryModel>();

        static KioskObjects()
        {
            RestoreSettings();
        }

        static JsonSerializer serializer = new JsonSerializer();
        public static async Task LoadAllObjects()
        {
            var pagesCount = TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetShopPagesCount();
            List<ShopModel> shops = new List<ShopModel>();
            for (int i = 0; i < pagesCount + 1; i++)
            {
                shops.AddRange(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetShops(i));
            }
            Shops = new ObservableCollection<ShopModel>(shops);


            var rawTerminalObjects = TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetTerminals();


            Terminals = new ObservableCollection<TerminalModel>(rawTerminalObjects.Where(o => o.Type == MapTerminalPointType.Termanals).ToList());
            Stairs = new ObservableCollection<TerminalModel>(rawTerminalObjects.Where(o => o.Type == MapTerminalPointType.Stairs).ToList());
            WCs = new ObservableCollection<TerminalModel>(rawTerminalObjects.Where(o => o.Type == MapTerminalPointType.WC).ToList());
            Escolators = new ObservableCollection<TerminalModel>(rawTerminalObjects.Where(o => o.Type == MapTerminalPointType.Escolator).ToList());
            Lifts = new ObservableCollection<TerminalModel>(rawTerminalObjects.Where(o => o.Type == MapTerminalPointType.Lift).ToList());
            ATMs = new ObservableCollection<TerminalModel>(rawTerminalObjects.Where(o => o.Type == MapTerminalPointType.ATMCash).ToList());
            //await Task.Run(() =>
            //{
            //    //Categories = new ObservableCollection<CategoryModel>(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetCategories());
            //    //Floors = ConvertToFloors(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetFloors());
            //    Shops = new ObservableCollection<ShopModel>(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetShops());
            //    return;
            //    //Gallery = new ObservableCollection<ShopGalleryModel>(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetShopsGallery());
            //});
        }

        public static int LoadingPercent = 0;
        public static void RestoreSettings()
        {
            Floors = ConvertToFloors(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetFloors());
            ConvertToFloorsFromJson(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetFloors());
        }
        public static string FilePath = Path.Combine(Environment.CurrentDirectory, "settings.json");

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
                    string url = Properties.Settings.Default.host + fl.FilePrefix + fl.File;
                    var jsonFile = Path.Combine(Environment.CurrentDirectory,"JSON", fl.File);
                if (!Directory.Exists("JSON"))
                    Directory.CreateDirectory("JSON");

                    WebClient client = new WebClient();
                    client.DownloadProgressChanged += Client_DownloadProgressChanged; 
                     
                        client.DownloadFile(url, Path.GetFullPath(jsonFile));  
                    

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
                        int itemIndex =  Floors.IndexOf(selected);
                        Floors[itemIndex] = floorFromJson;
                    }
                }
                catch (Exception ex)
                {
                   // MessageBox.Show(ex.Message); MessageBox.Show(ex.StackTrace);
                    Debug.WriteLine("Не удалось загрузить файл");
                }
            }
        }

        private static void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
           // MessageBox.Show("");
            LoadingPercent = (100 / (floorCount * 100)) * e.ProgressPercentage;
        }
    }
}

