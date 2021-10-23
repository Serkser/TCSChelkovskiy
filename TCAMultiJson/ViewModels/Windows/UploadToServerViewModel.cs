using Microsoft.Win32;
using NavigationMap.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCAMultiJson.Utilities;

namespace TCAMultiJson.ViewModels.Windows
{
    public class UploadToServerViewModel : INotifyPropertyChanged
    {

        private JsonSerializer serializer = new JsonSerializer();


        private string uploadJsonFilepath;
        public string UploadJsonFilepath
        {
            get { return uploadJsonFilepath; }
            set
            {
                uploadJsonFilepath = value;
                OnPropertyChanged("UploadJsonFilepath");
            }
        }

        private RelayCommand selectJsonFilePath;
        public RelayCommand SelectJsonFilePath
        {
            get
            {
                return selectJsonFilePath ??
                    (selectJsonFilePath = new RelayCommand(obj =>
                    {
                        Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
                        if (ofd.ShowDialog() == true)
                        {
                            UploadJsonFilepath = ofd.FileName;
                        }
                    }));
            }
        }
        private RelayCommand uploadFileToServer;
        public RelayCommand UploadFileToServer
        {
            get
            {
                return uploadFileToServer ??
                    (uploadFileToServer = new RelayCommand(obj =>
                    {
                        if (string.IsNullOrEmpty(UploadJsonFilepath))
                        {
                            MessageBox.Show("Укажите путь к JSON файлу"); return;
                        }

                        ObservableCollection<Floor> floors = new ObservableCollection<Floor>();
                        if (File.Exists(UploadJsonFilepath))
                        {
                            using (StreamReader file = File.OpenText(UploadJsonFilepath))
                            {
                                floors = (ObservableCollection<Floor>)serializer.Deserialize(file, typeof(ObservableCollection<Floor>));
                            }
                        }

                        Services.JsonToServerUploader<Floor> uploader = new Services.JsonToServerUploader<Floor>();
                        uploader.UploadListToServer(floors, "floor");
                        MessageBox.Show("Этажи успешно отправлены на сервер");
                    }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
