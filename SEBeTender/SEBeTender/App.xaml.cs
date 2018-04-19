using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SEBeTender
{
	public partial class App : Application
	{
        static tenderItemDatabase database;

		public App ()
		{
			InitializeComponent();

            MainPage = new SEBeTender.rootPage();
            //MainPage = new MainPage();
		}

        public static tenderItemDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new tenderItemDatabase(DependencyService.Get<IFileHelper>().getLocalFilePath("tenderSQLite.db3"));
                    Console.WriteLine("Database: " + database.getTenderItems());
                } else
                {
                    Console.WriteLine("My database isn't null!");
                }
                return database;
            }
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
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
