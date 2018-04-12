﻿using HtmlAgilityPack;
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
        string closingdatefrom, closingdateto, bidclosingdatefrom, bidclosingdateto = "";
        
        public searchTenderPage ()
		{
            BindingContext = this;
            InitializeComponent();

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

                    if(String.IsNullOrEmpty(tenderReferenceInput.Text) && String.IsNullOrEmpty(tenderTitleInput.Text) && selectedStation == "" && closingdatefrom == "" && closingdateto == ""
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
            }
    }
}