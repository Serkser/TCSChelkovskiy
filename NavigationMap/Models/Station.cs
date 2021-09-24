using NavigationMap.Core;

using System.Windows;

namespace NavigationMap.Models
{
    public class Station : ObservableObject
    {
        private AreaPoint _areaPoint;

        public AreaPoint AreaPoint
        {
            get => _areaPoint;
            set
            {
                _areaPoint = value;
                OnPropertyChanged();
            }
        }

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

        public Point Position
        {
            get => _areaPoint.Position;
            set
            {
                _areaPoint.Position = value;

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

        private string _login;

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public Station()
        {
            AreaPoint = new AreaPoint();
        }
    }
}
