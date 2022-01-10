using MobileApp.DataModels;
using Org.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class requestDisapproved : ContentView
    {
        public requestDisapproved()
        {
            InitializeComponent();

            IntializedRequest();

            RequestListView.RefreshCommand = new Command(() => {
                IntializedRequest();
            });
        }


        private async void IntializedRequest()
        {
            RequestListView.ItemsSource = null;
            List<listRequest> myRequestList = new List<listRequest>();
            try
            {
                using (var client = new HttpClient())
                {
                    zsg_nameandimage user = new zsg_nameandimage();
                    zsg_hosting hosting = new zsg_hosting();

                    var uri = hosting.getRequestdisapproved() + "?Username=" + user.getStrusername();
                    var result = await client.GetStringAsync(uri);


                    JSONObject jsonresult = new JSONObject(result);
                    int success = jsonresult.GetInt("success");

                    if (success == 1)
                    {
                        JSONArray req = jsonresult.GetJSONArray("request");


                        for (int i = 0; i < req.Length(); i++)
                        {
                            JSONObject rqst = req.GetJSONObject(i);

                            myRequestList.Add(new listRequest
                            {
                                id = i.ToString(),
                                Types = rqst.GetString("Types"),
                                DateOfRequest = rqst.GetString("DateOfRequest"),
                                Purpose = rqst.GetString("Purpose"),
                                Status = rqst.GetString("Status"),
                                Options = rqst.GetString("Options"),
                                Note = rqst.GetString("Note")

                            });
                        }

                        RequestListView.ItemsSource = myRequestList;

                    }
                    else if ("No Data" == jsonresult.GetString("message"))
                    {
                        //Snackbar.Make(lout, "No Data.", Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                    }
                    //else
                    //{
                    //    Snackbar.Make(lout, "Failed to Load", Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                    //}

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
            finally
            {
                RequestListView.IsRefreshing = false;
            }
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var itemRequest = ((ListView)sender).SelectedItem as listRequest;
            if (itemRequest == null)
                return;

            csFullDetailRequest cs = new csFullDetailRequest();
            cs.setData(
                itemRequest.id,
                itemRequest.Types,
                itemRequest.DateOfRequest,
                itemRequest.Purpose,
                itemRequest.Status,
                itemRequest.Options,
                itemRequest.Note
                );

            await Navigation.PushAsync(new FullDetailRequest());
        }

        private void ListView_Itemtapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}