using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SEBeTender
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class adminLoginPage : ContentPage
	{
		public adminLoginPage ()
		{
			InitializeComponent ();
		}

        async void onLoginBtnClicked(object sender, EventArgs e)
        {
            errorLbl.Text = "";
            bool isInputsValid = true;
            string username = "", password = "";
            if (String.IsNullOrEmpty(userIdEntry.Text) || String.IsNullOrWhiteSpace(userIdEntry.Text))
            {

                errorLbl.Text += "Please enter User Id. ";
                isInputsValid = false;
            }

            if (String.IsNullOrEmpty(passwordEntry.Text) || String.IsNullOrWhiteSpace(passwordEntry.Text))
            {
                errorLbl.Text += "Please enter your password.";
                isInputsValid = false;
            }

            if (isInputsValid)
            {
                activityIndicator.IsVisible = true;
                activityIndicator.IsRunning = true;
                username = userIdEntry.Text;
                password = passwordEntry.Text;

                //Send HTTP request to log user in 
                string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostAdminLogin(username, password));
                var httpResult = httpTask.ToString();

                //Data extraction to get admin login status from HTTP response
                //var extractionTask = await Task.Run<Object>(() => DataExtraction.getWebData(httpResult, "adminLoginPage"));
                var status = await DataExtraction.getWebData(httpResult, "adminLoginPage");

                activityIndicator.IsVisible = false;
                activityIndicator.IsRunning = false;

                //if login success, save the user account with Xamarin.Auth
                if (status.ToString() == "success")
                {
                    adminAuth.saveCredentials(username, password);
                    Console.WriteLine("Admin login: " + adminAuth.Username);
                    //Navigate to tender page
                    errorLbl.TextColor = Color.Green;

                    errorLbl.Text = "Login success! You will be redirected soon";
                    await Task.Delay(1000);
                    //errorLbl.TextColor = Color.Red;
                    //errorLbl.Text = "";

                    //await Navigation.PushAsync(new tenderPage());
                    //App.Current.MainPage = new rootPage { Detail = new NavigationPage(new tenderEligiblePage()) };
                    App.Current.MainPage = new rootPage(false);
                    //rootPage.changeDetailPage(typeof(tenderPage));
                }
                else
                {
                    Console.WriteLine("HTTP Result: " + httpResult);
                    Console.WriteLine("Status : " + status.ToString());
                    
                    errorLbl.Text = "Login failed! Please try again.";
                }
            }

        }
    }
}