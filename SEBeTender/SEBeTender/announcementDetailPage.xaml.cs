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
    public partial class announcementDetailPage : ContentPage
    {
        string announcementid = "";
        public announcementDetailPage()
        {
            
        }
        public announcementDetailPage(RootObject announcementItem)
        {
            InitializeComponent();
            //string username = "", password = "";
            editButton.IsVisible = false;
            deleteButton.IsVisible = false;

            announcementTitlelbl.Text = announcementItem.announcementTitle;
            announcementContentlbl.Text = announcementItem.announcementContent;
            publishedDatelbl.Text = announcementItem.publishedDate;
            postedBylbl.Text = announcementItem.postedBy;
            editedDatelbl.Text = announcementItem.editedDate.ToString();
            editedBylbl.Text = announcementItem.editedBy.ToString();

            announcementid = announcementItem.announcementID;

            checkAdminLoginStatus(announcementItem);

        }

        async void checkAdminLoginStatus(RootObject announcementItem)
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
                editButton.IsVisible = true;
                deleteButton.IsVisible = true;

                deleteButton.Clicked += OnDeleteButtonClicked;
            }

            
        }

        async void OnEditButtonClicked(object sender, EventArgs e)
        {
            if (announcementid == "")
            {
                Console.WriteLine("Invalid announcement ID: " + announcementid );
            }
            else
            {
                await Navigation.PushAsync(new editAnnouncement(announcementid));
                /*var page = App.Current.MainPage as rootPage;
                var editAnnouncementPage = new editAnnouncement(announcementid);
                page.changePage(editAnnouncementPage);*/
            }

        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Delete", "Are you sure you want to delete the announcement?", "Yes", "No");

            if (answer)
            {
                string username = "";
                username = adminAuth.Username;
                //Send HTTP request to check user exists
                Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.deleteAnnouncement(announcementid, username));
                var httpResult = httpTask.Result;

                if (httpResult == "deletesuccess")
                {
                    await DisplayAlert("Success", "Announcement successfully deleted", "OK");

                    var page = App.Current.MainPage as rootPage;
                    var announcementPage = new announcementPage();
                    page.changePage(announcementPage);
                } else
                {
                    int count = 0;
                    while (count < 3 && httpResult != "deletesuccess")
                    {
                        Console.WriteLine("Looping for failure delete");
                        httpTask = Task.Run<string>(() => HttpRequestHandler.deleteAnnouncement(announcementid, username));
                        httpResult = httpTask.Result;
                        count++;
                    }

                    if (httpResult != "deletesuccess")
                    {
                        await DisplayAlert("Failed", "Failed to delete announcement. Please try again later.", "OK");

                        var page = App.Current.MainPage as rootPage;
                        var announcementPage = new announcementPage();
                        page.changePage(announcementPage);
                    }
                }      
            }
        }
    }
}