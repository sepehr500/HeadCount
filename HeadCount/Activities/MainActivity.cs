using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using HeadCount.Core.Data;
using Android.Provider;
using Android.Content;
using HeadCount.Adapters;
using HeadCount.Activities;

namespace HeadCount
{
    [Activity(Label = "HeadCount", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var db = new DatabaseManager();

            // Set our view from the "main" layout resource
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Main);
            Button button = FindViewById<Button>(Resource.Id.button1);
            button.Click += delegate {
                StartActivity(typeof(FillDetailsActivity));
            };
        }
    }
}

