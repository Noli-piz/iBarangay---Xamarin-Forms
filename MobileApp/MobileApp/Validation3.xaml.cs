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
    public partial class Validation3 : ContentPage
    {
        private string strImageUrl1;
        private string strImageUrl2;
        private zsg_nameandimage name = new zsg_nameandimage();

        public Validation3(string strImageUrl1, string strImageUrl2)
        {
            InitializeComponent();

            this.strImageUrl1 = strImageUrl1;
            this.strImageUrl2 = strImageUrl2;

            etCedulaNo.Text = name.getCedulaNo();

            etCedulaNo.Focused += CedulaNo_Focused;
            etCedulaNo.Unfocused += CedulaNo_Unfocused;


            if (name.getCedulaNo() != null && name.getCedulaNo() != "")
            {
                lblCedulaNo.ScaleYTo(0.8);
                lblCedulaNo.ScaleXTo(0.8);
                lblCedulaNo.TranslateTo(0, -24);
            }

            lblBack.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Navigation.PopAsync();
                })
            });
        }

        private void CedulaNo_Focused(object sender, FocusEventArgs e)
        {
            lblCedulaNo.ScaleYTo(0.8);
            lblCedulaNo.ScaleXTo(0.8);
            lblCedulaNo.TranslateTo(0, -(lblCedulaNo.Height));
        }
        private void CedulaNo_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(etCedulaNo.Text))
            {
                lblCedulaNo.ScaleXTo(1);
                lblCedulaNo.ScaleYTo(1);
                lblCedulaNo.TranslateTo(0, 0);
            }
        }

        async void OnButtonNext(object sender, EventArgs args)
        {
            //await Navigation.PushAsync(new MyFlyoutPage());

            if (etCedulaNo.Text == "" || etCedulaNo.Text == null)
            {
                await this.DisplayToastAsync("Invalid Input.", 5000);
            }
            else
            {
                Verification();
            }
        }

        private async void Verification()
        {
            try
            {
                zsg_hosting hosting = new zsg_hosting();
                var uri = hosting.getVerification();

                string responseFromServer;
                using (var wb = new WebClient())
                {
                    var datas = new NameValueCollection();
                    datas["Username"] = name.getStrusername();
                    datas["IdImgUrl"] = strImageUrl1;
                    datas["IdAndFaceImgUrl"] = strImageUrl2;
                    datas["CedulaNo"] = etCedulaNo.Text;

                    var response = wb.UploadValues(uri, "POST", datas);
                    responseFromServer = Encoding.UTF8.GetString(response);
                }

                if (responseFromServer == "Operation Failed")
                {
                    await this.DisplayToastAsync("Operation Failed.", 5000);
                }
                else if (responseFromServer == "Operation Success")
                {

                    name.nameandimage();

                    await DisplayAlert("Completed.", "Please wait atleast 3-7 days to validate your account.", "OK");
                    //await Navigation.PushAsync(new MyFlyoutPage());
                    await Navigation.PopToRootAsync();
                }
                else
                {
                    await this.DisplayToastAsync(responseFromServer, 5000);
                }

            }
            catch (Exception ex)
            {
                await this.DisplayToastAsync("Something went wrong.", 5000);
                Console.WriteLine("ERROR: " + ex.Message);
            }
            finally
            {

            }
        }

    }
}