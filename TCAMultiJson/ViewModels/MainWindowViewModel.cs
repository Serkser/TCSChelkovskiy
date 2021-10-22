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
        private bool overrideAssignedShop;
        public bool OverrideAssignedShop
        {
            get { return overrideAssignedShop; }
            set
            {
                overrideAssignedShop = value;
                OnPropertyChanged("OverrideAssignedShop");
            }
        }
        private bool overrideAreaPositions;
        public bool OverrideAreaPositions
        {
            get { return overrideAreaPositions; }
            set
            {
                overrideAreaPositions = value;
                OnPropertyChanged("OverrideAreaPositions");
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
                           for (int i=0;i<ofd.FileNames.Length;i++)
                            {
                                InputJsonFilepaths.Add(ofd.FileNames[i]);
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
                        if (string.IsNullOrEmpty(ResultJsonFilepath))
                        {
                            MessageBox.Show("Укажите путь выходного файла"); return;
                        }
                        if (InputJsonFilepaths.Count < 2)
                        {
                            MessageBox.Show("Добавьте не менее 2-х файлов для объединения"); return;
                        }

                        Services.JsonGenerator generator = new Services.JsonGenerator();
                        generator.InputJsonFilepaths = InputJsonFilepaths;
                        generator.ResultJsonFilepath = ResultJsonFilepath;
                        generator.OverrideAssignedShops = OverrideAssignedShop;
                        generator.OverrideAreaPositions = OverrideAreaPositions;
                        generator.GenerateJSON();
                        MessageBox.Show("Файл успешно сгенерирован");

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
