using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Auth;
using System.Linq;

namespace SEBeTender
{
    class adminAuth
    {
        private static string adminUsername, adminPassword;
        public static void saveCredentials (string username, string password)
        {
            if(Device.RuntimePlatform == Device.iOS)
            {
                adminUsername = username;
                adminPassword = password;
            } else
            {
                if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password))
                {
                    Account adminAccount = new Account { Username = username };
                    adminAccount.Properties.Add("Password", password);
                    AccountStore.Create().Save(adminAccount, App.AppName);
                }
            }           
        }

        public static string Username
        {
            get
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    return adminUsername;
                } else
                {
                    var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
                    return (account != null) ? account.Username : null;
                }  
            }
        }

        public static string Password
        {
            get
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    return adminPassword;
                }
                else
                {
                    var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
                    return (account != null) ? account.Properties["Password"] : null;
                } 
            }
        }
    }
}
