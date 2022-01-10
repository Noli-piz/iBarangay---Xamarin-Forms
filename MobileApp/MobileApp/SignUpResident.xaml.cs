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
    public partial class SignUpResident : ContentPage
    {
        public SignUpResident()
        {
            InitializeComponent();
        }

        async void OnButtonClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync( new SignUpNonResident());
        }
    }
}