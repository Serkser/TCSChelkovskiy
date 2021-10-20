using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCAMultiJson.Utilities;

namespace TCAMultiJson.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        public MainWindowViewModel()
        {
            InputJsonFilepaths = new ObservableCollection<string>();
        }

        private string resultJsonFilepath;
        public string ResultJsonFilepath
        {
            get { return resultJsonFilepath; }
            set
            {
                resultJsonFilepath = value;
                OnPropertyChanged("ResultJsonFilepath");
            }
        }
        private ObservableCollection<string> inputJsonFilepaths;
        public ObservableCollection<string> InputJsonFilepaths
        {
            get { return inputJsonFilepaths; }
            set
            {
                inputJsonFilepaths = value;
                OnPropertyChanged("InputJsonFilepaths");
            }
        }

        private RelayCommand addJsonFiles;
        public RelayCommand AddJsonFiles
        {
            get
            {
                return addJsonFiles ??
                    (addJsonFiles = new RelayCommand(obj =>
                    {
                        Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
                        ofd.Multiselect = true;
                        ofd.Filter = "json files (*.json)|*.json";
                        if (ofd.ShowDialog() == true)
                        {
                            foreach(var path in ofd.FileNames)
                            {
                                InputJsonFilepaths.Add(path);
                            }
                        }                       
                    }));
            }
        }

        private RelayCommand removeJsonFile;
        public RelayCommand RemoveJsonFile
        {
            get
            {
                return removeJsonFile ??
                    (removeJsonFile = new RelayCommand(obj =>
                    {
                        if (obj != null)
                        {
                            InputJsonFilepaths.Remove((string)obj);
                        }
                    }));
            }
        }

        private RelayCommand generateJsonFile;
        public RelayCommand GenerateJsonFile
        {
            get
            {
                return generateJsonFile ??
                    (generateJsonFile = new RelayCommand(obj =>
                    {

                    }));
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
                        FolderBrowserDialog ofd = new FolderBrowserDialog();
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            ResultJsonFilepath = ofd.SelectedPath;
                        }
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
