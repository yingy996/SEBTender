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
            InitializeComponent();

            /*
            //Sending HTTP request to obtain the company profile data
            Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.GetRequest("http://www2.sesco.com.my/etender/vendor/vendor_contact_edit.jsp", true));
            var httpResult = httpTask.Result.ToString();

            //Extract company profile data from the response
            var profileData = DataExtraction.getWebData(httpResult, "userContactPerson");
            ContactPerson profile = (ContactPerson)profileData;
            
            name.Text = profile.Name;
            telephoneNo.Text = profile.TelephoneNo;
            faxNo.Text = profile.FaxNo;
            emailAddress.Text = profile.EmailAddress;*/
        }

        private async Task onUpdateBtnClicked(object sender, EventArgs e)
        {
            /*bool updateError = false;
            
            if (String.IsNullOrWhiteSpace(oldpass.Text))
            {
                await DisplayAlert("Invalid Password", "Old Password cannot be empty.", "OK");
                updateError = true;
            }
            else if (newpass.Text.Length < 6)
            {
                await DisplayAlert("Error", "Password must contains at least 6 character.", "OK");
                updateError = true;
            }
            else if (newpass.Text != renewpass.Text)
            {
                await DisplayAlert("Error", "Retype Password and New Password must be the same.", "OK");
                updateError = true;
            }*/
            
            Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.ChangePasswordRequest("http://www2.sesco.com.my/etender/vendor/vendor_change_password.jsp?check=yes", oldpass.Text, newpass.Text, renewpass.Text));
            var httpTaskResult = httpTask.Result.ToString();
            //Console.WriteLine(httpTaskResult);
                
            //Extract response message data from Change Password page
            var responseData = DataExtraction.getWebData(httpTaskResult, "userChangePassword");
            ChangePasswordResponse response = (ChangePasswordResponse)responseData;

            //Console.WriteLine(response.ErrorMessage);
            bool errPressence = response.ErrorPressence;

            //Console.WriteLine("Error Message : " + errMessage);

            if (errPressence == true)
            {
                string errMessage = response.ErrorMessage;
                await DisplayAlert("Change Password Error", errMessage, "OK");

                var page = App.Current.MainPage as rootPage;
                var refreshPage = new userChangePassword();
                page.changePage(refreshPage);
            }

            if (errPressence == false)
            {
                //await DisplayAlert("Success", "Your password has been successfully changed. Please re-login with your new password.", "OK");
                /*
                //App.Current.MainPage = new rootPage();
                var page = App.Current.MainPage as rootPage;
                var relogPage = new relogPage();
                page.changePage(relogPage);
                */
                App.Current.MainPage = new rootPage { Detail = new NavigationPage(new relogPage()) };
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