using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadCount.Core.Models
{
    public class Event
    {
        public string Name { get; set; }

        public string Message { get; set; }

        public List<Guest> Guests { get; set; }

        public bool IsFinished { get; set; }
    }
}
