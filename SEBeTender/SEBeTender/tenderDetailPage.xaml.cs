using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HtmlAgilityPack;

namespace SEBeTender
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class tenderDetailPage : ContentPage
	{
		public tenderDetailPage ()
		{
			
		}

        public tenderDetailPage(tenderItem aTenderItem)
        {
            InitializeComponent();

            string[] words = Regex.Split(aTenderItem.ClosingDate, ": ");
            string closingDate = words[1];
            
            tenderRefLbl.Text = aTenderItem.Reference;
            tenderTitleLbl.Text = aTenderItem.Title;
            oriStationLbl.Text = aTenderItem.OriginatingStation;
            closingDateLbl.Text = closingDate;
            bidCloseDateLbl.Text = aTenderItem.BidClosingDate;
            feeBeforeGSTLbl.Text = aTenderItem.FeeBeforeGST;
            feeGSTLbl.Text = aTenderItem.FeeGST;
            feeAfterGSTLbl.Text = aTenderItem.FeeAfterGST;
            nameLbl.Text = aTenderItem.Name;
            officePhoneLbl.Text = aTenderItem.OffinePhone;
            extensionLbl.Text = aTenderItem.Extension;
            mobilePhoneLbl.Text = aTenderItem.MobilePhone;
            emailLbl.Text = aTenderItem.Email;
            faxLbl.Text = aTenderItem.Fax;

            //Create hyperlink labels for downloadable files
            int currentGridRow = 32;
            foreach(KeyValuePair<string, string> file in aTenderItem.FileLinks)
            {
                Console.WriteLine("Key: "+ file.Key);
                //create the hyperlink label
                var label = new Label { Text = file.Key, TextColor = Color.DodgerBlue, Margin = new Thickness(0,0,0,5)};
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += (s, e) => { Device.OpenUri(new Uri(file.Value)); };
                label.GestureRecognizers.Add(tapGestureRecognizer);

                //add hyperlink label into grid
                displayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });               

                displayGrid.Children.Add(label, 0, currentGridRow);
                Grid.SetColumnSpan(label, 2);
                currentGridRow++;
            }
        }

        public tenderDetailPage(tenderBookmark tenderBookmark)
        {
            InitializeComponent();

            retrieveBookmarkTenderDetails(tenderBookmark);
        }

        async void retrieveBookmarkTenderDetails(tenderBookmark tenderBookmark)
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            //Get tender details
            string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostGetBookmarkDetails(tenderBookmark.tenderReferenceNumber));
            var httpResult = httpTask.ToString();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(httpResult);

            //Extract tender data from the response
            var tenders = await DataExtraction.getWebData(httpResult, "tender");
            List<tenderItem> tenderItems = (List<tenderItem>)tenders;

            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;

            if (tenderItems.Count > 0)
            {
                tenderItem tenderItem = tenderItems[0];

                string[] words = Regex.Split(tenderItem.ClosingDate, ": ");
                string closingDate = words[1];

                tenderRefLbl.Text = tenderItem.Reference;
                tenderTitleLbl.Text = tenderItem.Title;
                oriStationLbl.Text = tenderItem.OriginatingStation;
                closingDateLbl.Text = closingDate;
                bidCloseDateLbl.Text = tenderItem.BidClosingDate;
                feeBeforeGSTLbl.Text = tenderItem.FeeBeforeGST;
                feeGSTLbl.Text = tenderItem.FeeGST;
                feeAfterGSTLbl.Text = tenderItem.FeeAfterGST;
                nameLbl.Text = tenderItem.Name;
                officePhoneLbl.Text = tenderItem.OffinePhone;
                extensionLbl.Text = tenderItem.Extension;
                mobilePhoneLbl.Text = tenderItem.MobilePhone;
                emailLbl.Text = tenderItem.Email;
                faxLbl.Text = tenderItem.Fax;

                //Create hyperlink labels for downloadable files
                int currentGridRow = 32;
                foreach (KeyValuePair<string, string> file in tenderItem.FileLinks)
                {
                    //create the hyperlink label
                    var label = new Label { Text = file.Key, TextColor = Color.DodgerBlue, Margin = new Thickness(0, 0, 0, 5) };
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (s, e) => { Device.OpenUri(new Uri(file.Value)); };
                    label.GestureRecognizers.Add(tapGestureRecognizer);

                    //add hyperlink label into grid
                    displayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                    displayGrid.Children.Add(label, 0, currentGridRow);
                    Grid.SetColumnSpan(label, 2);
                    currentGridRow++;
                }
                Console.WriteLine("Tender returned: " + tenderItem.Reference + "Tender title: " + tenderItem.Title);
            } else
            {
                await DisplayAlert("Not found", "Tender '" + tenderBookmark.tenderReferenceNumber  +"' is not available!", "OK");
                var page = App.Current.MainPage as rootPage;
                var tenderBookmarkPage = new tenderBookmarkPage();
                page.changePage(tenderBookmarkPage);
            }
   
        }
    }
}