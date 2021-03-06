﻿using Newtonsoft.Json;
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
        private bool isCurrentUser = true;
		public editUserPage ()
		{
			InitializeComponent ();
            //if no user is given, get the current admin user details to display
            isCurrentUser = true;
            getCurrentUserDetails();
        }

        public editUserPage(adminUser user)
        {
            InitializeComponent();
            isCurrentUser = false;
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

            if (user.Username == adminAuth.Username)
            {
                rolePicker.IsEnabled = false;
            }
        }

        async Task getCurrentUserDetails()
        {
            adminUser user = new adminUser();
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            //Send HTTP request to get current user details
            string httpTask = await Task.Run<string>(() => HttpRequestHandler.getCurrentUserDetails());
            string httpResult = httpTask.ToString();

            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;

            if (httpResult != null)
            {
                if (httpResult == "No user found")
                {
                    errorLbl.IsVisible = true;
                }
                else if (httpResult == "Admin not logged in")
                {
                    errorLbl.Text = httpResult;
                    errorLbl.IsVisible = true;
                }
                else
                {
                    List<adminUser> adminUsers = JsonConvert.DeserializeObject<List<adminUser>>(httpResult);
                    user = adminUsers[0];

                    nameInput.Text = user.administratorName;
                    emailInput.Text = user.administratorEmail;
                    if (user.Role == "Administrator")
                    {
                        rolePicker.SelectedIndex = 0;
                    }
                    else
                    {
                        rolePicker.SelectedIndex = 1;
                    }

                    usernameInput.Text = user.Username;
                }
                rolePicker.IsEnabled = false;
            }
            else
            {
                errorLbl.IsVisible = true;
            }
        }

        async void onUpdateButtonClicked(object sender, EventArgs eventArgs)
        {
            errorLbl.Text = "";
            errorLbl.IsVisible = true;
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
                    
                    if (isCurrentUser)
                    {
                        var pageToChange = new editUserPage();
                        page.changePage(pageToChange);
                    } else
                    {
                        var pageToChange = new manageUserPage();
                        page.changePage(pageToChange);
                    }                                       
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