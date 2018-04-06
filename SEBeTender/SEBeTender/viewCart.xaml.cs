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
            var items = Enumerable.Range(0, 10);
            listView.ItemsSource = items;
        }
	}
}