using MobileApp.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FullDetailAnnouncement : ContentPage
    {
        public FullDetailAnnouncement()
        {
            InitializeComponent();

            csFullDetailAnnouncement cs = new csFullDetailAnnouncement();

            imgAlertLevel.Source = cs.getAlertLevel();
            lblSubject.Text = cs.getSubject();
            lblDate.Text = cs.getDate();
            lblDetails.Text = cs.getDetails();


            lblBack.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Navigation.PopAsync();
                })
            });
        }


    }
}