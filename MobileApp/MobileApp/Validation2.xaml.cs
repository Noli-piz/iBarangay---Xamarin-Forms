using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Validation2 : ContentPage
    {

        private string strImageUrl1;
        private string strImageUrl2;

        public Validation2( string strImageUrl1)
        {
            InitializeComponent();
            this.strImageUrl1 = strImageUrl1;

            lblBack.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Navigation.PopAsync();
                })
            });
        }

        async void OnButtonOpenCam(object sender, EventArgs args)
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();

                if (photo == null)
                {
                    await this.DisplayAlert("No Image.", "No Image Taken.", "OK");
                    return;
                }
                var stream = await photo.OpenReadAsync();

                UploadImage(stream);

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }

        async void OnButtonNext(object sender, EventArgs args)
        {
            if (strImageUrl2 == "" || strImageUrl2 == null)
            {
                await this.DisplayAlert("No Photo.", "Please take an image.", "OK");
            }
            else
            {
                await Navigation.PushAsync(new Validation3(strImageUrl1, strImageUrl2));
            }
        }


        //Upload Image to Azure Storage
        private async void UploadImage(Stream stream)
        {
            try
            {
                progressbar.IsVisible = true;
                btnNext.IsEnabled = false;
                var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=ibarangaystorage;AccountKey=SuJ5YP5ovCzjeBc9sLKwbbhrk8GIWjrSyO493EnTRLc7tpNxApS/sdsIvk+qXWOhohgVASKI6VjFgrCYGYiuEw==;EndpointSuffix=core.windows.net");
                var client = account.CreateCloudBlobClient();
                var container = client.GetContainerReference("profileimages");
                await container.CreateIfNotExistsAsync();
                var name = Guid.NewGuid().ToString();
                var blockBlob = container.GetBlockBlobReference($"{name}.png");
                await blockBlob.UploadFromStreamAsync(stream);
                string URL = blockBlob.Uri.OriginalString;
                strImageUrl2 = URL;
                imgValidationPhoto.Source = strImageUrl2;
                await this.DisplayToastAsync("Image Successfully Upload.", 3000);
                Console.WriteLine("IMAGE URL: " + strImageUrl2);
            }
            catch (Exception ex)
            {
                await this.DisplayToastAsync("Somthing went wrong.", 5000);
                Console.WriteLine("ERROR: " + ex.Message);
            }
            finally
            {
                progressbar.IsVisible = false;
                btnNext.IsEnabled = true;
            }
        }
    }
}