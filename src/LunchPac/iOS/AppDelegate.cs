using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace LunchPac.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            // Code for starting up the Xamarin Test Cloud Agent
            #if ENABLE_TEST_CLOUD
            Xamarin.Calabash.Start();
            #endif

            LoadApplication(new App(new iOSModule()));

            SetGeneralStyles();

            return base.FinishedLaunching(app, options);
        }

        public static void SetGeneralStyles()
        {
            var blueColor = UIColor.FromRGB(17, 38, 129);
            var titletextAttr = UINavigationBar.Appearance.GetTitleTextAttributes();
            titletextAttr.TextColor = blueColor;
            UINavigationBar.Appearance.TintColor = blueColor;
            UINavigationBar.Appearance.SetTitleTextAttributes(titletextAttr);
        }
    }

    public static class UIColorExtensions
    {
        public static UIColor FromHex(this UIColor color, int hexValue)
        {
            return UIColor.FromRGB(
                (((float)((hexValue & 0xFF0000) >> 16)) / 255.0f),
                (((float)((hexValue & 0xFF00) >> 8)) / 255.0f),
                (((float)(hexValue & 0xFF)) / 255.0f)
            );
        }
    }
}

