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
	public partial class userInfoPage : ContentPage
	{
		public userInfoPage ()
		{
			InitializeComponent ();
            if (Device.RuntimePlatform == Device.Android)
            {
                notificationBtn.IsVisible = true;

                if (Settings.NotificationSettings == "false")
                {
                    notificationBtn.Text = "Turn on Notification";
                }
            }
		}

        async void onEditCompanyBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new editCompanyProfile());
        }

        async void onEditContactBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new editContactPerson());
        }

        async void onEditLicenseBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new editUPKLicenseNo());
        }

        async void onEditCIDBBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new editCIDB());
        }

        async void onChangePasswordBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new userChangePassword());
        }

        async void onNotificationBtnClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button.Text == "Turn off Notification") {
                var answer = await DisplayAlert("Turn off Notification", "Turn off notification?", "YES", "NO");
                
                if (answer)
                {
                    
                    Settings.NotificationSettings = "false";
                    if (Settings.NotificationSettings == "false")
                    {
                        
                        if (Device.RuntimePlatform == Device.Android)
                        {
                            if (DependencyService.Get<INotificationSubscription>() != null)
                            {
                                DependencyService.Get<INotificationSubscription>().UnsubscribeNotification();
                                ((Button)sender).Text = "Turn on Notification";
                                Console.WriteLine("unsubscribe! DEPENDENCY");
                            } else
                            {
                                Console.WriteLine("NULL DEPENDENCY");
                            }                                                   
                        }
                    }
                }
            } else
            {
                var answer = await DisplayAlert("Turn on Notification", "Turn on notification?", "YES", "NO");

                if (answer)
                {
                    
                    Settings.NotificationSettings = "true";
                    if (Settings.NotificationSettings == "true")
                    {
                        
                        if (Device.RuntimePlatform == Device.Android)
                        {
                            if (DependencyService.Get<INotificationSubscription>() != null)
                            {
                                DependencyService.Get<INotificationSubscription>().SubscribeNotification();
                                ((Button)sender).Text = "Turn off Notification";
                                Console.WriteLine("subscribe! DEPENDENCY");
                            } else
                            {
                                Console.WriteLine("NULL DEPENDENCY");
                            }
                        }
                    }
                }
            }
        }
    }
}