using NavigationMap.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TCSchelkovskiyAPI.Models;

namespace TradeCenterAdmin.Services.MapObjectLoaders
{
    public class ServerJsonMapObjectsLoader : IMapObjectLoader
    {
        static JsonSerializer serializer = new JsonSerializer();
        public void LoadObjects(List<FloorModel> floors)
        {
            int floorCounter = 0;
            int floorCount = floors.Count;
            List<Floor> floorList = new List<Floor>();

            foreach (var fl in floors)
            {
                try
                {
                    floorCounter++;
                    Floor floorFromJson = new Floor();
                    string url = Properties.Settings.Default.host + fl.FilePrefix + fl.File;
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
                    var selected = Storage.KioskObjects.Floors.Where(o => o.Id == floorFromJson.Id).FirstOrDefault();
                    if (selected != null)
                    {

                        int itemIndex = Storage.KioskObjects.Floors.IndexOf(selected);
                        string img = Storage.KioskObjects.Floors[itemIndex].Image;
                        Storage.KioskObjects.Floors[itemIndex] = floorFromJson;
                        Storage.KioskObjects.Floors[itemIndex].Image = img;
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
