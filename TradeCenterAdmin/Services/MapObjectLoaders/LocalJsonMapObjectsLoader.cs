using NavigationMap.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCSchelkovskiyAPI.Models;

namespace TradeCenterAdmin.Services.MapObjectLoaders
{
    public class LocalJsonMapObjectsLoader : IMapObjectLoader
    {
        public static string FilePath = Path.Combine(Environment.CurrentDirectory, "settings.json");
        static JsonSerializer serializer = new JsonSerializer();
        public void LoadObjects(List<FloorModel> floors)
        {
            foreach (var fl in floors) { 
            }


            if (File.Exists(FilePath))
            {
                using (StreamReader file = File.OpenText(FilePath))
                {
                    Storage.KioskObjects.Floors = (ObservableCollection<Floor>)serializer.Deserialize(file, typeof(ObservableCollection<Floor>));
                }
            }


            //foreach (var fl in floors)
            //{
            //    var selected = Storage.KioskObjects.Floors.Where(o => o.Id == fl.Id).FirstOrDefault();
            //    if (selected != null)
            //    {

            //        int itemIndex = Storage.KioskObjects.Floors.IndexOf(selected);
            //        string img = Storage.KioskObjects.Floors[itemIndex].Image;
            //        Storage.KioskObjects.Floors[itemIndex].Image = img;
            //    }
            //}
        }
    }
}
