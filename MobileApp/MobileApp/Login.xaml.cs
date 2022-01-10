using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Controls;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        private bool TimerStopper = true;
        public Login()
        {
            InitializeComponent();

            OnButtonForgotPass();

            etUsername.Focused += Username_Focused;
            etUsername.Unfocused += Username_Unfocused;
            etPassword.Focused += Password_Focused;
            etPassword.Unfocused += Password_Unfocused;


            // Intialized User Login Info to the entry
            var myLoginStatus = Preferences.Get("my_loginstatus", "false");
            if (myLoginStatus == "true")
            {
                var myUsername = Preferences.Get("my_username", "0");
                var myPassword = Preferences.Get("my_password", "0");

                etUsername.Text = myUsername;
                etPassword.Text = myPassword;

                lblUsername.ScaleYTo(0.8);
                lblUsername.ScaleXTo(0.8);
                lblUsername.TranslateTo(0, -24);

                lblPassword.ScaleYTo(0.8);
                lblPassword.ScaleXTo(0.8);
                lblPassword.TranslateTo(0, -24);
            }
        }

        #region

        private void Username_Focused(object sender, FocusEventArgs e)
        {
            lblUsername.ScaleYTo(0.8);
            lblUsername.ScaleXTo(0.8);
            lblUsername.TranslateTo(0, -(lblUsername.Height));
        }
        private void Username_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(etUsername.Text))
            {
                lblUsername.ScaleXTo(1);
                lblUsername.ScaleYTo(1);
                lblUsername.TranslateTo(0, 0);
            }
        }


        private void Password_Focused(object sender, FocusEventArgs e)
        {
            lblPassword.ScaleYTo(0.8);
            lblPassword.ScaleXTo(0.8);
            lblPassword.TranslateTo(0, -(lblPassword.Height));
        }
        private void Password_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(etPassword.Text))
            {
                lblPassword.ScaleXTo(1);
                lblPassword.ScaleYTo(1);
                lblPassword.TranslateTo(0, 0);
            }
        }

        #endregion

        async void OnButtonLogin(object sender, EventArgs args)
        {
            if (etUsername.Text == null || etUsername.Text == "" || etPassword.Text == null || etPassword.Text == "")
            {
                    await DisplayAlert("Invalid input!", "Please enter a valid input.", "OK");
            }
            else
            {

                progressbar.IsVisible = true;
                Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
                {
                    AddProgress(progressbar, 10);

                    Console.WriteLine("Running");
                    if (TimerStopper == true)
                        return true;
                    else
                        return false;
                });

                await Task.Delay(2000);
                CheckLoginStatus(etUsername.Text , etPassword.Text);
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

                    //Load Data
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
                    progressbar.IsVisible = false;
                    await DisplayAlert("Account Banned.", "You have been banned by Barangay Hall", "OK");
                }
                else
                {
                    progressbar.IsVisible = false;
                    await this.DisplayToastAsync(responseFromServer, 5000);
                }

            }
            catch (Exception ex)
            {
                await this.DisplayToastAsync("Something went wrong. Please check your internet connection.", 5000);
                Console.WriteLine("ERROR: " + ex.Message);
            }
            finally
            {
                progressbar.IsVisible = false;
                TimerStopper = false;
            }

        }


        void OnButtonForgotPass()
        {
            lblForgotpass.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Navigation.PushAsync(new ForgetPassword1());

                })
            });
        }

        async void OnButtonSignup(object sender, EventArgs args)
        {
             await Navigation.PushAsync(new Signup1());
        }

        void AddProgress(CircularProgressBar view, double add)
        {
            var value = view.Progress + add;

            if (value > 100)
                value = 0;

            view.Progress = value;
        }

    }
}