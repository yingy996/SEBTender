using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HtmlAgilityPack;

using Plugin.Connectivity;

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

            if(CrossConnectivity.Current.IsConnected == false)
            {
                DisplayAlert("No connection","Please ensure that you have an internet connection to receive the latest tenders.","Okay");
            }

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

            //Initialize list of dbtenders which is almost similar to tenderitem but stores file links in json instead of dictionary so that the tenders can be inserted into database
            List<dbtenderItem> dbTenders = new List<dbtenderItem>();
            dbTenders = Task.Run<List<dbtenderItem>>(() => saveTenderToDatabase(tenderItems, dbTenders)).Result;




            /*if (App.Database == null)
            {
                listView.ItemsSource = tenderItems;
            }
            else
            {*/
                listView.ItemsSource = Task.Run<List<tenderItem>>(() => retrieveTenderFromDatabase()).Result;
            //}
                
            listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.ItemSelected += onItemSelected;
        }

        async Task<List<dbtenderItem>> saveTenderToDatabase(List<tenderItem> tenderitems, List<dbtenderItem> dbtenderitems)
        {
            
            List<dbtenderItem> dbTenderItems = dbtenderitems;
            foreach (var item in tenderitems)
            {
                string jsonFileLink = JsonConvert.SerializeObject(item.FileLinks).ToString();
                dbtenderItem dbTender = new dbtenderItem();
                dbTender.Reference = item.Reference;
                dbTender.Title = item.Title;
                dbTender.OriginatingStation = item.OriginatingStation;
                dbTender.ClosingDate = item.ClosingDate;
                dbTender.BidClosingDate = item.BidClosingDate;
                dbTender.FeeBeforeGST = item.FeeBeforeGST;
                dbTender.FeeAfterGST = item.FeeAfterGST;
                dbTender.FeeGST = item.FeeGST;
                dbTender.TendererClass = item.TendererClass;
                dbTender.Name = item.Name;
                dbTender.OffinePhone = item.OffinePhone;
                dbTender.Extension = item.Extension;
                dbTender.MobilePhone = item.MobilePhone;
                dbTender.Email = item.Email;
                dbTender.Fax = item.Fax;
                dbTender.jsonfileLinks = jsonFileLink;
                dbTender.CheckedValue = item.CheckedValue;
                dbTender.AddToCartQuantity = item.AddToCartQuantity;
                dbTender.BookmarkImage = item.BookmarkImage;

                dbTenderItems.Add(dbTender);
            }

            App.Database.saveTendersAsync(dbTenderItems);

            return dbTenderItems;
        }

        async Task<List<tenderItem>> retrieveTenderFromDatabase()
        {
            List<tenderItem> tenderItems = new List<tenderItem>();
            List<dbtenderItem> dbTenderItems = Task.Run<List<dbtenderItem>>(() => App.Database.getTendersAsync()).Result;
            foreach (var item in dbTenderItems)
            {
                Dictionary<string, string> fileLinks = JsonConvert.DeserializeObject<Dictionary<string, string>>(item.jsonfileLinks);
                tenderItem tenderitem = new tenderItem();
                tenderitem.Reference = item.Reference;
                tenderitem.Title = item.Title;
                tenderitem.OriginatingStation = item.OriginatingStation;
                tenderitem.ClosingDate = item.ClosingDate;
                tenderitem.BidClosingDate = item.BidClosingDate;
                tenderitem.FeeBeforeGST = item.FeeBeforeGST;
                tenderitem.FeeAfterGST = item.FeeAfterGST;
                tenderitem.FeeGST = item.FeeGST;
                tenderitem.TendererClass = item.TendererClass;
                tenderitem.Name = item.Name;
                tenderitem.OffinePhone = item.OffinePhone;
                tenderitem.Extension = item.Extension;
                tenderitem.MobilePhone = item.MobilePhone;
                tenderitem.Email = item.Email;
                tenderitem.Fax = item.Fax;
                tenderitem.FileLinks = fileLinks;
                tenderitem.CheckedValue = item.CheckedValue;
                tenderitem.AddToCartQuantity = item.AddToCartQuantity;
                tenderitem.BookmarkImage = item.BookmarkImage;

                tenderItems.Add(tenderitem);
            }

            return tenderItems;
        }

        
        

        /*async Task<List<tenderItem>> getTendersFromDatabase()
        {
            return await App.Database.getTendersAsync();
        }*/

        

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