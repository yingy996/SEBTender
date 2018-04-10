using System;
using System.Collections.Generic;
using System.Text;

namespace SEBeTender
{
    class tenderItem
    {
        private string reference, title, originatingStation, closingDate, biddingClosingDate, feeBeforeGST, feeGST, feeAfterGST, tendererClass;

        public string Reference
        {
            get { return reference; }
            set { reference = value; }   
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string OriginatingStation
        {
            get { return originatingStation; }
            set { originatingStation = value; }
        }

        public string ClosingDate
        {
            get { return closingDate; }
            set { closingDate = value; }
        }

        public string BidClosingDate
        {
            get { return biddingClosingDate; }
            set { biddingClosingDate = value; }
        }

        public string FeeBeforeGST
        {
            get { return feeBeforeGST; }
            set { feeBeforeGST = value; }
        }

        public string FeeAfterGST
        {
            get { return feeAfterGST; }
            set { feeAfterGST = value; }
        }

        public string FeeGST
        {
            get { return feeGST; }
            set { feeGST = value; }
        }

        public string TendererClass
        {
            get { return tendererClass; }
            set { tendererClass = value; }
        }
    }
}
