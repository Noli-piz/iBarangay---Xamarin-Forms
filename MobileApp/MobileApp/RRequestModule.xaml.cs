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
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RRequestModule : ContentPage
    {

        private List<String> listDeliveryOption;
        private List<String> listCertificate;
        private List<String> listDocumentFee;
        private List<String> listDeliveryFee;
        private zsg_hosting hosting = new zsg_hosting();
        private string strCertificate, strDeliveryOption;
        private int CountError = 0;
        public RRequestModule()
        {
            InitializeComponent();

            InitializedCertificates();
            InitializedDeliveryOpt();

            etPurpose.TextChanged += Purpose_TextChanged;
            pickerDelivery.SelectedIndexChanged += DeliveryOption_SelectChange;

            lblBack.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Navigation.PopAsync();
                })
            });
        }

        async void Purpose_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (etPurpose.Text.Length > 0)
            {
                vldPurpose.IsVisible = false;
            }
            else
            {
                vldPurpose.Text = "Enter a Purpose";
                vldPurpose.IsVisible = true;
            }
        }

        void DeliveryOption_SelectChange(object sender, EventArgs e)
        {
            vldDeliveryOption.IsVisible = false;
        }

        void OnSelectIndexChange(object sender , EventArgs e)
        {
            vldCertificate.IsVisible = false;

            int fee = pickerCertificate.SelectedIndex;

            lblDocumentFee.Text = "Document Fee: " + listDocumentFee[fee].ToString();
            lblDeliveryFee.Text = "Delivery Fee: " + listDeliveryFee[fee].ToString();
        }

        async void OnButtonClicked(object sender, EventArgs e)
        {
            CountError = 0;

            strCertificate = pickerCertificate.SelectedItem + "";
            strDeliveryOption = pickerDelivery.SelectedItem + "";
            if (strCertificate == "" || strCertificate == null)
            {
                vldCertificate.Text = "Select a valid Item";
                vldCertificate.IsVisible = true;
                CountError++;
            }
            if (etPurpose.Text == "" || etPurpose.Text == null)
            {
                vldPurpose.Text = "Enter a Purpose";
                vldPurpose.IsVisible = true;
                CountError++;
            }
            if (strDeliveryOption == "" || strDeliveryOption == null)
            {
                vldDeliveryOption.Text = "Select valid Delivery Option";
                vldDeliveryOption.IsVisible = true;
                CountError++;
            }
            
            if(CountError <=0 )
            {
                bool answer = await DisplayAlert("", "You are about to request " + strCertificate
                    + " for the purpose of " + etPurpose.Text
                    + ". \n\n Your request will be processed approximately 1-5 days.", "Confirm", "Back");

                if (answer == true)
                {
                    InsertRequest();
                }
            }
            else
            {
                await DisplayAlert("", "Please check all required fields.", "OK");
            }
        }

        private async void InsertRequest()
        {
            try
            {
                zsg_nameandimage name = new zsg_nameandimage();
                zsg_hosting hosting = new zsg_hosting();

                DateTime dateToday = DateTime.Now;

                var uri = hosting.getInsertrequest();

                string responseFromServer;
                using (var wb = new WebClient())
                {
                    var datas = new NameValueCollection();
                    datas["Username"] = name.getStrusername();
                    datas["Certificate"] = strCertificate;
                    datas["Purpose"] = etPurpose.Text;
                    datas["DateOfRequest"] = dateToday.ToString("yyyy-MM-dd");
                    datas["Status"] = "Pending";
                    datas["deliveryoption"] = strDeliveryOption;

                    var response = wb.UploadValues(uri, "POST", datas);
                    responseFromServer = Encoding.UTF8.GetString(response);
                }

                if (responseFromServer == "Operation Success")
                {

                    await this.DisplayAlert("Request Success.", "Wait for the Barangay Official to Approve your Request.", "OK");
                    await Navigation.PopAsync();

                }
                else
                {
                    await this.DisplayToastAsync(responseFromServer, 5000);
                }

            }
            catch (Exception ex)
            {
                await this.DisplayToastAsync("Something went wrong.", 5000);
                Console.WriteLine("ERROR:" + ex.Message);
            }
        }

        #region
        private async void InitializedCertificates()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = hosting.getCertificate();
                    var result = await client.GetStringAsync(uri);


                    JSONObject jsonresult = new JSONObject(result);
                    int success = jsonresult.GetInt("success");

                    if (success == 1)
                    {
                        JSONArray crt = jsonresult.GetJSONArray("certificate");

                        listCertificate = new List<string>();
                        listDocumentFee = new List<string>();
                        listDeliveryFee = new List<string>();

                        for (int i = 0; i < crt.Length(); i++)
                        {
                            JSONObject c = crt.GetJSONObject(i);
                            listCertificate.Add(c.GetString("Types"));
                            listDocumentFee.Add(c.GetString("DocFee"));
                            listDeliveryFee.Add(c.GetString("DeliveryFee"));
                        }

                        pickerCertificate.ItemsSource = listCertificate;

                    }
                }
            }
            catch (Exception ex)
            {
                await this.DisplayToastAsync("Something went wrong.", 5000);
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }


        private async void InitializedDeliveryOpt()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = hosting.getDeliveryoption();
                    var result = await client.GetStringAsync(uri);


                    JSONObject jsonresult = new JSONObject(result);
                    int success = jsonresult.GetInt("success");

                    if (success == 1)
                    {
                        JSONArray cvl = jsonresult.GetJSONArray("deliveryoptions");

                        listDeliveryOption = new List<string>();
                        for (int i = 0; i < cvl.Length(); i++)
                        {
                            JSONObject c = cvl.GetJSONObject(i);
                            listDeliveryOption.Add(c.GetString("Options"));
                        }

                        pickerDelivery.ItemsSource = listDeliveryOption;
                    }
                }
            }
            catch (Exception ex)
            {
                await this.DisplayToastAsync("Something went wrong.", 5000);
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }

        #endregion
    }
}