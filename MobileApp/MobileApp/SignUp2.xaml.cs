using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUp2 : ContentPage
    {

        private zsg_randomnum randomNumber = new zsg_randomnum();
        private Info inf = new Info();

        private int tries = 3;
        private int mins, secs;

        public SignUp2()
        {
            InitializeComponent();

            lblResend.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    randomNumber = new zsg_randomnum();
                    SendEmailAsync(randomNumber.randomNum());

                    mins = 4;
                    secs = 59;

                    lblResend.IsEnabled = false;

                    Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                    {
                        secs--;

                        if (secs == 0)
                        {
                            if (mins == 0)
                            {
                                lblResend.Text = "Resend Code?";
                                lblResend.IsEnabled = true;
                                return false;
                            }
                            else
                            {
                                mins--;
                                secs = 59;
                            }
                        }

                        string strmins = "0" + mins;
                        string strsecs = secs.ToString().Length <= 1 ? "0" + secs : secs + "";
                        lblResend.Text = "Resend OTP in " + strmins + ":" + strsecs;

                        return true;
                    });
                })
            });

            SendEmailAsync(randomNumber.randomNum());

            string editemail = inf.getStrEmail();
            string pattern = @"(?<=[\w]{4})[\w-\._\+%]*(?=[\w]{2}@)";
            string edittedemail = Regex.Replace(editemail, pattern, m => new string('*', m.Length));

            lblEmail.Text = " " + edittedemail;

            code1.TextChanged += CodeFocus1;
            code2.TextChanged += CodeFocus2;
            code3.TextChanged += CodeFocus3;
            code4.TextChanged += CodeFocus4;
            code5.TextChanged += CodeFocus5;
            code6.TextChanged += CodeFocus6;

            lblBack.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Navigation.PopAsync();
                })
            });

            Console.WriteLine("CODE: " + randomNumber.randomNum());
        }

        #region
        private void CodeFocus1(object sender, EventArgs args)
        {
            if (code1.Text.Length >= 1)
            {
                code2.Focus();
            }
        }

        private void CodeFocus2(object sender, EventArgs args)
        {
            if (code2.Text.Length >= 1)
            {
                code3.Focus();
            }
        }

        private void CodeFocus3(object sender, EventArgs args)
        {
            if (code3.Text.Length >= 1)
            {
                code4.Focus();
            }
        }

        private void CodeFocus4(object sender, EventArgs args)
        {
            if (code4.Text.Length >= 1)
            {
                code5.Focus();
            }
        }

        private void CodeFocus5(object sender, EventArgs args)
        {
            if (code5.Text.Length >= 1)
            {
                code6.Focus();
            }
        }

        private void CodeFocus6(object sender, EventArgs args) 
        {
            if (code6.Text.Length >= 1)
            {
                btnNext.Focus();
            }
        }

        #endregion

        async void OnButtonNext(object sender, EventArgs args)
        {
            string numText = code1.Text + code2.Text + code3.Text + code4.Text + code5.Text + code6.Text;

            if (code1.Text == "")
            {
                await this.DisplayToastAsync("Invalid Input.", 2000);
            }
            else if (code2.Text == "")
            {
                await this.DisplayToastAsync("Invalid Input.", 2000);
            }
            else if (code3.Text == "")
            {
                await this.DisplayToastAsync("Invalid Input.", 2000);
            }
            else if (code4.Text == "")
            {
                await this.DisplayToastAsync("Invalid Input.", 2000);
            }
            else if (code5.Text == "")
            {
                await this.DisplayToastAsync("Invalid Input.", 2000);
            }
            else if (code6.Text == "")
            {
                await this.DisplayToastAsync("Invalid Input.", 2000);
            }
            else if(numText == randomNumber.randomNum())
            {
                await Navigation.PushAsync(new Signup3());
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            }
            else
            {
                tries--;
                code1.Text = "";
                code2.Text = "";
                code3.Text = "";
                code4.Text = "";
                code5.Text = "";
                code6.Text = "";
                code1.Focus();

                await this.DisplayToastAsync("Verifcation Code is Wrong.", 5000);
                if (tries == 0)
                {

                }
            }
        }

        public async void SendEmailAsync(string randomNumber)
        {
            zsg_ApiKey ApiKey = new zsg_ApiKey();
            ApiKey.loadKeys();

            var client = new SendGridClient(ApiKey.getSendGridKey());
            var msg = new SendGridMessage()
            {

                From = new EmailAddress(ApiKey.getSendGridEmail(), "iBarangay<no-reply>"),
                Subject = "Verification Code",
                PlainTextContent = "Your verification code is: " + randomNumber,
                HtmlContent = "<p>Your verification code is: <strong> " + randomNumber + "</strong></p>"
            };

            msg.AddTo(new EmailAddress(inf.getStrEmail(), "ibarangay-user"));
            var response = await client.SendEmailAsync(msg);
            Console.WriteLine("Send Email ERROR: " + response.StatusCode + "");
        }
    }
}