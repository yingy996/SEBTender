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
		public userCalendar ()
		{
			InitializeComponent ();

            //SfCalendar calendar = new SfCalendar();
            //this.Content = calendar;

            GetBookmarkedTender();
		}

        private void GetBookmarkedTender()
        {
            SfCalendar calendar = new SfCalendar();
            calendar.ShowInlineEvents = true;

            //string closingdate = "2018-04-30 00:00:00";   //working date format
            //string closingdate = "30-05-2018 at 3:00 PM"; //not working *ORIGINAL TEXT FROM SESCO TENDER WEBSITE
            //string closingdate = "30-05-2018 3:00 PM"; //working
            string closingdate = "30-05-2018  3:00 PM";

            //CultureInfo MyCultureInfo = CultureInfo.CurrentCulture; //not working
            CultureInfo MyCultureInfo = new CultureInfo("fr-FR");

            CalendarInlineEvent events = new CalendarInlineEvent();
            events.StartTime = DateTime.Parse(closingdate, MyCultureInfo);
            events.EndTime = events.StartTime;
            //events.EndTime = DateTime.Parse(closingdate, MyCultureInfo);
            events.Subject = "Medamit to Limbang Town 132kV Transmission Line Pr...";

            CalendarEventCollection collection = new CalendarEventCollection();
            collection.Add(events);

            calendar.DataSource = collection;

            this.Content = calendar;

            Console.WriteLine("CLOSING DATE!!!! CLOSING DATE!!!! CLOSING DATE!!!!:" + DateTime.Parse(closingdate,MyCultureInfo));
        }

    }
}