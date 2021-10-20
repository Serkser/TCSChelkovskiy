using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeCenterAdmin.Services.MapObjectSavers
{
    public class ServerMapObjectsSaver : IMapObjectSaver
    {
        public void Save(IList<Floor> floors)
        {
            Services.JsonToServerUploader<Floor> uploader = new Services.JsonToServerUploader<Floor>();
            ObservableCollection<Floor> Floors = new ObservableCollection<Floor>(floors);
            uploader.UploadListToServer(Floors, "floor");
        }
    }
}
