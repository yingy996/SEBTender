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
            string username = "", password = "";
            editButton.IsVisible = false;
            deleteButton.IsVisible = false;
            
            username = adminAuth.Username;
            password = adminAuth.Password;

            //Send HTTP request to check user exists
            Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.PostadminloginCheck(username, password));
            var httpResult = httpTask.Result;

            //Console.WriteLine(httpResult);

            if (httpResult == "loggedin")
            {
                editButton.IsVisible = true;
                deleteButton.IsVisible = true;

                deleteButton.Clicked += OnDeleteButtonClicked;
            }

            announcementTitlelbl.Text = announcementItem.announcementTitle;
            announcementContentlbl.Text = announcementItem.announcementContent;
            publishedDatelbl.Text = announcementItem.publishedDate;
            postedBylbl.Text = announcementItem.postedBy;
            editedDatelbl.Text = announcementItem.editedDate.ToString();
            editedBylbl.Text = announcementItem.editedBy.ToString();

            announcementid = announcementItem.announcementID;

        }

        async void OnEditButtonClicked(object sender, EventArgs e)
        {
            if (announcementid == "")
            {
                Console.WriteLine(announcementid + "invalid");
            }
            else
            {
                var page = App.Current.MainPage as rootPage;
                var editAnnouncementPage = new editAnnouncement(announcementid);
                page.changePage(editAnnouncementPage);
            }

        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            string username = "";
            username = adminAuth.Username;
            //Send HTTP request to check user exists
            Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.deleteAnnouncement(announcementid, username));
            var httpResult = httpTask.Result;

            if(httpResult == "deletesuccess")
            {

                var answer = await DisplayAlert("Exit", "Are you sure you want to delete the announcement?", "Yes", "No");
                if (answer)
                {
                    var page = App.Current.MainPage as rootPage;
                    var announcementPage = new announcementPage();
                    page.changePage(announcementPage);
                }
            }

        }
    }
}