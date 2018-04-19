using System;
using System.Collections.Generic;
using System.Text;

namespace SEBeTender
{
    public class Cart
    {
        private List<tenderItem> tenderItems = new List<tenderItem>();
        private int itemQuantity = 0;
        private string totalFeeBeforeGST, feeGST, totalFeeAfterGST;

        public List<tenderItem> TenderItems
        {
            get { return tenderItems; }
            set { tenderItems = value; }
        }

        public int ItemQuantity
        {
            get { return itemQuantity; }
            set { itemQuantity = value; }
        }

        public string TotalFeeBeforeGST
        {
            get { return totalFeeBeforeGST; }
            set { totalFeeBeforeGST = value; }
        }

        public string FeeGST
        {
            get { return feeGST; }
            set { feeGST = value; }
        }

        public string TotalFeeAfterGST
        {
            get { return totalFeeAfterGST; }
            set { totalFeeAfterGST = value; }
        }


    }
}
