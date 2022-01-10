using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewInformation : ContentPage
    {
        private zsg_hosting hosting = new zsg_hosting();
        private TimeSpan ts = TimeSpan.FromSeconds(5000);

        string strImageUrl, strFname, strMname, strLname, strSname, strBirthplace, strBirthdate,
            strHouseNo, strCivilStatus, strGender, strPurok, strVoterStatus, strCedulaNo, strContactNo;

        string strPlaceRegistration, strFormerAddress, strFileUrl;

        public ReviewInformation(string ImageUrl, string Fname, string Mname, string Lname, string Sname, string Birthplace, DateTime Birthdate, 
            string HouseNo, string CivilStatus, string Gender, string Purok, string VoterStatus, string CedulaNo, string ContactNo, 
            string PlaceRegistration, string FormerAddress, string FileUrl)
        {
            InitializeComponent();



            ImgProfile.Source = ImageUrl;
            lblFName.Text = Fname;
            if (Mname != null && Mname != "") {
                lblMName.Text = Mname;
                lblMName.IsVisible = true;
                txtMName.IsVisible = true;
            }

            lblLName.Text = Lname;

            if (Sname != null && Sname != "")
            {
                lblSName.Text = Sname;
                lblSName.IsVisible = true;
                txtSName.IsVisible = true;
            }

            lblBirthplace.Text = Birthplace;
            lblBirthdate.Text = Birthdate.ToString("MMM dd, yyyy");
            lblHouseNoAndStreet.Text = HouseNo;
            lblCivilStatus.Text = CivilStatus;
            lblGender.Text = Gender;
            lblPurok.Text = Purok;
            lblVoterStatus.Text = VoterStatus == "Yes"? "Registered" : "Not- Registered";
            lblCedulaNo.Text = CedulaNo;
            lblContactNo.Text = ContactNo;


            if (PlaceRegistration != null && PlaceRegistration != "")
            {
                lblPlaceRegistration.Text = PlaceRegistration;
                lblPlaceRegistration.IsVisible = true;
                txtPlaceRegistration.IsVisible = true;
            }

            if (FormerAddress != null && FormerAddress != "")
            {
                lblFormerAddress.Text = FormerAddress;
                lblFormerAddress.IsVisible = true;
                txtFormerAddress.IsVisible = true;
            }

            strImageUrl = ImageUrl;
            strFname = Fname;
            strMname = Mname == "" ? "NONE" : Mname;
            strLname = Lname;
            strSname = Sname == "" ? "NONE" : Sname;
            strBirthplace = Birthplace;
            strBirthdate = Birthdate.ToString("yyyy-MM-dd");
            strHouseNo = HouseNo;
            strCivilStatus = CivilStatus;
            strGender = Gender;
            strPurok = Purok;
            strVoterStatus = VoterStatus;
            strCedulaNo = CedulaNo;
            strContactNo = ContactNo;

            strPlaceRegistration = PlaceRegistration;
            strFormerAddress = FormerAddress;
            strFileUrl = FileUrl;

            lblBack.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Navigation.PopAsync();
                })
            });
        }

        async void OnButtonClicked(Object sender, EventArgs e)
        {
            if (cbAccept.IsChecked)
            {
                InsertInformation();
            }
            else
            {
                await DisplayAlert("Review Information","Please check the checkbox if you are agree with the terms.","OK");
            }
        }

        private async void InsertInformation()
        {
            try
            {
                Info inf = new Info();
                var uri = hosting.getSignup3();

                try
                {
                    string responseFromServer;
                    using (var wb = new WebClient())
                    {
                        var datas = new NameValueCollection();

                        datas["Username"] = inf.getStrUsername();
                        datas["Password"] = inf.getStrPassword();
                        datas["Status"] = "False";
                        datas["Fname"] = strFname;
                        datas["Mname"] = strMname;
                        datas["Lname"] = strLname;
                        datas["Sname"] = strSname;
                        datas["Birthplace"] = strBirthplace;
                        datas["Birthdate"] = strBirthdate;
                        datas["CivilStatus"] = strCivilStatus;
                        datas["Gender"] = strGender;
                        datas["id_purok"] = strPurok;
                        datas["VoterStatus"] = strVoterStatus;
                        datas["DateOfRegistration"] = DateTime.Now.ToString("yyyy-MM-dd");
                        datas["ContactNo"] = strContactNo;
                        datas["CedulaNo"] = strCedulaNo;
                        datas["Email"] = inf.getStrEmail();
                        datas["Image"] = strImageUrl;
                        datas["HouseAndStreet"] = strHouseNo;

                        datas["PlaceRegistration"] = strPlaceRegistration;
                        datas["FormerAddress"] = strFormerAddress;
                        datas["File"] = strFileUrl;

                        var response = wb.UploadValues(uri, "POST", datas);
                        responseFromServer = Encoding.UTF8.GetString(response);
                    }


                    if (responseFromServer == "Sign up Failed")
                    {

                        await this.DisplaySnackBarAsync("Sign Up Failed", "OK", null, ts);

                    }
                    else if (responseFromServer == "Sign Up Success")
                    {

                        await DisplayAlert("Sign Up Success.", " You've successfully created an/new account.", "OK");
                        //await Navigation.PushAsync(new Login());
                        await Navigation.PopToRootAsync();
                    }
                    else
                    {
                        await this.DisplaySnackBarAsync(responseFromServer, "OK", null, ts);
                    }
                }
                catch (Exception ex)
                {
                    await this.DisplayToastAsync("Something went wrong.", 5000);
                    Console.WriteLine("1 ERROR: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                await this.DisplayToastAsync("Something went wrong.", 5000);
                Console.WriteLine("2 ERROR: " + ex.Message);
            }
        }
    }
}