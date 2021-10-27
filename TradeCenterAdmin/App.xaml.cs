using NavigationMap.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TradeCenterAdmin
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            Storage.KioskObjects.LoadAllObjects().Wait();           
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "error.txt");
            using (FileStream fs = File.Open(path,FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(e.Exception.Message);
                    sw.WriteLine();
                    sw.WriteLine(e.Exception.StackTrace);

                }
            }
        }
    }
}
