using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

[assembly: Xamarin.Forms.Dependency(typeof(SEBeTender.Droid.NotificationSubscriptionHandler))]
namespace SEBeTender.Droid
{
    public class NotificationSubscriptionHandler : INotificationSubscription
    {
        public NotificationSubscriptionHandler(){}

        public void SubscribeNotification()
        {
            Firebase.Messaging.FirebaseMessaging.Instance.SubscribeToTopic("news");
        }

        public void UnsubscribeNotification()
        {
            Firebase.Messaging.FirebaseMessaging.Instance.UnsubscribeFromTopic("news");
        }
    }
}