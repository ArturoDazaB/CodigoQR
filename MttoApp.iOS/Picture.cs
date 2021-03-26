using Foundation;
using MttoApp.iOS;
using System;
using UIKit;

using MttoApp.Servicios;

[assembly: Xamarin.Forms.Dependency(typeof(Picture_iOS))]

namespace MttoApp.iOS
{
    public class Picture_iOS : IPicture
    {
        public void SavePicture(string filename, byte[] imgdata)
        {
            var charImage = new UIImage(NSData.FromArray(imgdata));
            charImage.SaveToPhotosAlbum((image, error) =>
            {
                if (error != null)
                {
                    Console.WriteLine("\n========================================");
                    Console.WriteLine("========================================");
                    Console.WriteLine("\n" + error.ToString());
                    Console.WriteLine("========================================");
                    Console.WriteLine("========================================\n");
                }
            });
        }
    }
}