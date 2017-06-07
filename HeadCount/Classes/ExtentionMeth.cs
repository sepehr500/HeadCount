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

namespace HeadCount.Classes
{
    public static class ExtentionMeth
    {
        public static List<Contact> DealSelect(this List<Contact> item , string number)
        {
            return item.Select(z =>  {
                z.Selected  = z.PhoneNumber == number ? !z.Selected : z.Selected;
                return z;
            }).ToList();
        }
    }
}