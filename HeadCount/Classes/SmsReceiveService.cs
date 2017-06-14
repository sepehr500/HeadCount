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
using Couchbase.Lite;
using HeadCount.Core.Data;
using VaderSharp;

namespace HeadCount.Classes
{
    static class SmsReceiveService
    {
        public static string MaybeMessage = "Was that a yes or no?";
        public static void HandleMessage(string messagebody, string messagefrom)
        {
            var mDatabase = new DatabaseManager();
            var db = mDatabase.GetDb();
            Document document = db.GetDocument("1");
            var Event = (Event)document.Properties["event"];
            foreach (var item in Event.Guests.Where(y => y.Number == messagefrom))
            {
                if (item.Status == Core.Models.Status.NoResponse || item.Status == Core.Models.Status.Maybe)
                {
                    item.Response = messagebody;
                    item.Status = AnalyzeMessage(messagebody);
                    if (item.Status == Core.Models.Status.Maybe)
                    {
                        SendMessageService.SendMessage(item.Number, MaybeMessage);
                    }
                }
            }
            document.Update((UnsavedRevision newRevision) =>
            {
                var properties = newRevision.Properties;
                properties["event"] = Event;
                return true;
            });
        }

        public static HeadCount.Core.Models.Status AnalyzeMessage(string message)
        {
            message = message.ToLower();
            //SentimentIntensityAnalyzer analyzer = new SentimentIntensityAnalyzer();

            //var results = analyzer.PolarityScores(message);
            if (PositiveList().Where(y => message.Contains(y)).Count() > 0)
            {
                return Core.Models.Status.Yes;
            }
            if (NegList().Where(y => message.Contains(y)).Count() > 0)
            {
                return Core.Models.Status.No;
            }
            return Core.Models.Status.Maybe;

        }

        public static List<string> PositiveList()
        {
            return new List<string>() { "yes", "sure" , "ok", "good"};
        }
        public static List<string> NegList()
        {
            return new List<string>() {"no", "can't", "sorry", "cant"};
        }

    }
}