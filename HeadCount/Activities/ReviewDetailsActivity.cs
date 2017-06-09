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
using Newtonsoft.Json;
using HeadCount.Core.Models;
using System.Text.RegularExpressions;

namespace HeadCount.Activities
{
    [Activity(Label = "ReviewDetailsActivity")]
    public class ReviewDetailsActivity : Activity
    {
        Event Event;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            Event = JsonConvert.DeserializeObject<Event>(Intent.GetStringExtra("MainData"));

            SetContentView(Resource.Layout.ReviewDetails);

            var NameBox = FindViewById<TextView>(Resource.Id.textView1);
            var MessageBox = FindViewById<TextView>(Resource.Id.textView2);
            var NumberOfGuests = FindViewById<TextView>(Resource.Id.textView3);
            var SendMessageButton = FindViewById<Button>(Resource.Id.button1);

            NameBox.Text = Event.Name;
            MessageBox.Text = Event.Message;
            NumberOfGuests.Text = "Guests: " + Event.Guests.Count;

            SendMessageButton.Click += SendMessageButton_Click;

        }

        private void SendMessageButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

           }
}