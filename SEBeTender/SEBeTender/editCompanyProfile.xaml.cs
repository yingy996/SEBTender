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
            companyRegistreationNo.Text = profile.CompanyRegistreationNo;
            mailingAddress.Text = profile.MailingAddress;
            country.Text = profile.Country;
        }

        private void onUpdateBtnClicked(object sender, EventArgs e)
        {


            var page = App.Current.MainPage as rootPage;
            var userInfoPage = new userInfoPage();
            page.changePage(userInfoPage);
            //Application.Current.MainPage.Navigation.PopAsync();
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

        public string CompanyRegistreationNo { get; set; }

        public string MailingAddress { get; set; }

        public string Country { get; set; }
    }
}