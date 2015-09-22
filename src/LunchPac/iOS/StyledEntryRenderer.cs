
[assembly: Xamarin.Forms.ExportRenderer(typeof(LunchPac.StyledEntryRenderer), typeof(LunchPac.iOS.StyledEntryRenderer))]
namespace LunchPac.iOS
{
    using System;
    using Xamarin.Forms.Platform.iOS;
    using Xamarin.Forms;

    public class StyledEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);


            if (Control != null)
            {
                Control.Layer.BorderWidth = 0.0f;
            }


        }
    }
}

