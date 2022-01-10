using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileApp.DataModels;
using Org.Json;
using System.Net.Http;
using Xamarin.CommunityToolkit.Extensions;
using Plugin.FirebasePushNotification;
using Plugin.LocalNotification;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainAnnouncement : ContentPage
    {
        public MainAnnouncement()
        {
            InitializeComponent();

            IntializedAnnouncementAsync();

            AnnouncementListView.RefreshCommand = new Command(() => {
                IntializedAnnouncementAsync();
            });


            zsg_nameandimage user = new zsg_nameandimage();
            CrossFirebasePushNotification.Current.Subscribe(user.getStrusername());
            CrossFirebasePushNotification.Current.Subscribe("ibarangay");

            CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;
            CrossFirebasePushNotification.Current.OnNotificationReceived += Current_OnNotificationReceived;

            NotificationCenter.Current.NotificationTapped += Current_NotificationReceived;
            NotificationCenter.Current.NotificationReceived += Current_NotificationReceived;

        }

        //Firebase Notification
        #region
        private void Current_OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {
            Console.WriteLine($"TOKEN: {e.Token}");
        }

        private void Current_OnNotificationReceived(object source, FirebasePushNotificationDataEventArgs e)
        {
            Console.WriteLine("RECEIVED");

            string strTitle="", strBody="";

            foreach (var data in e.Data)
            {
                //Console.WriteLine($"NOTIFICATION: {data.Key} : {data.Value}");

                string checkDataKey = $"{data.Key}";
                string checkDatavalue = $"{data.Value}";
                if ( checkDataKey ==  "body" )
                {
                    strBody = checkDatavalue;
                    continue;
                }
                else if ( checkDataKey == "title" )
                {
                    strTitle = checkDatavalue;
                    continue;
                }
            }


            //Local
            var notification = new NotificationRequest
            {
                Title = strTitle,
                Description = strBody
            };
            NotificationCenter.Current.Show(notification);
        }
        #endregion

        //Local Notification
        private void Current_NotificationReceived(NotificationEventArgs e)
        {
            //Working on android
            DisplayAlert("Notification", e.Request.Title  +"\n"+ e.Request.Description, "OK");

        }


        // Load Announcement
        private async void IntializedAnnouncementAsync()
        {
            List<listAnnouncement> myAnnouncementList = new List<listAnnouncement>();
            try
            {
                using (var client = new HttpClient())
                {
                    zsg_hosting hosting = new zsg_hosting();
                    var uri = hosting.getAnnouncement();
                    var result = await client.GetStringAsync(uri);


                    JSONObject jsonresult = new JSONObject(result);
                    int success = jsonresult.GetInt("success");

                    if (success == 1)
                    {
                        JSONArray ann = jsonresult.GetJSONArray("announcement");

                        for (int i = 0; i < ann.Length(); i++)
                        {
                            JSONObject ancmnt = ann.GetJSONObject(i);

                            myAnnouncementList.Add(new listAnnouncement
                            {
                                id = ancmnt.GetInt("id_announcement").ToString(),
                                AlertLevel = ancmnt.GetString("ImageLocation"),
                                Subject = ancmnt.GetString("Subject"),
                                Date = ancmnt.GetString("Date"),
                                Details = ancmnt.GetString("Details")
                            });

                            //LLevel.Add(ancmnt.GetString("Level"));
                        }

                        AnnouncementListView.ItemsSource = myAnnouncementList;
                    }
                    else
                    {
                        await this.DisplayToastAsync("Failed to load.", 5000);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                await this.DisplayToastAsync("Unable to reach server. Please check your internet.", 5000);
                Console.WriteLine("ERROR: " + ex.Message);
            }
            catch (Exception ex)
            {
                await this.DisplayToastAsync("Something went wrong. Please check your internet connection.", 5000);
                Console.WriteLine("ERROR: " + ex.Message);
            }
            finally
            {
                AnnouncementListView.IsRefreshing = false;
            }
        }


        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var itemAnnouncement =  ((ListView)sender).SelectedItem as listAnnouncement;
            if (itemAnnouncement == null)
                return;

            csFullDetailAnnouncement cs = new csFullDetailAnnouncement();
            cs.setData(
                itemAnnouncement.id,
                itemAnnouncement.AlertLevel,
                itemAnnouncement.Subject,
                itemAnnouncement.Date,
                itemAnnouncement.Details
                );

            await Navigation.PushAsync(new FullDetailAnnouncement());
        }
        private void ListView_Itemtapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}