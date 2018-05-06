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
            

            //Sending HTTP request to obtain the tender page data

            //Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.GetRequest("http://www2.sesco.com.my/etender/vendor/vendor_tender_eligible.jsp", true));
            //var httpResult = httpTask.Result.ToString();

            retrieveEligibleTenders();
            
            listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.ItemSelected += onItemSelected;
            
        }

        async void retrieveEligibleTenders()
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            //Show tenders from database first if exists, then clear database to make way for new tenders
            List<tenderItem> dbtenders0 = await Task.Run<List<tenderItem>>(() => retrieveTenderFromDatabase(0));

            if (dbtenders0.Count > 0)
            {
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
                            foreach (var tenderItem in dbtenders0)
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

                listView.ItemsSource = dbtenders0;
                pageTitle.IsVisible = true;
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
                upBtn.IsVisible = true;


                await WaitAndExecuteUpdateTenders(10000);
            }
            else
            {
                //Sending HTTP request to obtain the tender page data
                string httpTask = await Task.Run<string>(() => HttpRequestHandler.GetRequest("http://www2.sesco.com.my/etender/vendor/vendor_tender_eligible.jsp", true));
                var httpResult = httpTask;

                //Extract tender data from the response
                var tenders = await DataExtraction.getWebData(httpResult, "eligibelTenderPage");
                List<tenderItem> tenderItems = (List<tenderItem>)tenders;


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
                listView.ItemsSource = tenderItems;

                //save all eligible tenders to database
                await saveToTenderDb(tenderItems, 0);
                pageTitle.IsVisible = true;
                activityIndicator.IsVisible = false;
                activityIndicator.IsRunning = false;
                //pageTitle.IsVisible = true;
                

                if (tenderItems.Count > 0)
                {
                    upBtn.IsVisible = true;
                }

                await WaitAndExecuteUpdateTenders(10800000);
            }
        }

        public async Task WaitAndExecuteUpdateTenders(int milisec)
        {
            await Task.Delay(milisec);
            

            //Sending HTTP request to obtain the tender page data
            string httpTask = await Task.Run<string>(() => HttpRequestHandler.GetRequest("http://www2.sesco.com.my/etender/vendor/vendor_tender_eligible.jsp", true));
            var httpResult = httpTask;

            //Extract tender data from the response
            var tenders = await DataExtraction.getWebData(httpResult, "eligibelTenderPage");
            List<tenderItem> tenderItems = (List<tenderItem>)tenders;

            //delete existing eligible tenders
            List<dbTenderItem> dbtenders0 = await App.Database.getTendersAsync(0);
            await deleteDatabaseEligibleTenders(dbtenders0);

            //save all eligible tenders to database
            await saveToTenderDb(tenderItems, 0);

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

            await DisplayAlert("Update Tenders", "Refresh Tenders", "Okay");
            listView.ItemsSource = tenderItems;



            await WaitAndExecuteUpdateTenders(10800000);

        }
            async Task<List<tenderItem>> retrieveTenderFromDatabase(int page)
        {
            List<tenderItem> tenderItems = new List<tenderItem>();
            List<dbTenderItem> dbTenderItems = await Task.Run<List<dbTenderItem>>(() => App.Database.getTendersAsync(page));
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

        async Task deleteDatabaseEligibleTenders(List<dbTenderItem> dbtenderItems)
        {
            foreach (dbTenderItem item in dbtenderItems)
            {
                await App.Database.DeleteTenderAsync(item);
            }
        }
        async Task saveToTenderDb(List<tenderItem> tenderItems, int page)
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
                } else
                {
                    return tenderBookmarks;
                }
            }
            return null;
        }

        void onUpButtonClicked()
        {
            //Move to the top of the list when user click on the "Up" button
            var topItem = listView.ItemsSource.Cast<object>().FirstOrDefault();
            listView.ScrollTo(topItem, ScrollToPosition.Start, true);
        }

        /*void OnCartTapped(object sender, EventArgs args)
        {
            var tenderSelected = ((TappedEventArgs)args).Parameter;
            tenderItem tender = (tenderItem)tenderSelected;

            DisplayAlert("Success", tender.AddToCartQuantity + " Item " + tender.Reference + " has been successfully added to cart ", "OK");
        }*/

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