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
            
            listView.ItemsSource = items;
            listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.ItemSelected += onItemSelected;
        }

        async void onItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            listView.SelectedItem = null;
            //var item = e.SelectedItem as tenderItem;

            /*if (item != null)
            {
                
                await Navigation.PushAsync(new tenderDetailPage(item));
            }*/
        }

        void OnCartTapped(object sender, EventArgs args)
        {
            Console.WriteLine("Im tapped!");
            DisplayAlert("Success", "Item has been successfully added to cart", "OK");
        }
    }
}