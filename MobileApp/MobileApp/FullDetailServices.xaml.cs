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
    public partial class FullDetailServices : ContentPage
    {
        public FullDetailServices()
        {
            InitializeComponent();

            
            csFullDetailMisService cs = new csFullDetailMisService();

            lblItemName.Text = cs.getItemName();
            lblQuantity.Text = cs.getQuantity();
            lblDateOfRequest.Text = cs.getDateOfRequest();
            lblPurpose.Text = cs.getPurpose();
            lblStatus.Text = cs.getStatus();
            lblOptions.Text = cs.getOptions();
            lblNote.Text = cs.getNote();
            lblRentDate.Text = cs.getRentDate() + " until " + cs.getEndRentDate();
            lblDeadline.Text = cs.getDeadline();

            if (cs.getNote() == null || cs.getNote() == "" || cs.getNote() == "null")
            {
                txtNote.IsVisible = false;
                lblNote.IsVisible = false;
            }

            if (cs.getDeadline() == "0000-00-00" || cs.getDeadline() == null || cs.getDeadline() == "" || cs.getDeadline() == "null")
            {
                txtDeadline.IsVisible = false;
                lblDeadline.IsVisible = false;
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