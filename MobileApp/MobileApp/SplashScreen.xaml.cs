using System;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Controls;
using Xamarin.Essentials;
using System.Net;
using System.Collections.Specialized;
using Xamarin.CommunityToolkit.Extensions;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashScreen : ContentPage
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
            {
                AddProgress(progressbar, 10);
                return true;
            });




            var myLoginStatus = Preferences.Get("my_loginstatus", "false");
            if (myLoginStatus == "false")
            {
                ToLoginPage();
            }
            else
            {
                var myUsername = Preferences.Get("my_username", "0");
                var myPassword = Preferences.Get("my_password", "0");

                if (myUsername == "0" && myPassword == "0")
                {
                    ToLoginPage();
                }
                else
                {
                    CheckLoginStatus(myUsername, myPassword);
                }
            }

        }

        async void CheckLoginStatus(string username, string password)
        {
            try
            {
                zsg_hosting hosting = new zsg_hosting();
                var uri = hosting.getLogin();

                string responseFromServer;
                using (var wb = new WebClient())
                {
                    var datas = new NameValueCollection();
                    datas["Username"] = username;
                    datas["Password"] = password;

                    var response = wb.UploadValues(uri, "POST", datas);
                    responseFromServer = Encoding.UTF8.GetString(response);
                }

                if (responseFromServer == "Login Success")
                {
                    //Store data to preferences
                    Preferences.Set("my_username", username);
                    Preferences.Set("my_password", password);
                    Preferences.Set("my_loginstatus", "true");

                    //Save the user's data to a class
                    zsg_nameandimage user = new zsg_nameandimage();
                    user.setStrusername(username);
                    user.nameandimage();

                    await Task.Delay(5000);
                    await Navigation.PushAsync(new MyFlyoutPage());
                    Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

                }
                else if (responseFromServer == "This Account is Banned by the Admin.")
                {
                    //Custom Dialog
                    await DisplayAlert("Alert", "You have been banned", "OK");
                    ToLoginPage();
                }
                else
                {
                    await this.DisplayToastAsync(responseFromServer, 5000);
                    ToLoginPage();
                }

            }
            catch (Exception ex)
            {
                await this.DisplayToastAsync("Something went wrong. Please check your internet connection.", 5000);
                Console.WriteLine("ERROR: " + ex.Message);
                ToLoginPage();

            }

        }

        async void ToLoginPage()
        {
            await Task.Delay(1000);
            await Navigation.PushAsync(new Login());
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
        }

        void AddProgress(CircularProgressBar view, double add)
        {
            var value = view.Progress + add;

            if (value > 100)
                value = 0;

            view.Progress = value;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}