using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Auth;
using Xamarin.Forms;

namespace SEBeTender
{
    static class adminAuthentication
    {
        public static void saveCredentials (string username, string password)
        {
            if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password))
            {
                Account adminAccount = new Account { Username = username };
                adminAccount.Properties.Add("Password", password);
                AccountStore.Create().Save(adminAccount, App.AppName);
            }
        }

        /*public static string Username
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
                return (account != null) ? account.Username : null;
            }
        }

        public static string Password
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
                return (account != null) ? account.Properties["Password"] : null;
            }
        }*/
    }
}
