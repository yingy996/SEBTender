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
	public partial class userInfoPage : ContentPage
	{
		public userInfoPage ()
		{
			InitializeComponent ();
		}

        async void onEditCompanyBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new editContactPage());
        }

        async void onEditContactBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new editContactPage());
        }

        async void onEditLicenseBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new editContactPage());
        }

        async void onEditCIDBBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new editContactPage());
        }

        async void onChangePasswordBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new editContactPage());
        }
    }
}