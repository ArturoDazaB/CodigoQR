using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Foundation;
using UIKit;

namespace MttoApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            //==============================================================================================================================
            //==============================================================================================================================
            //INICIALIZACION DE LA BASE DE DATOS
            string fileName = "mttoappdb.db3";
            string folderPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "..", "Labrary");
            string CompletePath = Path.Combine(folderPath, fileName);

            //==============================================================================================================================
            //==============================================================================================================================
            //INICIALIZACION DEL PLUGIN Rg.Plugin.Popup
            Rg.Plugins.Popup.Popup.Init();

            //==============================================================================================
            //==============================================================================================
            //INICIALIZACION DEL PLUGIN Zxing EN ANDROID
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();


            LoadApplication(new App(CompletePath));
            return base.FinishedLaunching(app, options);
        }
    }
}
