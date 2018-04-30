using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HtmlAgilityPack;

namespace SEBeTender
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class tenderPage : ContentPage
	{
        private string nextUrl;
        private string previousUrl;
        private bool isPreviousAvailable = false;
        private bool isNextAvailable = false;
		public tenderPage ()
		{
            BindingContext = this;

            InitializeComponent ();
            
            //Set "Previous" and "Next" hyperlink label. 
            var previousLblTapRecognizer = new TapGestureRecognizer();
            previousLblTapRecognizer.Tapped += onPreviousPageTapped;
            previousPage.GestureRecognizers.Add(previousLblTapRecognizer);
            previousPage.IsVisible = false;  //"Previous" label is set to invisible for first page

            var nextLblTapRecognizer = new TapGestureRecognizer();
            nextLblTapRecognizer.Tapped += onNextPageTapped;
            nextPage.GestureRecognizers.Add(nextLblTapRecognizer);

            //Sending HTTP request to obtain the tender page data
            Task<string> httpTask = Task.Run<string>(() => getPageData("http://www2.sesco.com.my/etender/notice/notice.jsp"));
            var httpResult = httpTask.Result.ToString();
            //Task<string> httpTask = getPageData("http://www2.sesco.com.my/etender/notice/notice.jsp");
            //string httpTask = getPageData("http://www2.sesco.com.my/etender/notice/notice.jsp").Result;

            //var httpResult = httpTask.ToString();

            //Extract tender data from the response
            var tenders = DataExtraction.getWebData(httpResult, "tender");
            List<tenderItem> tenderItems = (List<tenderItem>)tenders;

            //Get bookmark details from database
            if (userSession.username != "")
            {
                Task<List<tenderBookmark>> bookmarkHttpTask = Task.Run<List<tenderBookmark>>(() => retrieveBookmark());
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
            //Save tender items into database 
            saveToTenderDb(tenderItems);

            listView.ItemsSource = tenderItems;
            listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.ItemSelected += onItemSelected;

            
        }

        async void saveToTenderDb(List<tenderItem> tenderItems)
        {
            Console.WriteLine("Testing save, is in the method now");
            dbTenderItem dbTenderItem = new dbTenderItem();
            foreach (tenderItem item in tenderItems)
            {
                dbTenderItem.Reference = item.Reference;
                dbTenderItem.Title = item.Title;
                dbTenderItem.OriginatingStation = item.OriginatingStation;
                dbTenderItem.ClosingDate = item.ClosingDate;
                dbTenderItem.BidClosingDate = item.BidClosingDate;
                dbTenderItem.FeeBeforeGST = item.FeeBeforeGST;
                dbTenderItem.FeeAfterGST = item.FeeAfterGST;
                dbTenderItem.FeeGST = item.FeeGST;
                dbTenderItem.TendererClass = item.TendererClass;
                dbTenderItem.Name = item.Name;
                dbTenderItem.OffinePhone = item.OffinePhone;
                dbTenderItem.Extension = item.Extension;
                dbTenderItem.MobilePhone = item.MobilePhone;
                dbTenderItem.Email = item.Email;
                dbTenderItem.Fax = item.Fax;
                //dbTenderItem.FileLinks = JsonConvert.SerializeObject(item.FileLinks).ToString();
                dbTenderItem.FileLinks = "";
                dbTenderItem.CheckedValue = item.CheckedValue;
                dbTenderItem.AddToCartQuantity = item.AddToCartQuantity;
                dbTenderItem.BookmarkImage = item.BookmarkImage;

                await App.Database.SaveTenderAsync(dbTenderItem);
            }
            Console.WriteLine("Save Process done!");
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

        async Task<string> getPageData(string url)
        {

            string httpTask = await Task.Run<string>(() => HttpRequestHandler.GetRequest(url, false));
            var httpResult = httpTask.ToString();

            //Small data extraction to get "Next" and "Previous" page hyperlinks
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(httpResult);
            var aNodes = htmlDoc.DocumentNode.SelectNodes("//a");
            isNextAvailable = false;
            isPreviousAvailable = false;

            if (aNodes != null)
            {
                foreach (var aNode in aNodes)
                {
                    if (aNode.InnerHtml == "Previous")
                    {
                        previousUrl = "http://www2.sesco.com.my/etender/notice/" + aNode.Attributes["href"].Value;
                        isPreviousAvailable = true;
                    }
                    else if (aNode.InnerHtml == "Next")
                    {
                        nextUrl = "http://www2.sesco.com.my/etender/notice/" + aNode.Attributes["href"].Value;
                        isNextAvailable = true;
                    }
                }
            }

            return httpResult;
        }

        async void onItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as tenderItem;
            
            if (item != null)
            {
                listView.SelectedItem = null;
                await Navigation.PushAsync(new tenderDetailPage(item));
            }
        }

        void onUpButtonClicked()
        {
            //Move to the top of the list when user click on the "Up" button
            var topItem = listView.ItemsSource.Cast<object>().FirstOrDefault();
            listView.ScrollTo(topItem, ScrollToPosition.Start, true);
        }

        async void onNextPageTapped(object sender, EventArgs eventArgs)
        {                       
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            //Sending HTTP request to obtain the second tender page data
            string httpTask = await Task.Run<string>(() => getPageData(nextUrl));
            //Task<string> httpTask = Task.Run<string>(() => getPageData(nextUrl));
            var httpResult = httpTask.ToString();
            //string httpTask = await Task.Run<string>(() => HttpRequestHandler.GetRequest(nextUrl, false));
            //var httpResult = httpTask.ToString();

            //Small data extraction to get "Next" and "Previous" page hyperlinks
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(httpResult);
            /*var aNodes = htmlDoc.DocumentNode.SelectNodes("//a");
            isNextAvailable = false;
            isPreviousAvailable = false;

            foreach (var aNode in aNodes)
            {
                if (aNode.InnerHtml == "Previous")
                {
                    previousUrl = "http://www2.sesco.com.my/etender/notice/" + aNode.Attributes["href"].Value;
                    isPreviousAvailable = true;
                }
                else if (aNode.InnerHtml == "Next")
                {
                    nextUrl = "http://www2.sesco.com.my/etender/notice/" + aNode.Attributes["href"].Value;
                    isNextAvailable = true;
                }
            }
            */
            //Extract tender data from the response
            var tenderItems = await Task.Run<Object>(() => DataExtraction.getTenderPage(htmlDoc));
            //var tenders = DataExtraction.getWebData(httpResult, "tender");
            //List<tenderItem> tenderItems = (List<tenderItem>)tenders;

            listView.ItemsSource = (List<tenderItem>)tenderItems;
            listView.ItemTemplate = dataTemplate;

            if (isPreviousAvailable)
            {
                previousPage.IsVisible = true;
            } else
            {
                previousPage.IsVisible = false;
            }

            if (isNextAvailable)
            {
                nextPage.IsVisible = true;
            }
            else
            {
                nextPage.IsVisible = false;
            }

            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;
        }

        async void onPreviousPageTapped(object sender, EventArgs eventArgs)
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;
            //Sending HTTP request to obtain the second tender page data
            string httpTask = await Task.Run<string>(() => getPageData(previousUrl));
            var httpResult = httpTask.ToString();

            //Small data extraction to get "Next" and "Previous" page hyperlinks
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(httpResult);

            //Extract tender data from the response
            var tenderItems = await Task.Run<Object>(() => DataExtraction.getTenderPage(htmlDoc));
            //var tenders = DataExtraction.getWebData(httpResult, "tender");
            //List<tenderItem> tenderItems = (List<tenderItem>)tenders;

            listView.ItemsSource = (List<tenderItem>)tenderItems;
            listView.ItemTemplate = dataTemplate;

            if (isPreviousAvailable)
            {
                previousPage.IsVisible = true;
            }
            else
            {
                previousPage.IsVisible = false;
            }

            if (isNextAvailable)
            {
                nextPage.IsVisible = true;
            }
            else
            {
                nextPage.IsVisible = false;
            }
            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;
        }

        async void OnBookmarkTapped(object sender, EventArgs eventArgs)
        {
            //check if user is logged in
            if (userSession.userLoginCookie == "")
            {
                DisplayAlert("Login required", "Please login first to bookmark this item.", "OK");
            } else
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

                    while (count <3 && httpResult != "Success")
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
    }
}