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
	public partial class editContactPage : ContentPage
	{
		public editContactPage ()
		{
			InitializeComponent ();
		}

        async void onUpdateBtnClicked(object sender, EventArgs eventArgs)
        {
            var answer = await DisplayAlert("Confirm update?", "Confirm update contact person details?", "Yes", "No");
        }
	}
}