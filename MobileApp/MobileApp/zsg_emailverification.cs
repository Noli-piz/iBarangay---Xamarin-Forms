using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp
{
    public class zsg_emailverification
    {
        private static string email = "", username = "";

        public void setEmail(string eml)
        {
            email = eml;
        }

        public string getEmail()
        {
            return email;
        }

        public void setUsername(string usr)
        {
            username = usr;
        }

        public string getUsername()
        {
            return username;
        }

        public void reset()
        {
            email = "";
            username = "";
        }

    }
}
