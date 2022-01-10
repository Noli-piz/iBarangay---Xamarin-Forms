using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermsAndCondition : ContentPage
    {
        public TermsAndCondition()
        {
            InitializeComponent();

            lblBack.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Navigation.PopAsync();
                })
            });
        }

        async void OnButtonClickedAccept(object sender, EventArgs args)
        {

        }

        async void OnButtonClickedDecline(object sender, EventArgs args)
        {

        }
    }
}