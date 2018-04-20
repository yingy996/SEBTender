﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SEBeTender
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class logoutPage : ContentPage
	{
        public logoutPage ()
		{
			InitializeComponent ();
            LogoutUserAsync();
		}

        async void LogoutUserAsync()
        {
            logoutStatus.Text = "";

            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            var httpTask = await Task.Run<string>(() => HttpRequestHandler.PostUserLogout());
            var httpResult = httpTask.ToString();

            if (httpResult == "Success")
            {
                logoutStatus.Text = "You have successfully logout! You will be redirected to tender page shortly.";
                logoutStatus.TextColor = Color.Default;
                logoutStatus.FontAttributes = FontAttributes.None;
                await Task.Delay(500);

                Application.Current.MainPage = new rootPage { Detail = new NavigationPage(new loginPage()) };

            }
            else
            {
                logoutStatus.TextColor = Color.Red;
                logoutStatus.FontAttributes = FontAttributes.None;
                logoutStatus.Text = "Error: Logout Unsuccessful, " + httpResult;
            }

            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;
            
        }
    }
}