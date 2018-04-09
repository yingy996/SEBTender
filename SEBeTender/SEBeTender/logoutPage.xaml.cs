using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SEBeTender
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class logoutPage : ContentPage
	{
        public logoutPage ()
		{
			InitializeComponent ();
            LogoutUser();
		}

        void LogoutUser()
        {
            var httpTask = HttpRequestHandler.PostUserLogout();
            var httpResult = httpTask.ToString();

            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;
            
        }
    }
}