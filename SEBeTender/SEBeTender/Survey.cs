using System;
using System.Collections.Generic;
using System.Text;

namespace SEBeTender
{
    public class Survey
    {
        //Survey details
        public string surveyID { get; set; }
        public string surveyTitle { get; set; }
        public string description { get; set; }
        public string publishedBy { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string noOfResponse { get; set; }
        public string isEnded { get; set; }
        public List<surveyQuestion> surveyQuestions { get; set; }
    }
}