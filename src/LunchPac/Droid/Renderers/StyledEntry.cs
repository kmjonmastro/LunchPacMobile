using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using LunchPac;
using LunchPac.Droid;

[assembly: ExportRenderer(typeof(StyledEntryRenderer), typeof(StyledEntry))]
namespace LunchPac.Droid
{
    public class StyledEntry : EntryRenderer
    {
        // Override the OnElementChanged method so we can tweak this renderer post-initial setup
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundColor(Resources.GetColor(Resource.Color.txt_field_bg));
                Control.SetTextColor(Resources.GetColor(Resource.Color.white));
                Control.SetHintTextColor(Resources.GetColor(Resource.Color.darkgraycolor));
                Control.SetPadding(10, 0, 10, 0);
                Control.SetMinHeight(50);
                Control.SetTextSize(Android.Util.ComplexUnitType.Dip, 16);
            }
        }
    }
}

