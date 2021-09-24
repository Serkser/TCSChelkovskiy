using NavigationMap.Core;

namespace NavigationMap.Models
{
    public class AreaImage : ObservableObject
    {
        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private int _areaId;

        public int AreaId
        {
            get => _areaId;
            set
            {
                _areaId = value;
                OnPropertyChanged();
            }
        }

        private string _image;

        public string Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }
    }
}