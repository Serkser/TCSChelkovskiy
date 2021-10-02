using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TCSChelkovskiy.Services
{
    public static class ImageDownloader
    {
        public static Bitmap DownloadImage(string uri,string filename)
        {
            string url = "https://navigator.useful.su/";
            url += uri;
            using (WebClient client = new WebClient())
            {
             
                client.DownloadFile(url, Path.Combine(Environment.CurrentDirectory,filename));
                //MessageBox.Show(Path.Combine(Environment.CurrentDirectory, filename));
            }
            Bitmap img = null;
            try
            {
                img = (Bitmap)Image.FromFile(Path.Combine(Environment.CurrentDirectory, filename));
            }
            //Если картинка битая
            catch (OutOfMemoryException) {
                img = new Bitmap(1, 1);
            }
  
           
          
            return img;
        }
    } 
}
