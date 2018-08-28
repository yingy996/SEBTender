using System;
using System.Collections.Generic;
using System.Text;

namespace SEBeTender
{
    public class Poll
    {
        public string pollID { get; set; }
        public string pollQuestion { get; set; }
        public string postedBy { get; set; }
        public string publishedDate { get; set; }
        public string editedDate { get; set; }
        public string editedBy { get; set; }
        public string endDate { get; set; }
        public string isEnded { get; set; }
        public List<pollOption> pollOptions { get; set; }
    }
}
