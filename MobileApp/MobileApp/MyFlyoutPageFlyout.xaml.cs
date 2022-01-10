using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyFlyoutPageFlyout : ContentPage
    {
        public ListView ListView;
        static ViewCell lastCell;
        private zsg_nameandimage name = new zsg_nameandimage();


        public MyFlyoutPageFlyout()
        {
            InitializeComponent();

            BindingContext = new MyFlyoutPageFlyoutViewModel();
            ListView = MenuItemsListView;

            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;

            setNameAndImage();

            MenuItemsListView.RefreshCommand = new Command(() => {
                MenuItemsListView.IsRefreshing = true;
                RefreshCommand();
            });
        }

        class MyFlyoutPageFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MyFlyoutPageFlyoutMenuItem> MenuItems { get; set; }

            public MyFlyoutPageFlyoutViewModel()
            {

                MenuItems = new ObservableCollection<MyFlyoutPageFlyoutMenuItem>(new[]
                {
                    new MyFlyoutPageFlyoutMenuItem { Id = 0, Title = "Announcement" , IconSource="ic_announcement.xml" ,TargetType=typeof(MainAnnouncement)},
                    new MyFlyoutPageFlyoutMenuItem { Id = 1, Title = "Request" , IconSource="ic_request.xml" ,TargetType= typeof(MainRequest)},
                    new MyFlyoutPageFlyoutMenuItem { Id = 2, Title = "Miscellaneous Services" , IconSource="ic_service.xml" ,TargetType = typeof(MainMiscellaneousServices)},
                    new MyFlyoutPageFlyoutMenuItem { Id = 3, Title = "Log Out" , IconSource="ic_logout.xml" , TargetType = typeof(Login)},
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {

                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }

        private void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;

            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.LightGray;
                lastCell = viewCell;
            }

        }

        public void ResetViewCell()
        {
            lastCell.View.BackgroundColor = Color.Transparent;
        }

        private async void OnButtonUpdateInfo(object sender, EventArgs e )
        {
            //await Navigation.PushAsync(new UpdateInformation());
        }

        private async void RefreshCommand()
        {
            name.retrievedVerificationStatus();
            await Task.Delay(5000);
            if (name.getboolVerified() == true)
            {
                lblVerificationStatus.Text = "Validated";
            }
            else
            {
                lblVerificationStatus.Text = "Not Validated";
            }

            if (ImgProfile == null)
            {
                ImgProfile.Source = name.getStrImg();
            }

            if (lblName.Text == "Loading...")
            {
                lblName.Text = name.getFirstName() + " " + name.getMiddleName() + " " + name.getLastName() + " " + name.getSuffixName();
            }

            MenuItemsListView.IsRefreshing = false;
        }

        private void setNameAndImage()
        {

            if (name.getFirstName() != "" || name.getFirstName() != null)
            {
                lblName.Text = name.getFirstName() +" "+ name.getMiddleName() +" "+ name.getLastName() +" "+ name.getSuffixName();
                if (name.getboolVerified() == true)
                {
                    lblVerificationStatus.Text = "Validated";
                }
                else
                {
                    lblVerificationStatus.Text = "Not Validated";
                }
            }

            if (name.getStrImg() != null)
            {
                ImgProfile.Source = name.getStrImg();
            }
        }
    }
}