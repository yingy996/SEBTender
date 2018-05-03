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
	public partial class tenderEligiblePage : ContentPage
	{
		public tenderEligiblePage ()
		{
            BindingContext = this;
            InitializeComponent ();
            var items = Enumerable.Range(0, 10);

            //Sending HTTP request to obtain the tender page data
            Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.GetRequest("http://www2.sesco.com.my/etender/vendor/vendor_tender_eligible.jsp", true));
            var httpResult = httpTask.Result.ToString();
            
            //Extract tender data from the response
            var tenders = DataExtraction.getWebData(httpResult, "eligibelTenderPage");
            List<tenderItem> tenderItems = (List<tenderItem>)tenders;

            //Get bookmark details from database
            if (userSession.username != "")
            {
                Task<List<tenderBookmark>> bookmarkHttpTask = Task.Run<List<tenderBookmark>>(() => retrieveBookmark());
                if (bookmarkHttpTask.Result != null)
                {
                    List<tenderBookmark> tenderBookmarks = bookmarkHttpTask.Result.ToList();
                    if (tenderBookmarks.Count > 0)
                    {
                        foreach (var tenderItem in tenderItems)
                        {
                            foreach (var tenderBookmark in tenderBookmarks)
                            {
                                if (tenderItem.Reference == tenderBookmark.tenderReferenceNumber)
                                {
                                    tenderItem.BookmarkImage = "bookmarkfilled.png";
                                    break;
                                }
                            }
                        }

                    }
                }
                
            }

            listView.ItemsSource = tenderItems;
            listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.ItemSelected += onItemSelected;
            
        }

        async void onItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
            var item = e.SelectedItem as tenderItem;
            
            if (item != null)
            {
                listView.SelectedItem = null;
                //await Navigation.PushAsync(new tenderDetailPage(item));
            }
        }

        async Task<List<tenderBookmark>> retrieveBookmark()
        {
            string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostTenderBookmark(userSession.username));
            var httpResult = httpTask;
            List<tenderBookmark> tenderBookmarks = new List<tenderBookmark>();
            if (httpResult != null)
            {
                if (httpResult != "No bookmark found")
                {
                    tenderBookmarks = JsonConvert.DeserializeObject<List<tenderBookmark>>(httpResult);
                }
            }
            return tenderBookmarks;
        }

        void onUpButtonClicked()
        {
            //Move to the top of the list when user click on the "Up" button
            var topItem = listView.ItemsSource.Cast<object>().FirstOrDefault();
            listView.ScrollTo(topItem, ScrollToPosition.Start, true);
        }

        void OnCartTapped(object sender, EventArgs args)
        {
            var tenderSelected = ((TappedEventArgs)args).Parameter;
            tenderItem tender = (tenderItem)tenderSelected;

            DisplayAlert("Success", tender.AddToCartQuantity + " Item " + tender.Reference + " has been successfully added to cart ", "OK");
        }

        async void OnBookmarkTapped(object sender, EventArgs eventArgs)
        {
            //check if user is logged in
            if (userSession.userLoginCookie == "")
            {
                DisplayAlert("Login required", "Please login first to bookmark this item.", "OK");
            }
            else
            {
                var tenderSelected = ((TappedEventArgs)eventArgs).Parameter;
                tenderItem tender = (tenderItem)tenderSelected;
                //var image = sender as Image;
                Console.WriteLine("Image now is: " + ((Image)sender).Source.ToString());

                if (((Image)sender).Source.ToString() == "File: bookmarkfilled.png")
                {
                    ((Image)sender).Source = "bookmark.png";
                    DisplayAlert("Cancel bookmark", "Tender '" + tender.Reference + "' has been removed from bookmark!", "OK");

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
                else
                {
                    ((Image)sender).Source = "bookmarkfilled.png";
                    DisplayAlert("Add bookmark", "Tender '" + tender.Reference + "' has been successfully added to bookmark!", "OK");

                    string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostManageTenderBookmark(userSession.username, tender, "add"));
                    var httpResult = httpTask.ToString();
                    Console.WriteLine(httpResult);
                    int count = 0;

                    while (count < 3 && httpResult != "Success")
                    {
                        Console.WriteLine("Looping for failure add");
                        httpTask = await Task.Run<string>(() => HttpRequestHandler.PostManageTenderBookmark(userSession.username, tender, "add"));
                        httpResult = httpTask.ToString();
                        count++;
                    }
                }
            }


            //Display tender list with or without bookmark

            //send request to database everyone user tap on bookmark 

        }

        void onQuantityChanged(object sender, EventArgs args)
        {
            
            var entry = sender as Entry;
            var input = entry.Text;
            tenderItem tender = (tenderItem)entry.BindingContext;

            Console.WriteLine(tender);
            if (tender != null)
            {
                ((tenderItem)entry.BindingContext).AddToCartQuantity = input;

            } else
            {
                Console.WriteLine("It's is null!");
            }

        }
    }
}