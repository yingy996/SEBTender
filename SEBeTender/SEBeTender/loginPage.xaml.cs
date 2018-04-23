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
	public partial class loginPage : ContentPage
	{
		public loginPage ()
		{
			InitializeComponent ();

            //Set admin login hyperlink label
            var adminLoginLblTapRecognizer = new TapGestureRecognizer();
            adminLoginLblTapRecognizer.Tapped += onAdminLoginClicked;
            adminLoginLbl.GestureRecognizers.Add(adminLoginLblTapRecognizer);
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
                string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostUserLogin(username, password));
                var httpResult = httpTask.ToString();
                activityIndicator.IsVisible = false;
                activityIndicator.IsRunning = false;
                if (httpResult == "Success")
                {
                    //Navigate to tender page
                    errorLbl.TextColor = Color.Green;

                    errorLbl.Text = "Login success! You will be redirected soon";
                    await Task.Delay(1000);
                    errorLbl.TextColor = Color.Red;
                    errorLbl.Text = "";

                    //await Navigation.PushAsync(new tenderPage());
                    //App.Current.MainPage = new rootPage { Detail = new NavigationPage(new tenderEligiblePage()) };
                    App.Current.MainPage = new rootPage(true);
                    //rootPage.changeDetailPage(typeof(tenderPage));
                    
                } else
                {
                    //Display error message
                    errorLbl.Text = httpResult;
                }
                
            }

        }

        async void onAdminLoginClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new adminLoginPage());
        }
	}
}