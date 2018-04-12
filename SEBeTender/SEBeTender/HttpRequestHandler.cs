using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace SEBeTender
{
    class HttpRequestHandler
    {
        public static async Task<string> GetRequest(string url, bool isLoggedIn)
        {
            CookieContainer cookieContainer = new CookieContainer();
            var httpClientHandler = new HttpClientHandler() { CookieContainer = cookieContainer };

            HttpClient client = new HttpClient(httpClientHandler);
            string result = "";
            var uri = new Uri(string.Format(url, string.Empty));
            
            //Add user cookie if account is logged in
            if (isLoggedIn)
            {
                string[] cookieWords = Regex.Split(userSession.userLoginCookie, "=");
                string cookieName = cookieWords[0];
                string[] cookieValues = Regex.Split(cookieWords[1], "; ");
                string cookieValue = cookieValues[0];
                cookieContainer.Add(uri, new Cookie(cookieName, cookieValue));
                //client.DefaultRequestHeaders.Add("Cookie", userSession.userLoginCookie);
            }
            
            try
            {
                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
        }


        public static async Task<string> SearchPostRequest(string url, string tenderReference, string tenderTitle, string originatingStation, string closingDateFrom, string closingDateTo, string biddingClosingDateFrom, string biddingClosingDateTo)
        {
            HttpClient client = new HttpClient();
            string result = "";
            var parameters = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("SchReferno", tenderReference),
                new KeyValuePair<string,string>("SchTendertitle", tenderTitle),
                new KeyValuePair<string,string>("SchStation", originatingStation),
                new KeyValuePair<string,string>("SchFromClosedate", closingDateFrom),
                new KeyValuePair<string,string>("SchToClosedate", closingDateTo),
                new KeyValuePair<string,string>("SchFromEbidClosedate", biddingClosingDateFrom),
                new KeyValuePair<string,string>("SchToEbidClosedate", biddingClosingDateTo)


            });

            var uri = new Uri(string.Format(url, string.Empty));
            try
            {                                                                                                                                                                                                                         
                var response = await client.PostAsync(uri, parameters);
                Console.WriteLine("Response code: " + response.StatusCode);
                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    result = await response.Content.ReadAsStringAsync();
                    
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
         }

        public static async Task<string> PostUserLogin(string username, string password)
        {
            string cookieResult = "";
            string responseStatus = "";
            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("VenUserId", username),
                new KeyValuePair<string,string>("VenPassword", password)
            });

            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync("http://www2.sesco.com.my/etender/notice/notice_login_set_session.jsp", parameters);
                Console.WriteLine("Response code: " + response.StatusCode);
                responseStatus = response.StatusCode.ToString();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {                   
                    cookieResult = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
                    userSession.userLoginCookie = cookieResult;
                    
                } else
                {
                    //if login request failed, return error message
                    return "Error: " + responseStatus + ". Please try again.";
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return "Success";

        }
    }
}
