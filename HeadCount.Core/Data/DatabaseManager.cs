using Couchbase.Lite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadCount.Core.Data
{
    public class DatabaseManager
    {
        const string DbName = "HeadCount";

        public Database db { get; set; }

        public DatabaseManager()
        {
            try
            {
                db = Manager.SharedInstance.GetDatabase(DbName);
            }
            catch (Exception e)
            {
                return;
            }
        }
    }
}
