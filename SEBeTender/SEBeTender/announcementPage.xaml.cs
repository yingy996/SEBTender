using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//using JsonSystemAlias = System.Json;
//using Xamarin.Auth;
using Newtonsoft.Json;

namespace SEBeTender
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class announcementPage : ContentPage
    {
        public announcementPage()
        {
            BindingContext = this;
            InitializeComponent();
            //previousPage.IsVisible = false;

            //Console.WriteLine(HttpRequestHandler.getAnnouncementsResult());
            
            retrieveAnnouncement();
            
            listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.ItemSelected += onItemSelected;
            
        }

        async void retrieveAnnouncement()
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            //Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.getAnnouncementsResult().Result);
            string httpTask = await Task.Run<string>(() => HttpRequestHandler.getAnnouncementsResult());
            while (httpTask == null)
            {
                httpTask = await Task.Run<string>(() => HttpRequestHandler.getAnnouncementsResult());
            }
            //var httpResult = httpTask;

            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;
            pageTitle.IsVisible = true;

            if (httpTask != null)
            {
                List<RootObject> announcementItem = JsonConvert.DeserializeObject<List<RootObject>>(httpTask.ToString());

                //If announcement is available, display it in the listview, else display the error message
                if (announcementItem != null)
                {
                    if (announcementItem.Count > 0)
                    {
                        Console.WriteLine("Announcement is not null");
                        listView.ItemsSource = announcementItem;
                        
                        upBtn.IsVisible = true;
                    } else
                    {
                        errorMsg.IsVisible = true;
                    }
                } else
                {
                    errorMsg.IsVisible = true;
                }
                
            }
            else
            {
                Console.WriteLine("Announcement Task is null ");
                errorMsg.IsVisible = true;
            }
            if (Settings.Username != "" && (Settings.Role == "admin" || Settings.Role == "editor"))
            {
                if (String.IsNullOrEmpty(adminAuth.Username))
                {
                    adminAuth.saveCredentials(Settings.Username, Settings.Password);
                }
                
            }
            Console.WriteLine("Announcement page: " + adminAuth.Username);
        }

        void onUpButtonClicked()
        {
            //Move to the top of the list when user click on the "Up" button
            var topItem = listView.ItemsSource.Cast<object>().FirstOrDefault();
            listView.ScrollTo(topItem, ScrollToPosition.Start, true);
        }

        async void onItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            listView.SelectedItem = null;
            var item = e.SelectedItem as RootObject;

            if (item != null)
            {
                listView.SelectedItem = null;
                await Navigation.PushAsync(new announcementDetailPage(item));
            }
        }
        
    }

    public class RootObject
    {
        private string announcementcontent = "1";
        private string datePosted = "1";
        private string editeddate = "1";
        private string editedby = "1";

        public string announcementID { get; set; }

        public string announcementTitle { get; set; }

        public string announcementContentShort
        {
            get
            {
                if (announcementcontent.Length > 60)
                {
                    string tempContent = announcementcontent.Substring(0, 60);
                    tempContent = string.Concat(tempContent, "...");
                    return tempContent;
                }
                else
                {
                    return announcementcontent;
                }
            }
            set { announcementcontent = value; }
        }

        public string announcementContent
        {
            get
            {
                return announcementcontent;
            }
            set { announcementcontent = value; }
        }

        public string publishedDate
        {
            get
            {
                //datePosted = string.Concat("Posted on: ", datePosted);
                //return "Date Posted: " + datePosted;
                return datePosted;
            }

            set { datePosted = value; }
        }

        public string editedDate
        {
            get
            {
                if (editeddate == null)
                {
                    editeddate = "";
                    return editeddate;
                }
                else
                {
                    return editeddate;
                }
            }

            set { editeddate = value; }
        }

        public string editedBy
        {
            get
            {
                if (editedby == null)
                {
                    editedby = "";
                    return editedby;
                }
                else
                {
                    return editedby;
                }
            }

            set { editedby = value; }
        }

        public string postedBy { get; set; }

    }
}