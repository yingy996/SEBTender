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
	public partial class announcementPage : ContentPage
	{
		public announcementPage ()
		{
            BindingContext = this;
            InitializeComponent();
            previousPage.IsVisible = false;
            var items = Enumerable.Range(0, 10);

            listView.ItemsSource = items;
            listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.ItemSelected += onItemSelected;
        }

        void onUpButtonClicked()
        {
            //Move to the top of the list when user click on the "Up" button
            var topItem = listView.ItemsSource.Cast<object>().FirstOrDefault();
            listView.ScrollTo(topItem, ScrollToPosition.Start, true);
        }

        async void onItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            listView.SelectedItem = null;
            /*var item = e.SelectedItem as tenderItem;

            if (item != null)
            {
                listView.SelectedItem = null;
                await Navigation.PushAsync(new tenderDetailPage(item));
            }*/
        }
    }
}