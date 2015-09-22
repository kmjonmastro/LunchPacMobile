using System;
using Android.App;
using Android.Graphics.Drawables;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using LunchPac.Droid;
using System.ComponentModel;

[assembly:ExportRenderer(typeof(WebView), typeof(ZoomableWebViewRenderer))]

namespace LunchPac.Droid
{
    public class ZoomableWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Control != null)
            {
                Control.Settings.BuiltInZoomControls = true;
                Control.Settings.DisplayZoomControls = false;
            }
            base.OnElementPropertyChanged(sender, e);
        }
    }
}

