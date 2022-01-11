using Microsoft.WindowsAzure.Storage;
using Org.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Signup3 : ContentPage
    {
        private string strImageUrl;
        private List<String> listGender;
        private List<String> listPurok;
        private List<String> listVoterStatus;
        private List<String> listCivilStatus;
        private zsg_hosting hosting = new zsg_hosting();
        private TimeSpan ts = TimeSpan.FromSeconds(5000);
        private bool UploadImageSuccess = true;
        private string strCivil, strGender, strVoter, strPurok;

        public Signup3()
        {
            InitializeComponent();

            InitializedGender();
            InitializedCivil();
            InitializedVoter();
            InitializedPurok();


            etFname.TextChanged += Fname_Changed;
            etMname.TextChanged += Mname_Changed;
            etLname.TextChanged += Lname_Changed;
            etSname.TextChanged += Sname_Changed;
            etBirthplace.TextChanged += Birthplace_Changed;
            etHouseNoAndStreet.TextChanged += HouseNoAndStreet_Changed;
            etCedulaNo.TextChanged += CedulaNo_Changed;
            etContactNo.TextChanged += ContactNo_Changed;

            pickerCivil.SelectedIndexChanged += CivilStatus_Changed;
            pickerGender.SelectedIndexChanged += Gender_Changed;
            pickerPurok.SelectedIndexChanged += Purok_Changed;
            pickerVoter.SelectedIndexChanged += VoterStatus_Changed;


            etPlaceRegistration.TextChanged += PlaceRegistration_Changed;
            etFormerAddress.TextChanged += FormerAddress_Changed;

            //Focus/ Unfocus
            #region
            etFname.Focused += Fname_Focused;
            etFname.Unfocused += Fname_Unfocused;

            etMname.Focused += Mname_Focused;
            etMname.Unfocused += Mname_Unfocused;

            etLname.Focused += Lname_Focused;
            etLname.Unfocused += Lname_Unfocused;

            etSname.Focused += Sname_Focused;
            etSname.Unfocused += Sname_Unfocused;

            etBirthplace.Focused += Birthplace_Focused;
            etBirthplace.Unfocused += Birthplace_Unfocused;

            etHouseNoAndStreet.Focused += HouseNoAndStreet_Focused;
            etHouseNoAndStreet.Unfocused += HouseNoAndStreet_Unfocused;

            etCedulaNo.Focused += CedulaNo_Focused;
            etCedulaNo.Unfocused += CedulaNo_Unfocused;

            etContactNo.Focused += ContactNo_Focused;
            etContactNo.Unfocused += ContactNo_Unfocused;

            etPlaceRegistration.Focused += PlaceRegistration_Focused;
            etPlaceRegistration.Unfocused += PlaceRegistration_Unfocused;

            etFormerAddress.Focused += FormerAddress_Focused;
            etFormerAddress.Unfocused += FormerAddress_Unfocused;
            #endregion

            lblBack.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Navigation.PopAsync();
                })
            });
        }

        void rbAnswerChanged(object sender, EventArgs args)
        {
            RadioButton radButton = sender as RadioButton;
            var choose = radButton.Content.ToString();
            if (choose == "No")
            {
                stckForNonResident.IsVisible = true;

                if (strVoter == "Yes")
                {
                    fldPlaceRegistration.IsVisible = true;
                }
                else
                {
                    fldPlaceRegistration.IsVisible = false;
                }
            }
            else
            {
                stckForNonResident.IsVisible = false;
            }
        }


        //Events
        #region
        private void Fname_Changed(object sender, TextChangedEventArgs e)
        {
            var isValid = Regex.IsMatch(e.NewTextValue, "^[a-zA-Z]+$");
            if (e.NewTextValue.Length > 0)
            {
                ((Entry)sender).Text = isValid ? e.NewTextValue : e.NewTextValue.Remove(e.NewTextValue.Length - 1);
            }

            if (etFname.Text.Length > 0) 
            {
                vldFirstname.IsVisible = false;
            }
            else
            {
                vldFirstname.Text = "First Name is required";
                vldFirstname.IsVisible = true;
            }
        }

        private void Mname_Changed(object sender, TextChangedEventArgs e)
        {
            var isValid = Regex.IsMatch(e.NewTextValue, "^[a-zA-Z]+$");
            if (e.NewTextValue.Length > 0)
            {
                ((Entry)sender).Text = isValid ? e.NewTextValue : e.NewTextValue.Remove(e.NewTextValue.Length - 1);
            }
        }
        private void Lname_Changed(object sender, TextChangedEventArgs e)
        {
            var isValid = Regex.IsMatch(e.NewTextValue, "^[a-zA-Z]+$");
            if (e.NewTextValue.Length > 0)
            {
                ((Entry)sender).Text = isValid ? e.NewTextValue : e.NewTextValue.Remove(e.NewTextValue.Length - 1);
            }

            if (etLname.Text.Length > 0 ) 
            {
                vldLastname.IsVisible = false;
            }
            else
            {
                vldCedula.Text = "Cedula No. is required";
                vldCedula.IsVisible = true;
            }
        }

        private void Sname_Changed(object sender, TextChangedEventArgs e)
        {
            var isValid = Regex.IsMatch(e.NewTextValue, "^[a-zA-Z]+$");
            if (e.NewTextValue.Length > 0)
            {
                ((Entry)sender).Text = isValid ? e.NewTextValue : e.NewTextValue.Remove(e.NewTextValue.Length - 1);
            }
        }

        private void Birthplace_Changed(object sender, TextChangedEventArgs e)
        {
            if (etBirthplace.Text.Length > 0)
            {
                vldBirthplace.IsVisible = false;
            }
            else
            {
                vldBirthplace.Text = "Birthplace is required";
                vldBirthplace.IsVisible = true;
            }
        }

        private void HouseNoAndStreet_Changed(object sender, TextChangedEventArgs e)
        {
            vldHouseNoAndStreet.IsVisible = false;
        }

        private void CivilStatus_Changed(object sender, EventArgs e)
        {
            vldCivilStatus.IsVisible = false;
        }

        private void Gender_Changed(object sender, EventArgs e)
        {
            vldGender.IsVisible = false;
        }

        private void Purok_Changed(object sender, EventArgs e)
        {
            vldPurok.IsVisible = false;
        }

        private void VoterStatus_Changed(object sender, EventArgs e)
        {
            vldVoterStatus.IsVisible = false;
        }

        private void CedulaNo_Changed(object sender, TextChangedEventArgs e)
        {
            if (etCedulaNo.Text.Length > 0)
            {
                vldCedula.IsVisible = false;
            }
            else
            {
                vldCedula.Text = "Cedula No. is required";
                vldCedula.IsVisible = true;
            }
        }

        private void ContactNo_Changed(object sender, TextChangedEventArgs e)
        {
            if (etContactNo.Text.Length > 0)
            {
                vldContactNo.IsVisible = false;
            }
            else
            {
                vldContactNo.Text = "Contact No. is required";
                vldContactNo.IsVisible = true;
            }
        }

        private void PlaceRegistration_Changed(object sender, TextChangedEventArgs e)
        {
            if (etPlaceRegistration.Text.Length > 0)
            {
                vldPlaceRegistration.IsVisible = false;
            }
            else
            {
                vldPlaceRegistration.Text = "Place of voter's registration is required";
                vldPlaceRegistration.IsVisible = true;
            }
        }

        private void FormerAddress_Changed(object sender, TextChangedEventArgs e)
        {
            if (etFormerAddress.Text.Length > 0)
            {
                vldFormerAddress.IsVisible = false;
            }
            else
            {
                vldFormerAddress.Text = "Former Address is required";
                vldFormerAddress.IsVisible = true;
            }
        }
        #endregion

        //Focus/ Unfocus
        #region
        private void Fname_Focused(object sender, FocusEventArgs e)
        {
            lblFname.ScaleYTo(0.8);
            lblFname.ScaleXTo(0.8);
            lblFname.TranslateTo(0, -(lblFname.Height));

        }
        private void Fname_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(etFname.Text))
            {
                lblFname.ScaleXTo(1);
                lblFname.ScaleYTo(1);
                lblFname.TranslateTo(0, 0);
            }
        }
        private void Mname_Focused(object sender, FocusEventArgs e)
        {
            lblMname.ScaleYTo(0.8);
            lblMname.ScaleXTo(0.8);
            lblMname.TranslateTo(0, -(lblMname.Height));
        }
        private void Mname_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(etMname.Text))
            {
                lblMname.ScaleXTo(1);
                lblMname.ScaleYTo(1);
                lblMname.TranslateTo(0, 0);
            }
        }

        private void Lname_Focused(object sender, FocusEventArgs e)
        {
            lblLname.ScaleYTo(0.8);
            lblLname.ScaleXTo(0.8);
            lblLname.TranslateTo(0, -(lblLname.Height));
        }
        private void Lname_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(etLname.Text))
            {
                lblLname.ScaleXTo(1);
                lblLname.ScaleYTo(1);
                lblLname.TranslateTo(0, 0);
            }
        }

        private void Sname_Focused(object sender, FocusEventArgs e)
        {
            lblSname.ScaleYTo(0.8);
            lblSname.ScaleXTo(0.8);
            lblSname.TranslateTo(0, -(lblSname.Height));
        }
        private void Sname_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(etSname.Text))
            {
                lblSname.ScaleXTo(1);
                lblSname.ScaleYTo(1);
                lblSname.TranslateTo(0, 0);
            }
        }

        private void Birthplace_Focused(object sender, FocusEventArgs e)
        {
            lblBirthplace.ScaleYTo(0.8);
            lblBirthplace.ScaleXTo(0.8);
            lblBirthplace.TranslateTo(0, -(lblBirthplace.Height));
        }
        private void Birthplace_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(etBirthplace.Text))
            {
                lblBirthplace.ScaleXTo(1);
                lblBirthplace.ScaleYTo(1);
                lblBirthplace.TranslateTo(0, 0);
            }
        }

        private void HouseNoAndStreet_Focused(object sender, FocusEventArgs e)
        {
            lblHouseNoAndStreet.ScaleYTo(0.8);
            lblHouseNoAndStreet.ScaleXTo(0.8);
            lblHouseNoAndStreet.TranslateTo(0, -(lblHouseNoAndStreet.Height));
        }
        private void HouseNoAndStreet_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(etHouseNoAndStreet.Text))
            {
                lblHouseNoAndStreet.ScaleXTo(1);
                lblHouseNoAndStreet.ScaleYTo(1);
                lblHouseNoAndStreet.TranslateTo(0, 0);
            }
        }

        private void CedulaNo_Focused(object sender, FocusEventArgs e)
        {
            lblCedulaNo.ScaleYTo(0.8);
            lblCedulaNo.ScaleXTo(0.8);
            lblCedulaNo.TranslateTo(0, -(lblCedulaNo.Height));
        }
        private void CedulaNo_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(etCedulaNo.Text))
            {
                lblCedulaNo.ScaleXTo(1);
                lblCedulaNo.ScaleYTo(1);
                lblCedulaNo.TranslateTo(0, 0);
            }
        }

        private void ContactNo_Focused(object sender, FocusEventArgs e)
        {
            lblContactNo.ScaleYTo(0.8);
            lblContactNo.ScaleXTo(0.8);
            lblContactNo.TranslateTo(0, -(lblContactNo.Height));
        }
        private void ContactNo_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(etContactNo.Text))
            {
                lblContactNo.ScaleXTo(1);
                lblContactNo.ScaleYTo(1);
                lblContactNo.TranslateTo(0, 0);
            }
        }

        private void PlaceRegistration_Focused(object sender, FocusEventArgs e)
        {
            lblPlaceRegistration.ScaleYTo(0.8);
            lblPlaceRegistration.ScaleXTo(0.8);
            lblPlaceRegistration.TranslateTo(0, -(lblPlaceRegistration.Height));
        }
        private void PlaceRegistration_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(etPlaceRegistration.Text))
            {
                lblPlaceRegistration.ScaleXTo(1);
                lblPlaceRegistration.ScaleYTo(1);
                lblPlaceRegistration.TranslateTo(0, 0);
            }
        }

        private void FormerAddress_Focused(object sender, FocusEventArgs e)
        {
            lblFormerAddress.ScaleYTo(0.8);
            lblFormerAddress.ScaleXTo(0.8);
            lblFormerAddress.TranslateTo(0, -(lblFormerAddress.Height));
        }
        private void FormerAddress_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(etFormerAddress.Text))
            {
                lblFormerAddress.ScaleXTo(1);
                lblFormerAddress.ScaleYTo(1);
                lblFormerAddress.TranslateTo(0, 0);
            }
        }
        #endregion


        //Intialized Picker
        #region

        private async void InitializedGender()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = hosting.getGender();
                    var result = await client.GetStringAsync(uri);


                    JSONObject jsonresult = new JSONObject(result);
                    int success = jsonresult.GetInt("success");

                    if (success == 1)
                    {
                        JSONArray gdr = jsonresult.GetJSONArray("gender");

                        listGender = new List<string>();
                        for (int i = 0; i < gdr.Length(); i++)
                        {
                            JSONObject c = gdr.GetJSONObject(i);
                            listGender.Add(c.GetString("Identities"));
                        }

                        pickerGender.ItemsSource = listGender;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);

                listGender = new List<string>();
                listGender.Add("Male");
                listGender.Add("Female");
                pickerGender.ItemsSource = listGender;
            }
        }

        private async void InitializedPurok()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = hosting.getPurok();
                    var result = await client.GetStringAsync(uri);


                    JSONObject jsonresult = new JSONObject(result);
                    int success = jsonresult.GetInt("success");

                    if (success == 1)
                    {
                        JSONArray prk = jsonresult.GetJSONArray("purok");

                        listPurok = new List<string>();
                        for (int i = 0; i < prk.Length(); i++)
                        {
                            JSONObject c = prk.GetJSONObject(i);
                            listPurok.Add(c.GetString("Name"));
                        }

                        pickerPurok.ItemsSource = listPurok;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                InitializedPurok();
            }

        }

        private async void InitializedCivil()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = hosting.getCivilstatus();
                    var result = await client.GetStringAsync(uri);


                    JSONObject jsonresult = new JSONObject(result);
                    int success = jsonresult.GetInt("success");

                    if (success == 1)
                    {
                        JSONArray cvl = jsonresult.GetJSONArray("civilstatus");

                        listCivilStatus = new List<string>();
                        for (int i = 0; i < cvl.Length(); i++)
                        {
                            JSONObject c = cvl.GetJSONObject(i);
                            listCivilStatus.Add(c.GetString("Types"));
                        }

                        pickerCivil.ItemsSource = listCivilStatus;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                InitializedCivil();
            }
        }

        private void InitializedVoter()
        {
            listVoterStatus = new List<string>();
            listVoterStatus.Add("Yes");
            listVoterStatus.Add("No");
            pickerVoter.ItemsSource = listVoterStatus;
        }

        #endregion Intialized Picker

        async void OnVoterStatusChanged(object sender, EventArgs args)
        {

            strVoter = pickerVoter.SelectedItem + "";
            if (strVoter == "Yes")
            {
                fldPlaceRegistration.IsVisible = true;
            }
            else
            {
                fldPlaceRegistration.IsVisible = false;
            }
        }

        int ErrorCount = 0;
        async void OnButtonClicked(object sender, EventArgs args)
        {
            ErrorCount = 0;
            strCivil = pickerCivil.SelectedItem +"";
            strGender = pickerGender.SelectedItem +"";
            strPurok = pickerPurok.SelectedItem +"";
            strVoter = pickerVoter.SelectedItem +"";

            if (strImageUrl == "" || strImageUrl == null)
            {
                vldImage.Text = "Image is required";
                vldImage.IsVisible = true;
                ErrorCount++;
            }
            if (etFname.Text == "" || etFname.Text == null)
            {
                vldFirstname.Text = "First Name is required";
                vldFirstname.IsVisible = true;
                ErrorCount++;
            }
            if (etLname.Text == "" || etLname.Text == null)
            {
                vldLastname.Text = "Last Name is required";
                vldLastname.IsVisible = true;
                ErrorCount++;
            }
            if (etBirthplace.Text == "" || etBirthplace.Text == null)
            {
                vldBirthplace.Text = "Birthplace is required";
                vldBirthplace.IsVisible = true;
                ErrorCount++;
            }

            OnDatePicker(null, null);

            if (etHouseNoAndStreet.Text == "" || etHouseNoAndStreet.Text == null)
            {
                vldHouseNoAndStreet.Text = "House No. And Street is required";
                vldHouseNoAndStreet.IsVisible = true;
                ErrorCount++;
            }
            if (etContactNo.Text == "" || etContactNo.Text == null)
            {
                vldContactNo.Text = "Contact No. is required";
                vldContactNo.IsVisible = true;
                ErrorCount++;
            }
            //if (etCedulaNo.Text == "" || etCedulaNo.Text == null)
            //{
            //    vldCedula.Text = "Cedula No. is required";
            //    vldCedula.IsVisible = true;
            //    ErrorCount++;
            //}
            if (strCivil == "" || strCivil == null)
            {
                vldCivilStatus.Text = "Civil Status is required";
                vldCivilStatus.IsVisible = true;
                ErrorCount++;
            }
            if (strGender == "" || strGender == null)
            {
                vldGender.Text = "Gender is required";
                vldGender.IsVisible = true;
                ErrorCount++;
            }
            if (strPurok == "" || strPurok == null)
            {
                vldPurok.Text = "Purok is required";
                vldPurok.IsVisible = true;
                ErrorCount++;
            }
            if (strVoter == "" || strVoter == null)
            {
                vldVoterStatus.Text = "Voter Status is required";
                vldVoterStatus.IsVisible = true;
                ErrorCount++;
            }

            if (stckForNonResident.IsVisible == true) {
                if (strVoter == "Yes")
                {
                    if (etPlaceRegistration.Text == "" || etPlaceRegistration.Text == null)
                    {
                        vldPlaceRegistration.Text = "Place of voter's registration is required";
                        vldPlaceRegistration.IsVisible = true;
                        ErrorCount++;
                    }
                }

                if (etFormerAddress.Text == "" || etFormerAddress.Text == null)
                {
                    vldFormerAddress.Text = "Former Address is required";
                    vldFormerAddress.IsVisible = true;
                    ErrorCount++;
                }

                if (strFileUrl == "" || strFileUrl == null)
                {
                    vldUploadFile.Text = "Please upload a document proving your current residence.";
                    vldUploadFile.IsVisible = true;
                    ErrorCount++;
                }
            }


            if(ErrorCount <= 0)
            {
                string strPlaceRegistration="", FileUrl="", strFormerAddress="";

                if (stckForNonResident.IsVisible == true) {
                    strPlaceRegistration = etPlaceRegistration.Text;
                    strFormerAddress = etFormerAddress.Text;
                    FileUrl = strFileUrl;
                }

                etCedulaNo.Text = "NONE";

                //InsertInformation();
                await Navigation.PushAsync(new ReviewInformation(strImageUrl, etFname.Text, etMname.Text, etLname.Text, etSname.Text, etBirthplace.Text,
                    dtBirthday.Date, etHouseNoAndStreet.Text, strCivil, strGender, strPurok, strVoter, etCedulaNo.Text, etContactNo.Text,
                    strPlaceRegistration, strFormerAddress, FileUrl)) ;

            }
            else
            {
                await DisplayAlert("","Please fill-up all required fields.","OK");
            }

            //await Navigation.PushAsync(new Login());
        }

        async void OnButtonSelectImage(object sender, EventArgs args)
        {
            await CrossMedia.Current.Initialize();
            try
            {
                await DisplayAlert("Notice.", "Please select your front-facing professional portrait.", "OK");
                var mediaOptions = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Full
                };

                var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

                if (selectedImageFile == null)
                {
                    await DisplayAlert("Error", "Could not get the image", "OK");
                    return;
                }
                UploadImage(selectedImageFile.GetStream());
                if (UploadImageSuccess == true)
                {
                    ImgProfile.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());
                }
            }
            catch (MediaPermissionException ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }

        async void OnDatePicker(object sender, DateChangedEventArgs e)
        {
            int Age = CalculateAge(dtBirthday.Date);
            lblAge.Text = Age < 0 ? "0" : Age.ToString();

            if (Age < 18)
            {
                vldBirthdate.Text = "You must 18 or above to register.";
                vldBirthdate.IsVisible = true;
                ErrorCount++;
            }
            else
            {
                vldBirthdate.IsVisible = false;
            }
        }

        private static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
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
                        datas["Fname"] = etFname.Text;
                        datas["Mname"] = etMname.Text == "" ? "NONE" : etMname.Text;
                        datas["Lname"] = etLname.Text;
                        datas["Sname"] = etSname.Text == "" ? "NONE" : etSname.Text;
                        datas["Birthplace"] = etBirthplace.Text;
                        datas["Birthdate"] = dtBirthday.Date.ToString("yyyy-MM-dd");
                        datas["CivilStatus"] = strCivil;
                        datas["Gender"] = strGender;
                        datas["id_purok"] = strPurok;
                        datas["VoterStatus"] = strVoter;
                        datas["DateOfRegistration"] = DateTime.Now.ToString("yyyy-MM-dd");
                        datas["ContactNo"] = etContactNo.Text;
                        datas["CedulaNo"] = etCedulaNo.Text == "" ? "NONE" : etCedulaNo.Text;
                        datas["Email"] = inf.getStrEmail();
                        datas["Image"] = strImageUrl;
                        datas["HouseAndStreet"] = etHouseNoAndStreet.Text;

                        var response = wb.UploadValues(uri, "POST", datas);
                        responseFromServer = Encoding.UTF8.GetString(response);
                    }


                    if (responseFromServer == "Sign up Failed")
                    {

                        await this.DisplaySnackBarAsync("Sign Up Failed","OK", null, ts);
                    
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


        //Upload Image to Azure Storage
        private async void UploadImage(Stream stream)
        {
            try
            {
                btnSignup.IsEnabled = false;
                var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=ibarangaystorage;AccountKey=SuJ5YP5ovCzjeBc9sLKwbbhrk8GIWjrSyO493EnTRLc7tpNxApS/sdsIvk+qXWOhohgVASKI6VjFgrCYGYiuEw==;EndpointSuffix=core.windows.net");
                var client = account.CreateCloudBlobClient();
                var container = client.GetContainerReference("profileimages");
                await container.CreateIfNotExistsAsync();
                var name = Guid.NewGuid().ToString();
                var blockBlob = container.GetBlockBlobReference($"{name}.png");
                await blockBlob.UploadFromStreamAsync(stream);
                string URL = blockBlob.Uri.OriginalString;
                strImageUrl = URL;
                vldImage.IsVisible = false;
                lblSelectImage.IsVisible = false;
                await this.DisplayToastAsync("Image Successfully Upload.", 3000);
                UploadImageSuccess = true;
                Console.WriteLine("IMAGE URL: " + strImageUrl);
            }
            catch (Exception ex)
            {
                await this.DisplayToastAsync("Somthing went wrong.", 5000);
                Console.WriteLine("ERROR: " + ex.Message);
                UploadImageSuccess = false;
            }
            finally
            {
                btnSignup.IsEnabled = true;
            }
        }

        //File
        async void OnClickFileUpload(object sender, EventArgs args)
        {
            try
            {
                var result = await FilePicker.PickAsync();
                if (result != null)
                {
                    btnUpload.Text = $"File Name: {result.FileName}";
                    //if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                    //    result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    var stream = await result.OpenReadAsync();
                    //    //Image = ImageSource.FromStream(() => stream);
                    //    UploadFile(stream);
                    //}
                    var stream = await result.OpenReadAsync();
                    UploadFile(stream, result.FileName.ToString());
                }

            }
            catch
            {

            }
        }

        private string strFileUrl = "";

        //Upload File to Azure Storage
        private async void UploadFile(Stream stream, string Filename)
        {
            try
            {
                btnSignup.IsEnabled = false;
                btnUpload.IsEnabled = false;
                var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=ibarangaystorage;AccountKey=SuJ5YP5ovCzjeBc9sLKwbbhrk8GIWjrSyO493EnTRLc7tpNxApS/sdsIvk+qXWOhohgVASKI6VjFgrCYGYiuEw==;EndpointSuffix=core.windows.net");
                var client = account.CreateCloudBlobClient();
                var container = client.GetContainerReference("documentsfile");
                await container.CreateIfNotExistsAsync();
                var name = Guid.NewGuid().ToString();
                var blockBlob = container.GetBlockBlobReference($"{name + Filename}");
                await blockBlob.UploadFromStreamAsync(stream);
                string URL = blockBlob.Uri.OriginalString;
                strFileUrl = URL;
                btnUpload.IsEnabled = true;
                vldUploadFile.IsVisible = false;
                await this.DisplayToastAsync("File Successfully Upload.", 3000);
                Console.WriteLine("FILE URL: " + strFileUrl);
            }
            catch (Exception ex)
            {
                await this.DisplayToastAsync("Somthing went wrong.", 5000);
                Console.WriteLine("ERROR: " + ex.Message);
            }
            finally
            {
                btnSignup.IsEnabled = true;
                if (strFileUrl == "" || strFileUrl == null)
                {
                    btnUpload.Text = "UPLOAD FILE";
                    vldUploadFile.Text = "Please upload a document proving your current residence.";
                    vldUploadFile.IsVisible = true;
                    ErrorCount++;
                }
            }
        }
    }
}