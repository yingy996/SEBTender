using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SEBeTender
{
	public partial class App : Application
	{
        public static string AppName = "SEBeTender";
        static tenderDatabase database;
        public App ()
		{
            if (database != null)
            {
                database.deleteTendersAsync();
                
            }
            InitializeComponent();
            MainPage = new SEBeTender.rootPage();
            
            
            //MainPage = new MainPage();
		}

        public static tenderDatabase Database
        {
            get
            {
                if(database == null)
                {
                    if (DependencyService.Get<ILocalFileHelper>() != null)
                    {
                        database = new tenderDatabase(DependencyService.Get<ILocalFileHelper>().getLocalFilePath("TenderDB.db3"));
                    }
                    

                }

                return database;
            }
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
