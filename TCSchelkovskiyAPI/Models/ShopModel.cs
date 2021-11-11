using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCSchelkovskiyAPI.Models
{
    public class ShopModel : IComparable
    {
        public string IconURI { get; set; }
        public string Icon { get; set; }
        public string ImagesPrefix { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public FloorModel Floor { get; set; }
        public List<string> Images { get; set; }
        public List<CategoryModel> Categories { get; set; }


        public bool IsUsedOnMap { get; set; }
        public override string ToString()
        {
            if (IsUsedOnMap)
            {
                return $"Установлен :  {Name}  ({Floor.Floor} этаж)";
            }
            else
            {
                return $"{Name}  ({Floor.Floor} этаж)";
            }
         
        }

        public int CompareTo(object obj)
        {
            ShopModel p = obj as ShopModel;
            if (p != null)
                return this.IsUsedOnMap.CompareTo(p.IsUsedOnMap);
            else
                throw new Exception("Невозможно сравнить два объекта");
        }
    }

    public class ShopModelFloorComparer : IComparer<ShopModel>
    {
        public int Compare(ShopModel p1, ShopModel p2)
        {
            if (p1.Floor.Floor > p2.Floor.Floor)
                return 1;
            else if (p1.Floor.Floor < p2.Floor.Floor)
                return -1;
            else
                return 0;
        }
    }
}
