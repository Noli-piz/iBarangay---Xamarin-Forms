using Android.Graphics;
using Org.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace MobileApp
{
    class zsg_nameandimage
    {
        private static String Fname, Mname, Lname, Sname, Image;
        private static String strusername, strname, stremail, strImg;
        private static Boolean boolVerified = false;
        private static Bitmap bitImg;
        private static String Birthplace, Gender, CivilStatus, ContactNo, CedulaNo, Purok, VoterStatus, HouseNoAndStreet;
        private static int BYear, BMonth, BDay, RYear, RMonth, RDay;

        private zsg_hosting sg_host = new zsg_hosting();
        public void nameandimage()
        {
            RetrieveInfo();
        }

        public async void retrievedVerificationStatus()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    zsg_hosting hosting = new zsg_hosting();
                    var uri = hosting.getPersonalinfo() + "?Username=" + strusername;
                    var result = await client.GetStringAsync(uri);

                    JSONObject jsonresult = new JSONObject(result);
                    int success = jsonresult.GetInt("success");

                    if (success == 1)
                    {
                        JSONArray information = jsonresult.GetJSONArray("info");

                        JSONObject info = information.GetJSONObject(0);
                        if (info.GetString("Valid") == "0" || info.GetString("Valid") == "false" || info.GetString("Valid") == "False")
                        {
                            boolVerified = false;
                        }
                        else
                        {
                            boolVerified = true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }

        public void reset()
        {
            stremail = "";
            strusername = "";
            strname = "";
            stremail = "";
            strImg = "";
            bitImg = null;
            Fname = "";
            Mname = "";
            Lname = "";
            Sname = "";
        }

        private async void RetrieveInfo()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    zsg_hosting hosting = new zsg_hosting();
                    var uri = hosting.getPersonalinfo() + "?Username=" + strusername;
                    var result = await client.GetStringAsync(uri);

                    JSONObject jsonresult = new JSONObject(result);
                    int success = jsonresult.GetInt("success");

                    if (success == 1)
                    {
                        JSONArray information = jsonresult.GetJSONArray("info");

                        JSONObject info = information.GetJSONObject(0);
                        Fname = info.GetString("Fname");
                        Mname = info.GetString("Mname");
                        Lname = info.GetString("Lname");
                        Sname = info.GetString("Sname");
                        Image = info.GetString("Image"); 
                        Purok = info.GetString("Purok");
                        VoterStatus = info.GetString("VoterStatus");

                        Birthplace = info.GetString("Birthplace");
                        ContactNo = info.GetString("ContactNo");
                        CedulaNo = info.GetString("CedulaNo");

                        CivilStatus = info.GetString("CivilStatus");
                        Gender = info.GetString("Gender");
                        HouseNoAndStreet = info.GetString("HouseNoAndStreet");

                        BYear = Int32.Parse(info.GetString("BYear"));
                        BMonth = Int32.Parse(info.GetString("BMonth"));
                        BDay = Int32.Parse(info.GetString("BDay"));

                        RYear = Int32.Parse(info.GetString("RYear"));
                        RMonth = Int32.Parse(info.GetString("RMonth"));
                        RDay = Int32.Parse(info.GetString("RDay"));

                        if (info.GetString("Valid") == "0" || info.GetString("Valid") == "false" || info.GetString("Valid") == "False")
                        {
                            boolVerified = false;
                        }
                        else
                        {
                            boolVerified = true;
                        }

                        strname = Fname + " " + Mname + " " + Lname + " " + Sname;
                        strImg = Image;
                        //DownloadImage(Image);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }

        private async void DownloadImage(string url)
        {
            try
            {
                Bitmap imageBitmap = null;
                using (var webClient = new WebClient())
                {
                    var imageBytes = webClient.DownloadData(url);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    }
                }

                bitImg = imageBitmap;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                bitImg = null;
            }
        }


        //Setters Getters
        public String getStrusername()
        {
            return strusername;
        }

        public void setStrusername(String strusernames)
        {
            strusername = strusernames;
        }

        public String getStrname()
        {
            return strname;
        }

        public String getFirstName()
        {
            return Fname;
        }
        public String getMiddleName()
        {
            return Mname;
        }
        public String getLastName()
        {
            return Lname;
        }
        public String getSuffixName()
        {
            return Sname;
        }

        public String getBirthPlace() { return Birthplace; }
        public String getCiviStatus() { return CivilStatus; }
        public String getGender() { return Gender; }
        public String getHouseNoAndStreet() { return HouseNoAndStreet; }
        public String getContactNo() { return ContactNo; }
        public String getCedulaNo() { return CedulaNo; }
        public String getPurok() { return Purok; }
        public String getVoterStatus() { return VoterStatus; }

        public int getBYear() { return BYear; }
        public int getBMonth() { return BMonth; }
        public int getBDay() { return BDay; }

        public int getRYear() { return RYear; }
        public int getRMonth() { return RMonth; }
        public int getRDay() { return RDay; }

        public String getStremail()
        {
            return stremail;
        }

        public String getStrImg()
        {
            return strImg;
        }
        public Boolean getboolVerified()
        {
            return boolVerified;
        }

        public Bitmap getImg()
        {
            return bitImg;
        }
    }
}
