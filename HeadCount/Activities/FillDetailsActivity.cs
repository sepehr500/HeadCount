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
using HeadCount.Core.Models;
using Newtonsoft.Json;

namespace HeadCount.Activities
{
    [Activity(Label = "FillDetailsActivity")]
    public class FillDetailsActivity : Activity
    {

        public Event Event { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Event = new Event();

            SetContentView(Resource.Layout.FillDetails);

            var NameBox = FindViewById<EditText>(Resource.Id.EventNameBox);
            var InviteBox = FindViewById<EditText>(Resource.Id.InviteBox);
            var NextButton = FindViewById<Button>(Resource.Id.button1);

            NameBox.TextChanged += NameBox_TextChanged;

            InviteBox.TextChanged += InviteBox_TextChanged;

            NextButton.Click += NextButton_Click;

       }

        private void NextButton_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this , typeof(SelectContactsActivity));
            intent.PutExtra("MainData", JsonConvert.SerializeObject(Event));
            StartActivity(intent);
        }

        private void InviteBox_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            Event.Message = e.Text.ToString();
        }

        private void NameBox_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            Event.Name = e.Text.ToString();
        }
    }
}