using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TCSChelkovskiy.Annotations;
using TCSChelkovskiy.Utilities;
using TCSchelkovskiyAPI.Models;

namespace TCSChelkovskiy.Models
{
    public class BannerContainer: INotifyPropertyChanged
    {

        private BannerModel _bannerModel;
        private DisposableImage _image;
        public BannerModel BannerModel
        {
            get => _bannerModel;
            set
            {
                _bannerModel = value;
                OnPropertyChanged();
            }
        }

        public DisposableImage Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
