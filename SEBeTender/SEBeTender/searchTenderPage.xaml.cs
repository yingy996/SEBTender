using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections;
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
        string selectedSource = "";
        string closingdatefrom = "";
        string closingdateto = "";
        string bidclosingdatefrom = "";
        string bidclosingdateto = "";
        
        //for use in storing originating sources for use when passing in custom bookmarked search
        List<string> globalSourceList = new List<string>();

        public searchTenderPage()
        {
            BindingContext = this;
            InitializeComponent();
            string username = "", password = "";
            username = adminAuth.Username;
            password = adminAuth.Password;

            displaySearchFields();
        }

        public searchTenderPage(customSearchesItem aCustomSearchItem)
        {
            BindingContext = this;
            InitializeComponent();
            string username = "", password = "";
            username = adminAuth.Username;
            password = adminAuth.Password;

            displayBookmarkedSearchFields(aCustomSearchItem);
        }

        async void displaySearchFields()
        {
            stkTab2.IsVisible = false;

            var tapRecognizer = new TapGestureRecognizer();
            tapRecognizer.Tapped += OnSearchBookmarkTapped;
            bookmarkImg.GestureRecognizers.Add(tapRecognizer);

            //---------DatePicker Control Section---------------
            //set datepicker text color to light gray to simulate not-filled
            closingdateFrom.TextColor = Color.LightGray;
            closingdateTo.TextColor = Color.LightGray;

            closingdateFrom.DateSelected += DatePicker_DateSelected;
            closingdateTo.DateSelected += DatePicker_DateSelected;
            //---------End DatePicker Control Section-----------
            
            normalTabButton.TextColor = Color.White;
            keywordTabButton.TextColor = Color.White;
            //normalTabButton.BackgroundColor = Color.FromHex("#4A6FB8");
            //keywordTabButton.BackgroundColor = Color.FromHex("#527DD4");
            searchButton.Clicked += OnSubmitButtonClicked;
            clearButton.Clicked += OnClearButtonClicked;

            keywordSubmitButton.Clicked += OnKeywordSubmitButtonClicked;
            keywordClearButton.Clicked += OnClearButtonClicked;

            await retrieveOriginatingSource();
        }

        async void displayBookmarkedSearchFields(customSearchesItem aCustomSearchItem)
        {
            await retrieveOriginatingSource();

            var tapRecognizer = new TapGestureRecognizer();
            tapRecognizer.Tapped += OnSearchBookmarkTapped;
            bookmarkImg.GestureRecognizers.Add(tapRecognizer);

            //---------DatePicker Control Section---------------
            //set datepicker text color to light gray to simulate not-filled
            closingdateFrom.TextColor = Color.LightGray;
            closingdateTo.TextColor = Color.LightGray;
            closingdateFrom.DateSelected += DatePicker_DateSelected;
            closingdateTo.DateSelected += DatePicker_DateSelected;
            //---------End DatePicker Control Section-----------

            stkTab2.IsVisible = false;
            normalTabButton.TextColor = Color.White;
            keywordTabButton.TextColor = Color.White;
            normalTabButton.BackgroundColor = Color.FromHex("#4A6FB8");
            keywordTabButton.BackgroundColor = Color.FromHex("#527DD4");
            searchButton.Clicked += OnSubmitButtonClicked;
            clearButton.Clicked += OnClearButtonClicked;

            keywordSubmitButton.Clicked += OnKeywordSubmitButtonClicked;
            keywordClearButton.Clicked += OnClearButtonClicked;

            searchButton.Clicked += OnSubmitButtonClicked;
            clearButton.Clicked += OnClearButtonClicked;

            if (!string.IsNullOrEmpty(aCustomSearchItem.searchID))
            {
                if (aCustomSearchItem.tenderTitle == "NULL")
                {
                    tenderTitleInput.Text = "";
                    Console.WriteLine("IS NULL OR EMPTY");
                }
                else
                {
                    tenderTitleInput.Text = aCustomSearchItem.tenderTitle;
                }

                if (aCustomSearchItem.tenderReference == "NULL")
                {
                    tenderReferenceInput.Text = "";
                }
                else
                {
                    tenderReferenceInput.Text = aCustomSearchItem.tenderReference;

                }

                
                for (int x = 0; x < globalSourceList.Count; x++)
                {
                    if (globalSourceList[x] == aCustomSearchItem.originatingSource)
                    {
                        sourcePicker.SelectedIndex = x;
                        selectedSource = aCustomSearchItem.originatingSource;
                    }
                    else if (x == globalSourceList.Count && globalSourceList[x] != aCustomSearchItem.originatingSource)
                    {
                        
                        await DisplayAlert("Error", "There are no available tenders with the chosen originating station. Please choose the originating station again", "Okay");
                    }
                }

                  

                if (!string.IsNullOrEmpty(aCustomSearchItem.closingDateFrom))
                {
                    closingdateFrom.Date = DateTime.Parse(aCustomSearchItem.closingDateFrom);
                    closingdateFrom.TextColor = Color.Black;
                }

                if (!string.IsNullOrEmpty(aCustomSearchItem.closingDateTo))
                {
                    closingdateTo.Date = DateTime.Parse(aCustomSearchItem.closingDateTo);
                    closingdateTo.TextColor = Color.Black;
                }
            }
        }

        //Custom tab control
        private void normalTabClicked(object sender, EventArgs e)
        {
            normalTabButton.BackgroundColor = Color.FromHex("#4A6FB8");
            keywordTabButton.BackgroundColor = Color.FromHex("#527DD4");
            stkTab1.IsVisible = true;
            stkTab2.IsVisible = false;
        }

        private void keywordTabClicked(object sender, EventArgs e)
        {
            normalTabButton.BackgroundColor = Color.FromHex("#527DD4");
            keywordTabButton.BackgroundColor = Color.FromHex("#4A6FB8");
            stkTab1.IsVisible = false;
            stkTab2.IsVisible = true;
        }

        async Task retrieveOriginatingSource()
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;
            //Retrieve list of originating source from online tender database
            /*string httpTask = await Task.Run<string>(() => HttpRequestHandler.GetRequest("http://www2.sesco.com.my/etender/notice/notice_search.jsp", false));
            var httpResult = httpTask;*/
            string httpTask = await Task.Run<string>(() => HttpRequestHandler.searchGetOriginatingSource("https://pockettender.000webhostapp.com/process_appSearchTenders.php"));
            var httpResult = httpTask;
            
            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;

            //--------Originating Source Picker Control Section----------------------------------------------
            //retrieve list of originating sources
            var originatingSourceObjects = new List<originatingSourceObject>();
            originatingSourceObjects = JsonConvert.DeserializeObject<List<originatingSourceObject>>(httpTask);

            var sourceList = new List<string>();
            sourceList.Add("All");
            foreach (var originatingSourceObject in originatingSourceObjects)
            {
                sourceList.Add(originatingSourceObject.originatingSource);
            }
            sourcePicker.ItemsSource = sourceList;
            sourcePicker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
            //---------End Station Picker Control Section-----------------------------------------

            //setting globalHtmlDoc to be used in another constructor with parameter
            globalSourceList = sourceList;
        }

        public class originatingSourceObject
        {
            public string originatingSource { get; set; }
        }

        //Event Handler Arguments
        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;

            if (picker.SelectedIndex > 0)
            {
                selectedSource = sourcePicker.Items[sourcePicker.SelectedIndex];

            }
            else
            {
                selectedSource = "all";
            }
        }

        void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (sender == closingdateFrom)
            {
                closingdatefrom = e.NewDate.ToString();
                closingdateFrom.TextColor = Color.Black;
                Console.WriteLine("CLOSING DATE FROM" + closingdatefrom);
            }
            else if (sender == closingdateTo)
            {
                closingdateto = e.NewDate.ToString();
                closingdateTo.TextColor = Color.Black;
            }
        }

        async void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            if (sender == searchButton)
            {
                if (String.IsNullOrEmpty(tenderReferenceInput.Text) && String.IsNullOrEmpty(tenderTitleInput.Text) && selectedSource == "" && closingdatefrom == "" && closingdateto == ""/*&& bidclosingdatefrom == "" && bidclosingdateto == ""*/)
                {
                    DisplayAlert("Error", "Please enter at least one search field", "Okay");

                }
                else
                {
                    //Sending HTTP request to obtain the tender page search result data
                    Task<string> httpSearchTask = Task.Run<string>(() => HttpRequestHandler.searchTendersFromDatabase("https://pockettender.000webhostapp.com/process_appSearchTenders.php", tenderReferenceInput.Text, tenderTitleInput.Text, selectedSource, closingdatefrom, closingdateto/*, bidclosingdatefrom, bidclosingdateto*/));
                    var httpSearchResult = httpSearchTask.Result.ToString();
                    //Console.WriteLine(httpSearchResult);
                    await Navigation.PushAsync(new tenderSearchResultPage(httpSearchResult, null));
                }
            }

        }

        void OnClearButtonClicked(object sender, EventArgs e)
        {
            if (sender == clearButton)
            {
                tenderReferenceInput.Text = "";
                tenderTitleInput.Text = "";
                sourcePicker.SelectedIndex = 0;
                selectedSource = "";
                closingdatefrom = "";
                closingdateto = "";
                bidclosingdatefrom = "";
                bidclosingdateto = "";
                closingdateFrom.TextColor = Color.LightGray;
                closingdateTo.TextColor = Color.LightGray;
            }
        }

        async void OnKeywordSubmitButtonClicked(Object sender, EventArgs e)
        {
            if(sender == keywordSubmitButton)
            {
                if (String.IsNullOrEmpty(tenderKeywordInput.Text))
                {
                    DisplayAlert("Error", "Please enter at least a character", "Okay");
                }
                else
                {
                    Console.WriteLine(tenderKeywordInput.Text);
                    List<dbTenderItem> dbsearchTenderItem = await App.Database.keywordSearchTenders(tenderKeywordInput.Text);
                    await Navigation.PushAsync(new tenderSearchResultPage("Search Local Database", dbsearchTenderItem));
                }
            }
            
        }

        void OnKeywordClearButtonClicked(object sender, EventArgs e)
        {
            if(sender == keywordClearButton)
            {
                tenderKeywordInput.Text = "";
            }
        }

        async void OnSearchBookmarkTapped(object sender, EventArgs eventArgs)
        {
            //check if user is logged in
            if (userSession.userLoginCookie == "")
            {
                await DisplayAlert("Login required", "Please login first to saved your custom search", "OK");
            }
            else
            {   //if none of the fields are edited, output error
                if (String.IsNullOrEmpty(tenderReferenceInput.Text) && String.IsNullOrEmpty(tenderTitleInput.Text) && selectedSource == "" && closingdatefrom == "" && closingdateto == "")
                {
                    await DisplayAlert("Error", "Please enter at least one search field", "Okay");

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
                    string originatingSource = selectedSource;
                    string closingDateFrom = closingdatefrom;
                    string closingDateTo = closingdateto;
                    

                    DisplayAlert("Add bookmark", "Search preferences added and can be viewed at the Custom Searches page", "OK");
                    


                    string postbookmarkhttptask = await Task.Run<string>(() => HttpRequestHandler.PostManageSearchBookmark(randomnumber, tenderReference, tenderTitle, originatingSource, closingDateFrom, closingDateTo, userSession.username, identifier, "add"));
                    var postbookmarkhttpresult = postbookmarkhttptask.ToString();
                    Console.WriteLine(postbookmarkhttpresult);
                    int count = 0;

                    while (count < 3 && postbookmarkhttpresult != "Success")
                    {
                        Console.WriteLine("Looping for failure add");
                        postbookmarkhttptask = await Task.Run<string>(() => HttpRequestHandler.PostManageSearchBookmark(randomnumber, tenderReference, tenderTitle, originatingSource, closingDateFrom, closingDateTo, userSession.username, identifier, "add"));
                        postbookmarkhttpresult = postbookmarkhttptask.ToString();
                        count++;
                    }

                }
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