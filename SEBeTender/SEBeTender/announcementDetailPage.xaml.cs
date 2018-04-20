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
    public partial class announcementDetailPage : ContentPage
    {
        public announcementDetailPage()
        {

        }
        public announcementDetailPage(RootObject announcementItem)
        {
            InitializeComponent();



            announcementTitlelbl.Text = announcementItem.announcementTitle;
            announcementContentlbl.Text = announcementItem.announcementContent;
            publishedDatelbl.Text = announcementItem.publishedDate;
            postedBylbl.Text = announcementItem.postedBy;
            editedDatelbl.Text = announcementItem.editedDate.ToString();
            editedBylbl.Text = announcementItem.editedBy.ToString();



        }
    }
}