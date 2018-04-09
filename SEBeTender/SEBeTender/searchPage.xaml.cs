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
    public partial class searchPage : ContentPage
    {
        public searchPage()
        {
            InitializeComponent();

            //Send Http request to retrieve search page originating station drop down
            Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.GetRequest("http://www2.sesco.com.my/etender/notice/notice_search.jsp"));
            var httpResult = httpTask.Result.ToString();

            //Small data extraction to extract Station dropdown selects/options to fill Picker
            HtmlDocument htmlDoc = new HtmlDocument();
            HtmlNode.ElementsFlags.Remove("option");
            htmlDoc.LoadHtml(httpResult);
            var stationList = new List<string>();
            foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//select[@name='SchStation']//option"))
            {
                stationList.Add(node.InnerText);
                Console.WriteLine("Value=" + node.Attributes["value"].Value);
                Console.WriteLine("InnerText=" + node.InnerText);
                Console.WriteLine();
            }

            stationPicker.ItemsSource = stationList;
            
            Console.WriteLine((string)stationPicker.SelectedItem);

            

        

        //Sending HTTP request to obtain the tender page search result data
        /*Task<string> httpSearchTask = Task.Run<string>(() => HttpRequestHandler.SearchPostRequest("http://www2.sesco.com.my/etender/notice/notice.jsp", tenderReferenceInput.Text, tenderTitleInput.Text, stationPicker.Items[stationPicker.SelectedIndex], ));
        var httpSearchResult = httpTask.Result.ToString();*/


    }
    }


}