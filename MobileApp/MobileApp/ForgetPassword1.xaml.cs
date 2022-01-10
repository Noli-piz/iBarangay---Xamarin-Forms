using Org.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgetPassword1 : ContentPage
    {
        private TimeSpan ts = TimeSpan.FromSeconds(5000);
        private zsg_emailverification email = new zsg_emailverification();


        public ForgetPassword1()
        {
            InitializeComponent();

            etUsername.Focused += Username_Focused;
            etUsername.Unfocused += Username_Unfocused;


            lblBack.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Navigation.PopAsync();
                })
            });
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


        async void OnButtonNext(object sender, EventArgs args)
        {

            if (etUsername.Text == "" || etUsername.Text == null)
            {
                await this.DisplayToastAsync("Please enter a Username.", 5000);
            }
            else
            {
                ValidateUsername();
            }

            //await Navigation.PushAsync(new ForgetPassword2());
        }


        //Check if Username is exist.
        private async void ValidateUsername()
        {
            try
            {
                zsg_hosting hosting = new zsg_hosting();

                var uri = hosting.getForgotPass();

                string responseFromServer;
                using (var wb = new WebClient())
                {
                    var datas = new NameValueCollection();
                    datas["Username"] = etUsername.Text;

                    var response = wb.UploadValues(uri, "POST", datas);
                    responseFromServer = Encoding.UTF8.GetString(response);
                }

                if (responseFromServer == "Username is Exist")
                {
                    RetrieveEmail();
                }
                else
                {
                    await this.DisplayToastAsync(responseFromServer, 5000);
                }
            }
            catch (Exception ex)
            {
                await this.DisplayToastAsync("Please check your connection", 5000);
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }
        

        async void RetrieveEmail()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    zsg_hosting hosting = new zsg_hosting();
                    var uri = hosting.getEmail() + "?Username=" + etUsername.Text;
                    var result = await client.GetStringAsync(uri);

                    JSONObject jsonresult = new JSONObject(result);
                    int success = jsonresult.GetInt("success");

                    if (success == 1)
                    {
                        JSONArray information = jsonresult.GetJSONArray("info");
                        JSONObject info = information.GetJSONObject(0);

                        email.setEmail(info.GetString("Email"));

                        zsg_ApiKey ap = new zsg_ApiKey();
                        ap.setSendGridKey(info.GetString("ApiKey"));

                        await Navigation.PushAsync(new ForgetPassword2());
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                await this.DisplayToastAsync("Unable to reach server. Please try again later.", 5000);
                Console.WriteLine("1 ERROR: " + ex.Message);
            }
            catch (Exception ex)
            {
                await this.DisplayToastAsync("Something went wrong.", 3000);
                Console.WriteLine(ex.ToString() +"2 ERROR: " + ex.Message);
            }
        }
    }
}