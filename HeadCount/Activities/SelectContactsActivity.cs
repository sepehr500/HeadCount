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
using HeadCount.Services;
using HeadCount.Classes;
using HeadCount.Core.Models;
using Newtonsoft.Json;

namespace HeadCount.Activities
{
    [Activity(Label = "SelectContactsActivity")]
    public class SelectContactsActivity : Activity
    {
        ContactPickerAdapter adapt;

        List<Contact> MainList;

        ListView contactsListView;


        Event Event;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.SelectContacts);

            MainList = ContactManagerService.getContacts(this);
            adapt = new ContactPickerAdapter(this, Clone(MainList));

            Event = JsonConvert.DeserializeObject<Event>(Intent.GetStringExtra("MainData"));


            contactsListView = FindViewById<ListView>(Resource.Id.ContactsListView);
            var nextButton = FindViewById<Button>(Resource.Id.button1);
            var SearchView = FindViewById<SearchView>(Resource.Id.search);

            SearchView.QueryTextChange += SearchView_QueryTextChange;
            contactsListView.ItemClick += ContactsListView_ItemClick;
            nextButton.Click += NextButton_Click;

            contactsListView.Adapter = adapt;

        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ContactsListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var cList = (ContactPickerAdapter)contactsListView.Adapter;
            var item = cList.contactList[e.Position].PhoneNumber;
            MainList = MainList.DealSelect(item);
            ((ContactPickerAdapter)contactsListView.Adapter).MarkTrue(item);
        }

        protected List<Contact> DealSelect(List<Contact> item , string number)
        {
            return item.Select(z =>  {
                z.Selected  = z.PhoneNumber == number ? !z.Selected : z.Selected;
                return z;
            }).ToList();
        }

        private void SearchView_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            ((ContactPickerAdapter)contactsListView.Adapter).FilterList(Clone(MainList), e.NewText);

        }

        public List<Contact> Clone(List<Contact> list)
        {
            return list.ConvertAll(y => new Contact(y));
        }
    }
}