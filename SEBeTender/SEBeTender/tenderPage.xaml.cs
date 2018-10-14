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
        //store the url for next tender page
        private string nextUrl;

        //store the url for previous tender page
        private string previousUrl;

        //if true, then previous url available
        private bool isPreviousAvailable = false;

        //if true, then next url available
        private bool isNextAvailable = false;

        //store the current tender page which will be inserted into the tender database column
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
            nextPage.IsVisible = false;

            retrieveAndDisplayFirstPageTenders();

            listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.ItemSelected += onItemSelected;
        }

        public async void retrieveAndDisplayFirstPageTenders()
        {
            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;
            //Show tenders from database first if exists, then clear database to make way for new tenders
            List<tenderItem> dbtenders1 = await Task.Run<List<tenderItem>>(() => retrieveTenderFromDatabase(1));
            List<tenderItem> dbtenders2 = await Task.Run<List<tenderItem>>(() => retrieveTenderFromDatabase(2));
            if (dbtenders1.Count > 0)
            {
                if (userSession.username != "")
                {
                    List<tenderBookmark> bookmarkHttpTask = await Task.Run<List<tenderBookmark>>(() => retrieveBookmark());
                    List<tenderBookmark> tenderBookmarks = bookmarkHttpTask.ToList();
                    if (tenderBookmarks.Count > 0)
                    {
                        foreach (var tenderItem in dbtenders1)
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

                listView.ItemsSource = dbtenders1;
                if (dbtenders2.Count > 0)
                {
                    nextPage.IsVisible = true;
                }
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
                await WaitAndExecuteUpdateTenders(10000);
            }
            else
            {
                //Sending HTTP request to obtain the SEB tender page data
                //string httpTask = await Task.Run<string>(() => getPageData("http://www2.sesco.com.my/etender/notice/notice.jsp"));
                //var httpResult = httpTask;

                //Extract tender data from the response
                //var tenders = await DataExtraction.getWebData(httpResult, "tender");

                //Retrieve Telekom tenders
                //string httpTaskTelekom = await Task.Run<string>(() => getPageData("https://www.tm.com.my/DoingBusinessWithTM/pages/notices.aspx?Year=2018"));
                //var httpResultTelekom = httpTaskTelekom;
                //Console.WriteLine("TELEKOM: " + httpTaskTelekom);
                //Extract tender data from the response
                //var tendersTelekom = await DataExtraction.getWebData(httpResultTelekom, "telekom");

                //List<tenderItem> tenderItems = (List<tenderItem>)tendersTelekom;
                //tenderItems.AddRange((List<tenderItem>)tenders);
                //Get bookmark details from online database

                List<scrapped_tender> scrappedTenders = new List<scrapped_tender>();
                List<tenderItem> tenderItems = new List<tenderItem>();
                //Retrieve tenders from server
                string url = "https://pockettender.000webhostapp.com/process_getTenders.php";
                string httpTaskResult = await Task.Run<string>(() => HttpRequestHandler.GetRequest(url, false));

                if (httpTaskResult != null)
                {
                    if (httpTaskResult != "No tender found")
                    {
                        scrappedTenders = JsonConvert.DeserializeObject<List<scrapped_tender>>(httpTaskResult); 

                        if (scrappedTenders != null)
                        {
                            Console.WriteLine("Number of scrapped tenders: " + scrappedTenders.Count);
                            //Convert scrapped tender item into tender item
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

                                //{"bidCloseDate":"N\/A","feeBeforeGST":"RM 0.00","feeGST":"RM 0.00","feeAfterGST":"RM 0.00"}
                                //{ "name":"Francesca Lim","officePhone":"082-441188","extension":"1126","mobilePhone":null,"email":null,"fax":null}
                                //{"PMS106-14-Instruction.pdf":"http:\/\/www2.sesco.com.my\/noticeDoc\/PMS106-14-Instruction.pdf"}

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
                        }                      

                        if (userSession.username != "")
                        {
                            List<tenderBookmark> bookmarkHttpTask = await Task.Run<List<tenderBookmark>>(() => retrieveBookmark());
                            List<tenderBookmark> tenderBookmarks = bookmarkHttpTask.ToList();
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
                        Console.WriteLine("Num of tender items: " + tenderItems.Count);
                        listView.ItemsSource = tenderItems;
                    }
                }


                //Save page 1 tenders to database
                await saveToTenderDb(tenderItems, 1);

                //save subsequent page tenders to database;
                await storeAllTenders();

                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;

                await WaitAndExecuteUpdateTenders(10800000);
            }
        }

        public async Task WaitAndExecuteUpdateTenders(int milisec)
        {
            await Task.Delay(milisec);

            //Sending HTTP request to obtain the tender page data
            /*string httpTask = await Task.Run<string>(() => getPageData("http://www2.sesco.com.my/etender/notice/notice.jsp"));
            var httpResult = httpTask;

            //Extract tender data from the response
            var tenders = await DataExtraction.getWebData(httpResult, "tender");

            //Retrieve Telekom tenders
            string httpTaskTelekom = await Task.Run<string>(() => getPageData("https://www.tm.com.my/DoingBusinessWithTM/pages/notices.aspx?Year=2018"));
            var httpResultTelekom = httpTaskTelekom;
            //Console.WriteLine("TELEKOM:" + httpResultTelekom);
            //Extract tender data from the response
            var tendersTelekom = await DataExtraction.getWebData(httpResultTelekom, "telekom");

            List<tenderItem> tenderItems = (List<tenderItem>)tendersTelekom;
            tenderItems.AddRange((List<tenderItem>)tenders);
            //List<tenderItem> tenderItems = (List<tenderItem>)tenders;
            */

            List<scrapped_tender> scrappedTenders = new List<scrapped_tender>();
            List<tenderItem> tenderItems = new List<tenderItem>();
            //Retrieve tenders from server
            string url = "https://pockettender.000webhostapp.com/process_getTenders.php";
            string httpTaskResult = await Task.Run<string>(() => HttpRequestHandler.GetRequest(url, false));

            if (httpTaskResult != null)
            {
                if (httpTaskResult != "No tender found")
                {
                    scrappedTenders = JsonConvert.DeserializeObject<List<scrapped_tender>>(httpTaskResult);
                    
                    //Convert scrapped tender item into tender item
                    if (scrappedTenders != null)
                    {
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

                            //{"bidCloseDate":"N\/A","feeBeforeGST":"RM 0.00","feeGST":"RM 0.00","feeAfterGST":"RM 0.00"}
                            //{ "name":"Francesca Lim","officePhone":"082-441188","extension":"1126","mobilePhone":null,"email":null,"fax":null}
                            //{"PMS106-14-Instruction.pdf":"http:\/\/www2.sesco.com.my\/noticeDoc\/PMS106-14-Instruction.pdf"}

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
                    }                    

                    if (userSession.username != "")
                    {
                        List<tenderBookmark> bookmarkHttpTask = await Task.Run<List<tenderBookmark>>(() => retrieveBookmark());
                        List<tenderBookmark> tenderBookmarks = bookmarkHttpTask.ToList();
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

                    listView.ItemsSource = tenderItems;
                }
            }
            DisplayAlert("Update Tenders", "Updating tender listing. Please wait while the update is running...", "Okay");
            //Display the activity indicator to show activity running in the background 
            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;

            //Disable next page and previous page button to disallow user from navigating to other page while update is running
            previousPage.IsEnabled = false;
            nextPage.IsEnabled = false;

            //delete existing tenders from database
            deleteAllTenders();

            //Save page 1 tenders to database
            await saveToTenderDb(tenderItems, 1);

            //save subsequent page tenders to database;
            await storeAllTenders();

            List<tenderItem> dbtenders1 = await Task.Run<List<tenderItem>>(() => retrieveTenderFromDatabase(1));
            

            //Get bookmark details from online database
            if (userSession.username != "")
            {
                List<tenderBookmark> bookmarkHttpTask = await Task.Run<List<tenderBookmark>>(() => retrieveBookmark());
                List<tenderBookmark> tenderBookmarks = bookmarkHttpTask.ToList();
                if (tenderBookmarks.Count > 0)
                {
                    foreach (var tenderItem in dbtenders1)
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

            //await DisplayAlert("Update Tenders", "Refresh Tenders", "Okay");
            listView.ItemsSource = tenderItems;

            activityIndicator.IsRunning = false;
            activityIndicator.IsVisible = false;
            previousPage.IsEnabled = true;
            nextPage.IsEnabled = true;

            await WaitAndExecuteUpdateTenders(10800000);
        }

        async Task saveToTenderDb(List<tenderItem> tenderItems, int page)
        {
            List<dbTenderItem> dbTenderItems = new List<dbTenderItem>();
            
            foreach (tenderItem item in tenderItems)
            {
                dbTenderItem dbTenderItem = new dbTenderItem();
                dbTenderItem.Company = item.Company;
                dbTenderItem.Reference = item.Reference;
                dbTenderItem.TenderSource = item.TenderSource;
                dbTenderItem.Category = item.Category;
                dbTenderItem.Agency = item.Agency;
                dbTenderItem.Title = item.Title;
                dbTenderItem.OriginatingSource = item.OriginatingStation;
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
                dbTenderItem.FileLinks = JsonConvert.SerializeObject(item.FileLinks).ToString();
                //dbTenderItem.FileLinks = "";
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
            List<dbTenderItem> dbTenderItems = await Task.Run<List<dbTenderItem>>(() => App.Database.getTendersAsync(page));
            foreach (var item in dbTenderItems)
            {
                Dictionary<string, string> fileLinks = JsonConvert.DeserializeObject<Dictionary<string, string>>(item.FileLinks);
                tenderItem tenderitem = new tenderItem();
                tenderitem.Company = item.Company;
                tenderitem.Reference = item.Reference;
                tenderitem.TenderSource = item.TenderSource;
                tenderitem.Category = item.Category;
                tenderitem.Agency = item.Agency;
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

            return tenderItems;
        }

        /*async void deleteTenders(List<tenderItem> tenderItems)
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

        async Task storeAllTenders()
        {
            //denotes the tender page for use in this method
            int i = 1;

            while (isNextAvailable != false)
            {
                string httpTask = await Task.Run<string>(() => getPageData(nextUrl));

                i = i + 1;

                var httpResult = httpTask.ToString();

                //Small data extraction to get "Next" and "Previous" page hyperlinks
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(httpResult);

                //Extract tender data from the response
                var tenderItems = await Task.Run<Object>(() => DataExtraction.getTenderPage(htmlDoc));

                saveToTenderDb((List<tenderItem>)tenderItems, i);
            }

            List<tenderItem> dbtenders = await Task.Run<List<tenderItem>>(() => retrieveTenderFromDatabase(2));

            if (dbtenders.Count > 0)
            {
                nextPage.IsVisible = true;
            }
            //await WaitAndExecuteUpdateTenders(10800000, storeAllTenders);
        }

        async void deleteAllTenders()
        {
            App.Database.deleteAllTenders();
        }

        async void onNextPageTapped(object sender, EventArgs eventArgs)
        {
            Page = Page + 1;
            nextPage.IsEnabled = false;
            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;
            //Show tenders of next page from database first
            List<tenderItem> tenderItems = await Task.Run<List<tenderItem>>(() => retrieveTenderFromDatabase(Page));

            if (userSession.username != "")
            {
                List<tenderBookmark> bookmarkHttpTask = await Task.Run<List<tenderBookmark>>(() => retrieveBookmark());
                List<tenderBookmark> tenderBookmarks = bookmarkHttpTask.ToList();
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

            listView.ItemsSource = tenderItems;
            listView.ItemTemplate = dataTemplate;
            
            /*//Delete tenders of next page from the database
            deleteTenders(tenderItems);

            //retrieve newest appropriate page tenders via web scraping
            string httpTask = await Task.Run<string>(() => getPageData(nextUrl));
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
            */

            List<tenderItem> dbprevious = await Task.Run<List<tenderItem>>(() => retrieveTenderFromDatabase(Page - 1));
            List<tenderItem> dbnext = await Task.Run<List<tenderItem>>(() => retrieveTenderFromDatabase(Page + 1));
            if (dbprevious.Count > 0)
            {
                previousPage.IsVisible = true;
            } else
            {
                previousPage.IsVisible = false;
            }

            if (dbnext.Count > 0)
            {
                nextPage.IsVisible = true;
            }
            else
            {
                nextPage.IsVisible = false;
            }
            nextPage.IsEnabled = true;
            activityIndicator.IsRunning = false;
            activityIndicator.IsVisible = false;
        }

        async void onPreviousPageTapped(object sender, EventArgs eventArgs)
        {
            if (Page > 1)
            {
                Page = Page - 1;
            }
            previousPage.IsEnabled = false;
            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;
            //Show tenders of previous page from database first
            List<tenderItem> tenderItems = await Task.Run<List<tenderItem>>(() => retrieveTenderFromDatabase(Page));

            if (userSession.username != "")
            {
                List<tenderBookmark> bookmarkHttpTask = await Task.Run<List<tenderBookmark>>(() => retrieveBookmark());
                List<tenderBookmark> tenderBookmarks = bookmarkHttpTask.ToList();
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

            listView.ItemsSource = tenderItems;
            listView.ItemTemplate = dataTemplate;

            /*//Delete tenders of previous page from the database
            deleteTenders(tenderItems);

            //retrieve newest appropriate page tenders via web scraping
            string httpTask = await Task.Run<string>(() => getPageData(previousUrl));
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
            */

            List<tenderItem> dbprevious = await Task.Run<List<tenderItem>>(() => retrieveTenderFromDatabase(Page - 1));
            List<tenderItem> dbnext = await Task.Run<List<tenderItem>>(() => retrieveTenderFromDatabase(Page + 1));

            if (dbprevious.Count > 0)
            {
                
                previousPage.IsVisible = true;
            }
            else
            {
                previousPage.IsVisible = false;
            }

            if (dbnext.Count > 0)
            {
                nextPage.IsVisible = true;
            }
            else
            {
                nextPage.IsVisible = false;
            }

            //If current page is page 1, hide previous page button
            if (Page == 1)
            {
                previousPage.IsVisible = false;
            }
            previousPage.IsEnabled = true;
            activityIndicator.IsRunning = false;
            activityIndicator.IsVisible = false;
        }

        async void OnBookmarkTapped(object sender, EventArgs eventArgs)
        {
            //check if user is logged in
            if (userSession.username == "")
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