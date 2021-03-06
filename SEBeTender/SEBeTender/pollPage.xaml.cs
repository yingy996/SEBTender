﻿using Newtonsoft.Json;
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
	public partial class pollPage : ContentPage
	{
        string selectedOption = "";
        Poll poll = new Poll();
        bool isPollPresent = false;

		public pollPage ()
		{
			InitializeComponent ();

            retrievePoll();
            /*if (!String.IsNullOrEmpty(adminAuth.Username))
            {
                checkAdminLoginStatus();
            }*/
        }

        async void retrievePoll()
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;
            
            string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostGetPollQuestion());
            while (httpTask == null)
            {
                httpTask = await Task.Run<string>(() => HttpRequestHandler.PostGetPollQuestion());
            }

            if (httpTask != null)
            {
                if (httpTask != "There is no poll for now.")
                {
                    List<Poll> pollList = JsonConvert.DeserializeObject<List<Poll>>(httpTask.ToString());

                    //If poll is available, get the poll details, else display the error message
                    if (pollList != null)
                    {
                        if (pollList.Count > 0)
                        {
                            //Get poll details
                            poll = pollList[0];
                            Console.WriteLine("Poll question is: " + poll.pollQuestion);
                            string pollID = poll.pollID;
                            string pollQuestion = poll.pollQuestion;

                            //Get poll options
                            string httpOptionTask = await Task.Run<string>(() => HttpRequestHandler.PostGetPollOptions(pollID));
                            while (httpOptionTask == null)
                            {
                                httpOptionTask = await Task.Run<string>(() => HttpRequestHandler.PostGetPollOptions(pollID));
                            }

                            if (httpOptionTask != null)
                            {
                                List<pollOption> pollOptionList = JsonConvert.DeserializeObject<List<pollOption>>(httpOptionTask.ToString());
                                poll.pollOptions = pollOptionList;
                                List<string> pollOptionStrings = new List<string>();
                                foreach (pollOption option in poll.pollOptions)
                                {
                                    pollOptionStrings.Add(option.optionTitle);
                                }
                                if (pollOptionList != null)
                                {
                                    if (pollOptionList.Count > 0)
                                    {

                                        //Display poll details and options    
                                        isPollPresent = true;
                                        Console.WriteLine("YES TESTING IF ISPRESENT " + isPollPresent);
                                        pollQuestionLbl.Text = pollQuestion;
                                        pollOptionPicker.ItemsSource = pollOptionStrings;
                                        pollQuestionLbl.IsVisible = true;
                                        optionFrame.IsVisible = true;
                                        //submitButton.IsVisible = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            errorMsg.IsVisible = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Poll Task is null ");
                        errorMsg.IsVisible = true;
                    }
                } else
                {
                    errorMsg.IsVisible = true;
                }
                               
            }
            //await Task.Run(() => checkAdminLoginStatus());
            checkAdminLoginStatus();
            //activityIndicator.IsVisible = false;
            //activityIndicator.IsRunning = false;
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;

            selectedOption = pollOptionPicker.Items[pollOptionPicker.SelectedIndex];

            Console.WriteLine("Selected option: " + selectedOption);
        }

        async void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            if (selectedOption == "")
            {
                await DisplayAlert("Failed", "Please select an answer", "OK");
            } else
            {
                //Submit user's aswer for poll
                activityIndicator.IsVisible = true;
                activityIndicator.IsRunning = true;
                string optionID = "";
                foreach (pollOption option in poll.pollOptions)
                {
                    if (selectedOption == option.optionTitle)
                    {
                        optionID = option.optionID;
                    }
                }
                string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostSubmitPollAnswer(poll.pollID, optionID, userSession.username, ""));
                string httpResult = httpTask.ToString();

                activityIndicator.IsVisible = false;
                activityIndicator.IsRunning = false;

                if (httpResult == "Answer has been successfully submitted. Thank you for participating!")
                {
                    await DisplayAlert("Success", httpResult, "OK");
                    var page = App.Current.MainPage as rootPage;
                    var pollPage = new pollPage();
                    page.changePage(pollPage);
                }
                else
                {
                    await DisplayAlert("Failed", httpResult, "OK");
                }
            }
        }

        async void OnCreateButtonClicked(object sender, EventArgs e)
        {
            Console.WriteLine("Created");
            await Navigation.PushAsync(new createPollPage());
        }

        async void OnEditButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new editPoll(poll));
        }

        async void OnCloseButtonClicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Close poll", "Are you sure you want to close this poll?", "Yes", "No");
            if (answer)
            {
                //If user confirm to close the poll, send the HTTP request
                activityIndicator.IsVisible = true;
                activityIndicator.IsRunning = true;

                string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostClosePoll(poll.pollID, adminAuth.Username, adminAuth.Password));
                string httpResult = httpTask.ToString();

                activityIndicator.IsVisible = false;
                activityIndicator.IsRunning = false;

                if (httpResult == "Successfully closed poll!")
                {
                    await DisplayAlert("Success", "Poll has been successfully closed!", "OK");
                    var page = App.Current.MainPage as rootPage;
                    var pollPage = new pollPage();
                    page.changePage(pollPage);
                }
                else
                {
                    await DisplayAlert("Failed", httpResult, "OK");
                }
            }
        }

        async void checkAdminLoginStatus()
        {
            //activityIndicator.IsVisible = true;
            //activityIndicator.IsRunning = true;
            string username = adminAuth.Username;
            string password = adminAuth.Password;

            //Send HTTP request to check user exists
            string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostadminloginCheck(username, password));
            var httpResult = httpTask;
            //Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.PostadminloginCheck(username, password));
            //var httpResult = httpTask.Result;
            
            //Console.WriteLine(httpResult);

            if (httpResult == "loggedin")
            {
                Console.WriteLine("ADMIN LOGGED IN");
                submitButton.IsVisible = false;
                if (isPollPresent == false)
                {
                    //If poll is not present, display "Create" button
                    Console.WriteLine("isPollPresent = " + isPollPresent);
                    createButton.IsVisible = true;
                } else
                {
                    //Display "Edit" button if poll is present
                    editButton.IsVisible = true;
                    closeButton.IsVisible = true;
                }
                submitButton.IsVisible = false;
            } else
            {
                Console.WriteLine("ADMIN NOT LOGGED IN");
                createButton.IsVisible = false;
                editButton.IsVisible = false;
                closeButton.IsVisible = false;

                if (isPollPresent == true)
                {
                    submitButton.IsVisible = true;
                }
            }

            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;
        }

    }
}