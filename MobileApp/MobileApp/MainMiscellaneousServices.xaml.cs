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
    public partial class MainMiscellaneousServices : ContentPage
    {
        public MainMiscellaneousServices()
        {
            InitializeComponent();
        }

        async void OnButtonRequest(object source, EventArgs e)
        {
            await Navigation.PushAsync(new RMiscellaneousServices());
        }
    }
}