using System;
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App; 

namespace FCMPushNotifications.Droid
{
    public class AndroidNotificationManager
    {
        private const string channelId = "fcmpushnotifications_channel_id";
        private const string topic = "fcmpushnotifications_channel_topic";
        private const string channelName = "fcmpushnotifications_channel";
        private const string channelDescription = "The default channel for notifications.";
        private const string titleKey = "title";
        private const string messageKey = "message";
        private const int notificationId = 100;

        bool channelInitialized = false;
        int messageId = 0;
        int pendingIntentId = 0;

        NotificationManager manager;

        public static AndroidNotificationManager Instance { get; private set; }

        public string ChannelId => channelId;

        public string ChannelName => channelName;

        public string ChannelDescription => channelDescription;

        public string TitleKey => titleKey;

        public string MessageKey => messageKey;

        public int NotificationId => notificationId;

        public string Topic => topic;

        public void Initialize()
        {
            Instance = this;
        }
        public bool IsPlayServicesAvailable()
        {
            string errorOccured = string.Empty;
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(Android.App.Application.Context);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    errorOccured = GoogleApiAvailability.Instance.GetErrorString(resultCode);

                return false;
            }
            else
            {
                return true;
            }
        }

        public void SendNotification(string title, string message, DateTime? notifyTime = null)
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }
            Intent intent = new Intent(Android.App.Application.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);

            PendingIntent pendingIntent = PendingIntent.GetActivity(Android.App.Application.Context, pendingIntentId++, intent, PendingIntentFlags.UpdateCurrent);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(Android.App.Application.Context, ChannelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(Android.App.Application.Context.Resources, Resource.Drawable.icon))
                .SetSmallIcon(Resource.Drawable.icon)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

            Notification notification = builder.Build();
            manager.Notify(messageId++, notification);
        }
        
        public void CreateNotificationChannel()
        {
            manager = (NotificationManager)Android.App.Application.Context.GetSystemService(Android.App.Application.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(ChannelName);
                var channel = new NotificationChannel(ChannelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = ChannelDescription
                };
                manager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;
        }
    }
}
