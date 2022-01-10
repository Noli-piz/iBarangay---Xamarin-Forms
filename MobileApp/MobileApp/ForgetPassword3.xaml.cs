using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgetPassword3 : ContentPage
    {
        public ForgetPassword3()
        {
            InitializeComponent();

            etPassword.Focused += Password_Focused;
            etPassword.Unfocused += Password_Unfocused;
            etConfirmPassword.Focused += ConfirmPassword_Focused;
            etConfirmPassword.Unfocused += ConfirmPassword_Unfocused;

            lblBack.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Navigation.PopAsync();
                })
            });
        }

        #region
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

        private void ConfirmPassword_Focused(object sender, FocusEventArgs e)
        {
            lblConfirmPassword.ScaleYTo(0.8);
            lblConfirmPassword.ScaleXTo(0.8);
            lblConfirmPassword.TranslateTo(0, -(lblConfirmPassword.Height));
        }
        private void ConfirmPassword_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(etConfirmPassword.Text))
            {
                lblConfirmPassword.ScaleXTo(1);
                lblConfirmPassword.ScaleYTo(1);
                lblConfirmPassword.TranslateTo(0, 0);
            }
        }

        #endregion


        async void OnButtonClicked(object sender, EventArgs args)
        {

            if (etPassword.Text == "")
            {
                await this.DisplayToastAsync("Password cannot be empty.", 3000);
            }
            else if (etConfirmPassword.Text == "")
            {
                await this.DisplayToastAsync("Confirm Password cannot be empty.", 3000);
            }
            else if (etPassword.Text != etConfirmPassword.Text)
            {
                await this.DisplayToastAsync("Password and Confirm password is not equal.", 3000);
            }
            else
            {
                UpdatePassword();
            }

        }


        private async void UpdatePassword()
        {
            try
            {
                zsg_hosting hosting = new zsg_hosting();
                zsg_emailverification email = new zsg_emailverification();

                var uri = hosting.getUpdatePass();

                string responseFromServer;
                using (var wb = new WebClient())
                {
                    var datas = new NameValueCollection();
                    datas["Email"] = email.getEmail();
                    datas["Password"] = etPassword.Text;

                    var response = wb.UploadValues(uri, "POST", datas);
                    responseFromServer = Encoding.UTF8.GetString(response);
                }

                if (responseFromServer == "Updated Successfully")
                {
                    await this.DisplayAlert("Password Successfuly Updated", "You will redirected to the Login Page", "OK");
                    //await Navigation.PushAsync(new Login());
                    await Navigation.PopToRootAsync();
                }
                else
                {
                    await this.DisplayToastAsync(responseFromServer, 5000);
                }
            }
            catch (Exception ex)
            {
                await this.DisplayToastAsync("Please check your connection.", 3000);
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }
    }
}