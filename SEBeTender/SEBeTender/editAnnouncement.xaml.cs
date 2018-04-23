using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;

namespace SEBeTender
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class editAnnouncement : ContentPage
    {
        string editID = "";
        string updatedTitle, updatedContent = "";
        bool updateTitleError, updateContentError = false;

        public editAnnouncement(string announcementid)
        {
            BindingContext = this;
            InitializeComponent();

            editID = announcementid;
            Task<string> httpTask = Task.Run<string>(() => getEditPageResultAsync(editID).Result);
            Console.WriteLine(httpTask.Result);

            List<RootObject> announcementItem = JsonConvert.DeserializeObject<List<RootObject>>(httpTask.Result);

            editID = announcementItem[0].announcementID;
            editTitle.Text = announcementItem[0].announcementTitle;
            editContent.Text = announcementItem[0].announcementContent;
        }

        public static async Task<string> getEditPageResultAsync(string announcementid)
        {
            string result = "";

            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.GetAsync("http://sebannouncement.000webhostapp.com/getEditPageMobile.php?announcementid=" + announcementid);

                result = response.Content.ReadAsStringAsync().Result;

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;


        }

        private void editTitle_Completed(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(editTitle.Text))
            {
                updatedTitle = editTitle.Text;
                updateTitleError = false;
            }
            else
            {
                updateTitleError = true;
            }

        }

        private void editContent_Completed(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(editContent.Text))
            {
                updatedContent = editContent.Text;
                updateContentError = false;
            }
            else
            {
                updateContentError = true;
            }
        }

        private void onCancelBtnClick(object sender, EventArgs e)
        {
            var page = App.Current.MainPage as rootPage;
            var announcementPage = new announcementPage();
            page.changePage(announcementPage);
            //Application.Current.MainPage.Navigation.PopAsync();
        }

        async void onUpdateBtnClick(object sender, EventArgs e)
        {
            if (updateTitleError == true)
            {
                await DisplayAlert("Invalid Title", "Title cannot be empty.", "OK");
            }

            if (updateContentError == true)
            {
                await DisplayAlert("Invalid Content", "Content cannot be empty.", "OK");
            }

            if (updateTitleError == false && updateContentError == false)
            {
                if (adminAuth.Username == null)
                {
                    Console.WriteLine("No username");
                }
                else
                {
                    Console.WriteLine("Test Username:" + adminAuth.Username);
                    Console.WriteLine("TEST Password:" + adminAuth.Password);
                }

                if (adminAuth.Password == null)
                {
                    Console.WriteLine("No PASSWORD");
                }

                if (adminAuth.Username != null && adminAuth.Password != null)
                {
                    activityIndicator.IsVisible = true;
                    activityIndicator.IsRunning = true;

                    string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostEditAnnouncement(adminAuth.Username, adminAuth.Password, editID, updatedTitle, updatedContent));
                    string httpResult = httpTask.ToString();

                    activityIndicator.IsVisible = false;
                    activityIndicator.IsRunning = false;

                    if (httpResult == "You have succesfully edited the announcement post!")
                    {
                        await DisplayAlert("Success", httpResult, "OK");
                        var page = App.Current.MainPage as rootPage;
                        var announcementPage = new announcementPage();
                        page.changePage(announcementPage);

                    }
                    else
                    {
                        await DisplayAlert("Failed", httpResult, "OK");
                    }
                }
                else
                {
                    Console.WriteLine("User is not logged in");
                }
            }
        }

    }
}