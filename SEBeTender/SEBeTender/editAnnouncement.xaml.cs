using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//using JsonSystemAlias = System.Json;
//using Xamarin.Auth;
using Newtonsoft.Json;
namespace SEBeTender
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class editAnnouncement : ContentPage
    {
        public editAnnouncement(string announcementid)
        {
            BindingContext = this;
            InitializeComponent();

            //editTitle.Text = "aaaa";
            //editContent.Text = "aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa";

            string testid = "2490165";
            Task<string> httpTask = Task.Run<string>(() => getEditPageResultAsync(testid).Result);
            Console.WriteLine(httpTask.Result);

            List<RootObject> announcementItem = JsonConvert.DeserializeObject<List<RootObject>>(httpTask.Result);

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
    }

    public class RotObject
    {
        public string announcementID { get; set; }
        public string __invalid_name__0 { get; set; }
        public string announcementTitle { get; set; }
        public string __invalid_name__1 { get; set; }
        public string announcementContent { get; set; }
        public string __invalid_name__2 { get; set; }
        public string publishedDate { get; set; }
        public string __invalid_name__3 { get; set; }
        public object editedDate { get; set; }
        public object __invalid_name__4 { get; set; }
        public object editedBy { get; set; }
        public object __invalid_name__5 { get; set; }
        public string postedBy { get; set; }
        public string __invalid_name__6 { get; set; }
    }

}