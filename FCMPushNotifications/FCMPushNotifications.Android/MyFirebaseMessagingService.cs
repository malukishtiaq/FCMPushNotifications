using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Firebase.Messaging;

namespace FCMPushNotifications.Droid
{
    [Service(Name = "FCMPushNotifications.Droid.MyFirebaseMessagingService")]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        public override void OnNewToken(string p0)
        {
            base.OnNewToken(p0);
            Console.WriteLine($"FCM Token {p0}");
        }
        public override void OnMessageReceived(RemoteMessage message)
        {
            AndroidNotificationManager androidNotification = new AndroidNotificationManager();

            androidNotification.SendNotification(message.From, message.GetNotification().Body);
        }
    }
}
