using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using MobileApp.Droid;
using MobileApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(NoOutlinePicker),typeof(NoOutlinePickerRenderer))]
namespace MobileApp.Droid
{
    public class NoOutlinePickerRenderer : PickerRenderer
    {
        public NoOutlinePickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Picker> e)
        {
            base.OnElementChanged(e);
            this.Control.Background = null;
        }
    }
}