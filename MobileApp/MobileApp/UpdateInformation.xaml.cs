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
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateInformation : ContentPage
    {
        private string strImageUrl, strRegistrationDate;
        private List<String> listGender;
        private List<String> listPurok;
        private List<String> listVoterStatus;
        private List<String> listCivilStatus;
        private zsg_hosting hosting = new zsg_hosting();
        private TimeSpan ts = TimeSpan.FromSeconds(5000);
        private bool UploadImageSuccess = true;
        private string strCivil, strGender, strVoter, strPurok;
        private zsg_nameandimage name = new zsg_nameandimage();

        public UpdateInformation()
        {
            InitializeComponent();

            IntializedInformation();
            InitializedGender();
            InitializedCivil();
            InitializedVoter();
            InitializedPurok();


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
            #endregion

            lblBack.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Navigation.PopAsync();
                })
            });

            if (name.getboolVerified() == true)
            {
                etFname.IsEnabled = false;
                etLname.IsEnabled = false;
                etCedulaNo.IsEnabled = false;
            }
        }


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
                        pickerGender.SelectedItem = name.getGender();
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
                        pickerPurok.SelectedItem = name.getPurok();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
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
                        pickerCivil.SelectedItem = name.getCiviStatus();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }

        private void InitializedVoter()
        {
            listVoterStatus = new List<string>();
            listVoterStatus.Add("Yes");
            listVoterStatus.Add("No");
            pickerVoter.ItemsSource = listVoterStatus;
            pickerVoter.SelectedItem = name.getVoterStatus();
        }

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
        #endregion

        private void IntializedInformation()
        {
            try
            {
                etFname.Text = name.getFirstName();
                etMname.Text = name.getMiddleName();
                etLname.Text = name.getLastName();
                etSname.Text = name.getSuffixName();

                etBirthplace.Text = name.getBirthPlace();
                etCedulaNo.Text = name.getCedulaNo();
                etContactNo.Text = name.getContactNo();
                etHouseNoAndStreet.Text = name.getHouseNoAndStreet();

                strImageUrl = name.getStrImg();
                ImgProfile.Source = name.getStrImg();

                String strBYear = name.getBYear().ToString();
                String strBMonth = name.getBMonth().ToString();
                String strBDay =  name.getBDay().ToString();
                if (name.getBMonth() < 10)
                {
                    strBMonth = "0" + strBMonth;
                }

                if (name.getBDay() < 10)
                {
                    strBDay = "0" + strBDay;
                }
                String strBirthDate = strBYear + "-" + strBMonth + "-" + strBDay;
                DateTime oDate = DateTime.ParseExact(strBirthDate, "yyyy-MM-dd", null);
                dtBirthday.Date = oDate;

                strRegistrationDate = name.getRYear() + "-" + name.getRMonth() + "-" + name.getRDay();


                if (etFname.Text != "" && etFname.Text != null)
                {

                    lblFname.ScaleYTo(0.8);
                    lblFname.ScaleXTo(0.8);
                    lblFname.TranslateTo(0, -24);
                }
                if (etMname.Text != "" && etMname.Text != null)
                {

                    lblMname.ScaleYTo(0.8);
                    lblMname.ScaleXTo(0.8);
                    lblMname.TranslateTo(0, -24);
                }
                if (etLname.Text != "" && etLname.Text != null)
                {

                    lblLname.ScaleYTo(0.8);
                    lblLname.ScaleXTo(0.8);
                    lblLname.TranslateTo(0, -24);
                }
                if (etSname.Text != "" && etSname.Text != null)
                {

                    lblSname.ScaleYTo(0.8);
                    lblSname.ScaleXTo(0.8);
                    lblSname.TranslateTo(0, -24);
                }
                if (etBirthplace.Text != "" && etBirthplace.Text != null)
                {

                    lblBirthplace.ScaleYTo(0.8);
                    lblBirthplace.ScaleXTo(0.8);
                    lblBirthplace.TranslateTo(0, -70);
                }
                if (etHouseNoAndStreet.Text != "" && etHouseNoAndStreet.Text != null)
                {
                    lblHouseNoAndStreet.ScaleYTo(0.8);
                    lblHouseNoAndStreet.ScaleXTo(0.8);
                    lblHouseNoAndStreet.TranslateTo(0, -70);
                }
                if (etCedulaNo.Text != "" && etCedulaNo.Text != null)
                {

                    lblCedulaNo.ScaleYTo(0.8);
                    lblCedulaNo.ScaleXTo(0.8);
                    lblCedulaNo.TranslateTo(0, -24);
                }
                if (etContactNo.Text != "" && etContactNo.Text != null)
                {
                    lblContactNo.ScaleYTo(0.8);
                    lblContactNo.ScaleXTo(0.8);
                    lblContactNo.TranslateTo(0, -24);
                }
                
            }
            catch(Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }

        async void OnButtonClicked(object sender, EventArgs args)
        {
            strCivil = pickerCivil.SelectedItem + "";
            strGender = pickerGender.SelectedItem + "";
            strPurok = pickerPurok.SelectedItem + "";
            strVoter = pickerVoter.SelectedItem + "";

            if (strImageUrl == "" || strImageUrl == null)
            {
                await this.DisplaySnackBarAsync("Please select your picture.", "OK", null, ts);
            }
            else if (etFname.Text == "" || etFname.Text == null)
            {
                await this.DisplaySnackBarAsync("Please enter valid First Name", "OK", null, ts);
            }
            else if (etLname.Text == "" || etLname.Text == null)
            {
                await this.DisplaySnackBarAsync("Please enter valid Last Name", "OK", null, ts);
            }
            else if (etBirthplace.Text == "" || etBirthplace.Text == null)
            {
                await this.DisplaySnackBarAsync("Please enter valid Birthplace", "OK", null, ts);
            }
            else if (etHouseNoAndStreet.Text == "" || etHouseNoAndStreet.Text == null)
            {
                await this.DisplaySnackBarAsync("Please enter valid House No. And Street.", "OK", null, ts);
            }
            else if (etContactNo.Text == "" || etContactNo.Text == null)
            {
                await this.DisplaySnackBarAsync("Please enter valid Contact No.", "OK", null, ts);
            }
            else if (strCivil == "" || strCivil == null)
            {
                await this.DisplaySnackBarAsync("Invalid Civil Status.", "OK", null, ts);
            }
            else if (strGender == "" || strGender == null)
            {
                await this.DisplaySnackBarAsync("Invalid Gender.", "OK", null, ts);
            }
            else if (strPurok == "" || strPurok == null)
            {
                await this.DisplaySnackBarAsync("Invalid  Purok.", "OK", null, ts);
            }
            else if (strVoter == "" || strVoter == null)
            {
                await this.DisplaySnackBarAsync("Invalid Voter Starus.", "OK", null, ts);
            }
            else
            {
                UpdateInfo();
            }

            //await Navigation.PushAsync(new Login());
        }

        private async void UpdateInfo()
        {
            try
            {
                String[] field = new String[16];
                field[0] = "Username";
                field[1] = "Fname";
                field[2] = "Mname";
                field[3] = "Lname";
                field[4] = "Sname";
                field[5] = "Birthplace";
                field[6] = "Birthdate";
                field[7] = "CivilStatus";
                field[8] = "Gender";
                field[9] = "id_purok";
                field[10] = "VoterStatus";
                field[11] = "DateOfRegistration";
                field[12] = "ContactNo";
                field[13] = "CedulaNo";
                field[14] = "Image";
                field[15] = "HouseNoAndStreet";

                String[] data = new String[16];
                data[0] = name.getStrusername();
                data[1] = etFname.Text;
                data[2] = etMname.Text == "" ? "NONE" : etMname.Text;
                data[3] = etLname.Text;
                data[4] = etSname.Text == "" ? "NONE" : etSname.Text;
                data[5] = etBirthplace.Text;
                data[6] = dtBirthday.Date.ToString("yyyy-MM-dd");
                data[7] = strCivil;
                data[8] = strGender;
                data[9] = strPurok;
                data[10] = strVoter;
                data[11] = strRegistrationDate;
                data[12] = etContactNo.Text;
                data[13] = etCedulaNo.Text;
                data[14] = strImageUrl;
                data[15] = etHouseNoAndStreet.Text;


                zsg_hosting hosting = new zsg_hosting();
                var uri = hosting.getUpdatePersonalinfo();

                try
                {
                    string responseFromServer;
                    using (var wb = new WebClient())
                    {
                        var datas = new NameValueCollection();
                        for (int i = 0; i < field.Length; i++)
                        {
                            datas[field[i].ToString()] = data[i].ToString();
                        }

                        var response = wb.UploadValues(uri, "POST", datas);
                        responseFromServer = Encoding.UTF8.GetString(response);
                    }

                    if (responseFromServer == "Update Failed")
                    {
                        await this.DisplaySnackBarAsync("Sign Up Failed", "OK", null, ts);
                    }
                    else if (responseFromServer == "Update Success")
                    {
                        name.nameandimage();

                        await DisplayAlert("Update Information.", "You Successfuly Update your Information. You can relogin to refresh your information properly.", "OK");
                        await Navigation.PopToRootAsync();

                    }
                    else
                    {
                        await this.DisplayToastAsync(responseFromServer, 5000);
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

        async void OnButtonSelectImage(object sender, EventArgs args)
        {
            await CrossMedia.Current.Initialize();
            try
            {
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

        //Upload Image to Azure Storage
        private async void UploadImage(Stream stream)
        {
            try
            {
                btnUpdate.IsEnabled = false;
                var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=ibarangaystorage;AccountKey=SuJ5YP5ovCzjeBc9sLKwbbhrk8GIWjrSyO493EnTRLc7tpNxApS/sdsIvk+qXWOhohgVASKI6VjFgrCYGYiuEw==;EndpointSuffix=core.windows.net");
                var client = account.CreateCloudBlobClient();
                var container = client.GetContainerReference("profileimages");
                await container.CreateIfNotExistsAsync();
                var name = Guid.NewGuid().ToString();
                var blockBlob = container.GetBlockBlobReference($"{name}.png");
                await blockBlob.UploadFromStreamAsync(stream);
                string URL = blockBlob.Uri.OriginalString;
                strImageUrl = URL;
                await this.DisplayToastAsync("Image Successfully Upload.", 5000);
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
                btnUpdate.IsEnabled = true;
            }
        }
    }
}