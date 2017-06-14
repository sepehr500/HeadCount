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
using Android.Provider;
using Android.Database;
using static Android.Provider.Contacts;
using Java.Lang;
using HeadCount.Classes;
using HeadCount.Core.Models;

namespace HeadCount.Adapters
{

    public class EventViewerAdapter : BaseAdapter<Guest>
    {
        public List<Guest> guestList;

        Activity activity;


        public void UpdateGuestList(List<Guest> list)
        {
            guestList = list;
            NotifyDataSetChanged();
        }
        public void FilterList(List<Guest> list , string search )
        {
        }

        public EventViewerAdapter(Activity activity, List<Guest> data)
        {
            this.activity = activity;
            //filter = new SuggestionsFilter(this);

            guestList = data;


        }

        public override int Count
        {
            get { return guestList.Count; }
        }


        public override Guest this[int position]
        {
            get { return guestList[position]; }
        } 

        public override Java.Lang.Object GetItem(int position)
        {
            return null; // could wrap a Contact in a Java.Lang.Object to return it here if needed
        }

        public override long GetItemId(int position)
        {
            return 12;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.ContactListItem, parent, false);
            var contactName = view.FindViewById<TextView>(Resource.Id.ContactName);
            var contactImage = view.FindViewById<ImageView>(Resource.Id.ContactImage);

            contactName.Text = guestList[position].Name;

            switch (guestList[position].Status)
            {
                case Status.NoResponse:
                    view.SetBackgroundColor(new Android.Graphics.Color(105, 105, 105));
                    break;
                case Status.Yes:
                    view.SetBackgroundColor(new Android.Graphics.Color(34, 139, 34));
                    break;
                case Status.No:
                    view.SetBackgroundColor(new Android.Graphics.Color(255, 0, 0));
                    break;
                case Status.Maybe:
                    view.SetBackgroundColor(new Android.Graphics.Color(255, 255, 0));
                    break;
                default:
                    view.SetBackgroundColor(new Android.Graphics.Color(105, 105, 105));
                    break;
            }


            if ( guestList[position].ImageId== null)
            {
                contactImage = view.FindViewById<ImageView>(Resource.Id.ContactImage);
                contactImage.SetImageResource(Resource.Drawable.ContactImage);
            }
            else
            {
                contactImage.SetImageURI(Android.Net.Uri.Parse(guestList[position].ImageId));
            }
            return view;
        }

    }
}