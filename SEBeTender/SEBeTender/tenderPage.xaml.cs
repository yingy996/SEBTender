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

            var items = Enumerable.Range(0, 10);
            listView.ItemsSource = items;
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