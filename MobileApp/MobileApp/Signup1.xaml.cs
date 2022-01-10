using Org.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Signup1 : ContentPage
    {
        public Signup1()
        {
            InitializeComponent();
            RetrieveApi();

            etEmail.Focused += Email_Focused;
            etEmail.Unfocused += Email_Unfocused;
            etUsername.Focused += Username_Focused;
            etUsername.Unfocused += Username_Unfocused;
            etPassword.Focused += Password_Focused;
            etPassword.Unfocused += Password_Unfocused;
            etConfirmPassword.Focused += ConfirmPassword_Focused;
            etConfirmPassword.Unfocused += ConfirmPassword_Unfocused;

            etEmail.TextChanged += Email_TextChanged;
            etUsername.TextChanged += Username_TextChanged;
            etPassword.TextChanged += Password_TextChanged;
            etConfirmPassword.TextChanged += ConfirmPassword_TextChanged;

            lblBack.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Navigation.PopAsync();
                })
            });
        }


        void Email_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckEmail(etEmail.Text) == false)
            {
                vldEmail.Text = "Not a valid email";
                vldEmail.IsVisible = true;
            }
            else
            {
                vldEmail.IsVisible = false;
            }
        }

        void Username_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (etUsername.Text.Length < 6)
            {
                vldUsername.Text = "Please enter Username with atleast six(6) characters";
                vldUsername.IsVisible = true;
            }
            else
            {
                vldUsername.IsVisible = false;
            }
        }
        void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (etPassword.Text.Length < 8)
            {
                vldPassword.Text = "Please enter Password with atleast eight(8) characters";
                vldPassword.IsVisible = true;
            }
            else
            {
                vldPassword.IsVisible = false;
            }
        }
        void ConfirmPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            vldConfirmPassword.IsVisible = false;
        }

        #region

        private void Email_Focused(object sender, FocusEventArgs e)
        {
            lblEmail.ScaleYTo(0.8);
            lblEmail.ScaleXTo(0.8);
            lblEmail.TranslateTo(0, -(lblEmail.Height));
        }
        private void Email_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(etEmail.Text))
            {
                lblEmail.ScaleXTo(1);
                lblEmail.ScaleYTo(1);
                lblEmail.TranslateTo(0, 0);
            }
        }

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


        async void OnButtonNext(object sender, EventArgs args)
        {
            int CountError = 0;
            if (etEmail.Text == "" || etEmail.Text == null)
            {
                vldEmail.Text = "Email is required";
                vldEmail.IsVisible = true;
                CountError++;
            }
            if(etUsername.Text == "" || etUsername.Text == null)
            {
                vldUsername.Text = "Username is required";
                vldUsername.IsVisible = true;
                CountError++;
            } 
            if (etUsername.Text.Length < 6 )
            {
                vldUsername.Text = "Please enter Username with atleast six(6) characters";
                vldUsername.IsVisible = true;
                CountError++;
            }
            if (etPassword.Text == "" || etPassword.Text == null)
            {
                vldPassword.Text = "Password is required";
                vldPassword.IsVisible = true;
                CountError++;
            }
            if (etPassword.Text.Length < 8 )
            {
                vldPassword.Text = "Please enter Password with atleast eight(8) characters";
                vldPassword.IsVisible = true;
                CountError++;
            }
            if (etConfirmPassword.Text =="" || etConfirmPassword.Text == null)
            {
                vldConfirmPassword.Text = "Confirm Password is required.";
                vldConfirmPassword.IsVisible = true;
                CountError++;
            }
            if ( etPassword.Text != etConfirmPassword.Text)
            {
                await DisplayAlert("Invalid input!", "Password and Confirm Password are not equal.", "OK");
                CountError++;
            }

            if(CountError <=0 )
            {
                if (CheckEmail(etEmail.Text) == true)
                {
                    ValidateUsername();
                }
                else
                {
                    vldEmail.Text = "Not a valid email";
                }
            }
        }


        //Check Username or Email is Exist
        private void ValidateUsername()
        {
            try
            {
                progressbar.IsVisible = true;
                zsg_hosting hosting = new zsg_hosting();
                zsg_nameandimage name = new zsg_nameandimage();

                var uri = hosting.getCheckusername();

                string responseFromServer;
                using (var wb = new WebClient())
                {
                    var datas = new NameValueCollection();
                    datas["Username"] = etUsername.Text;
                    datas["Email"] = etEmail.Text;

                    var response = wb.UploadValues(uri, "POST", datas);
                    responseFromServer = Encoding.UTF8.GetString(response);
                }

                if (responseFromServer == "Username is Available")
                {
                    Info inf = new Info();
                    inf.Infos(etEmail.Text, etUsername.Text, etPassword.Text);

                    Navigation.PushAsync(new SignUp2());
                }
                else
                {
                    DisplayAlert("Invalid Email or Username.", responseFromServer, "OK");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Something went wrong.", "Please check your internet connection.", "OK");
                Console.WriteLine("ERROR" + ex.Message);
            }
            finally
            {
                progressbar.IsVisible = false;
            }
        }

        private bool CheckEmail(String email)
        {

            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
                && Regex.IsMatch(email, @"^(?=.{1,64}@.{4,64}$)(?=.{6,100}$).*");
            //try
            //{
            //    MailAddress m = new System.Net.Mail.MailAddress(email);
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("ERROR" + ex.Message);
            //    return false;
            //}
        }

        async void RetrieveApi()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    zsg_hosting hosting = new zsg_hosting();
                    var uri = hosting.getApiKey();
                    var result = await client.GetStringAsync(uri);

                    JSONObject jsonresult = new JSONObject(result);
                    int success = jsonresult.GetInt("success");

                    if (success == 1)
                    {
                        JSONArray information = jsonresult.GetJSONArray("info");
                        JSONObject info = information.GetJSONObject(0);

                        zsg_ApiKey ap = new zsg_ApiKey();
                        ap.setSendGridKey(info.GetString("ApiKey"));
                    }
                }
            }
            catch (Exception ex)
            {
                await this.DisplayToastAsync("Something went wrong.", 5000);
                Console.WriteLine("ERROR: " + ex.Message);
                await Task.Delay(10000);
                RetrieveApi();
            }
        }
    }
}