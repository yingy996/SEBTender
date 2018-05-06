using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace SEBeTender
{
    public class dbTenderItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Reference { get; set; }
        public string Title { get; set; }
        public string OriginatingStation { get; set; }
        public string ClosingDate { get; set; }
        public string BidClosingDate { get; set; }
        public string FeeBeforeGST { get; set; }
        public string FeeAfterGST { get; set; }
        public string FeeGST { get; set; }
        public string TendererClass { get; set; }
        public string Name { get; set; }
        public string OffinePhone { get; set; }
        public string Extension { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string FileLinks { get; set; }
        public string CheckedValue { get; set; }
        public string AddToCartQuantity { get; set; }
        public string BookmarkImage { get; set; }
        public int Page { get; set; }

    }
}
