using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace SEBeTender
{
    class HttpRequestHandler
    {
        public static async Task<string> GetRequest(string url)
        {
            HttpClient client = new HttpClient();
            string result = "";
            var uri = new Uri(string.Format(url, string.Empty));
            
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
                new KeyValuePair<string,string>("SchTendertitle", tenderReference),
                new KeyValuePair<string,string>("SchStation", originatingStation),
                new KeyValuePair<string,string>("SchFromClosedate", closingDateFrom),
                new KeyValuePair<string,string>("SchToClosedate", closingDateTo),
                new KeyValuePair<string,string>("SchReferno", tenderReference),


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
    }
}
