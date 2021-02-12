using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Firebase.Messaging;
using System.Threading.Tasks;

namespace FCMPushNotifications.Droid
{
    [Activity(Label = "FCMPushNotifications", Icon = "@mipmap/icon", Theme = "@style/MainTheme",
        MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode
        | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        AndroidNotificationManager NotificationManager;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Firebase.FirebaseApp.InitializeApp(this);

            LoadApplication(new App());

            NotificationManager = new AndroidNotificationManager();
            NotificationManager.Initialize();
            if (NotificationManager.IsPlayServicesAvailable())
            {
                NotificationManager.CreateNotificationChannel();
            }
            
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnStart()
        {
            base.OnStart();
            Task.Run(()=>
            {
                Task task = Task.Delay(1000)
                .ContinueWith(t => FirebaseMessaging.Instance.SubscribeToTopic(NotificationManager.Topic));
            });
        }
    }
}