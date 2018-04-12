using System;
using System.Collections.Generic;
using System.Text;

namespace SEBeTender
{
	public class cartItem
	{
        private string reference, title, quantity, docFee, totalDocFee, deleteItemLink;
        //private string noOfItems, docFeeAmount, gst, totalAmount;

        public string Reference
        {
            get { return reference;  }
            set { reference = value;  }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public string DocFee
        {
            get { return docFee; }
            set { docFee = value; }
        }

        public string TotalDocFee
        {
            get { return totalDocFee; }
            set { totalDocFee = value; }
        }

        public string DeleteItemLink
        {
            get { return deleteItemLink; }
            set { deleteItemLink = value; }
        }
    }
    /*
    public class cartPriceSummary
    {
        private string noOfItems, docFeeAmount, gst, totalAmount;

        public string NoOfItems
        {
            get { return noOfItems; }
            set { noOfItems = value; }
        }

        public string DocFeeAmount
        {
            get { return docFeeAmount; }
            set { docFeeAmount = value; }
        }

        public string GST
        {
            get { return gst; }
            set { gst = value; }
        }

        public string TotalAmount
        {
            get { return totalAmount; }
            set { totalAmount = value; }
        }
    }*/
}