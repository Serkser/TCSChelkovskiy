using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static async Task LoadAllObjects()
        {
          await Task.Run(() =>
            {
                Categories = new ObservableCollection<CategoryModel>(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetCategories());
                Floors = ConvertToFloors(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetFloors());
                Shops = new ObservableCollection<ShopModel>(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetShops());
                Gallery = new ObservableCollection<ShopGalleryModel>(TCSchelkovskiyAPI.TCSchelkovskiyAPI.GetShopsGallery());

            });
        }

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
