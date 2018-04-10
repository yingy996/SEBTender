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
            var items = Enumerable.Range(0, 3);
            listView.ItemsSource = items;

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