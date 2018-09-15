using System;
using System.Collections.Generic;
using System.Text;

namespace SEBeTender
{
    public class surveyQuestion
    {
        public string questionID { get; set; }
        public string questionTitle { get; set; }
        public string surveyID { get; set; }
        public string questionType { get; set; }
        public List<surveyOption> surveyOptions { get; set; }
        public string responseAnswer { get; set; }
    }
}
