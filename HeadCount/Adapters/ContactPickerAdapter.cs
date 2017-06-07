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
        public List<Contact> contactListCopy;
        Activity activity;

        public Filter Filter { get; set; }

        public void updateContactList(List<Contact> list)
        {
            this.contactList = list.ConvertAll(item => new Contact(item));
            NotifyDataSetChanged();
        }

        public ContactPickerAdapter(Activity activity, List<Contact> data)
        {
            this.activity = activity;
            //filter = new SuggestionsFilter(this);

            contactList = data;

            Filter = new ContactFilter(this);
            contactListCopy = contactList;

        }

        public override int Count
        {
            get { return contactList.Count; }
        }

        public override Contact this[int position]
        {
            get { return contactListCopy[position]; }
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

                //var contactUri = ContentUris.WithAppendedId(ContactsContract.Contacts.ContentUri, contactList[position].Id);
                //var contactPhotoUri = Android.Net.Uri.WithAppendedPath(contactUri, Contacts.Photos.ContentDirectory);


                contactImage.SetImageURI(Android.Net.Uri.Parse(contactList[position].PhotoId));
            }
            return view;
        }

        //void FillContacts()
        //{
        //    var uri = ContactsContract.Contacts.ContentUri;

        //    string[] projection = {
        //        ContactsContract.Contacts.InterfaceConsts.Id,
        //        ContactsContract.Contacts.InterfaceConsts.DisplayName,
        //        ContactsContract.Contacts.InterfaceConsts.PhotoId
        //    };

        //    // ManagedQuery is deprecated in Honeycomb (3.0, API11)
        //    //var cursor = activity.ManagedQuery (uri, projection, null, null, null);

        //    // ContentResolver requires you to close the query yourself
        //    //USE THIS ONE
        //    //var cursor = activity.ContentResolver.Query(uri, projection, null, null, null);
        //    //int indexNumber =   cursor.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number);

        //    var phones = activity.ContentResolver.Query(ContactsContract.CommonDataKinds.Phone.ContentUri, null, null, null, null);

        //    contactList = new List<Contact>();
        //    while (phones.MoveToNext())
        //    {
        //        contactList.Add(new Contact
        //        {
        //            DisplayName = phones.GetString(phones.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.DisplayName)),
        //            PhoneNumber = phones.GetString(phones.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number)),
        //            PhotoId= phones.GetString(phones.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.PhotoUri))
        //        });

        //    }
        //    phones.Close();
        //    // CursorLoader introduced in Honeycomb (3.0, API11)
        //    //var loader = new CursorLoader(activity, uri, projection, null, null, null);
        //    //var cursor = (ICursor)loader.LoadInBackground();


        //    //if (cursor.MoveToFirst())
        //    //{
        //    //    do
        //    //    {
        //    //        contactList.Add(new Contact
        //    //        {
        //    //            Id = cursor.GetLong(cursor.GetColumnIndex(projection[0])),
        //    //            DisplayName = cursor.GetString(cursor.GetColumnIndex(projection[1])),
        //    //            PhotoId = cursor.GetString(cursor.GetColumnIndex(projection[2]))
        //    //        });
        //    //    } while (cursor.MoveToNext());
        //    //}
        //}
    }

    class ContactFilter : Filter
    {
        private readonly ContactPickerAdapter contactPickerAdapter;

        public ContactFilter(ContactPickerAdapter contactPickerAdapter)
        {
            this.contactPickerAdapter = contactPickerAdapter;
        }

        protected override FilterResults PerformFiltering(ICharSequence constraint)
        {
            var returnObj = new FilterResults();
            var results = new List<Contact>();
            if (this.contactPickerAdapter.contactList == null)
            {
                this.contactPickerAdapter.contactList = this.contactPickerAdapter.contactListCopy;
            }
            if (constraint == null) return returnObj;
            if (contactPickerAdapter.contactList != null && contactPickerAdapter.contactList.Any())
            {
                results.AddRange(contactPickerAdapter.contactList
                       .Where(item => item.DisplayName.ToLower().Contains(constraint.ToString())));

            }
            returnObj.Values = FromArray(results.Select(r => r.ToJavaObject()).ToArray());
            returnObj.Count = results.Count;
            constraint.Dispose();
            return returnObj;
        }


         public class JavaHolder : Java.Lang.Object
        {
            public readonly object Instance;

            public JavaHolder(object instance)
            {
                Instance = instance;
            }
        }

        protected override void PublishResults(ICharSequence constraint, FilterResults results)
        {
            using (var values = results.Values)
                contactPickerAdapter.contactListCopy= values.ToArray<Java.Lang.Object>()
                    .Select(r => r.ToNetObject<Contact>()).ToList();

            contactPickerAdapter.NotifyDataSetChanged();

            // Don't do this and see GREF counts rising
            constraint.Dispose();
            results.Dispose();
        }
    }
}