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
            previousPage.IsVisible = false;

            Console.WriteLine(HttpRequestHandler.getAnnouncementsResult());

            var items = Enumerable.Range(0, 10);


            Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.getAnnouncementsResult().Result);
            Console.WriteLine(httpTask.Result);

            List<RootObject> announcementItem = JsonConvert.DeserializeObject<List<RootObject>>(httpTask.Result);

            listView.ItemsSource = announcementItem;
            listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.ItemSelected += onItemSelected;
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

        public string announcementContent
        {
            get
            {
                if (announcementcontent.Length > 50)
                {
                    announcementcontent = announcementcontent.Substring(0, 50);
                    announcementcontent = string.Concat(announcementcontent, "...");
                    return announcementcontent;
                }
                else
                {
                    return announcementcontent;
                }
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