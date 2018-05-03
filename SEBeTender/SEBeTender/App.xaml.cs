using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.FirebasePushNotification;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace SEBeTender
{
    public partial class App : Application
    {
        SEBeTender.MainPage mPage;
        public static string AppName = "SEBeTender";

        public App()
        {
            InitializeComponent();

            if (checkUserLogin() == false)
            {
                //User not logged in, show default tender listing page
                MainPage = new SEBeTender.rootPage();
            } else
            {
                //User logged in, show logged in menu and 'available tenders for purchase' page
                MainPage = new SEBeTender.rootPage(true);
            }
            
            //MainPage = new MainPage();
        }

        private static bool checkUserLogin()
        {
            if (String.IsNullOrEmpty(Settings.Username)) //user not logged in
            {
                return false;
            } else
            {
                //Send HTTP request to log user in
                Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.PostUserLogin(Settings.Username, Settings.Password));
                var httpResult = httpTask.Result.ToString();

                if (httpResult == "Success")
                {
                    return true;
                } else
                {
                    return false;
                }
                
            }
        }

        public static string NotificationSettings {
            get { return Settings.NotificationSettings; } 
            set { Settings.NotificationSettings = value; }
        }

		protected override void OnStart ()
		{
            // Handle when your app starts
            adminAuth.DeleteCredentials();
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
