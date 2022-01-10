using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyFlyoutPage : FlyoutPage
    {

        private zsg_nameandimage name = new zsg_nameandimage();
        public MyFlyoutPage()
        {
            InitializeComponent();
            FlyoutPage.ListView.ItemSelected += ListView_ItemSelected;
        }


        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            

            var item = e.SelectedItem as MyFlyoutPageFlyoutMenuItem;
            if (item == null)
                return;

            if (item.Title == "Log Out")
            {
                bool answer = await DisplayAlert("Log Out?", "Are you sure you want to logout?", "Yes", "No");

                if (answer == true)
                {
                    CrossFirebasePushNotification.Current.UnsubscribeAll();
                    Preferences.Set("my_loginstatus", "false");
                    name.reset();
                    Approved( item.Title, item.TargetType);
                }
                else
                {
                    MyFlyoutPageFlyout s = new MyFlyoutPageFlyout();
                    s.ResetViewCell();

                    IsPresented = false;
                    FlyoutPage.ListView.SelectedItem = null;
                    return;
                }
            }
            else
            {
                if (name.getboolVerified() == true) {

                    Approved(item.Title, item.TargetType);
                }
                else if(item.Title != "Announcement")
                {
                    bool answer = await DisplayAlert("Not Verified", "Unable to access because you are still not Verified",  "GET VERIFIED", "DO IT LATER");
                    if (answer == true)
                    {
                        await Navigation.PushAsync(new Validation1());
                        ReseMyFlyout();
                    }
                    else
                    {
                        ReseMyFlyout();
                    }
                }
                else
                {
                    IsPresented = false;
                    FlyoutPage.ListView.SelectedItem = null;
                    return;
                }
            }
        }

        private void ReseMyFlyout()
        {
            MyFlyoutPageFlyout s = new MyFlyoutPageFlyout();
            s.ResetViewCell();

            IsPresented = false;
            FlyoutPage.ListView.SelectedItem = null;
            return;
        }

        private void Approved(string Title, System.Type TargetType)
        {
            var page = (Page)Activator.CreateInstance(TargetType);
            page.Title = Title;

            Detail = new NavigationPage(page);

            IsPresented = false;
            FlyoutPage.ListView.SelectedItem = null;
        }
    }
}