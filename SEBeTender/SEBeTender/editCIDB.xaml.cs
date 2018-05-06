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
	public partial class editCIDB : ContentPage
	{
		public editCIDB ()
		{
            BindingContext = this;
            InitializeComponent ();

            var items = Enumerable.Range(0, 2);
            listView.ItemsSource = items;
            
        }
    }
}