using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SEBeTender
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class tenderBookmarkPage : ContentPage
	{
		public tenderBookmarkPage ()
		{
            BindingContext = this;
            InitializeComponent ();

            var items = Enumerable.Range(0, 10);
            //listView.ItemsSource = items;


            if (userSession.username != "") {
                retrieveBookmark();
            } else
            {
                errorMsg.Text = "Please login to view bookmark";
                errorMsg.IsVisible = true;
            }


            listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.ItemSelected += onItemSelected;
        }

        async void retrieveBookmark()
        {         
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;
            string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostTenderBookmark(userSession.username));
            var httpResult = httpTask;
            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;
            pageTitle.IsVisible = true;
            if (httpResult != null)
            {
                if (httpResult != "No bookmark found")
                {
                    List<tenderBookmark> tenderBookmarks = JsonConvert.DeserializeObject<List<tenderBookmark>>(httpResult);
                    listView.ItemsSource = tenderBookmarks;
                    upBtn.IsVisible = true;
                } else
                {
                    errorMsg.IsVisible = true;
                }
                
            } else
            {
                Console.WriteLine("bookmark is not null ");
                errorMsg.IsVisible = true;
            }
            
        }

        void onUpButtonClicked()
        {
            //Move to the top of the list when user click on the "Up" button
            var topItem = listView.ItemsSource.Cast<object>().FirstOrDefault();
            listView.ScrollTo(topItem, ScrollToPosition.Start, true);
        }

        async void onItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as tenderBookmark;

            if (item != null)
            {
                listView.SelectedItem = null;
                await Navigation.PushAsync(new tenderDetailPage(item));
            }
        }

        async void onDeleteTapped(object sender, EventArgs args)
        {
            var tenderSelected = ((TappedEventArgs)args).Parameter;
            tenderBookmark tenderBookmark = (tenderBookmark)tenderSelected;
            tenderItem tender = new tenderItem();
            tender.Reference = tenderBookmark.tenderReferenceNumber;
            List<tenderBookmark> tempBookmarkList = (List<tenderBookmark>)listView.ItemsSource;

            var answer = await DisplayAlert("Remove bookmark", "Are you sure you want to remove bookmark '" + tender.Reference + "'?", "YES", "NO");

            if (answer)
            {
                //remove bookmark from listview
                foreach (var bookmarkItem in tempBookmarkList.ToList())
                {
                    if (bookmarkItem == tenderBookmark)
                    {
                        int index = tempBookmarkList.IndexOf(bookmarkItem);
                        tempBookmarkList.Remove(bookmarkItem);
                    }
                }

                //Refresh listview
                listView.ItemsSource = tempBookmarkList.ToList();

                //Display error message when there are no bookmark
                if (tempBookmarkList.Count <= 0)
                {
                    errorMsg.Text = "No bookmark found.";
                    errorMsg.IsVisible = true;
                    upBtn.IsVisible = false;
                }

                //Remove bookmark from database
                string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostManageTenderBookmark(userSession.username, tender, "delete"));
                var httpResult = httpTask.ToString();
                Console.WriteLine(httpResult);
                int count = 0;

                while (count < 3 && httpResult != "Success")
                {
                    Console.WriteLine("Looping for failure delete");
                    httpTask = await Task.Run<string>(() => HttpRequestHandler.PostManageTenderBookmark(userSession.username, tender, "delete"));
                    httpResult = httpTask.ToString();
                    count++;
                }

                
            }
            
        }
    } 
}