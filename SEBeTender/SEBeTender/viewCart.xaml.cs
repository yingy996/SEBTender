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
	public partial class viewCart : ContentPage
	{

        public viewCart ()
        {   
            BindingContext = this;
			InitializeComponent ();

            //Output of dummy data
            ///var items = Enumerable.Range(0, 3);
            ///listView.ItemsSource = items;

            //Sending HTTP request to obtain the cart page data
            Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.GetRequest("http://www2.sesco.com.my/etender/vendor/vendor_view_cart.jsp"));
            var httpResult = httpTask.Result.ToString();

            //Extract cart data from the response
            var cart = DataExtraction.getWebData(httpResult, "cartpage");
            List<cartItem> cartItems = (List<cartItem>)cart;

            listView.ItemsSource = cartItems;
            listView.SeparatorVisibility = SeparatorVisibility.None;
            
        }

        async void onConfirmButtonClicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Confirmation", "Are you sure you want to checkout the cart items?", "Confirm", "Cancel");
            
            if (answer == true)
            {
                await Navigation.PushAsync(new checkoutPage());
            }
        }

        async void onClearCartClicked(object sender, EventArgs e)
        {
            listView.ItemsSource = null;
            
        }

    }
}