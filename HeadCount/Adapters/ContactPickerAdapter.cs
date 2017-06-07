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

namespace HeadCount.Adapters
{
    public class Contact
    {
        public long Id { get; set; }

        public string DisplayName { get; set; }

        public string PhoneNumber { get; set; }

        public string PhotoId { get; set; }

        public bool Selected { get; set; } = false;

        public Contact()
        {

        }
        public Contact(Contact contact)
        {
            Id = contact.Id;
            DisplayName = contact.DisplayName;
            PhoneNumber = contact.PhoneNumber;
            PhotoId = contact.PhotoId;
            Selected = contact.Selected;
        }
    }
    public class ContactPickerAdapter : BaseAdapter<Contact>
    {
        public List<Contact> contactList;
        Activity activity;

        public Filter Filter { get; set; }

        public void UpdateContactList(List<Contact> list)
        {
            contactList = list;
            NotifyDataSetChanged();
        }
        public void FilterList(List<Contact> list , string search )
        {
            UpdateContactList(list.Where(x => x.DisplayName.ToLower().Contains(search.ToLower())).ToList());
        }
        public void MarkTrue(string number)
        {
            contactList = contactList.DealSelect(number);
            NotifyDataSetChanged();

        }

        protected List<Contact> DealSelect(List<Contact> item , string number)
        {
            return item.Select(z =>  {
                z.Selected  = z.PhoneNumber == number ? !z.Selected : z.Selected;
                return z;
            }).ToList();
        }

        public ContactPickerAdapter(Activity activity, List<Contact> data)
        {
            this.activity = activity;
            //filter = new SuggestionsFilter(this);

            contactList = data;


        }

        public override int Count
        {
            get { return contactList.Count; }
        }

        public override Contact this[int position]
        {
            get { return contactList[position]; }
        } 

        public override Java.Lang.Object GetItem(int position)
        {
            return null; // could wrap a Contact in a Java.Lang.Object to return it here if needed
        }

        public override long GetItemId(int position)
        {
            return contactList[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.ContactListItem, parent, false);
            var contactName = view.FindViewById<TextView>(Resource.Id.ContactName);
            var contactImage = view.FindViewById<ImageView>(Resource.Id.ContactImage);

            contactName.Text = contactList[position].DisplayName;

            var selectColor = contactList[position].Selected ? new Android.Graphics.Color(34, 139, 34) : new Android.Graphics.Color(0, 0, 225);

            view.SetBackgroundColor(selectColor);

            if (contactList[position].PhotoId == null)
            {
                contactImage = view.FindViewById<ImageView>(Resource.Id.ContactImage);
                contactImage.SetImageResource(Resource.Drawable.ContactImage);
            }
            else
            {
                contactImage.SetImageURI(Android.Net.Uri.Parse(contactList[position].PhotoId));
            }
            return view;
        }

    }
}