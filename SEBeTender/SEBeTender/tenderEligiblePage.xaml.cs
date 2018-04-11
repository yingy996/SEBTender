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
	public partial class tenderEligiblePage : ContentPage
	{
		public tenderEligiblePage ()
		{
            BindingContext = this;
            InitializeComponent ();
            var items = Enumerable.Range(0, 10);

            //Sending HTTP request to obtain the tender page data
            Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.GetRequest("http://www2.sesco.com.my/etender/vendor/vendor_tender_eligible.jsp", true));
            var httpResult = httpTask.Result.ToString();
            Console.WriteLine(httpResult);
            //Extract tender data from the response
            var tenders = DataExtraction.getWebData(httpResult, "eligibelTenderPage");
            List<tenderItem> tenderItems = (List<tenderItem>)tenders;

            listView.ItemsSource = tenderItems;
            listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.ItemSelected += onItemSelected;
            
        }

        async void onItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
            var item = e.SelectedItem as tenderItem;
            Console.WriteLine("Item clicked here");
            if (item != null)
            {
                listView.SelectedItem = null;
                //await Navigation.PushAsync(new tenderDetailPage(item));
            }
        }

        void Test(string param)
        {
            Console.WriteLine("Parameter: " + param);
        }

        void onUpButtonClicked()
        {
            //Move to the top of the list when user click on the "Up" button
            var topItem = listView.ItemsSource.Cast<object>().FirstOrDefault();
            listView.ScrollTo(topItem, ScrollToPosition.Start, true);
        }

        void OnCartTapped(object sender, EventArgs args)
        {
            var tenderSelected = ((TappedEventArgs)args).Parameter;
            tenderItem tender = (tenderItem)tenderSelected;
            DisplayAlert("Success", tender.AddToCartQuantity + " Item " + tender.Reference + " has been successfully added to cart", "OK");
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