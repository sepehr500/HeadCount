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
using Couchbase.Lite;
using HeadCount.Core.Data;
using HeadCount.Adapters;

namespace HeadCount.Activities
{
    [Activity(Label = "EventViewerActivity")]
    public class EventViewerActivity : Activity
    {
        Event Event;

        Database db;

        EventViewerAdapter adapt;

        ListView listView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EventViewer);




            var documentId = Intent.GetStringExtra("MainData");

            Event = GetDocument("1");
            db.Changed += Db_Changed;

            adapt = new EventViewerAdapter(this, Event.Guests);
            listView = FindViewById<ListView>(Resource.Id.ContactsListView);

            listView.Adapter = adapt;
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }


        protected override void OnResume()
        {
            base.OnResume();
            Event = GetDocument("1");
            adapt.UpdateGuestList(Event.Guests);

        }
        public Event GetDocument(string DocumentId)
        {
            var mDatabase = new DatabaseManager();
            db = mDatabase.GetDb();
            Document document = db.GetDocument(DocumentId);
            return (Event)document.Properties["event"];
        }

        private void Db_Changed(object sender, DatabaseChangeEventArgs e)
        {

            Event = GetDocument("1");
            adapt.UpdateGuestList(Event.Guests);
        }
    }
}