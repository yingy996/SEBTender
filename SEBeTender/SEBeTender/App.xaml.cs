﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SEBeTender
{
	public partial class App : Application
	{
        public static string AppName = "SEBeTender";

        public App ()
		{
			InitializeComponent();

            MainPage = new SEBeTender.rootPage();
            //MainPage = new MainPage();
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
