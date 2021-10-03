using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;

namespace TCSChelkovskiy.Services
{
    public static class BitmapToImageSourceConverter
    {
       public static BitmapImage BitmapToImageSource(Bitmap bitmap,string filename)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();


                if (File.Exists(Path.Combine(Environment.CurrentDirectory, filename)))
                {
                    try
                    {
                        File.Delete(Path.Combine(Environment.CurrentDirectory, filename));
                    }
                    catch { }

                }
                return bitmapimage;
            }
        }

       
    }
}
