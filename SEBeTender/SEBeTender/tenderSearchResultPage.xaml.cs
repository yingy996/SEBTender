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
	public partial class tenderSearchResultPage : ContentPage
	{
        private string nextUrl;
        private string previousUrl;
        private bool isPreviousAvailable = false;
        private bool isNextAvailable = false;

        public tenderSearchResultPage ()
		{

        }

        public tenderSearchResultPage(string searchTenderResult, List<dbTenderItem> dbTenderItems)
        {
            //BindingContext = this;
            Console.WriteLine(searchTenderResult);
            var label = new Label { Text = "text" };
            //StackLayout stackLayout = new StackLayout();
            //var childToRaise = stackLayout.Children.First();

            InitializeComponent();

            //Set "Previous" and "Next" hyperlink label. 
            /*var previousLblTapRecognizer = new TapGestureRecognizer();
            previousLblTapRecognizer.Tapped += onPreviousPageTapped;
            previousPage.GestureRecognizers.Add(previousLblTapRecognizer);*/
            previousPage.IsVisible = false;  //"Previous" label is set to invisible for first page

            /*var nextLblTapRecognizer = new TapGestureRecognizer();
            nextLblTapRecognizer.Tapped += onNextPageTapped;
            nextPage.GestureRecognizers.Add(nextLblTapRecognizer);*/
            nextPage.IsVisible = false;

            if(searchTenderResult == "Search Local Database")
            {
                displayKeywordResult(dbTenderItems);
            }
            else
            {
                displayNormalSearch(searchTenderResult);
            }
        }

        async void displayNormalSearch(string httpTaskResult)
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            List<scrapped_tender> scrappedTenders = new List<scrapped_tender>();
            List<tenderItem> tenderItems = new List<tenderItem>();
            
            scrappedTenders = JsonConvert.DeserializeObject<List<scrapped_tender>>(httpTaskResult);
            foreach (scrapped_tender scrappedTender in scrappedTenders)
            {
                tenderItem tender = new tenderItem();
                tender.Company = scrappedTender.originatingSource;
                tender.TenderSource = scrappedTender.tenderSource;
                tender.Reference = scrappedTender.reference;
                tender.Agency = scrappedTender.agency;
                tender.Title = scrappedTender.title;
                tender.Category = scrappedTender.category;
                tender.OriginatingStation = scrappedTender.originatingSource;
                tender.ClosingDate = scrappedTender.closingDate;

                if (scrappedTender.docInfoJson != null)
                {
                    dynamic docInfo = JsonConvert.DeserializeObject(scrappedTender.docInfoJson);
                    if (docInfo.bidCloseDate != null)
                    {
                        tender.BidClosingDate = docInfo.bidCloseDate;
                    }

                    if (docInfo.feeBeforeGST != null)
                    {
                        tender.FeeBeforeGST = docInfo.feeBeforeGST;
                    }

                    if (docInfo.feeGST != null)
                    {
                        tender.FeeGST = docInfo.feeGST;
                    }

                    if (docInfo.feeAfterGST != null)
                    {
                        tender.FeeAfterGST = docInfo.feeAfterGST;
                    }
                }

                if (scrappedTender.originatorJson != null)
                {
                    dynamic originatorInfo = JsonConvert.DeserializeObject(scrappedTender.originatorJson);
                    if (originatorInfo.name != null)
                    {
                        tender.Name = originatorInfo.name;
                    }

                    if (originatorInfo.officePhone != null)
                    {
                        tender.OffinePhone = originatorInfo.officePhone;
                    }

                    if (originatorInfo.extension != null)
                    {
                        tender.Extension = originatorInfo.extension;
                    }

                    if (originatorInfo.mobilePhone != null)
                    {
                        tender.MobilePhone = originatorInfo.mobilePhone;
                    }

                    if (originatorInfo.email != null)
                    {
                        tender.Email = originatorInfo.email;
                    }

                    if (originatorInfo.fax != null)
                    {
                        tender.Fax = originatorInfo.fax;
                    }
                }

                if (scrappedTender.fileLinks != null)
                {
                    Dictionary<string, string> fileLinks = JsonConvert.DeserializeObject<Dictionary<string, string>>(scrappedTender.fileLinks);
                    tender.FileLinks = fileLinks;
                    //{"Folder 1.zip":"http:\/\/www2.sesco.com.my\/noticeDoc\/Folder 1.zip","Folder 2.zip":"http:\/\/www2.sesco.com.my\/noticeDoc\/Folder 2.zip","Folder 3.zip":"http:\/\/www2.sesco.com.my\/noticeDoc\/Folder 3.zip","Folder 4.zip":"http:\/\/www2.sesco.com.my\/noticeDoc\/Folder 4.zip"}
                }
                tenderItems.Add(tender);
            }

            pageTitle.IsVisible = true;
            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;
            if (tenderItems.Count > 0)
            {
                upBtn.IsVisible = true;
            }
            else
            {
                errorMsg.IsVisible = true;
            }

            listView.ItemsSource = tenderItems;
            listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.ItemSelected += onItemSelected;
        }

        async void displayKeywordResult(List<dbTenderItem> dbTenderItems)
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            
            List<tenderItem> tenderItems = new List<tenderItem>();
            foreach (var item in dbTenderItems)
            {
                Dictionary<string, string> fileLinks = JsonConvert.DeserializeObject<Dictionary<string, string>>(item.FileLinks);
                tenderItem tenderitem = new tenderItem();
                tenderitem.Reference = item.Reference;
                tenderitem.Title = item.Title;
                tenderitem.OriginatingStation = item.OriginatingSource;
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

            //Get bookmark details from database
            if (userSession.username != "")
            {
                //string httpTask = await Task.Run<string>(() => HttpRequestHandler.GetRequest("http://www2.sesco.com.my/etender/vendor/vendor_tender_eligible.jsp", true));
                //Task<List<tenderBookmark>> bookmarkHttpTask = Task.Run<List<tenderBookmark>>(() => retrieveBookmark());
                List<tenderBookmark> tenderBookmarks = await retrieveBookmark();
                if (tenderBookmarks != null)
                {
                    //List<tenderBookmark> tenderBookmarks = bookmarkHttpTask;
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

            pageTitle.IsVisible = true;
            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;
            if (tenderItems.Count > 0)
            {
                upBtn.IsVisible = true;
            } else
            {
                errorMsg.IsVisible = true;
            }

            listView.ItemsSource = tenderItems;
            listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.ItemSelected += onItemSelected;
        }

        async Task<List<tenderBookmark>> retrieveBookmark()
        {
            string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostTenderBookmark(userSession.username));
            var httpResult = httpTask;
            List<tenderBookmark> tenderBookmarks = new List<tenderBookmark>();

            while (httpResult == null)
            {
                httpTask = await Task.Run<string>(() => HttpRequestHandler.PostTenderBookmark(userSession.username));
                httpResult = httpTask;
            }
            Console.WriteLine("as");

            if (httpResult != null)
            {
                if (httpResult != "No bookmark found")
                {
                    tenderBookmarks = JsonConvert.DeserializeObject<List<tenderBookmark>>(httpResult);
                    return tenderBookmarks;
                }
                else
                {
                    return tenderBookmarks;
                }
            }
            return null;
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

        void onUpButtonClicked()
        {
            //Move to the top of the list when user click on the "Up" button
            var topItem = listView.ItemsSource.Cast<object>().FirstOrDefault();
            listView.ScrollTo(topItem, ScrollToPosition.Start, true);
        }

        /*async void onNextPageTapped(object sender, EventArgs eventArgs)
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;
            //Sending HTTP request to obtain the second tender page data
            string httpTask = await Task.Run<string>(() => HttpRequestHandler.GetRequest(nextUrl, false));
            var httpResult = httpTask.ToString();

            //Small data extraction to get "Next" and "Previous" page hyperlinks
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(httpResult);
            var aNodes = htmlDoc.DocumentNode.SelectNodes("//a");
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

        async void onPreviousPageTapped(object sender, EventArgs eventArgs)
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;
            //Sending HTTP request to obtain the second tender page data
            string httpTask = await Task.Run<string>(() => HttpRequestHandler.GetRequest(previousUrl, false));
            var httpResult = httpTask.ToString();

            //Small data extraction to get "Next" and "Previous" page hyperlinks
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(httpResult);
            var aNodes = htmlDoc.DocumentNode.SelectNodes("//a");
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
        }*/
    }
}

