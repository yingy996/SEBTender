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
    public partial class editCompanyProfile : ContentPage
    {
        string name, regno, coucode;

        public editCompanyProfile()
        {
            BindingContext = this;
            InitializeComponent();
            
            //Sending HTTP request to obtain the company profile data
            Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.GetRequest("http://www2.sesco.com.my/etender/vendor/vendor_company_edit.jsp", true));
            var httpResult = httpTask.Result.ToString();

            //Extract company profile data from the response
            var profileData = DataExtraction.getWebData(httpResult, "userCompanyProfile");
            CompanyProfile profile = (CompanyProfile)profileData;

            companyName.Text = profile.CompanyName;
            companyRegistrationNo.Text = profile.CompanyRegistrationNo;
            mailingAddress.Text = profile.MailingAddress;
            country.Text = profile.Country;

            name = profile.CompanyName;
            regno = profile.CompanyRegistrationNo;
            coucode = profile.Country;
        }

        private async Task onUpdateBtnClicked(object sender, EventArgs e)
        {
            //Sending HTTP request to update Mailing Address
            Task<string> httpTask = Task.Run<string>(() => HttpRequestHandler.EditCompanyProfileRequest("http://www2.sesco.com.my/etender/vendor/vendor_company_editSubmit.jsp", name, regno, mailingAddress.Text, coucode));
            var httpTaskResult = httpTask.Result.ToString();
            Console.WriteLine(httpTaskResult);

            if (httpTaskResult == "OK")
            {
                await DisplayAlert("Success", "Company mailing address has been updated.", "OK");

            }

            var page = App.Current.MainPage as rootPage;
            var userInfoPage = new userInfoPage();
            page.changePage(userInfoPage);
        }
        
        private void onCancelBtnClicked(object sender, EventArgs e)
        {
            var page = App.Current.MainPage as rootPage;
            var userInfoPage = new userInfoPage();
            page.changePage(userInfoPage);
        }
    }

    public class CompanyProfile
    {
        public string CompanyName { get; set; }

        public string CompanyRegistrationNo { get; set; }

        public string MailingAddress { get; set; }

        public string Country { get; set; }
    }
}