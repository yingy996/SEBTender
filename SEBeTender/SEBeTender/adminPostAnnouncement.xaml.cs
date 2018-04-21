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
	public partial class adminPostAnnouncement : ContentPage
	{
		public adminPostAnnouncement ()
		{
			InitializeComponent ();
		}

        void OnClearButtonClicked(object sender, EventArgs e)
        {
            titleInput.Text = "";
            contentInput.Text = "";
        }

        async void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            if (adminAuth.Username == null)
            {
                Console.WriteLine("No username");
            } else
            {
                Console.WriteLine("Test Username:" + adminAuth.Username);
                Console.WriteLine("TEST Password:" + adminAuth.Password);
            }

            if (adminAuth.Password == null)
            {
                Console.WriteLine("No PASSWORD");
            }
               if (adminAuth.Username != null && adminAuth.Password != null)
            {
                activityIndicator.IsVisible = true;
                activityIndicator.IsRunning = true;

                string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostAddAnnouncement(adminAuth.Username, adminAuth.Password, titleInput.Text, contentInput.Text));
                string httpResult = httpTask.ToString();

                activityIndicator.IsVisible = false;
                activityIndicator.IsRunning = false;
                if (httpResult == "You have succesfully posted your announcement!")
                {
                    await DisplayAlert("Success", "Announcement has been successfully posted!", "OK");
                    var page = App.Current.MainPage as rootPage;
                    var announcementPage = new announcementPage();
                    page.changePage(announcementPage);

                } else
                {
                    await DisplayAlert("Failed", httpResult, "OK");
                }
            } else
            {
                Console.WriteLine("User is not logged in");
            }
            
        }
    }
}