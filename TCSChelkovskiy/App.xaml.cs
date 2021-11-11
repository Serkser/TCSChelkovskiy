using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TCSChelkovskiy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            string pathToHost = Path.Combine(Environment.CurrentDirectory, "host.txt");
            if (File.Exists(pathToHost))
            {
                using (StreamReader sr = File.OpenText(pathToHost))
                {
                    string host = sr.ReadToEnd();
                    TCSchelkovskiyAPI.TCSchelkovskiyAPI.HOST = host;
                }
            }
            
            Memory.KioskObjects.LoadAllObjects().Wait();
          
        }
    }
}
