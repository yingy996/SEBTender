using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;

namespace SEBeTender
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class customSearchPage : ContentPage
	{
		public customSearchPage ()
		{
            BindingContext = this;
            InitializeComponent();

            if (userSession.username != "")
            {
                retrieveCustomSearches();
            }
            else
            {
                errorMsg.Text = "Please login to view custom searches";
                errorMsg.IsVisible = true;
            }

            listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.ItemSelected += onItemSelected;
        }

        async void retrieveCustomSearches()
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;
            string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostCustomSearches(userSession.username));
            var httpResult = httpTask;
            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;
            pageTitle.IsVisible = true;
            if (httpResult != null)
            {
                if (httpResult != "No custom searches found")
                {
                    List<customSearchesItem> customerSearches = JsonConvert.DeserializeObject<List<customSearchesItem>>(httpResult);
                    listView.ItemsSource = customerSearches;
                    upBtn.IsVisible = true;
                }
                else
                {
                    errorMsg.IsVisible = true;
                }

            }
            else
            {
                Console.WriteLine("search is not null ");
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
            var item = e.SelectedItem as customSearchesItem;

            if (item != null)
            {
                listView.SelectedItem = null;
                await Navigation.PushAsync(new searchTenderPage(item));
            }
        }

        async void onDeleteTapped(object sender, EventArgs args)
        {
            var customSearchesSelected = ((TappedEventArgs)args).Parameter;
            customSearchesItem customSearches = (customSearchesItem)customSearchesSelected;
            List<customSearchesItem> tempcustomSearches = (List<customSearchesItem>)listView.ItemsSource;

            var answer = await DisplayAlert("Remove Favourite Search", "Are you sure you want to remove favourite search '" + customSearches.identifier + "'?", "YES", "NO");

            if (answer)
            {
                //remove bookmark from listview
                foreach (var searchesItem in tempcustomSearches.ToList())
                {
                    if (searchesItem == customSearches)
                    {
                        int index = tempcustomSearches.IndexOf(searchesItem);
                        tempcustomSearches.Remove(searchesItem);
                    }
                }

                //Refresh listview
                listView.ItemsSource = tempcustomSearches.ToList();

                //Display error message when there are no bookmark
                if (tempcustomSearches.Count <= 0)
                {
                    errorMsg.Text = "No favourite searches found.";
                    errorMsg.IsVisible = true;
                    upBtn.IsVisible = false;
                }

                //Remove bookmark from database
                string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostManageSearchBookmark(customSearches.searchID, customSearches.tenderReference, customSearches.tenderTitle, customSearches.originatingSource, customSearches.closingDateFrom, customSearches.closingDateTo, userSession.username, customSearches.identifier, "delete"));
                Console.WriteLine(customSearches.identifier);
                var httpResult = httpTask.ToString();
                Console.WriteLine(httpResult);
                int count = 0;

                while (count < 3 && httpResult != "Success")
                {
                    Console.WriteLine("Looping for failure delete");
                    httpTask = await Task.Run<string>(() => HttpRequestHandler.PostManageSearchBookmark(customSearches.searchID, customSearches.tenderReference, customSearches.tenderTitle, customSearches.originatingSource, customSearches.closingDateFrom, customSearches.closingDateTo, userSession.username, customSearches.identifier, "delete"));
                    httpResult = httpTask.ToString();
                    count++;
                }


            }

        }
    }
}