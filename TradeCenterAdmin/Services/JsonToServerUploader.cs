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

namespace TradeCenterAdmin.Services
{
    public class JsonToServerUploader<T> where T:Floor
    {
        static JsonSerializer serializer = new JsonSerializer();
        public void UploadToServer(T obj,string filename,int number)
        {
            string name = filename + number + ".json";
            string filepath = Path.Combine(Environment.CurrentDirectory,name);
            using (StreamWriter sw = new StreamWriter(filepath))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, obj);
                }
            }
            TCSchelkovskiyAPI.TCSchelkovskiyAPI.UploadFloorJsonToServer(filepath, number);
            if (File.Exists(filepath)){ File.Delete(filepath); }
        }

        public void UploadListToServer(ObservableCollection<T> items, string filename)
        {
            foreach (var obj in items)
            {
                string filepath = Path.Combine(Environment.CurrentDirectory, filename+obj.Id + ".json");
                using (StreamWriter sw = new StreamWriter(filepath))
                {
                    string imgpath = obj.Image;
                    obj.Image = string.Empty;
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, obj);
                    }
                    obj.Image = imgpath;
                }
                string response = TCSchelkovskiyAPI.TCSchelkovskiyAPI.UploadFloorJsonToServer(filepath,obj.Id);
                if (File.Exists(filepath)) { File.Delete(filepath); }
            }        
        }

       
    }
}
