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
	public partial class accountRegistrationPage : ContentPage
	{
		public accountRegistrationPage ()
		{
			InitializeComponent ();
		}

        async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            bool isErrorPresent = false;
            string errorMessage = "";
            
            //Validate inputs
            if (fullNameEntry.Text == "")
            {
                isErrorPresent = true;
                errorMessage = "Please enter your Full Name";
            } else
            {
                if (!Regex.IsMatch(fullNameEntry.Text, @"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$"))
                {
                    isErrorPresent = true;
                    errorMessage = "Wrong format for Full Name. Full name must be alphabetic.";
                }
            }

            if (emailEntry.Text == "")
            {
                isErrorPresent = true;
                errorMessage = "Please enter your Full Name";
            } else
            {
                if (!Regex.IsMatch(emailEntry.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                {
                    isErrorPresent = true;
                    errorMessage = "Wrong format for Email. e.g. anthony@mail.com";
                }
            }

            if (usernameEntry.Text == "")
            {
                isErrorPresent = true;
                errorMessage = "Please enter your Full Name";
            } else
            {
                if (!Regex.IsMatch(usernameEntry.Text, @"^[a-zA-Z0-9]*$"))
                {
                    isErrorPresent = true;
                    errorMessage = "Wrong format for Username. Username may only contain alphabets and numbers.";
                }
            }

            if (passwordEntry.Text == "")
            {
                isErrorPresent = true;
                errorMessage = "Please enter your Full Name";
            }
            else
            {
                if (!Regex.IsMatch(passwordEntry.Text, @"^[a-zA-Z0-9]*$"))
                {
                    isErrorPresent = true;
                    errorMessage = "Wrong format for Password. Password may only contain alphabets and numbers.";
                }
            }

            if (confPasswordEntry.Text != passwordEntry.Text)
            {
                isErrorPresent = true;
                errorMessage = "Confirm password does not match";
            }

            if (isErrorPresent)
            {
                //Display error message if inputs have error
                await DisplayAlert("Failed", errorMessage, "OK");
            } else
            {
                //Send HTTP request to register user
                activityIndicator.IsVisible = true;
                activityIndicator.IsRunning = true;

                string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostAddPoll(adminAuth.Username, adminAuth.Password, pollQuestionInput.Text, selectedOption, options));
                string httpResult = httpTask.ToString();

                if (httpResult == "Registration successful")
                {
                    await DisplayAlert("Success", "You have successfully registered an account! Login now.", "OK");
                    var page = App.Current.MainPage as rootPage;
                    var loginPage = new loginPage();
                    page.changePage(loginPage);
                }
            }
        }
    }
}