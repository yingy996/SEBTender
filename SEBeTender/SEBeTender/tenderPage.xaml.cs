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
	public partial class tenderPage : ContentPage
	{
		public tenderPage ()
		{
            BindingContext = this;

			InitializeComponent ();

            //Sending HTTP request to obtain the tender page data
            Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.GetRequest("http://html-agility-pack.net/"));
            var httpResult = httpTask.Result.ToString();
            //Console.WriteLine(httpResult);
            
            //Extract tender data from the response
            var tenders = DataExtraction.getWebData(httpResult, "tender");
            //Console.WriteLine(tenders);
            List<tenderItem> tenderItems = (List<tenderItem>)tenders;
            string test = tenderItems.ElementAt(0).OriginatingStation;
            //Console.WriteLine(test);
            var items = Enumerable.Range(0, 10);
            listView.ItemsSource = tenderItems;
            listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.ItemSelected += onItemSelected;
        }

        void onItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as tenderItem;
            
            //if (item != null)
            //{
                listView.SelectedItem = null;
                
            //}
        }

        void onUpButtonClicked()
        {
            var topItem = listView.ItemsSource.Cast<object>().FirstOrDefault();
            listView.ScrollTo(topItem, ScrollToPosition.Start, true);
        }
    }
}