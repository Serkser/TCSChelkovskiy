using System.Windows;

namespace NavigationMap.Models
{
    public interface IMapElement
    {
        Point Position { get; set; }
        int FloorId { get; }

    }
}
