using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SEBeTender
{
	public class rootPage : MasterDetailPage
	{
        menuPage menuPage;
         
		public rootPage ()
		{
            menuPage = new menuPage();
            Master = menuPage;
            var displayPage = new tenderPage(); 
            Detail = new NavigationPage(displayPage);   
            
            menuPage.menu.ItemSelected += onItemSelected;
            
		}

        public rootPage (bool isLoggedIn)
        {
            //set the menu list items for logged in user
            menuData data = new menuData(true);
            menuPage = new menuPage();
            menuPage.menu.ItemsSource = data;

            Master = menuPage;
            var displayPage = new tenderEligiblePage();
            Detail = new NavigationPage(displayPage);

            menuPage.menu.ItemSelected += onItemSelected;
        }

        void onItemSelected (object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as menuItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                menuPage.menu.SelectedItem = null;
                IsPresented = false;
            }
        }

	}
}