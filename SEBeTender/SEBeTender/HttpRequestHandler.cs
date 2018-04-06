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
            var uri = new Uri(string.Format("http://www2.sesco.com.my/etender/notice/notice.jsp", string.Empty));
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
    }
}
