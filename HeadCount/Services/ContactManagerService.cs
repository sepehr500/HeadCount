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
using HeadCount.Adapters;
using Android.Provider;

namespace HeadCount.Services
{
    static class ContactManagerService
    {
        public static List<Contact> getContacts(Activity activity)
        {
            var uri = ContactsContract.Contacts.ContentUri;

            var contactList = new List<Contact>();
            string[] projection = {
                ContactsContract.Contacts.InterfaceConsts.Id,
                ContactsContract.Contacts.InterfaceConsts.DisplayName,
                ContactsContract.Contacts.InterfaceConsts.PhotoId
            };

            // ManagedQuery is deprecated in Honeycomb (3.0, API11)
            //var cursor = activity.ManagedQuery (uri, projection, null, null, null);

            // ContentResolver requires you to close the query yourself
            //USE THIS ONE
            //var cursor = activity.ContentResolver.Query(uri, projection, null, null, null);
            //int indexNumber =   cursor.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number);

            var phones = activity.ContentResolver.Query(ContactsContract.CommonDataKinds.Phone.ContentUri, null, null, null, null);

            contactList = new List<Contact>();
            while (phones.MoveToNext())
            {
                contactList.Add(new Contact()
                {
                    DisplayName = phones.GetString(phones.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.DisplayName)),
                    PhoneNumber = phones.GetString(phones.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number)),
                    PhotoId = phones.GetString(phones.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.PhotoUri))
                });

            }
            phones.Close();
            return contactList;
            // CursorLoader introduced in Honeycomb (3.0, API11)
            //var loader = new CursorLoader(activity, uri, projection, null, null, null);
            //var cursor = (ICursor)loader.LoadInBackground();


            //if (cursor.MoveToFirst())
            //{
            //    do
            //    {
            //        contactList.Add(new Contact
            //        {
            //            Id = cursor.GetLong(cursor.GetColumnIndex(projection[0])),
            //            DisplayName = cursor.GetString(cursor.GetColumnIndex(projection[1])),
            //            PhotoId = cursor.GetString(cursor.GetColumnIndex(projection[2]))
            //        });
            //    } while (cursor.MoveToNext());
            //}

        }
    }
}