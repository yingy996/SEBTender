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
	public partial class editContactPerson : ContentPage
	{
        public editContactPerson ()
		{
            BindingContext = this;
            InitializeComponent();

            //Sending HTTP request to obtain the contact person data
            Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.GetRequest("http://www2.sesco.com.my/etender/vendor/vendor_contact_edit.jsp", true));
            var httpResult = httpTask.Result.ToString();

            //Extract contact person data from the response
            var profileData = DataExtraction.getWebData(httpResult, "userContactPerson");
            ContactPerson profile = (ContactPerson)profileData;

            name.Text = profile.Name;
            telephoneNo.Text = profile.TelephoneNo;
            faxNo.Text = profile.FaxNo;
            emailAddress.Text = profile.EmailAddress;
        }

        private async Task onUpdateBtnClicked(object sender, EventArgs e)
        {
            bool updateError = false;

            if (String.IsNullOrWhiteSpace(emailAddress.Text))
            {
                await DisplayAlert("Invalid Email", "Email Address is required.", "OK");
                updateError = true;
            } 
            else if (IsValidEmail(emailAddress.Text) == false)
            {
                await DisplayAlert("Invalid Email", "Invalid Email Address format.", "OK");
                updateError = true;
            }

            if (updateError == false)
            {
                //Sending HTTP request to update Mailing Address
                Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.EditContactPersonRequest("http://www2.sesco.com.my/etender/vendor/vendor_contact_editSubmit.jsp", name.Text, telephoneNo.Text, faxNo.Text, emailAddress.Text));
                var httpTaskResult = httpTask.Result.ToString();
                Console.WriteLine(httpTaskResult);

                if (httpTaskResult == "OK")
                {
                    await DisplayAlert("Success", "Contact person info has been updated.", "OK");
                }

                var page = App.Current.MainPage as rootPage;
                var userInfoPage = new userInfoPage();
                page.changePage(userInfoPage);
            }
        }

        private void onCancelBtnClicked(object sender, EventArgs e)
        {
            var page = App.Current.MainPage as rootPage;
            var userInfoPage = new userInfoPage();
            page.changePage(userInfoPage);
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }

    public class ContactPerson
    {
        public string Name { get; set; }

        public string TelephoneNo { get; set; }

        public string FaxNo { get; set; }

        public string EmailAddress { get; set; }
    }
}