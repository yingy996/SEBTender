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
            //Send request to retrieve response from Change Password page
            Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.ChangePasswordRequest("http://www2.sesco.com.my/etender/vendor/vendor_change_password.jsp?check=yes", oldpass.Text, newpass.Text, renewpass.Text));
            var httpTaskResult = httpTask.Result.ToString();

            //Extract response message data from Change Password page
            var responseData = DataExtraction.getChangePasswordResponse(httpTaskResult);
            ChangePasswordResponse response = (ChangePasswordResponse)responseData;

            bool errPressence = response.ErrorPressence;
            string errMessage = response.ErrorMessage;

            if (!String.IsNullOrWhiteSpace(errMessage))
            {
                await DisplayAlert("Change Password Error", errMessage, "OK");

                var page = App.Current.MainPage as rootPage;
                var refreshPage = new userChangePassword();
                page.changePage(refreshPage);
            }
            else
            {
                var page = App.Current.MainPage as rootPage;
                var relogPage = new relogPage();
                page.changePage(relogPage);
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