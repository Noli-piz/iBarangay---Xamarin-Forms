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
    public partial class RMiscellaneousServices : ContentPage
    {
        private List<String> listDeliveryOption;
        private List<String> listItem;
        private zsg_hosting hosting = new zsg_hosting();
        private int intQuantity = 0, intAvailable = 0, CountError = 0;
        private string strItem, strDeliveryOption;

        public RMiscellaneousServices()
        {
            InitializeComponent();

            IntializedItem();
            InitializedDeliveryOpt();

            dtEndRentDate.MinimumDate = DateTime.Now.AddDays(1);

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

        async void OnRentDatePicker(object sender, DateChangedEventArgs e)
        {
            vldRent.IsVisible = false;
        }

        void OnTextChange(object sender, TextChangedEventArgs e)
        {
            if (etQuantity.Text.Length > 0)
            {
                String Newvalue = etQuantity.Text.ToString();
                if (Convert.ToInt32(Newvalue) > intAvailable)
                {
                    this.DisplayToastAsync("Not Enough Quantity.", 3000);
                    etQuantity.Text = intAvailable.ToString();
                }
                else
                {
                    intQuantity = Convert.ToInt32(Newvalue);
                }
            }
        }

        void OnSelectIndexChange(object sender, EventArgs e)
        {
            vldItem.IsVisible = false;
            strItem = pickerItem.SelectedItem +"";
            RetrieveQuantity();
        }

        void DeliveryOption_SelectChange(object sender, EventArgs e)
        {
            vldDeliveryOption.IsVisible = false;
        }

        void OnMin(object sender, EventArgs e)
        {
            if (intQuantity > 0)
            {
                intQuantity--;
                etQuantity.Text = intQuantity.ToString();
            }

            vldQuantity.IsVisible = false;
        }

        void OnPlus(object sender, EventArgs e)
        {
            if (intQuantity < intAvailable)
            {
                intQuantity++;
                etQuantity.Text = intQuantity.ToString();
            }

            vldQuantity.IsVisible = false;
        }

       
        async void OnButtonClicked(object sender, EventArgs e)
        {
            CountError = 0;

            strItem = pickerItem.SelectedItem + "";
            strDeliveryOption = pickerDelivery.SelectedItem + "";

            if (strItem == "" || strItem == null)
            {
                vldItem.Text = "Select a valid Item";
                vldItem.IsVisible = true;
                CountError++;
            }
            if (etPurpose.Text == "" || etPurpose.Text == null)
            {
                vldPurpose.Text = "Enter a Purpose";
                vldPurpose.IsVisible = true;
                CountError++;
            }
            if (etQuantity.Text == "0" || etQuantity.Text =="" || etQuantity.Text == null)
            {
                vldQuantity.Text = "Not Valid Quantity";
                vldQuantity.IsVisible = true;
                CountError++;
            }
            if (strDeliveryOption == "" || strDeliveryOption == null)
            {
                vldDeliveryOption.Text = "Select valid Delivery Option";
                vldDeliveryOption.IsVisible = true;
                CountError++;
            }

            if (dtEndRentDate.Date < dtRentDate.Date)
            {
                vldRent.Text = "Please select the proper date range of rent.";
                vldRent.IsVisible = true;
                CountError++;
            }

            if(CountError <=0 )
            {
                bool answer = await DisplayAlert("", "You are about to borrow  x" + intQuantity.ToString() + "  " + strItem 
                    + " for the purpose of " + etPurpose.Text  + " on " + dtRentDate.Date.ToString("MMM dd, yyyy") +" until " + dtEndRentDate.Date.ToString("MMM dd, yyyy")
                    + ". \n\n Your request will be processed approximately 1-5 days."
                    + "\n\nTake note that any broken or missing item shall be paid by the borrowee on the approved"
                    + " date of return." , "Confirm", "Back");

                if (answer == true)
                {
                    InsertService();
                }
            }
            else
            {
                await DisplayAlert("", "Please check properly all the fields.", "OK");
            }
        }


        private async void InsertService()
        {
            try
            {
                zsg_nameandimage name = new zsg_nameandimage();
                zsg_hosting hosting = new zsg_hosting();

                DateTime dateToday = DateTime.Now;

                var uri = hosting.getInsertservice();

                string responseFromServer;
                using (var wb = new WebClient())
                {
                    var datas = new NameValueCollection();
                    datas["Username"] = name.getStrusername();
                    datas["Item"] = strItem;
                    datas["Quantity"] = intQuantity.ToString();
                    datas["Purpose"] = etPurpose.Text;
                    datas["DateOfRequest"] = dateToday.ToString("yyyy-MM-dd");
                    datas["Status"] = "Pending";
                    datas["deliveryoption"] = strDeliveryOption;
                    datas["RentDate"] = dtRentDate.Date.ToString("yyyy-MM-dd");
                    datas["EndRentDate"] = dtEndRentDate.Date.ToString("yyyy-MM-dd");

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

        private async void IntializedItem()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = hosting.getBarangayitems();
                    var result = await client.GetStringAsync(uri);


                    JSONObject jsonresult = new JSONObject(result);
                    int success = jsonresult.GetInt("success");

                    if (success == 1)
                    {
                        JSONArray cvl = jsonresult.GetJSONArray("items");

                        listItem = new List<string>();
                        for (int i = 0; i < cvl.Length(); i++)
                        {
                            JSONObject c = cvl.GetJSONObject(i);
                            listItem.Add(c.GetString("ItemName"));
                        }

                        pickerItem.ItemsSource = listItem;
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


        private async void RetrieveQuantity()
        {
            try
            {

                zsg_hosting hosting = new zsg_hosting();

                using (var client = new HttpClient())
                {
                    var uri = hosting.getItemquantity() + "?ItemName=" + strItem;
                    var result = await client.GetStringAsync(uri);


                    JSONObject jsonresult = new JSONObject(result);
                    int success = jsonresult.GetInt("success");

                    if (success == 1)
                    {
                        JSONArray info = jsonresult.GetJSONArray("quantity");
                        JSONObject strAvai = info.GetJSONObject(0);

                        lblDeliveryFee.Text = "Delivery Fee: " + strAvai.GetString("DeliveryFee");
                        lblPenaltyFee.Text = "Penalty Fee: " + strAvai.GetString("PenaltyFee");
                        

                        intAvailable = Int32.Parse(strAvai.GetString("Quantity"));
                        lblAvailable.Text = intAvailable.ToString();

                        if (intAvailable < intQuantity)
                        {
                            etQuantity.Text = intAvailable.ToString();
                            intQuantity = intAvailable;
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                await this.DisplayToastAsync("Something went wrong.", 3000);
                Console.WriteLine("ERROR: " + ex);
            }
        }
    }
}