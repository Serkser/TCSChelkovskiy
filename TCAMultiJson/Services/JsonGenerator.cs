using NavigationMap.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace TCAMultiJson.Services
{
    public class JsonGenerator
    {
        public ObservableCollection<string> InputJsonFilepaths { get; set; } = new ObservableCollection<string>();
        public string ResultJsonFilepath { get; set; }


        private JsonSerializer serializer = new JsonSerializer();
        private ObservableCollection<Floor> ResultFloors = new ObservableCollection<Floor>();
        private List<int> HandledKioskIDs = new List<int>();
        public void GenerateJSON()
        {
            int counter = 0;
            foreach (var path in InputJsonFilepaths)
            {
                counter++;
                var currentFloors = LoadFloorsFromJSON(path);

                if (counter == 1)
                {
                    ResultFloors = currentFloors;
                    continue;
                }
                UnionFloorObjects(currentFloors);
            }

            SaveGeneratedJSON();
        }

        private ObservableCollection<Floor> LoadFloorsFromJSON(string filepath)
        {
            ObservableCollection<Floor> floors = new ObservableCollection<Floor>();
            if (File.Exists(filepath))
            {
                using (StreamReader file = File.OpenText(filepath))
                {
                    floors = (ObservableCollection<Floor>)serializer.Deserialize(file, typeof(ObservableCollection<Floor>));
                }
            }
            return floors;
        }

        private void UnionFloorObjects(ObservableCollection<Floor> inputFloors) 
        { 

            //Мержим области
            for (int f=0;f< inputFloors.Count;f++)
            {
                for (int a = 0; a < inputFloors[f].Areas.Count; a++)
                {
                    var inputArea = inputFloors[f].Areas[a];
                    if (inputArea == null) { continue; }

                    var resultFloorArea = ResultFloors[f].Areas.Where(o => o.Id == inputArea.Id).FirstOrDefault();
                    if (resultFloorArea == null)
                    {
                        ResultFloors[f].Areas.Add(inputArea);
                    }
                }
            }
            //Мержим пути
            for (int f = 0; f < inputFloors.Count; f++)
            {
                for (int a = 0; a < inputFloors[f].Areas.Count; a++)
                {
                    for (int w = 0; w < inputFloors[f].Areas[a].Ways.Count; w++)
                    {
                        var inputWay = inputFloors[f].Areas[a].Ways[w];
                        if (inputWay == null) { continue; }


                        var resultFloorArea = ResultFloors[f].Areas.Where(o => o.Id == inputFloors[f].Areas[a].Id).FirstOrDefault();
                        if (resultFloorArea == null) { continue; }

                        var resultFloorAreaWay = resultFloorArea.Ways.Where(o => o.Id == inputFloors[f].Areas[a].Ways[w].Id).FirstOrDefault();
                        if (resultFloorAreaWay == null)
                        {
                            ResultFloors[f].Areas.Where(o => o.Id == resultFloorArea.Id).FirstOrDefault().
                                Ways.Add(inputWay);
                        }
                    }
                }
            }

            //Мержим точки
            for (int f = 0; f < inputFloors.Count; f++)
            {
                for (int s = 0; s < inputFloors[f].Stations.Count; s++)
                {
                    var inputFloorStation = inputFloors[f].Stations[s];
                    if (inputFloorStation == null) { continue; }

                    var resultFloorStation = ResultFloors[f].Stations.Where(o => o.Id == inputFloorStation.Id).FirstOrDefault();
                    if (resultFloorStation == null)
                    {
                        ResultFloors[f].Stations.Add(inputFloorStation);
                    }
                }
            }
        }

        private void SaveGeneratedJSON()
        {
            string resultFileName = Path.Combine(ResultJsonFilepath, "settings.json");

            if (File.Exists(resultFileName)) { File.Delete(resultFileName); }
            using (StreamWriter sw = new StreamWriter(resultFileName))
            {
                serializer.Serialize(sw, ResultFloors);
            }
        }

    }
}
