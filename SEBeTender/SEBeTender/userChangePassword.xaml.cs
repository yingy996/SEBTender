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
	public partial class userChangePassword : ContentPage
	{
		public userChangePassword ()
		{
            BindingContext = this;
            InitializeComponent ();
		}

        private async Task onUpdateBtnClicked(object sender, EventArgs e)
        {
            string httpTask = await Task.Run<string>(() => HttpRequestHandler.ChangePasswordRequest("http://www2.sesco.com.my/etender/vendor/vendor_change_password.jsp?check=yes", oldpass.Text, newpass.Text, renewpass.Text));
            var httpTaskResult = httpTask.ToString();
            //Console.WriteLine(httpTaskResult);

            //Extract response message data from Change Password page
            var responseData = await DataExtraction.getWebData(httpTaskResult, "userChangePassword");
            ChangePasswordResponse response = (ChangePasswordResponse)responseData;

            //Console.WriteLine(response.ErrorMessage);
            bool errPressence = response.ErrorPressence;
            string errMessage = response.ErrorMessage;

            //Console.WriteLine("Error Message : " + errMessage);

            if (!String.IsNullOrWhiteSpace(errMessage))
            {
                await DisplayAlert("Change Password Error", errMessage, "OK");

                var page = App.Current.MainPage as rootPage;
                var refreshPage = new userChangePassword();
                page.changePage(refreshPage);
            }
            else
            {
                //await DisplayAlert("Success", "Your password has been successfully changed. Please re-login with your new password.", "OK");

                //App.Current.MainPage = new rootPage();
                var page = App.Current.MainPage as rootPage;
                var relogPage = new relogPage();
                page.changePage(relogPage);

                //App.Current.MainPage = new rootPage { Detail = new NavigationPage(new relogPage()) };
            }

        }

        private void onCancelBtnClicked(object sender, EventArgs e)
        {
            var page = App.Current.MainPage as rootPage;
            var userInfoPage = new userInfoPage();
            page.changePage(userInfoPage);
        }
    }

    public class ChangePasswordResponse
    {
        public bool ErrorPressence { get; set; }

        public string ErrorMessage { get; set; }

    }
}