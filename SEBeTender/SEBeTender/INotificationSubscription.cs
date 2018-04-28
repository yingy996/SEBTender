using System;
using System.Collections.Generic;
using System.Text;

namespace SEBeTender
{
    public interface INotificationSubscription
    {
        void SubscribeNotification();
        void UnsubscribeNotification();
    }
}
