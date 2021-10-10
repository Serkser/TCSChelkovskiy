using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TradeCenterAdmin.Utilities;

namespace TradeCenterAdmin.Services
{
    public static class ImageDownloader
    {
        public static async Task<DisposableImage> DownloadImage(string uri, string filename)
        {
            return await Task.Run(() =>
            {
                if (!Directory.Exists("AllImages"))
                    Directory.CreateDirectory("AllImages");
                var imageFile = Path.Combine("AllImages", filename);
                if (!File.Exists(imageFile))
                {
                    string url = Properties.Settings.Default.host;
                    url += uri;
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(url, Path.GetFullPath(imageFile));
                    }
                }
                return new DisposableImage(Path.GetFullPath(imageFile));

            });

        }
    }
}
