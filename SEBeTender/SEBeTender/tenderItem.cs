using System;
using System.Collections.Generic;
using System.Text;

namespace SEBeTender
{
    public class tenderItem
    {
        private string reference, title, originatingStation, closingDate, biddingClosingDate, feeBeforeGST, feeGST, feeAfterGST, tendererClass, name, officePhone, extension, mobilePhone, email, fax, checkedValue, addToCartQuantity = "1"; //checkedValue is used to store the checkbox value to post the data to add tender to cart 
        private string bookmarkImage = "bookmark.png";
        private Dictionary<string, string> fileLinks = new Dictionary<string, string>();

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

        public Dictionary<string, string> FileLinks
        {
            get { return fileLinks; }
            set { fileLinks = value; }
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
