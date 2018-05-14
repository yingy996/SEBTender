using Newtonsoft.Json;
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
	public partial class manageUserPage : ContentPage
	{
		public manageUserPage ()
		{
			InitializeComponent ();
            //var items = Enumerable.Range(0, 10);
            //listView.ItemsSource = items;
            listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.ItemSelected += onItemSelected;
            getUserList();
        }

        async Task getUserList()
        {
            //Retrieve admin user list from database
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            string httpTask = await Task.Run<string>(() => HttpRequestHandler.getManageAdminUserList());
            var httpResult = httpTask;

            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;
            pageTitle.IsVisible = true;
            if (httpResult != null)
            {
                if (httpResult == "No user found")
                {
                    errorMsg.IsVisible = true;
                } else if (httpResult == "Admin not logged in")
                {
                    errorMsg.Text = httpResult;
                    errorMsg.IsVisible = true;
                } else
                {
                    List<adminUser> adminUsers = JsonConvert.DeserializeObject<List<adminUser>>(httpResult);
                    listView.ItemsSource = adminUsers;
                    upBtn.IsVisible = true;                    
                }

            }
            else
            {
                errorMsg.IsVisible = true;
            }
        }

        async void onItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as tenderBookmark;

            if (item != null)
            {
                listView.SelectedItem = null;
                //await Navigation.PushAsync(new tenderDetailPage(item));
            }
        }

        void onUpButtonClicked()
        {
            //Move to the top of the list when user click on the "Up" button
            if (listView.ItemsSource != null)
            {
                var topItem = listView.ItemsSource.Cast<object>().FirstOrDefault();
                listView.ScrollTo(topItem, ScrollToPosition.Start, true);
            }
        }

        async void onEditBtnTapped(object sender, EventArgs eventArgs)
        {
            var userSelected = ((TappedEventArgs)eventArgs).Parameter;
            adminUser user = (adminUser)userSelected;

            await Navigation.PushAsync(new editUserPage(user));
        }

        async void onDeleteBtnTapped(object sender, EventArgs eventArgs)
        {
            var userSelected = ((TappedEventArgs)eventArgs).Parameter;
            adminUser user = (adminUser)userSelected;

            List<adminUser> tempAdminList = (List<adminUser>)listView.ItemsSource;

            var answer = await DisplayAlert("Remove User", "Are you sure you want to remove user '" + user.Username + "'?", "YES", "NO");

            if (answer)
            {
                //remove user from listview
                foreach (var admin in tempAdminList.ToList())
                {
                    if (admin == user)
                    {
                        int index = tempAdminList.IndexOf(admin);
                        tempAdminList.Remove(admin);
                    }
                }

                //Refresh listview
                listView.ItemsSource = tempAdminList.ToList();

                //Display error message when there are no user
                if (tempAdminList.Count <= 0)
                {
                    errorMsg.Text = "No user found.";
                    errorMsg.IsVisible = true;
                    upBtn.IsVisible = false;
                }

                //Remove user from database
                string httpTask = await Task.Run<string>(() => HttpRequestHandler.deleteAdminUser(user));
                var httpResult = httpTask.ToString();
                Console.WriteLine(httpResult);
                int count = 0;

                while (count < 3 && httpResult != "Success")
                {
                    Console.WriteLine("Looping for failure delete");
                    httpTask = await Task.Run<string>(() => HttpRequestHandler.deleteAdminUser(user));
                    httpResult = httpTask.ToString();
                    count++;
                }
            }
        }
	}
}