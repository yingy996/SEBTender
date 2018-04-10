using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

namespace SEBeTender
{
    class DataExtraction
    {
        public static Object getWebData(string webData, string page)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(webData);
            //var output = "";

            if (page == "tender")
            {
                var output = getTenderPage(htmlDocument);
                return output;
            }
            return "No result";
        }

        public static Object getTenderPage(HtmlDocument htmlDocument)
        {
            var htmlNodes = htmlDocument.DocumentNode.SelectNodes("//tbody/tr");
            int rowCount = 0;
            
            //List to store tender items
            List<tenderItem> tenderItems = new List<tenderItem>();

            //Loop through the tender items to get the tender details for each of the tenders
            foreach (var trNode in htmlNodes)
            {
                
                //If the row is not the first row, create the tender item
                if (rowCount > 0)
                {
                    var tdNodes = trNode.ChildNodes;
                    var tdNodeCount = tdNodes.Count;
                    int count = 0;

                    tenderItem tender = new tenderItem();

                    //get the details of the tender item
                    foreach (var tdNode in tdNodes)
                    {
                        if (tdNodeCount > 3) 
                        {
                            if (!String.IsNullOrWhiteSpace(tdNode.InnerHtml))
                            {
                                if (rowCount == 1)
                                {
                                    Console.WriteLine(count);
                                    Console.WriteLine(tdNode.InnerHtml.Trim());
                                }
                                
                                switch (count)
                                {
                                    case 0:
                                        string aNodeString = tdNode.InnerHtml.Trim();
                                        var htmlDoc = new HtmlDocument();
                                        htmlDoc.LoadHtml(aNodeString);
                                        var aNode = htmlDoc.DocumentNode.SelectSingleNode("//a");
                                 
                                        tender.Reference = aNode.InnerHtml;
                                        break;
                                    case 1:
                                        tender.Title = tdNode.FirstChild.InnerHtml;
                                        break;
                                    case 2:
                                        tender.OriginatingStation = tdNode.InnerHtml;
                                        break;
                                    case 3:
                                        tender.ClosingDate = "Closing date: " + tdNode.InnerHtml;
                                        break;
                                    case 4:
                                        tender.BidClosingDate = tdNode.InnerHtml;
                                        break;
                                    case 5:
                                        tender.FeeBeforeGST = tdNode.InnerHtml;
                                        break;
                                    case 6:
                                        tender.FeeGST = tdNode.InnerHtml;
                                        break;
                                    case 7:
                                        tender.FeeAfterGST = tdNode.InnerHtml;
                                        break;
                                }
                                count++;
                            }
                        } else
                        {
                            if (!String.IsNullOrWhiteSpace(tdNode.InnerHtml))
                            {
                                tender.TendererClass = tdNode.InnerHtml;
                            }
                        }
                    }
                    //Console.WriteLine(tender.OriginatingStation);
                    if (tdNodeCount > 3)
                    {
                        tenderItems.Add(tender);
                    }
                    
                    rowCount++;
                } else
                {
                    rowCount++;
                }
                
            }

            return tenderItems;            
        }
    }
}
