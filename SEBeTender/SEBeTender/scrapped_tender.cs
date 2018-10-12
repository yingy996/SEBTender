using System;
using System.Collections.Generic;
using System.Text;

namespace SEBeTender
{
    public class scrapped_tender
    {
        public string id { get; set; }
        public string reference { get; set; }
        public string title { get; set; }
        public string category { get; set; }
        public string originatingSource { get; set; }
        public string tenderSource{ get; set; }
        public string agency { get; set; }
        public string closingDate { get; set; }
        public string startDate { get; set; }
        public string docInfoJson { get; set; }
        public string originatorJson { get; set; }
        public string fileLinks { get; set; }
    }
}
