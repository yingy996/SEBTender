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
            Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.PostadminloginCheck(username, password).Result);
            var httpResult = httpTask.Result;

            //Console.WriteLine(httpResult);

            if (httpResult == "loggedin")
            {
                editButton.IsVisible = true;
                deleteButton.IsVisible = true;

                deleteButton.Clicked += OnButtonClicked;
            }

            announcementTitlelbl.Text = announcementItem.announcementTitle;
            announcementContentlbl.Text = announcementItem.announcementContent;
            publishedDatelbl.Text = announcementItem.publishedDate;
            postedBylbl.Text = announcementItem.postedBy;
            editedDatelbl.Text = announcementItem.editedDate.ToString();
            editedBylbl.Text = announcementItem.editedBy.ToString();

            announcementid = announcementItem.announcementID;

        }

        void OnButtonClicked(object sender, EventArgs e)
        {
            //Send HTTP request to check user exists
            Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.deleteAnnouncement(announcementid).Result);
            var httpResult = httpTask.Result;

            if(httpResult == "deletesuccess")
            {
                Console.WriteLine("CHANGE TO ANNOUNCEMENT LIST PAGE UHUH LOOK AT MY DANCE, I GOT A NICE PIGU");
            }

        }
    }
}