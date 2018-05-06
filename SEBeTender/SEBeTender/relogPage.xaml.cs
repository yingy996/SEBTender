using System;
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
	public partial class relogPage : ContentPage
	{
		public relogPage ()
		{
            InitializeComponent();
            RelogUserAsync();
        }

        async void RelogUserAsync()
        {
            logoutStatus.Text = "";

            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            var httpTask = await Task.Run<string>(() => HttpRequestHandler.PostUserLogout());
            var httpResult = httpTask.ToString();

            if (httpResult == "Success")
            {
                //Clear user session 
                userSession.userLoginCookie = "";
                userSession.username = "";
                Settings.Username = string.Empty;
                Settings.Password = string.Empty;
                logoutStatus.Text = "You have successfully logout! You will be redirected to login page shortly.";
                logoutStatus.TextColor = Color.Default;
                logoutStatus.FontAttributes = FontAttributes.None;

                activityIndicator.IsVisible = false;
                activityIndicator.IsRunning = false;
                await Task.Delay(1000);

                //App.Current.MainPage = new rootPage { Detail = new NavigationPage(new loginPage()) 
                App.Current.MainPage = new rootPage();
                var page = App.Current.MainPage as rootPage;
                var loginPage = new loginPage();
                page.changePage(loginPage);

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