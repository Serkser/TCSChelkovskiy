using NavigationMap.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeCenterAdmin.Services.MapObjectSavers
{
    public class LocalJsonMapObjectsSaver : IMapObjectSaver
    {
        public static string FilePath = Path.Combine(Environment.CurrentDirectory, "settings.json");
        static JsonSerializer serializer = new JsonSerializer();
        public void Save(IList<Floor> floors)
        {
            if (File.Exists(FilePath)) { File.Delete(FilePath); }
            ObservableCollection<Floor> Floors = new ObservableCollection<Floor>(floors);
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                serializer.Serialize(sw, Floors);
            }
        }
    }
}
