using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SEBeTender
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class registerAdminPage : ContentPage
	{
		public registerAdminPage ()
		{
			InitializeComponent ();
		}

        async void onRegisterButtonClicked(object sender, EventArgs eventArgs)
        {
            string name = "", email = "", username = "", password = "", confPassword = "";
            var selectedRole = rolePicker.Items[rolePicker.SelectedIndex];

            if (selectedRole == "Administrator")
            {
                selectedRole = "admin";
            } else if (selectedRole == "Editor")
            {
                selectedRole = "editor";
            }

            //Input error checking
            bool isInputsValid = true;
            errorLbl.Text = "";
            if (String.IsNullOrEmpty(confPasswordInput.Text) || String.IsNullOrWhiteSpace(confPasswordInput.Text))
            {
                errorLbl.Text = "Please enter Confirm Password. ";
                isInputsValid = false;
            } else
            {
                confPassword = confPasswordInput.Text;
            }

            if (String.IsNullOrEmpty(passwordInput.Text) || String.IsNullOrWhiteSpace(passwordInput.Text))
            {
                errorLbl.Text = "Please enter Password. ";
                isInputsValid = false;
            } else
            {
                password = passwordInput.Text;
            }

            if (isInputsValid)
            {
                if (confPassword != password)
                {
                    errorLbl.Text = "Password does not match. ";
                    isInputsValid = false;
                }
            }

            if (String.IsNullOrEmpty(usernameInput.Text) || String.IsNullOrWhiteSpace(usernameInput.Text))
            {
                errorLbl.Text = "Please enter Username. ";
                isInputsValid = false;
            } else {
                username = usernameInput.Text;
                if (!username.All(Char.IsLetterOrDigit))
                {
                    errorLbl.Text = "Username must be alphanumeric. ";
                    isInputsValid = false;
                }
            }

            if (String.IsNullOrEmpty(emailInput.Text) || String.IsNullOrWhiteSpace(emailInput.Text))
            {
                errorLbl.Text = "Please enter Email. ";
                isInputsValid = false;
            } else
            {
                email = emailInput.Text;
            }

            if (String.IsNullOrEmpty(nameInput.Text) || String.IsNullOrWhiteSpace(nameInput.Text))
            {
                errorLbl.Text = "Please enter Name. ";
                isInputsValid = false;
            } else {
                name = nameInput.Text;
                if (!Regex.IsMatch(name, @"^[a-zA-Z ]*$"))
                {
                    errorLbl.Text = "Name must be alphabetic. ";
                    isInputsValid = false;
                }
            }

            if (isInputsValid)
            {
                activityIndicator.IsVisible = true;
                activityIndicator.IsRunning = true;

                //Send HTTP request if inputs are valid
                string httpTask = await Task.Run<string>(() => HttpRequestHandler.registerNewAdmin(name, email, selectedRole, username, password, confPassword));
                string httpResult = httpTask.ToString();

                activityIndicator.IsVisible = false;
                activityIndicator.IsRunning = false;

                if (httpResult == "Account successfully registered!")
                {
                    await DisplayAlert("Success", "Account has been successfully registered!", "OK");
                    var page = App.Current.MainPage as rootPage;
                    var announcementPage = new announcementPage();
                    page.changePage(announcementPage);
                }
                else
                {
                    await DisplayAlert("Failed", httpResult, "OK");
                    errorLbl.Text =httpResult;
                }
            }
        }
	}
}