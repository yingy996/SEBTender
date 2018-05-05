using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.FirebasePushNotification;
using Xamarin.Forms;

namespace SEBeTender
{
    public partial class App : Application
    {
        static tenderDatabase database;
       
        public static string AppName = "SEBeTender";

        public App()
        {
            InitializeComponent();

            MainPage = new SEBeTender.rootPage();
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
