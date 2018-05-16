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
	public partial class changePasswordPage : ContentPage
	{
		public changePasswordPage ()
		{
			InitializeComponent ();
		}

        async void onUpdateBtnClicked(object sender, EventArgs e)
        {
            string oldPassword = "", newPassword = "", confPassword = "";
            bool isInputsValid = true;
            errorLbl.Text = "";

            //Inputs validation
            if (String.IsNullOrEmpty(confPasswordInput.Text) || String.IsNullOrWhiteSpace(confPasswordInput.Text))
            {
                errorLbl.Text = "Please enter Confirm Password. ";
                isInputsValid = false;
            }
            else
            {
                confPassword = confPasswordInput.Text;
            }

            if (String.IsNullOrEmpty(newPasswordInput.Text) || String.IsNullOrWhiteSpace(newPasswordInput.Text))
            {
                errorLbl.Text = "Please enter New Password. ";
                isInputsValid = false;
            }
            else
            {
                newPassword = newPasswordInput.Text;
            }

            //if new password and confirm password are not empty, check if they matches
            if (isInputsValid)
            {
                if (confPassword != newPassword)
                {
                    errorLbl.Text = "Confirm Password does not match. ";
                    isInputsValid = false;
                } else
                {
                    if (String.IsNullOrEmpty(oldPasswordInput.Text) || String.IsNullOrWhiteSpace(oldPasswordInput.Text))
                    {
                        errorLbl.Text = "Please enter Old Password. ";
                        isInputsValid = false;
                    }
                    else
                    {
                        oldPassword = oldPasswordInput.Text;
                    }

                    if(isInputsValid)
                    {
                        activityIndicator.IsVisible = true;
                        activityIndicator.IsRunning = true;

                        //Send HTTP request to update password
                        string httpTask = await Task.Run<string>(() => HttpRequestHandler.adminChangePassword(oldPassword, newPassword, confPassword));
                        string httpResult = httpTask.ToString();

                        activityIndicator.IsVisible = false;
                        activityIndicator.IsRunning = false;

                        if (httpResult == "Successfully changed password!")
                        {
                            await DisplayAlert("Success", "Password has been successfully changed! You will be logged out now.", "OK");
                            adminAuth.DeleteCredentials();

                            App.Current.MainPage = new rootPage();
                            var page = App.Current.MainPage as rootPage;
                            var loginPage = new loginPage();
                            page.changePage(loginPage);
                        }
                        else
                        {
                            await DisplayAlert("Failed", httpResult, "OK");
                            errorLbl.Text = httpResult;
                        }
                    }                   
                }
            }


        }
    }
}