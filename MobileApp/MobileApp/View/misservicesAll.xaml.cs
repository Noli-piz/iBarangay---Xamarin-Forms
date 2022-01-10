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
    public partial class misservicesAll : ContentView
    {
        public misservicesAll()
        {
            InitializeComponent();

            IntializedService();

            ServiceListView.RefreshCommand = new Command(() => {
                IntializedService();
            });
        }

        private async void IntializedService()
        {
            ServiceListView.ItemsSource = null;
            List<listMisService> myServiceList = new List<listMisService>();
            try
            {
                using (var client = new HttpClient())
                {
                    zsg_nameandimage user = new zsg_nameandimage();
                    zsg_hosting hosting = new zsg_hosting();

                    var uri = hosting.getServiceall() + "?Username=" + user.getStrusername();
                    var result = await client.GetStringAsync(uri);


                    JSONObject jsonresult = new JSONObject(result);
                    int success = jsonresult.GetInt("success");

                    if (success == 1)
                    {
                        JSONArray req = jsonresult.GetJSONArray("service");


                        for (int i = 0; i < req.Length(); i++)
                        {
                            JSONObject rqst = req.GetJSONObject(i);

                            myServiceList.Add(new listMisService
                            {
                                id = i.ToString(),
                                ItemName = rqst.GetString("ItemName"),
                                Quantity = rqst.GetString("Quantity"),
                                DateOfRequest = rqst.GetString("DateOfRequest"),
                                Purpose = rqst.GetString("Purpose"),
                                Status = rqst.GetString("Status"),
                                Options = rqst.GetString("Options"),
                                Note = rqst.GetString("Note"),
                                RentDate = rqst.GetString("RentDate"),
                                EndRentDate = rqst.GetString("EndRentDate"),
                                Deadline = rqst.GetString("Deadline")
                            });

                        }

                        ServiceListView.ItemsSource = myServiceList;
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
                ServiceListView.IsRefreshing = false;
            }
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var itemService= ((ListView)sender).SelectedItem as listMisService;
            if (itemService == null)
                return;

            csFullDetailMisService cs = new csFullDetailMisService();
            cs.setData(
                itemService.id,
                itemService.ItemName,
                itemService.Quantity,
                itemService.DateOfRequest,
                itemService.Purpose,
                itemService.Status,
                itemService.Options,
                itemService.Note,
                itemService.RentDate,
                itemService.EndRentDate,
                itemService.Deadline
                );

            await Navigation.PushAsync(new FullDetailServices());
        }

        private void ListView_Itemtapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}