using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;

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
                Task<Object> getTenderTask = Task.Run<Object>(() => getTenderPage(htmlDocument));
                var output = getTenderTask.Result;
                //var output = getTenderPage(htmlDocument);
                return output;
            } else if(page == "searchtenderpage")
            {
                Task<Object> getTenderTask = Task.Run<Object>(() => getTenderPage(htmlDocument));
                var output = getTenderTask.Result;
                return output;
            } else if (page == "eligibelTenderPage")
            {
                Task<Object> getEligibleTenderTask = Task.Run<Object>(() => getEligibleTenderPage(htmlDocument));
                var output = getEligibleTenderTask.Result;
                return output;
            }
            return "No result";
        }

        public static async Task<Object> getTenderPage(HtmlDocument htmlDocument)
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
                    //Get contact details of the tender 
                    var tdNodes = trNode.ChildNodes;
                    var tdNodeCount = tdNodes.Count;
                    int count = 0;

                    tenderItem tender = new tenderItem();

                    foreach (var tdNode in tdNodes)
                    {
                        if (tdNodeCount > 3)
                        {
                            if (!String.IsNullOrWhiteSpace(tdNode.InnerHtml))
                            {
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
                        }
                        else
                        {
                            if (!String.IsNullOrWhiteSpace(tdNode.InnerHtml))
                            {
                                tender.TendererClass = tdNode.InnerHtml;
                            }
                        }
                    }

                    if (tdNodeCount > 3)
                    {
                        //Get the ORIGINATOR details of the tender item
                        string url = "http://www2.sesco.com.my/etender/notice/notice_originator.jsp?Referno=" + WebUtility.UrlEncode(tender.Reference);

                        string httpTask = await Task.Run<string>(() => HttpRequestHandler.GetRequest(url, false));
                        var httpResult = httpTask.ToString();

                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(httpResult);
                        var originatorTrNodes = htmlDoc.DocumentNode.SelectNodes("//table/tr/td/table/tr");
                        int originatorTrRowCount = 0;

                        foreach (var originatorTrNode in originatorTrNodes)
                        {
                            var originatorTdNodes = originatorTrNode.ChildNodes;
                            foreach (var originatorTdNode in originatorTdNodes)
                            {
                                if (!String.IsNullOrWhiteSpace(originatorTdNode.InnerHtml) && !originatorTdNode.FirstChild.HasChildNodes)
                                {

                                    //The originator info starts on row 3, thus row 0,1,2 are skipped
                                    switch (originatorTrRowCount)
                                    {
                                        case 3:
                                            tender.Name = originatorTdNode.InnerHtml;
                                            break;
                                        case 4:
                                            tender.OffinePhone = originatorTdNode.InnerHtml;
                                            break;
                                        case 5:
                                            tender.Extension = originatorTdNode.InnerHtml;
                                            break;
                                        case 6:
                                            tender.MobilePhone = originatorTdNode.InnerHtml;
                                            break;
                                        case 7:
                                            tender.Email = originatorTdNode.InnerHtml;
                                            break;
                                        case 8:
                                            tender.Fax = originatorTdNode.InnerHtml;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            originatorTrRowCount++;
                        }

                        //Get the downloadable files of the tender item
                        string url2 = "http://www2.sesco.com.my/etender/notice/notice_tender.jsp?Referno=" + WebUtility.UrlEncode(tender.Reference);

                        string httpTask2 = await Task.Run<string>(() => HttpRequestHandler.GetRequest(url2,false));
                        var httpResult2 = httpTask2.ToString();

                        var htmlDoc2 = new HtmlDocument();
                        htmlDoc2.LoadHtml(httpResult2);

                        var filesTdNodes = htmlDoc2.DocumentNode.SelectNodes("//table/tr/td");

                        var fileLinkNodes = filesTdNodes.Elements("a");


                        foreach (var fileLinkNode in fileLinkNodes)
                        {
                            if (fileLinkNode.NodeType == HtmlNodeType.Element)
                            {
                                string fileName = fileLinkNode.InnerHtml;
                                string fileLink = fileLinkNode.Attributes["href"].Value;
                                string[] linkSplit = Regex.Split(fileLink, "noticeDoc/");
                                string link = "http://www2.sesco.com.my/noticeDoc/" + Uri.EscapeUriString(linkSplit[1]);
                                tender.FileLinks[fileName] = link;
                            }
                        }

                        tenderItems.Add(tender);
                    }

                    rowCount++;
                }
                else
                {
                    rowCount++;
                }

            }

            return tenderItems;
        }

        private static async Task<Object> getEligibleTenderPage(HtmlDocument htmlDocument)
        {
            var divNodes = htmlDocument.DocumentNode.SelectNodes("//td/div");
            int divCount = 0;
            int childNodeCount = 0;

            //List to store tender items
            List<tenderItem> tenderItems = new List<tenderItem>();
            tenderItem tender = new tenderItem();

            foreach (var divNode in divNodes)
            {
                //the tender data starts at 11th div
                if(divCount > 10)
                {
                    //Get available tender details
                    foreach(var childNode in divNode.ChildNodes)
                    {
                        if (!String.IsNullOrWhiteSpace(childNode.InnerHtml))
                        {
                            if (!childNode.Attributes.Contains("href"))
                            {
                                switch (childNodeCount)
                                {
                                    case 0:
                                        tender = new tenderItem();
                                        break;
                                    case 1:
                                        tender.Reference = childNode.InnerHtml.Trim();
                                        break;
                                    case 2:
                                        tender.Title = childNode.InnerHtml.Trim();
                                        break;
                                    case 3:
                                        tender.TendererClass = "Tenderer class: " + childNode.InnerHtml.Trim();
                                        break;
                                    case 4:
                                        tender.BidClosingDate = "Bid closes: " + childNode.InnerHtml.Trim();
                                        break;
                                    case 5:
                                        tender.ClosingDate = "Closing date: " + childNode.InnerHtml.Trim();
                                        break;
                                    case 6:
                                        tender.FeeBeforeGST = childNode.InnerHtml.Trim();
                                        
                                        break;
                                }
                                childNodeCount++;
                            }
                        }
                    }
                }
                if (childNodeCount == 7)
                {
                    tenderItems.Add(tender);
                    childNodeCount = 0;
                }
                divCount++;
            }

            //Get the checkedValue for the tender items
            var inputNodes = htmlDocument.DocumentNode.SelectNodes("//td/input");

            foreach (var inputNode in inputNodes)
            {
                string inputValue = inputNode.Attributes["value"].Value;
                if (inputValue != "1")
                {

                    //string[] inputValueWords = Regex.Split(inputValue, "|");
                    string[] inputValueWords = inputValue.Split('|');
                    
                    foreach (var item in tenderItems)
                    {
                        if (inputValueWords[0].Trim() == item.Reference)
                        {
                            var index = tenderItems.IndexOf(item);
                            tenderItems[index].CheckedValue = inputValue;
                            //item.CheckedValue = inputValue;
                        }
                    }
                    //Console.WriteLine("Node: " + inputValue);
                }
            }

            return tenderItems;
        }

    }
}
