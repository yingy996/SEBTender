using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.FirebasePushNotification;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace SEBeTender
{
    public partial class App : Application
    {
        static tenderDatabase database;
       
        public static string AppName = "SEBeTender";

        public App()
        {
            InitializeComponent();

            if (checkUserLogin() == "false")
            {
                //User not logged in, show default tender listing page
                MainPage = new SEBeTender.rootPage();
            } else
            {
                //User logged in, show logged in menu and 'available tenders for purchase' page
                if (checkUserLogin() == "user")
                {
                    MainPage = new SEBeTender.rootPage(true);
                } else
                {
                    MainPage = new SEBeTender.rootPage(false);
                }
                
            }
            
            //MainPage = new MainPage();    
        }


        public static tenderDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new tenderDatabase(DependencyService.Get<ILocalFileHelper>().getLocalFilePath("tenderDb.db3"));
                }
                return database;
            }
       }

        private static string checkUserLogin()
        {
            if (String.IsNullOrEmpty(Settings.Username)) //user not logged in
            {
                return "false";
            } else
            {
                if (Settings.Role == "user")
                {
                    //Send HTTP request to log user in
                    Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.PostUserLogin(Settings.Username, Settings.Password));
                    var httpResult = httpTask.Result.ToString();

                    if (httpResult == "Success")
                    {
                        return "user";
                    }
                    else
                    {
                        return "false";
                    }
                } else
                {
                    //Login as admin
                    Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.PostAdminLogin(Settings.Username, Settings.Password));
                    var httpResult = httpTask.Result.ToString();

                    if (httpResult != "admin" && httpResult != "editor")
                    {
                        adminAuth.saveCredentials(Settings.Username, Settings.Password);
                        userSession.adminRole = httpResult;
                        return "admin";
                    } else
                    {
                        return "false";
                    }
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
