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
    public partial class FullDetailRequest : ContentPage
    {
        public FullDetailRequest()
        {
            InitializeComponent();

            csFullDetailRequest cs = new csFullDetailRequest();

            lblTypes.Text = cs.getType();
            lblDateOfRequest.Text = cs.getDateOfRequest();
            lblPurpose.Text = cs.getPurpose();
            lblStatus.Text = cs.getStatus();
            lblOptions.Text = cs.getOptions();
            lblNote.Text = cs.getNote();

            if (cs.getNote() == null || cs.getNote() == "" || cs.getNote() == "null")
            {
                txtNote.IsVisible = false;
                lblNote.IsVisible = false;
            }

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