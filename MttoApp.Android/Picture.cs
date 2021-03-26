using Android.Content;
using Android.Widget;
using MttoApp.Droid;
using System;

using MttoApp.Servicios;

[assembly: Xamarin.Forms.Dependency(typeof(Picture_Droid))]

namespace MttoApp.Droid
{
    public class Picture_Droid : IPicture
    {
        [Obsolete]
        public void SavePicture(string filename, byte[] imgdata)
        {
            //SE EVALUA SI EL NOMBRE ENVIADO CONTIENE INFORMACION
            if (!string.IsNullOrEmpty(filename))
            {
                //SE BUSCA LA DIRECCION PARA LA MEMORIA EXTERNA
                var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim).AbsolutePath;
                //SE COMPLETA LA DIRECCION EN LA CUAL SE GUARDARA LA IMAGEN
                string filepath = System.IO.Path.Combine(dir, filename);

                //SE VERIFICA SI SE PUEDE ESCRIBIR SOBRE LA MEMORIA
                if (Android.OS.Environment.MediaMounted.Equals(Android.OS.Environment.ExternalStorageState))
                {
                    try
                    {
                        System.IO.File.WriteAllBytes(filepath, imgdata);
                        var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                        mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(filepath)));
                        Xamarin.Forms.Forms.Context.SendBroadcast(mediaScanIntent);

                        Toast.MakeText(Android.App.Application.Context, "Se almaceno la imagen satisfactoriamente", ToastLength.Short).Show();
                    }
                    catch (System.Exception e)
                    {
                        System.Console.WriteLine("\n==================================================");
                        System.Console.WriteLine("==================================================");
                        System.Console.WriteLine("\nERROR: " + e.ToString() + "\n");
                        System.Console.WriteLine("==================================================");
                        System.Console.WriteLine("==================================================\n");

                        Toast.MakeText(Android.App.Application.Context, "Se produjo un error al intentar guardar la imagen", ToastLength.Short).Show();
                    }
                }
            }
        }
    }
}