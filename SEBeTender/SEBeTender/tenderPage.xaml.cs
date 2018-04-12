﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HtmlAgilityPack;

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
            var label = new Label { Text = "text" };
            //StackLayout stackLayout = new StackLayout();
            //var childToRaise = stackLayout.Children.First();

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
            Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.GetRequest("http://www2.sesco.com.my/etender/notice/notice.jsp", false));
            var httpResult = httpTask.Result.ToString();

            //Small data extraction to get "Next" and "Previous" page hyperlinks
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(httpResult);
            var aNodes = htmlDoc.DocumentNode.SelectNodes("//a");
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
            

            //Extract tender data from the response
            var tenders = DataExtraction.getWebData(httpResult, "tender");
            List<tenderItem> tenderItems = (List<tenderItem>)tenders;

            listView.ItemsSource = tenderItems;
            listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.ItemSelected += onItemSelected;
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
        }
    }
}