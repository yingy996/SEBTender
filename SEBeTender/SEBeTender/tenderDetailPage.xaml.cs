using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace SEBeTender
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class tenderDetailPage : ContentPage
	{
        bool isOriginatorInfoPresent = false;
        public tenderDetailPage ()
		{			
		}

        public tenderDetailPage(tenderItem aTenderItem)
        {
            BindingContext = this;
            
            InitializeComponent();
            int currentGridRow;     
            /*string closingDate = "";
            if (aTenderItem.Company == "Sarawak Energy")
            {
                isOriginatorInfoPresent = true;
                string[] words = Regex.Split(aTenderItem.ClosingDate, ": ");
                closingDate = words[1];
            }*/
            
            tenderRefLbl.Text = aTenderItem.Reference;
            tenderTitleLbl.Text = aTenderItem.Title;
            oriStationLbl.Text = aTenderItem.OriginatingStation;
            closingDateLbl.Text = aTenderItem.ClosingDate;
            bidCloseDateLbl.Text = aTenderItem.BidClosingDate;
            feeBeforeGSTLbl.Text = aTenderItem.FeeBeforeGST;
            feeGSTLbl.Text = aTenderItem.FeeGST;
            feeAfterGSTLbl.Text = aTenderItem.FeeAfterGST;

            if (aTenderItem.Company == "Sarawak Energy")
            {
                nameLbl.Text = aTenderItem.Name;
                officePhoneLbl.Text = aTenderItem.OffinePhone;
                extensionLbl.Text = aTenderItem.Extension;
                mobilePhoneLbl.Text = aTenderItem.MobilePhone;
                emailLbl.Text = aTenderItem.Email;
                faxLbl.Text = aTenderItem.Fax;
            } else
            {
                displayGrid.Children.Remove(bidCloseDateTitleLbl);
                displayGrid.Children.Remove(bidCloseLayout);
                displayGrid.Children.Remove(boxView11);
                displayGrid.Children.Remove(docFeeTitleLbl);
                displayGrid.Children.Remove(feeBeforeGSTLbl);
                displayGrid.Children.Remove(boxView13);
                displayGrid.Children.Remove(gstTitleLbl);
                displayGrid.Children.Remove(feeGSTLbl);
                displayGrid.Children.Remove(boxView15);
                displayGrid.Children.Remove(feeAfterGSTTitleLbl);
                displayGrid.Children.Remove(feeAfterGSTLbl);
                displayGrid.Children.Remove(boxView17);

                displayGrid.Children.Remove(originatorTitleLbl);
                displayGrid.Children.Remove(nameTitleLbl);
                displayGrid.Children.Remove(offPhoneTitleLbl);
                displayGrid.Children.Remove(extTitleLbl);
                displayGrid.Children.Remove(mobPhoneTitleLbl);
                displayGrid.Children.Remove(emailTitleLbl);
                displayGrid.Children.Remove(faxTitleLbl);

                displayGrid.Children.Remove(nameLbl);
                displayGrid.Children.Remove(officePhoneLbl);
                displayGrid.Children.Remove(extensionLbl);
                displayGrid.Children.Remove(mobilePhoneLbl);
                //displayGrid.Children.Remove(emailLbl);
                displayGrid.Children.Remove(emailLayout);
                displayGrid.Children.Remove(faxLbl);

                displayGrid.Children.Remove(boxView20);
                displayGrid.Children.Remove(boxView22);
                displayGrid.Children.Remove(boxView24);
                displayGrid.Children.Remove(boxView26);
                displayGrid.Children.Remove(boxView28);
                displayGrid.Children.Remove(boxView30);

                for (int i = 0; i < 21; i++)
                {
                    displayGrid.RowDefinitions.RemoveAt(displayGrid.RowDefinitions.Count()-1);
                }
            }

            Console.WriteLine("Tender source is : " + aTenderItem.TenderSource);
            if (aTenderItem.TenderSource == "1") 
            {
                //If tender is from myProcurement, display its category and agency
                currentGridRow = displayGrid.RowDefinitions.Count();

                //Create labels for category and add into the grid
                var categoryLabel = new Label { Text = "Category: ", Margin = new Thickness(0, 5, 0, 0), FontAttributes = FontAttributes.Bold };
                var categoryDataLabel = new Label { Text = aTenderItem.Category, Margin = new Thickness(0, 5, 0, 0) };
                displayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                displayGrid.Children.Add(categoryLabel, 0, currentGridRow);
                displayGrid.Children.Add(categoryDataLabel, 1, currentGridRow);
                currentGridRow++;

                //Create boxView as separator
                var categoryBoxView = new BoxView { BackgroundColor = Color.LightGray, HeightRequest = 1, Margin = new Thickness(0, 5, 0, 0), VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.FillAndExpand };
                displayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                displayGrid.Children.Add(categoryBoxView, 0, currentGridRow);
                Grid.SetColumnSpan(categoryBoxView, 2);
                currentGridRow++;

                //Create labels for agency and add into the grid
                var agencyLabel = new Label { Text = "Agency: ", Margin = new Thickness(0, 5, 0, 0), FontAttributes = FontAttributes.Bold };
                var agencyLayout = new StackLayout { Margin = new Thickness(0, 5, 0, 0) };               
                var agencyDataLabel = new Label { Text = aTenderItem.Agency };
                agencyLayout.Children.Add(agencyDataLabel);

                displayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                displayGrid.Children.Add(agencyLabel, 0, currentGridRow);
                displayGrid.Children.Add(agencyLayout, 1, currentGridRow);
                currentGridRow++;

                //Create boxView as separator
                var agencyBoxView = new BoxView { BackgroundColor = Color.LightGray, HeightRequest = 1, Margin = new Thickness(0, 5, 0, 0), VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.FillAndExpand };
                displayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                displayGrid.Children.Add(agencyBoxView, 0, currentGridRow);
                Grid.SetColumnSpan(agencyBoxView, 2);
            }
            
            //Create hyperlink labels for downloadable files
            if (aTenderItem.FileLinks.Count > 0)
            {
                currentGridRow = displayGrid.RowDefinitions.Count();
                Console.WriteLine("Current grid row : " + currentGridRow);
                //<Label Text="Downloadable Files: " x:Name="downloadLbl" Grid.Row="31" Grid.Column="0" FontSize="16" Grid.ColumnSpan="2" FontAttributes="Bold" Margin="0,20,0,10"/>
                var downloadLabel = new Label { Text = "Downloadable Files: ", Margin = new Thickness(0, 20, 0, 10), FontSize = 16, FontAttributes = FontAttributes.Bold };
                displayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                displayGrid.Children.Add(downloadLabel, 0, currentGridRow);
                Grid.SetColumnSpan(downloadLabel, 2);
                currentGridRow++;
                foreach (KeyValuePair<string, string> file in aTenderItem.FileLinks)
                {
                    Console.WriteLine("Key: " + file.Key + "row: " + currentGridRow);
                    //create the hyperlink label
                    var label = new Label { Text = file.Key, TextColor = Color.DodgerBlue, Margin = new Thickness(0, 0, 0, 5) };
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (s, e) => { Device.OpenUri(new Uri(file.Value)); };
                    label.GestureRecognizers.Add(tapGestureRecognizer);

                    //add hyperlink label into grid
                    displayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

                    displayGrid.Children.Add(label, 0, currentGridRow);
                    Grid.SetColumnSpan(label, 2);
                    currentGridRow++;
                }
            }            
        }

        public tenderDetailPage(tenderBookmark tenderBookmark)
        {
            InitializeComponent();

            retrieveBookmarkTenderDetails(tenderBookmark);
        }

        async void retrieveBookmarkTenderDetails(tenderBookmark tenderBookmark)
        {
            int currentGridRow;
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            //Get tender details
            string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostGetBookmarkDetails(tenderBookmark.tenderReferenceNumber, tenderBookmark.tenderTitle));
            var httpResult = httpTask.ToString();

            //var htmlDoc = new HtmlDocument();
            //htmlDoc.LoadHtml(httpResult);

            //Extract tender data from the response
            //var tenders = await DataExtraction.getWebData(httpResult, "tender");
            if (httpResult != "Tender not found" && httpResult != "Missing parameter(s)")
            {
                scrapped_tender scrappedTender = JsonConvert.DeserializeObject<scrapped_tender>(httpResult);
                tenderItem aTenderItem = new tenderItem();

                //Copying tender details from scrapped_tender object to tender item object 
                aTenderItem.Company = scrappedTender.originatingSource;
                aTenderItem.TenderSource = scrappedTender.tenderSource;
                aTenderItem.Reference = scrappedTender.reference;
                aTenderItem.Agency = scrappedTender.agency;
                aTenderItem.Title = scrappedTender.title;
                aTenderItem.Category = scrappedTender.category;
                aTenderItem.OriginatingStation = scrappedTender.originatingSource;
                aTenderItem.ClosingDate = scrappedTender.closingDate;

                if (scrappedTender.docInfoJson != null)
                {
                    dynamic docInfo = JsonConvert.DeserializeObject(scrappedTender.docInfoJson);
                    if (docInfo.bidCloseDate != null)
                    {
                        aTenderItem.BidClosingDate = docInfo.bidCloseDate;
                    }

                    if (docInfo.feeBeforeGST != null)
                    {
                        aTenderItem.FeeBeforeGST = docInfo.feeBeforeGST;
                    }

                    if (docInfo.feeGST != null)
                    {
                        aTenderItem.FeeGST = docInfo.feeGST;
                    }

                    if (docInfo.feeAfterGST != null)
                    {
                        aTenderItem.FeeAfterGST = docInfo.feeAfterGST;
                    }
                }

                if (scrappedTender.originatorJson != null)
                {
                    dynamic originatorInfo = JsonConvert.DeserializeObject(scrappedTender.originatorJson);
                    if (originatorInfo.name != null)
                    {
                        aTenderItem.Name = originatorInfo.name;
                    }

                    if (originatorInfo.officePhone != null)
                    {
                        aTenderItem.OffinePhone = originatorInfo.officePhone;
                    }

                    if (originatorInfo.extension != null)
                    {
                        aTenderItem.Extension = originatorInfo.extension;
                    }

                    if (originatorInfo.mobilePhone != null)
                    {
                        aTenderItem.MobilePhone = originatorInfo.mobilePhone;
                    }

                    if (originatorInfo.email != null)
                    {
                        aTenderItem.Email = originatorInfo.email;
                    }

                    if (originatorInfo.fax != null)
                    {
                        aTenderItem.Fax = originatorInfo.fax;
                    }
                }

                if (scrappedTender.fileLinks != null)
                {
                    Dictionary<string, string> fileLinks = JsonConvert.DeserializeObject<Dictionary<string, string>>(scrappedTender.fileLinks);
                    aTenderItem.FileLinks = fileLinks;
                    //{"Folder 1.zip":"http:\/\/www2.sesco.com.my\/noticeDoc\/Folder 1.zip","Folder 2.zip":"http:\/\/www2.sesco.com.my\/noticeDoc\/Folder 2.zip","Folder 3.zip":"http:\/\/www2.sesco.com.my\/noticeDoc\/Folder 3.zip","Folder 4.zip":"http:\/\/www2.sesco.com.my\/noticeDoc\/Folder 4.zip"}
                }            
                //List<tenderItem> tenderItems = (List<tenderItem>)tenders;

                activityIndicator.IsVisible = false;
                activityIndicator.IsRunning = false;

                //if (tenderItems.Count > 0)
                if (aTenderItem != null)
                {
                    tenderRefLbl.Text = aTenderItem.Reference;
                    tenderTitleLbl.Text = aTenderItem.Title;
                    oriStationLbl.Text = aTenderItem.OriginatingStation;
                    closingDateLbl.Text = aTenderItem.ClosingDate;
                    bidCloseDateLbl.Text = aTenderItem.BidClosingDate;
                    feeBeforeGSTLbl.Text = aTenderItem.FeeBeforeGST;
                    feeGSTLbl.Text = aTenderItem.FeeGST;
                    feeAfterGSTLbl.Text = aTenderItem.FeeAfterGST;

                    if (aTenderItem.Company == "Sarawak Energy")
                    {
                        nameLbl.Text = aTenderItem.Name;
                        officePhoneLbl.Text = aTenderItem.OffinePhone;
                        extensionLbl.Text = aTenderItem.Extension;
                        mobilePhoneLbl.Text = aTenderItem.MobilePhone;
                        emailLbl.Text = aTenderItem.Email;
                        faxLbl.Text = aTenderItem.Fax;
                    }
                    else
                    {
                        displayGrid.Children.Remove(bidCloseDateTitleLbl);
                        displayGrid.Children.Remove(bidCloseLayout);
                        displayGrid.Children.Remove(boxView11);
                        displayGrid.Children.Remove(docFeeTitleLbl);
                        displayGrid.Children.Remove(feeBeforeGSTLbl);
                        displayGrid.Children.Remove(boxView13);
                        displayGrid.Children.Remove(gstTitleLbl);
                        displayGrid.Children.Remove(feeGSTLbl);
                        displayGrid.Children.Remove(boxView15);
                        displayGrid.Children.Remove(feeAfterGSTTitleLbl);
                        displayGrid.Children.Remove(feeAfterGSTLbl);
                        displayGrid.Children.Remove(boxView17);

                        displayGrid.Children.Remove(originatorTitleLbl);
                        displayGrid.Children.Remove(nameTitleLbl);
                        displayGrid.Children.Remove(offPhoneTitleLbl);
                        displayGrid.Children.Remove(extTitleLbl);
                        displayGrid.Children.Remove(mobPhoneTitleLbl);
                        displayGrid.Children.Remove(emailTitleLbl);
                        displayGrid.Children.Remove(faxTitleLbl);

                        displayGrid.Children.Remove(nameLbl);
                        displayGrid.Children.Remove(officePhoneLbl);
                        displayGrid.Children.Remove(extensionLbl);
                        displayGrid.Children.Remove(mobilePhoneLbl);
                        //displayGrid.Children.Remove(emailLbl);
                        displayGrid.Children.Remove(emailLayout);
                        displayGrid.Children.Remove(faxLbl);

                        displayGrid.Children.Remove(boxView20);
                        displayGrid.Children.Remove(boxView22);
                        displayGrid.Children.Remove(boxView24);
                        displayGrid.Children.Remove(boxView26);
                        displayGrid.Children.Remove(boxView28);
                        displayGrid.Children.Remove(boxView30);

                        for (int i = 0; i < 21; i++)
                        {
                            displayGrid.RowDefinitions.RemoveAt(displayGrid.RowDefinitions.Count() - 1);
                        }
                    }

                    Console.WriteLine("Tender source is : " + aTenderItem.TenderSource);
                    if (aTenderItem.TenderSource == "1")
                    {
                        //If tender is from myProcurement, display its category and agency
                        currentGridRow = displayGrid.RowDefinitions.Count();

                        //Create labels for category and add into the grid
                        var categoryLabel = new Label { Text = "Category: ", Margin = new Thickness(0, 5, 0, 0), FontAttributes = FontAttributes.Bold };
                        var categoryDataLabel = new Label { Text = aTenderItem.Category, Margin = new Thickness(0, 5, 0, 0) };
                        displayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                        displayGrid.Children.Add(categoryLabel, 0, currentGridRow);
                        displayGrid.Children.Add(categoryDataLabel, 1, currentGridRow);
                        currentGridRow++;

                        //Create boxView as separator
                        var categoryBoxView = new BoxView { BackgroundColor = Color.LightGray, HeightRequest = 1, Margin = new Thickness(0, 5, 0, 0), VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.FillAndExpand };
                        displayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                        displayGrid.Children.Add(categoryBoxView, 0, currentGridRow);
                        Grid.SetColumnSpan(categoryBoxView, 2);
                        currentGridRow++;

                        //Create labels for agency and add into the grid
                        var agencyLabel = new Label { Text = "Agency: ", Margin = new Thickness(0, 5, 0, 0), FontAttributes = FontAttributes.Bold };
                        var agencyLayout = new StackLayout { Margin = new Thickness(0, 5, 0, 0) };
                        var agencyDataLabel = new Label { Text = aTenderItem.Agency };
                        agencyLayout.Children.Add(agencyDataLabel);

                        displayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                        displayGrid.Children.Add(agencyLabel, 0, currentGridRow);
                        displayGrid.Children.Add(agencyLayout, 1, currentGridRow);
                        currentGridRow++;

                        //Create boxView as separator
                        var agencyBoxView = new BoxView { BackgroundColor = Color.LightGray, HeightRequest = 1, Margin = new Thickness(0, 5, 0, 0), VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.FillAndExpand };
                        displayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                        displayGrid.Children.Add(agencyBoxView, 0, currentGridRow);
                        Grid.SetColumnSpan(agencyBoxView, 2);
                    }

                    //Create hyperlink labels for downloadable files
                    if (aTenderItem.FileLinks != null)
                    {
                        if (aTenderItem.FileLinks.Count > 0)
                        {
                            currentGridRow = displayGrid.RowDefinitions.Count();
                            Console.WriteLine("Current grid row : " + currentGridRow);
                            //<Label Text="Downloadable Files: " x:Name="downloadLbl" Grid.Row="31" Grid.Column="0" FontSize="16" Grid.ColumnSpan="2" FontAttributes="Bold" Margin="0,20,0,10"/>
                            var downloadLabel = new Label { Text = "Downloadable Files: ", Margin = new Thickness(0, 20, 0, 10), FontSize = 16, FontAttributes = FontAttributes.Bold };
                            displayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                            displayGrid.Children.Add(downloadLabel, 0, currentGridRow);
                            Grid.SetColumnSpan(downloadLabel, 2);
                            currentGridRow++;
                            foreach (KeyValuePair<string, string> file in aTenderItem.FileLinks)
                            {
                                Console.WriteLine("Key: " + file.Key + "row: " + currentGridRow);
                                //create the hyperlink label
                                var label = new Label { Text = file.Key, TextColor = Color.DodgerBlue, Margin = new Thickness(0, 0, 0, 5) };
                                var tapGestureRecognizer = new TapGestureRecognizer();
                                tapGestureRecognizer.Tapped += (s, e) => { Device.OpenUri(new Uri(file.Value)); };
                                label.GestureRecognizers.Add(tapGestureRecognizer);

                                //add hyperlink label into grid
                                displayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

                                displayGrid.Children.Add(label, 0, currentGridRow);
                                Grid.SetColumnSpan(label, 2);
                                currentGridRow++;
                            }
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Not found", "Tender '" + tenderBookmark.tenderReferenceNumber + "' is not available!", "OK");
                    var page = App.Current.MainPage as rootPage;
                    var tenderBookmarkPage = new tenderBookmarkPage();
                    page.changePage(tenderBookmarkPage);
                }
            } else
            {
                if (httpResult == "Tender not found")
                {
                    await DisplayAlert("Tender Not found", "Tender '" + tenderBookmark.tenderReferenceNumber + "' is not available!", "OK");
                    var page = App.Current.MainPage as rootPage;
                    var tenderBookmarkPage = new tenderBookmarkPage();
                    page.changePage(tenderBookmarkPage);
                } else
                {
                    await DisplayAlert("Failed", httpResult.ToString(), "OK");
                    var page = App.Current.MainPage as rootPage;
                    var tenderBookmarkPage = new tenderBookmarkPage();
                    page.changePage(tenderBookmarkPage);
                }              
            }
        }
    }
}