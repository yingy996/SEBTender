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
            
            editTitle.Text = "aaaa";
            editContent.Text = "aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa aaaa";

            Console.WriteLine(getAnnouncementsResult());

        }



    }
}