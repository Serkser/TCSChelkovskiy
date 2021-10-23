using NavigationMap.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Windows;

namespace TCAMultiJson.Services
{
    public class JsonGenerator
    {
        public ObservableCollection<string> InputJsonFilepaths { get; set; } = new ObservableCollection<string>();
        public string ResultJsonFilepath { get; set; }
        public bool OverrideAssignedShops { get; set; }
        public bool OverrideAreaPositions { get; set; }


        private JsonSerializer serializer = new JsonSerializer();
        private ObservableCollection<Floor> ResultFloors = new ObservableCollection<Floor>();
        private ObservableCollection<Floor> firstJson = new ObservableCollection<Floor>();
        public void GenerateJSON()
        {
            for (int i=0;i<InputJsonFilepaths.Count;i++)
            {
              
                if (i == 0)
                {
                    ResultFloors = LoadFloorsFromJSON(InputJsonFilepaths[0]);
                }
                else
                {
                    ResultFloors = LoadFloorsFromJSON(Path.Combine(Environment.CurrentDirectory, "intermediate", "i.json"));
                }
                if (i+1 == InputJsonFilepaths.Count) { break; }
                var currentFloors = LoadFloorsFromJSON(InputJsonFilepaths[i+1]);

                UnionFloorObjects(currentFloors);
                if (OverrideAssignedShops)
                {
                    Debug.WriteLine("Замена магазинов");
                    OverrideAssignedAreaShops(currentFloors);
                }
                if (OverrideAreaPositions)
                {
                    Debug.WriteLine("Замена точек арены");
                    OverrideAreaPointsPositions(currentFloors);
                }

                //Промежуточный результат
                if (!Directory.Exists("intermediate"))
                {
                    Directory.CreateDirectory("intermediate");
                }
                SaveGeneratedJSON(Path.Combine(Environment.CurrentDirectory, "intermediate", "i.json"));
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
                    if (inputArea.Points.Count == 0) { continue; }


                    //Если по 1-й точке нет области, ищем область по id. если не находим на карте, то добавляем такую область

                    var resultFloorArea = ResultFloors[f].Areas.Where(p=>p.Points.Count>0).Where(o => (int)o.Points[0].X == (int)inputArea.Points[0].X
                        && (int)o.Points[0].Y == (int)inputArea.Points[0].Y).FirstOrDefault();
                        if (resultFloorArea == null)
                        {
                            resultFloorArea = ResultFloors[f].Areas.Where(o => o.Id == inputArea.Id).FirstOrDefault();
                            if (resultFloorArea == null)
                            {
                                ResultFloors[f].Areas.Add(inputArea);
                            }
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


                        int waysCount = inputFloors[f].Areas[a].Ways.Where(o => o.Id == inputFloors[f].Areas[a].Ways[w].Id).Count();
                        var resultFloorAreaWay = resultFloorArea.Ways.Where(o => o.Id == inputFloors[f].Areas[a].Ways[w].Id).FirstOrDefault();

                        if (resultFloorAreaWay == null)
                        {
                            foreach (var way in inputFloors[f].Areas[a].Ways.Where(o => o.Id == inputFloors[f].Areas[a].Ways[w].Id).ToList())
                            {
                                ResultFloors[f].Areas.Where(o => o.Id == resultFloorArea.Id).FirstOrDefault().
                              Ways.Add(way);
                            }
                       
                        }
                        else
                        {

                        }
                    }

                    //var inputArea = inputFloors[f].Areas[a];
                    //if (inputArea == null) { continue; }
                    //if (inputArea.Points.Count == 0) { continue; }

                    //var resultFloorArea = ResultFloors[f].Areas.Where(o => o.Points.Count > 0).Where(o => o.Points[0].X == inputArea.Points[0].X &&
                    //    o.Points[0].Y == inputArea.Points[0].Y).FirstOrDefault();
                    //int resultFloorAreaIndex = ResultFloors[f].Areas.IndexOf(resultFloorArea);
                    //if (resultFloorArea != null)
                    //{
                    //    if (inputArea.EditDate > resultFloorArea.EditDate)
                    //    {
                    //        ResultFloors[f].Areas[resultFloorAreaIndex].Ways.Clear();
                    //        foreach (var way in inputFloors[f].Areas[a].Ways)
                    //        {
                    //            ResultFloors[f].Areas[resultFloorAreaIndex].Ways.Add(way);
                    //        }

                    //    }
                    //}
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
            
            ////Присваиваем этаж областям и путям
            //for (int f = 0; f < ResultFloors.Count; f++)
            //{
            //    for (int a = 0; a < ResultFloors[f].Areas.Count; a++)
            //    {
            //        ResultFloors[f].Areas[a].FloorId = ResultFloors[f].Id;
            //        for (int w=0;w < ResultFloors[f].Areas[a].Ways.Count; w++)
            //        {
            //            ResultFloors[f].Areas[a].Ways[w].FloorId = ResultFloors[f].Id;
            //        }
            //        for (int p = 0; p< ResultFloors[f].Areas[a].Points.Count; p++)
            //        {
            //            ResultFloors[f].Areas[a].Points[p].FloorId = ResultFloors[f].Id;
            //        }
            //    }
            //}
            
        }
        private void OverrideAssignedAreaShops(ObservableCollection<Floor> inputFloors)
        {
            for (int f = 0; f < inputFloors.Count; f++)
            {
                for (int a = 0; a < inputFloors[f].Areas.Count; a++)
                {
                    var inputArea = inputFloors[f].Areas[a];
                    var resultArea = ResultFloors[f].Areas.Where(o => o.Points[0].X == inputArea.Points[0].X &&
                     o.Points[0].Y == inputArea.Points[0].Y).FirstOrDefault();
                    var resultAreaIndex = ResultFloors[f].Areas.IndexOf(resultArea);
                    if (resultArea != null)
                    {
                        if (inputArea.EditDate > resultArea.EditDate)
                        {
                            Debug.WriteLine("Замена магазина арены "+ inputArea.Name);
                            ResultFloors[f].Areas[resultAreaIndex] = inputArea;
                    
                        }
                    }                  
                }
            }
        }
        private void OverrideAreaPointsPositions(ObservableCollection<Floor> inputFloors)
        {
            for (int f = 0; f < inputFloors.Count; f++)
            {
                for (int a = 0; a < inputFloors[f].Areas.Count; a++)
                {
                    var inputArea = inputFloors[f].Areas[a];
                    if (inputArea == null) { continue; }
                    if (inputArea.Points.Count ==0) { continue; }

                    var resultFloorArea = ResultFloors[f].Areas.Where(o=>o.Points.Count >0).Where(o => o.Points[0].X == inputArea.Points[0].X &&
                     o.Points[0].Y == inputArea.Points[0].Y).FirstOrDefault();
                    int resultFloorAreaIndex = ResultFloors[f].Areas.IndexOf(resultFloorArea);
                    if (resultFloorArea != null)
                    {
                        if (inputArea.EditDate > resultFloorArea.EditDate)
                        {
                            Debug.WriteLine("Замена точек арены " + inputArea.Name);
                            ResultFloors[f].Areas[resultFloorAreaIndex] = inputFloors[f].Areas[a];

                        }
                    }
                }
            }
        }

        private void SaveGeneratedJSON(string filepath= null)
        {
            string resultFileName = Path.Combine(ResultJsonFilepath, "settings.json");
            if (!string.IsNullOrEmpty(filepath))
            {
                resultFileName = filepath;
            }

            if (File.Exists(resultFileName)) { File.Delete(resultFileName); }
            using (StreamWriter sw = new StreamWriter(resultFileName))
            {
                serializer.Serialize(sw, ResultFloors);
            }
        }

    }
}
