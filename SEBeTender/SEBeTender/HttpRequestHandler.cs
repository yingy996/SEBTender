using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

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
