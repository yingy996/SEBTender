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

            //Sending HTTP request to obtain the company profile data
            Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.GetRequest("http://www2.sesco.com.my/etender/vendor/vendor_contact_edit.jsp", true));
            var httpResult = httpTask.Result.ToString();

            //Extract company profile data from the response
            var profileData = DataExtraction.getWebData(httpResult, "userContactPerson");
            ContactPerson profile = (ContactPerson)profileData;

            name.Text = profile.Name;
            telephoneNo.Text = profile.TelephoneNo;
            faxNo.Text = profile.FaxNo;
            emailAddress.Text = profile.EmailAddress;
        }

        private void onUpdateBtnClicked(object sender, EventArgs e)
        {
            bool updateError = false;

            if (String.IsNullOrWhiteSpace(emailAddress.Text))
            {
                DisplayAlert("Invalid Email", "Email Address is required.", "OK");
                updateError = true;
            } 
            
            if (IsValidEmail(emailAddress.Text) == false)
            {
                DisplayAlert("Invalid Email", "Invalid Email Address format.", "OK");
                updateError = true;
            }

            if (updateError == false)
            { 

                var page = App.Current.MainPage as rootPage;
                var userInfoPage = new userInfoPage();
                page.changePage(userInfoPage);
                //Application.Current.MainPage.Navigation.PopAsync();
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