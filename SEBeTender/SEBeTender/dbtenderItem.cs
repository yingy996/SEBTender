using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace SEBeTender
{
     public class dbtenderItem
    {
        //private int ID = 0;
        private string reference = "1";
        private string title = "1";
        private string  originatingStation = "1";
        private string closingDate = "1";
        private string biddingClosingDate = "1";
        private string feeBeforeGST = "1";
        private string feeGST = "1";
        private string feeAfterGST = "1";
        private string tendererClass = "1";
        private string name = "1";
        private string officePhone = "1";
        private string extension = "1";
        private string mobilePhone = "1";
        private string email = "1";
        private string fax = "1";
        private string checkedValue = "1";
        private string addToCartQuantity = "1"; //checkedValue is used to store the checkbox value to post the data to add tender to cart 
        private string bookmarkImage = "bookmark.png";
        
        private string jsonfilelinks = "";

        public dbtenderItem()
        {
          Id = 0;
          reference = "1";
          title = "1";
          originatingStation = "1";
          closingDate = "1";
          biddingClosingDate = "1";
          feeBeforeGST = "1";
          feeGST = "1";
          feeAfterGST = "1";
          tendererClass = "1";
          name = "1";
          officePhone = "1";
          extension = "1";
          mobilePhone = "1";
          email = "1";
          fax = "1";
          checkedValue = "1";
          addToCartQuantity = "1";
          bookmarkImage = "bookmark.png";
          jsonfilelinks = "";
        }

        /*[PrimaryKey, AutoIncrement]
        public int id
        {
            get { return ID; }
            set { ID = value; }
        }*/

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get;
            set;
        }

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

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string OffinePhone
        {
            get { return officePhone; }
            set { officePhone = value; }
        }

        public string Extension
        {
            get { return extension; }
            set { extension = value; }
        }

        public string MobilePhone
        {
            get { return mobilePhone; }
            set { mobilePhone = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }

        public string JsonfileLinks
        {
            get { return jsonfilelinks; }
            set { jsonfilelinks = value; }
        }

        public string CheckedValue
        {
            get { return checkedValue; }
            set { checkedValue = value; }
        }

        public string AddToCartQuantity
        {
            get { return addToCartQuantity; }
            set { addToCartQuantity = value; }
        }

        public string BookmarkImage
        {
            get { return bookmarkImage; }
            set { bookmarkImage = value; }
        }
    }
}
