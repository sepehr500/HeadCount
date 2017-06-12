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
using Android.Telephony;

namespace HeadCount.Classes
{
    public static class SendMessageService
    {       
        public static void SendMessages(List<string> numbers , string message)
        {
            foreach (var item in numbers)
            {
                SendMessage(item, message);

            }
        }

        public static void SendMessage(string number, string message)
        {
          SmsManager.Default.SendTextMessage(number, null,message, null, null);
        }
    }
}