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
using System.Threading.Tasks;
using HeadCount.Core.Data;
using Couchbase.Lite;
using HeadCount.Core.Models;

namespace HeadCount.Droid
{



    [BroadcastReceiver(Enabled = true, Label = "SMSBroadcastReceiver")]
    [IntentFilter(new[] { "android.provider.Telephony.SMS_RECEIVED" })]
    public class SmsBroadcastReceiver : BroadcastReceiver
    {


        public delegate void MessageReceivedEventHandler(string message, string contactinfo);

        public event MessageReceivedEventHandler Received;

        private const string Tag = "SMSBroadcastReceiver";
        private const string IntentAction = "android.provider.Telephony.SMS_RECEIVED";

        public string message { get; set; }
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action.Equals(IntentAction))
            {

                var bundle = intent.Extras;
                if (bundle != null)
                {
                    var pdus = bundle.Get("pdus");
                    var castedPdus = JNIEnv.GetArray<Java.Lang.Object>(pdus.Handle);
                    var messages = new SmsMessage[castedPdus.Length];
                    for (int i = 0; i < castedPdus.Length; i++)
                    {

                        var bytes = new byte[JNIEnv.GetArrayLength(castedPdus[i].Handle)];
                        JNIEnv.CopyArray(castedPdus[i].Handle, bytes);
                        messages[i] = SmsMessage.CreateFromPdu(bytes);
                    }
                    foreach (var message in messages)
                    {
                        string messagefrom = message.DisplayOriginatingAddress;
                        string messagebody = message.MessageBody;
                        this.message = messagebody;
                        var mDatabase = new DatabaseManager();
                        var db = mDatabase.GetDb();
                        Document document = db.GetDocument("1");
                        var Event = (Event)document.Properties["event"];
                        foreach (var item in Event.Guests.Where(y => y.Number == messagefrom))
                        {
                            item.Response = messagebody;
                            item.Status = Core.Models.Status.Maybe;
                        }
                        document.Update((UnsavedRevision newRevision) =>
                        {
                            var properties = newRevision.Properties;
                            properties["event"] = Event;
                            return true;
                        });

                        if (Received != null) Received(messagebody, messagefrom);
                    }
                }


            }

        }
    }
}