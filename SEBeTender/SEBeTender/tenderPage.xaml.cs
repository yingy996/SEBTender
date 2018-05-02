using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HtmlAgilityPack;
//ON NEXT PAGE, IF DATABASE TENDER SIMILAR, NO NEED RE-DISPLAY
namespace SEBeTender
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class tenderPage : ContentPage
	{
        private string nextUrl;
        private string dbNextUrl;
        private string previousUrl;
        private bool isPreviousAvailable = false;
        private bool isNextAvailable = false;
        private int Page = 1;
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
            Task<string> httpTask = Task.Run<string>(() => getPageData("http://www2.sesco.com.my/etender/notice/notice.jsp", false));
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

            //Show tenders from database first if exists, then replace if different with newest from SEBtender
            if(App.Database.getTendersAsync(1) != null)
            {
                listView.ItemsSource = Task.Run<List<tenderItem>>(() => retrieveTenderFromDatabase(1)).Result;
                deleteTenders(Task.Run<List<tenderItem>>(() => retrieveTenderFromDatabase(1)).Result);


            }


            


            //Save page 1 tenders to database
            saveToTenderDb(tenderItems, 1);

            //save >1 pages tenders to database
            storeAllTenders();

            listView.ItemsSource = Task.Run<List<tenderItem>>(() => retrieveTenderFromDatabase(1)).Result;
            
        }

        async void saveToTenderDb(List<tenderItem> tenderItems, int page)
        {
            List<dbTenderItem> dbTenderItems = new List<dbTenderItem>();
            
            foreach (tenderItem item in tenderItems)
            {
                dbTenderItem dbTenderItem = new dbTenderItem();
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
                dbTenderItem.Page = page;
                dbTenderItems.Add(dbTenderItem);
            }

            
            await App.Database.SaveTendersasync(dbTenderItems);
            Console.WriteLine("Save Process done!");
        }

        async Task<List<tenderItem>> retrieveTenderFromDatabase(int page)
        {
            List<tenderItem> tenderItems = new List<tenderItem>();
            List<dbTenderItem> dbTenderItems = Task.Run<List<dbTenderItem>>(() => App.Database.getTendersAsync(page)).Result;
            foreach (var item in dbTenderItems)
            {
                Dictionary<string, string> fileLinks = JsonConvert.DeserializeObject<Dictionary<string, string>>(item.FileLinks);
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

        async void deleteTenders(List<tenderItem> tenderItems)
        {
            foreach (tenderItem item in tenderItems)
            {
                dbTenderItem dbTenderItem = new dbTenderItem();

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
                dbTenderItem.Page = Page;
                await App.Database.DeleteTenderAsync(dbTenderItem);
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

        async Task<string> getPageData(string url, bool isDB)
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
                        if (isDB == true)
                        {
                            dbNextUrl = "http://www2.sesco.com.my/etender/notice/" + aNode.Attributes["href"].Value;
                        }
                        else
                        {
                            nextUrl = "http://www2.sesco.com.my/etender/notice/" + aNode.Attributes["href"].Value;
                            isNextAvailable = true;
                        }
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

        async void storeAllTenders()
        {
            //check if next tender page exists
            string httpTask = await Task.Run<string>(() => getPageData("http://www2.sesco.com.my/etender/notice/notice.jsp", true));
            int i = 1;
            while (isNextAvailable != false)
            {
                i = i + 1;

                //string httpTask = await Task.Run<string>(() => getPageData(nextUrl));
                var httpResult = httpTask.ToString();


                //Small data extraction to get "Next" and "Previous" page hyperlinks
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(httpResult);

                //Extract tender data from the response
                var tenderItems = await Task.Run<Object>(() => DataExtraction.getTenderPage(htmlDoc));

                saveToTenderDb((List<tenderItem>)tenderItems, i);

                //listView.ItemsSource = (List<tenderItem>)tenderItems;
                //listView.ItemTemplate = dataTemplate;

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

                httpTask = await Task.Run<string>(() => getPageData(nextUrl, true));
            }
        }

        async void deleteAllTenders()
        {
            App.Database.deleteAllTenders();
        }
        async void onNextPageTapped(object sender, EventArgs eventArgs)
        {
            Page = Page + 1;

            //Show tenders of next page from database first
            List<tenderItem> tenderItems = Task.Run<List<tenderItem>>(() => retrieveTenderFromDatabase(Page)).Result;
            listView.ItemsSource = tenderItems;
            listView.ItemTemplate = dataTemplate;

            //Delete tenders of next page from the database
            deleteTenders(tenderItems);

            //retrieve newest appropriate page tenders via web scraping
            string httpTask = await Task.Run<string>(() => getPageData(nextUrl, false));
            var httpResult = httpTask.ToString();
            
            //Small data extraction to get "Next" and "Previous" page hyperlinks
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(httpResult);
            
            //Extract tender data from the response
            var NewtenderItems = await Task.Run<Object>(() => DataExtraction.getTenderPage(htmlDoc));



            //Insert tenders of next page in the database
            saveToTenderDb((List<tenderItem>)NewtenderItems, Page);

            //Show updated tenders of next page from the database
            listView.ItemsSource = Task.Run<List<tenderItem>>(() => retrieveTenderFromDatabase(Page)).Result;
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

            
        }

        async void onPreviousPageTapped(object sender, EventArgs eventArgs)
        {
            Page = Page - 1;

            //Show tenders of previous page from database first
            List<tenderItem> tenderItems = Task.Run<List<tenderItem>>(() => retrieveTenderFromDatabase(Page)).Result;
            listView.ItemsSource = tenderItems;
            listView.ItemTemplate = dataTemplate;

            //Delete tenders of previous page from the database
            deleteTenders(tenderItems);

            //retrieve newest appropriate page tenders via web scraping
            string httpTask = await Task.Run<string>(() => getPageData(previousUrl, false));
            var httpResult = httpTask.ToString();

            //Small data extraction to get "Next" and "Previous" page hyperlinks
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(httpResult);

            //Extract tender data from the response
            var NewtenderItems = await Task.Run<Object>(() => DataExtraction.getTenderPage(htmlDoc));



            //Insert tenders of next page in the database
            saveToTenderDb((List<tenderItem>)NewtenderItems, Page);

            //Show updated tenders of next page from the database
            listView.ItemsSource = Task.Run<List<tenderItem>>(() => retrieveTenderFromDatabase(Page)).Result;
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