using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SEBeTender
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class searchTenderPage : ContentPage
    {
        string selectedStation = "";
        string closingdatefrom = "";
        string closingdateto = "";
        string bidclosingdatefrom = "";
        string bidclosingdateto = "";

        public searchTenderPage()
        {
            BindingContext = this;
            InitializeComponent();
            string username = "", password = "";
            username = adminAuth.Username;
            password = adminAuth.Password;

            
            //Send Http request to retrieve search page originating station drop down
            Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.GetRequest("http://www2.sesco.com.my/etender/notice/notice_search.jsp", false));
            var httpResult = httpTask.Result.ToString();

            //--------Station Picker Control Section---------------------------------------------
            //Small data extraction to extract Station dropdown selects/options to fill Picker
            HtmlDocument htmlDoc = new HtmlDocument();
            HtmlNode.ElementsFlags.Remove("option");
            htmlDoc.LoadHtml(httpResult);
            var stationList = new List<string>();
            foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//select[@name='SchStation']//option"))
            {
                stationList.Add(node.InnerText);
            }


            stationPicker.ItemsSource = stationList;
            //set default origin station Picker value
            //stationPicker.SelectedIndex = 0;
            stationPicker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
            //---------End Station Picker Control Section-----------------------------------------
            var tapRecognizer = new TapGestureRecognizer();
            tapRecognizer.Tapped += OnSearchBookmarkTapped;
            bookmarkImg.GestureRecognizers.Add(tapRecognizer);
            //bookmarkImg.OnTapped += OnSearchBookmarkTapped;
            //imagebutton.Clicked += OnSearchBookmarkTapped;
            //---------DatePicker Control Section---------------
            //set datepicker text color to light gray to simulate not-filled
            closingdateFrom.TextColor = Color.LightGray;
            closingdateTo.TextColor = Color.LightGray;
            bidclosingdateFrom.TextColor = Color.LightGray;
            bidclosingdateTo.TextColor = Color.LightGray;

            closingdateFrom.DateSelected += DatePicker_DateSelected;
            closingdateTo.DateSelected += DatePicker_DateSelected;
            bidclosingdateFrom.DateSelected += DatePicker_DateSelected;
            bidclosingdateTo.DateSelected += DatePicker_DateSelected;
            //---------End DatePicker Control Section---------------------------------------------

            searchButton.Clicked += OnSubmitButtonClicked;
            clearButton.Clicked += OnClearButtonClicked;

            //Event Handler Arguments
            void OnPickerSelectedIndexChanged(object sender, EventArgs e)
            {
                Picker picker = (Picker)sender;

                if (picker.SelectedIndex > 0)
                {
                    selectedStation = stationPicker.Items[stationPicker.SelectedIndex];

                }
                else
                {
                    selectedStation = null;
                }
                Console.WriteLine("Selected station: " + selectedStation);
            }

            void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
            {
                if (sender == closingdateFrom)
                {
                    closingdatefrom = e.NewDate.ToString();
                    closingdateFrom.TextColor = Color.Black;
                }
                else if (sender == closingdateTo)
                {
                    closingdateto = e.NewDate.ToString();
                    closingdateTo.TextColor = Color.Black;
                }
                else if (sender == bidclosingdateFrom)
                {
                    bidclosingdatefrom = e.NewDate.ToString();
                    bidclosingdateFrom.TextColor = Color.Black;
                }
                else if (sender == bidclosingdateTo)
                {
                    bidclosingdateto = e.NewDate.ToString();
                    bidclosingdateTo.TextColor = Color.Black;
                }
            }

            async void OnSubmitButtonClicked(object sender, EventArgs e)
            {
                if (sender == searchButton)
                {
                    /*Console.WriteLine("Ref: " + tenderReferenceInput.Text);
                    Console.WriteLine("Title: " + tenderTitleInput.Text);
                    Console.WriteLine("Station: " + selectedStation);
                    Console.WriteLine("closingdatefrom: " + closingdatefrom);
                    Console.WriteLine("closingdateto: " + closingdateto);
                    Console.WriteLine("bidclosingdatefrom: " + bidclosingdatefrom);
                    Console.WriteLine("bidclosingdateto: " + bidclosingdateto);*/

                    if (String.IsNullOrEmpty(tenderReferenceInput.Text) && String.IsNullOrEmpty(tenderTitleInput.Text) && selectedStation == "" && closingdatefrom == "" && closingdateto == ""
                        && bidclosingdatefrom == "" && bidclosingdateto == "")
                    {
                        DisplayAlert("Error", "Please enter at least one search field", "Okay");

                    }
                    else
                    {


                        //Sending HTTP request to obtain the tender page search result data
                        Task<string> httpSearchTask = Task.Run<string>(() => HttpRequestHandler.SearchPostRequest("http://www2.sesco.com.my/etender/notice/notice.jsp", tenderReferenceInput.Text, tenderTitleInput.Text, selectedStation, closingdatefrom, closingdateto, bidclosingdatefrom, bidclosingdateto));
                        var httpSearchResult = httpSearchTask.Result.ToString();
                        //Console.WriteLine(httpSearchResult);
                        await Navigation.PushAsync(new tenderSearchResultPage(httpSearchResult));
                    }



                }

            }

            async void OnSearchBookmarkTapped(object sender, EventArgs eventArgs)
            {
                //check if user is logged in
                if (userSession.userLoginCookie == "")
                {
                    DisplayAlert("Login required", "Please login first to saved your custom search", "OK");
                }
                else
                {   //if none of the fields are edited, output error
                    if (String.IsNullOrEmpty(tenderReferenceInput.Text) && String.IsNullOrEmpty(tenderTitleInput.Text) && selectedStation == "" && closingdatefrom == "" && closingdateto == ""
                        && bidclosingdatefrom == "" && bidclosingdateto == "")
                    {
                        DisplayAlert("Error", "Please enter at least one search field", "Okay");

                    }
                    else
                    {
                        string identifier = await InputBox(this.Navigation);
                        //Generate random number for Custom Search ID
                        Random rand1 = new Random();
                        string randomnumber = "";
                        

                        for (int i = 0; i < 7; i++)
                        {
                            randomnumber = rand1.Next().ToString();
                        }

                        
                        string tenderReference = tenderReferenceInput.Text;
                        string tenderTitle = tenderTitleInput.Text;
                        string originatingStation = selectedStation;
                        string closingDateFrom = closingdatefrom;
                        string closingDateTo = closingdateto;
                        string biddingclosingDateFrom = bidclosingdatefrom;
                        string biddingclosingDateTo = bidclosingdateto;
                        
                        DisplayAlert("Add bookmark", "Search preferences added and can be viewed at the Custom Searches page", "OK");


                        string postbookmarkhttptask = await Task.Run<string>(() => HttpRequestHandler.PostManageSearchBookmark(randomnumber, tenderReference, tenderTitle, originatingStation, closingDateFrom, closingDateTo, biddingclosingDateFrom, biddingclosingDateTo, userSession.username, identifier, "add"));
                        var postbookmarkhttpresult = httpTask.ToString();
                        Console.WriteLine(httpResult);
                        int count = 0;

                        while (count < 3 && httpResult != "Success")
                        {
                            Console.WriteLine("Looping for failure add");
                            postbookmarkhttptask = await Task.Run<string>(() => HttpRequestHandler.PostManageSearchBookmark(randomnumber, tenderReference, tenderTitle, originatingStation, closingDateFrom, closingDateTo, biddingclosingDateFrom, biddingclosingDateTo, userSession.username, identifier, "add"));
                            postbookmarkhttpresult = httpTask.ToString();
                            count++;
                        }

                    }
                }
            }

            void OnClearButtonClicked(object sender, EventArgs e)
            {
                if (sender == clearButton)
                {
                    tenderReferenceInput.Text = "";
                    tenderTitleInput.Text = "";
                    stationPicker.SelectedIndex = 0;
                    selectedStation = "";
                    closingdatefrom = "";
                    closingdateto = "";
                    bidclosingdatefrom = "";
                    bidclosingdateto = "";
                    closingdateFrom.TextColor = Color.LightGray;
                    closingdateTo.TextColor = Color.LightGray;
                    bidclosingdateFrom.TextColor = Color.LightGray;
                    bidclosingdateTo.TextColor = Color.LightGray;

                }
            }

            Task<string> InputBox(INavigation navigation)
            {
                // wait in this proc, until user did his input 
                var tcs = new TaskCompletionSource<string>();

                var lblTitle = new Label { Text = "Add Custom Search", HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold };
                var lblMessage = new Label { Text = "Enter identifier for future reference:" };
                var txtInput = new Entry { Text = "" };

                var btnOk = new Button
                {
                    Text = "Ok",
                    WidthRequest = 100,
                    BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8),
                };
                btnOk.Clicked += async (s, e) =>
                {
                    // close page
                    var result = txtInput.Text;
                    await navigation.PopModalAsync();
                    // pass result
                    tcs.SetResult(result);
                };

                var btnCancel = new Button
                {
                    Text = "Cancel",
                    WidthRequest = 100,
                    BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8)
                };
                btnCancel.Clicked += async (s, e) =>
                {
                    // close page
                    await navigation.PopModalAsync();
                    // pass empty result
                    tcs.SetResult(null);
                };

                var slButtons = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children = { btnOk, btnCancel },
                };

                var layout = new StackLayout
                {
                    Padding = new Thickness(0, 40, 0, 0),
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Orientation = StackOrientation.Vertical,
                    Children = { lblTitle, lblMessage, txtInput, slButtons },
                };

                // create and show page
                var page = new ContentPage();
                page.Content = layout;
                navigation.PushModalAsync(page);
                // open keyboard
                txtInput.Focus();

                // code is waiting her, until result is passed with tcs.SetResult() in btn-Clicked
                // then proc returns the result
                return tcs.Task;
            }


        }
    }

    
}