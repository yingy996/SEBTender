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
            } else if (page == "searchtenderpage")
            {
                Task<Object> getTenderTask = Task.Run<Object>(() => getTenderPage(htmlDocument));
                var output = getTenderTask.Result;
                return output;
            } else if (page == "cartpage")
            {
                Task<Object> getCartTask = Task.Run<Object>(() => getCartItem(htmlDocument));
                var output = getCartTask.Result;
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

                        string httpTask = await Task.Run<string>(() => HttpRequestHandler.GetRequest(url));
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

                        string httpTask2 = await Task.Run<string>(() => HttpRequestHandler.GetRequest(url2));
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

        public static async Task<Object> getCartItem(HtmlDocument htmlDocument)
        {
            var htmlNodes = htmlDocument.DocumentNode.SelectNodes("//tbody/tr");
            int rowCount = 0;

            //Create list to store cart items
            List<cartItem> cartItems = new List<cartItem>();

            //Loop through the cart items to get the individual cart item's data
            foreach (var trNode in htmlNodes)
            {
                //If the row is not the first row, create new cartItem object
                if (rowCount > 0)
                {
                    //Traverse from /tr to /td to retrieve cart item detail
                    var tdNodes = trNode.ChildNodes;
                    //var tdNodeCount = tdNodes.Count;
                    int count = 0;

                    cartItem item = new cartItem();

                    foreach (var tdNode in tdNodes)
                    {
                        if (!String.IsNullOrWhiteSpace(tdNode.InnerHtml))
                        {
                            switch (count)
                            {
                                case 0:
                                    item.Reference = tdNode.FirstChild.InnerHtml;
                                    Console.WriteLine(item.Reference);
                                    break;
                                case 1:
                                    item.Title = tdNode.FirstChild.InnerHtml;
                                    Console.WriteLine(item.Title);
                                    break;
                                case 2:
                                    item.Quantity = tdNode.FirstChild.InnerHtml;
                                    Console.WriteLine(item.Quantity);
                                    break;
                                case 3:
                                    item.DocFee = tdNode.FirstChild.InnerHtml;
                                    Console.WriteLine(item.DocFee);
                                    break;
                                case 4:
                                    item.TotalDocFee = tdNode.FirstChild.InnerHtml;
                                    Console.WriteLine(item.TotalDocFee);
                                    break;
                                case 5:
                                    string aNodeString = tdNode.InnerHtml.Trim();
                                    var htmlDoc = new HtmlDocument();
                                    htmlDoc.LoadHtml(aNodeString);
                                    var aNode = htmlDoc.DocumentNode.SelectSingleNode("//a");

                                    item.DeleteItemLink = aNode.InnerHtml;
                                    Console.WriteLine(item.DeleteItemLink);
                                    break;
                            }

                            count++;
                        }
                    }

                    rowCount++;
                }
                else
                {
                    rowCount++;
                }
            }

            return cartItems;
        }

        /*public static async Task<Object> getSearchTenderPage(HtmlDocument htmlDocument)
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

                        string httpTask = await Task.Run<string>(() => HttpRequestHandler.GetRequest(url));
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

                        string httpTask2 = await Task.Run<string>(() => HttpRequestHandler.GetRequest(url2));
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
        }*/

    }
}
