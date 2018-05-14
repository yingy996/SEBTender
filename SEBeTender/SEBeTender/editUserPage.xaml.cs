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
	public partial class editUserPage : ContentPage
	{
		public editUserPage ()
		{
			InitializeComponent ();
		}

        public editUserPage(adminUser user)
        {
            InitializeComponent();
            nameInput.Text = user.administratorName;
            emailInput.Text = user.administratorEmail;
            if(user.Role == "Administrator")
            {
                rolePicker.SelectedIndex = 0;
            } else
            {
                rolePicker.SelectedIndex = 1;
            }

            usernameInput.Text = user.Username;
        }

        async void onUpdateButtonClicked(object sender, EventArgs eventArgs)
        {
            string name = "", email = "", username = "";
            var selectedRole = rolePicker.Items[rolePicker.SelectedIndex];

            if (selectedRole == "Administrator")
            {
                selectedRole = "admin";
            }
            else if (selectedRole == "Editor")
            {
                selectedRole = "editor";
            }

            //Input error checking
            bool isInputsValid = true;
            errorLbl.Text = "";

            if (String.IsNullOrEmpty(usernameInput.Text) || String.IsNullOrWhiteSpace(usernameInput.Text))
            {
                errorLbl.Text = "Please enter Username. ";
                isInputsValid = false;
            }
            else
            {
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
            }
            else
            {
                email = emailInput.Text;
            }

            if (String.IsNullOrEmpty(nameInput.Text) || String.IsNullOrWhiteSpace(nameInput.Text))
            {
                errorLbl.Text = "Please enter Name. ";
                isInputsValid = false;
            }
            else
            {
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
                string httpTask = await Task.Run<string>(() => HttpRequestHandler.editAdmin(name, email, selectedRole, username));
                string httpResult = httpTask.ToString();

                activityIndicator.IsVisible = false;
                activityIndicator.IsRunning = false;

                if (httpResult == "Successfully updated user information!")
                {
                    await DisplayAlert("Success", httpResult, "OK");
                    var page = App.Current.MainPage as rootPage;
                    var manageUserPage = new manageUserPage();
                    page.changePage(manageUserPage);
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