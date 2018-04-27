using Newtonsoft.Json;
using Syncfusion.SfCalendar.XForms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SEBeTender
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class userCalendar : ContentPage
	{
        //private Random rnd = new Random();

        public userCalendar ()
		{
			InitializeComponent ();
            BindingContext = this;
            //SfCalendar calendar = new SfCalendar();
            //this.Content = calendar;

            if (userSession.username != "")
            {
                RetrieveBookmarkedTender();
            }
            else
            {
                DisplayAlert("Error", "Please login to view your tender bookmark calendar!", "OK");
            }

            //DisplayBookmarkedTender();
		}

        async void RetrieveBookmarkedTender()
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostTenderBookmark(userSession.username));
            var httpResult = httpTask;

            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;

            if (httpResult != null)
            {
                if (httpResult != "No bookmark found")
                {
                    List<tenderBookmark> tenderBookmarks = JsonConvert.DeserializeObject<List<tenderBookmark>>(httpResult);
                    //listView.ItemsSource = tenderBookmarks;

                    DisplayBookmarkedTender(tenderBookmarks);

                }

            }
            else
            {
                Console.WriteLine("bookmark is not null ");
            }
        }

        private void DisplayBookmarkedTender(List<tenderBookmark> tenderBookmarks)
        {
            //SfCalendar calendar = new SfCalendar();
            calendar.ShowInlineEvents = true;

            CalendarEventCollection collection = new CalendarEventCollection();

            //string closingdate = "2018-04-30 00:00:00";   //working date format
            //string closingdate = "30-05-2018 at 3:00 PM"; //not working *ORIGINAL TEXT FROM SESCO TENDER WEBSITE
            //string closingdate = "30-05-2018 3:00 PM"; //working
            //string closingdate = "30-05-2018  3:00 PM";

            //CultureInfo MyCultureInfo = CultureInfo.CurrentCulture; //not working
            //CultureInfo MyCultureInfo = new CultureInfo("fr-FR");

            foreach (var bookmark in tenderBookmarks)
            {
                CalendarInlineEvent events = new CalendarInlineEvent();
                events.StartTime = DateTime.Parse(bookmark.closingDate);
                events.EndTime = events.StartTime;
                events.Subject = bookmark.tenderTitle;

                // random event color
                //Color randomColor = Color.FromRgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                events.Color = Color.Navy;

                collection.Add(events);
            }
            /*
            CalendarInlineEvent events = new CalendarInlineEvent();
            events.StartTime = DateTime.Parse(closingdate, MyCultureInfo);
            events.EndTime = events.StartTime;
            //events.EndTime = DateTime.Parse(closingdate, MyCultureInfo);
            events.Subject = "Medamit to Limbang Town 132kV Transmission Line Pr...";

            CalendarEventCollection collection = new CalendarEventCollection();
            collection.Add(events);
            */
            calendar.DataSource = collection;

            //this.Content = calendar;

            //Console.WriteLine("CLOSING DATE!!!! CLOSING DATE!!!! CLOSING DATE!!!!:" + DateTime.Parse(closingdate,MyCultureInfo));
        }

    }
}