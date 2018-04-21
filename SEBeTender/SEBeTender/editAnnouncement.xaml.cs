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
		public editAnnouncement ()
		{
            BindingContext = this;
            InitializeComponent();
            
            //editTitle.Text = "aaaa";
            //editContent.Text = "aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa";

            //Console.WriteLine(getAnnouncementPostData());


        }

        async Task<string> getAnnouncementPostData()
        {
            try
            {

                HttpClient client = new HttpClient();

                //client.BaseAddress = new Uri("https://sebannouncement.000webhostapp.com/");

                var response = await client.GetAsync("https://sebannouncement.000webhostapp.com/getAnnouncementMobile.php");

                string result = response.Content.ReadAsStringAsync().Result;

                return result;

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.ToString(), "Ok");
                return null;
            }


        }

    }
}