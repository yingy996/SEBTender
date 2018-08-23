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
		public pollPage ()
		{
			InitializeComponent ();
            
            pollQuestionLbl.Text = "What is your favourite way of making bill payment?";
            var optionList = new List<string>();
            optionList.Add("Testing 1");
            optionList.Add("Testing 2");
            pollOptionPicker.ItemsSource = optionList;

            if (!String.IsNullOrEmpty(adminAuth.Username))
            {
                checkAdminLoginStatus();
            }
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;

            selectedOption = pollOptionPicker.Items[pollOptionPicker.SelectedIndex];

            Console.WriteLine("Selected option: " + selectedOption);
        }

        async void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            Console.WriteLine("Submitted");
        }

        async void OnCreateButtonClicked(object sender, EventArgs e)
        {
            Console.WriteLine("Created");
            await Navigation.PushAsync(new createPollPage());
        }

        async void checkAdminLoginStatus()
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;
            string username = adminAuth.Username;
            string password = adminAuth.Password;

            //Send HTTP request to check user exists
            string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostadminloginCheck(username, password));
            var httpResult = httpTask;
            //Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.PostadminloginCheck(username, password));
            //var httpResult = httpTask.Result;
            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;
            //Console.WriteLine(httpResult);

            if (httpResult == "loggedin")
            {
                createButton.IsVisible = true;
            }
        }

    }
}