using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;

namespace SEBeTender
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class adminLogoutPage : ContentPage
    {
        public adminLogoutPage()
        {
            InitializeComponent();

            LogoutAdmin();
        }

        async void LogoutAdmin()
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.GetAsync("https://sebannouncement.000webhostapp.com/logout.php");
                Console.WriteLine("Response code: " + response.StatusCode);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    userSession.userLoginCookie = "";
                    Console.WriteLine( "Status code: " + response.StatusCode + ", Logout Successful");

                    logoutStatus.TextColor = Color.Default;
                    logoutStatus.FontAttributes = FontAttributes.None;
                    logoutStatus.Text = "Logout successful, redirecting back to login page.";

                    adminAuth.DeleteCredentials();

                    await Task.Delay(1000);
                    activityIndicator.IsVisible = false;
                    activityIndicator.IsRunning = false;

                    App.Current.MainPage = new rootPage();
                    var page = App.Current.MainPage as rootPage;
                    var loginPage = new loginPage();
                    page.changePage(loginPage);
    
                    //App.Current.MainPage = new rootPage { Detail = new NavigationPage(new loginPage()) };

                }
                else
                {
                    Console.WriteLine("Status code: " + response.StatusCode + ", Logout unsuccessful");

                    logoutStatus.TextColor = Color.Red;
                    logoutStatus.FontAttributes = FontAttributes.None;
                    logoutStatus.Text = "Error: Logout Unsuccessful please try again.";

                    await Task.Delay(1000);
                    activityIndicator.IsVisible = false;
                    activityIndicator.IsRunning = false;

                    var page = App.Current.MainPage as rootPage;
                    var announcementPage = new announcementPage();
                    page.changePage(announcementPage);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}