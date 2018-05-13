﻿using System;
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
                    userSession.username = username;
                    //for future automated login (user just need to login for once and will be kept logged in afterward)
                    Settings.Username = username;
                    Settings.Password = password;
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

        public static async Task<string> PostAdminLogin(string username, string password)
        {
            string result = "";
            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("password", password)
            });

            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync("https://sebannouncement.000webhostapp.com/login.php", parameters);

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

        public static async Task<string> PostUserLogout()
        {
            string responseStatus = "";
            HttpClient httpClient = new HttpClient();

            try
            {
                var response = await httpClient.GetAsync("http://www2.sesco.com.my/etender/vendor/vendor_logout.jsp");
                Console.WriteLine("Response code: " + response.StatusCode);
                responseStatus = response.StatusCode.ToString();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    userSession.userLoginCookie = "";
                    Console.WriteLine("Logout Successfully");

                } else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                }

            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return "Success";
        }

        public static async Task<string> getAnnouncementsResult()
        {
            try
            {

                HttpClient client = new HttpClient();

                //client.BaseAddress = new Uri("https://sebannouncement.000webhostapp.com/");

                var response = await client.GetAsync("https://sebannouncement.000webhostapp.com/getAnnouncementMobile.php");

                string result = response.Content.ReadAsStringAsync().Result;

                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Fetch announcement error: " + ex);
                return null;
            }
        }

        public static async Task<string> PostadminloginCheck(string username, string password)
        {
            string result = "";
            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("password", password)
            });
                                                       
            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync("https://sebannouncement.000webhostapp.com/checkAccountExists.php", parameters);
                result = response.Content.ReadAsStringAsync().Result;

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
        }

        public static async Task<string> PostAddAnnouncement(string username, string password, string title, string content)
        {
            string result = "";
            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("password", password),
                new KeyValuePair<string,string>("title", title),
                new KeyValuePair<string,string>("content", content)
            });

            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync("https://sebannouncement.000webhostapp.com/process_appPostAnnouncement.php", parameters);

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

        public static async Task<string> PostEditAnnouncement(string username, string password, string editID, string title, string content)
        {
            string result = "";
            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("password", password),
                new KeyValuePair<string,string>("editID", editID),
                new KeyValuePair<string,string>("title", title),
                new KeyValuePair<string,string>("content", content)
            });

            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync("https://sebannouncement.000webhostapp.com/process_appEditAnnouncement.php", parameters);

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

        public static async Task<string> deleteAnnouncement(string announcementid, string username)
        {
            string result = "";
            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("announcementid", announcementid),
                new KeyValuePair<string,string>("username", username)
            });
            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync("https://sebannouncement.000webhostapp.com/deleteAnnouncement.php", parameters);


                result = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Heyaaaa");
                Console.WriteLine(result);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
        }

        public static async Task<string> PostTenderBookmark(string username)
        {
            string result = "";

            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("username", username)
            });

            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync("https://sebannouncement.000webhostapp.com/process_getUserBookmark.php", parameters);

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

        public static async Task<string> PostManageTenderBookmark(string username, tenderItem tender, string action)
        {
            string result = "";

            string tenderClosingDate = tender.ClosingDate;
            if (action != "delete")
            {
                tenderClosingDate = tenderClosingDate.Replace("Closing date: ", "");
                tenderClosingDate = tenderClosingDate.Replace(" at ", " ");
            }

            //default add tender bookmark action
            var parameters = new FormUrlEncodedContent(new[] {

                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("tenderReferenceNumber", tender.Reference),
                new KeyValuePair<string,string>("tenderTitle", tender.Title),
                new KeyValuePair<string,string>("closingDate", tenderClosingDate)
                });

            if (action == "delete")
            {
                parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("tenderReferenceNumber", tender.Reference),
                new KeyValuePair<string,string>("isDelete", "1"),
                });
            }
            
            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync("https://sebannouncement.000webhostapp.com/process_manageTenderBookmark.php", parameters);

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

        public static async Task<string> PostGetBookmarkDetails(string tenderRefNo)
        {
            HttpClient client = new HttpClient();
            string result = "";
            var parameters = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("SchReferno", tenderRefNo),
                new KeyValuePair<string,string>("SchTendertitle", ""),
                new KeyValuePair<string,string>("SchStation", ""),
                new KeyValuePair<string,string>("SchFromClosedate", ""),
                new KeyValuePair<string,string>("SchToClosedate", ""),
                new KeyValuePair<string,string>("SchFromEbidClosedate", ""),
                new KeyValuePair<string,string>("SchToEbidClosedate", "")
            });

            var uri = new Uri(string.Format("http://www2.sesco.com.my/etender/notice/notice.jsp", string.Empty));
            try
            {
                var response = await client.PostAsync(uri, parameters);
                Console.WriteLine("Response code: " + response.StatusCode);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
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

        public static async Task<string> PostManageSearchBookmark(string searchID, string tenderReference, string tenderTitle, string originatingStation, string closingDateFrom, string closingDateTo, string biddingclosingDateFrom, string biddingclosingDateTo, string username, string identifier, string action)
        {
            string result = "";

            //default add tender bookmark action
            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("searchID", searchID),
                new KeyValuePair<string,string>("tenderReference", tenderReference),
                new KeyValuePair<string,string>("tenderTitle", tenderTitle),
                new KeyValuePair<string,string>("originatingStation", originatingStation),
                new KeyValuePair<string,string>("closingDateFrom", closingDateFrom),
                new KeyValuePair<string,string>("closingDateTo", closingDateTo),
                new KeyValuePair<string,string>("biddingclosingDateFrom", biddingclosingDateFrom),
                new KeyValuePair<string,string>("biddingclosingDateTo", biddingclosingDateTo),
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("identifier", identifier)
                });

            if (action == "delete")
            {
                parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("searchID", searchID),
                new KeyValuePair<string,string>("tenderReference", tenderReference),
                new KeyValuePair<string,string>("tenderTitle", tenderTitle),
                new KeyValuePair<string,string>("originatingStation", originatingStation),
                new KeyValuePair<string,string>("closingDateFrom", closingDateFrom),
                new KeyValuePair<string,string>("closingDateTo", closingDateTo),
                new KeyValuePair<string,string>("biddingclosingDateFrom", biddingclosingDateFrom),
                new KeyValuePair<string,string>("biddingclosingDateTo", biddingclosingDateTo),
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("identifier", identifier),
                new KeyValuePair<string,string>("isDelete", "1")
                });
            }

            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync("https://sebannouncement.000webhostapp.com/process_manageCustomSearch.php", parameters);

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

        public static async Task<string> PostCustomSearches(string username)
        {
            string result = "";

            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("username", username)
            });

            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync("https://sebannouncement.000webhostapp.com/process_getuserCustomSearches.php", parameters);

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


        public static async Task<string> EditCompanyProfileRequest(string url, string name, string regno, string mailingAddress, string country)
        {
            string responseStatus = "";

            CookieContainer cookieContainer = new CookieContainer();
            var httpClientHandler = new HttpClientHandler() { CookieContainer = cookieContainer };
            HttpClient httpClient = new HttpClient(httpClientHandler);

            var uri = new Uri(string.Format(url, string.Empty));

            string[] cookieWords = Regex.Split(userSession.userLoginCookie, "=");
            string cookieName = cookieWords[0];
            string[] cookieValues = Regex.Split(cookieWords[1], "; ");
            string cookieValue = cookieValues[0];
            cookieContainer.Add(uri, new Cookie(cookieName, cookieValue));

            var parameters = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("VenName", name),
                new KeyValuePair<string,string>("VenComReg", regno),
                new KeyValuePair<string,string>("VenAdd", mailingAddress),
                new KeyValuePair<string,string>("VenCouCode", country)

            });

            try
            {
                var response = await httpClient.PostAsync(uri, parameters);
                Console.WriteLine("Response code: " + response.StatusCode);
                responseStatus = response.StatusCode.ToString();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return responseStatus;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return responseStatus;
        }

        public static async Task<string> EditContactPersonRequest(string url, string name, string telno, string faxno, string email)
        {
            string responseStatus = "";

            CookieContainer cookieContainer = new CookieContainer();
            var httpClientHandler = new HttpClientHandler() { CookieContainer = cookieContainer };
            HttpClient httpClient = new HttpClient(httpClientHandler);

            var uri = new Uri(string.Format(url, string.Empty));

            string[] cookieWords = Regex.Split(userSession.userLoginCookie, "=");
            string cookieName = cookieWords[0];
            string[] cookieValues = Regex.Split(cookieWords[1], "; ");
            string cookieValue = cookieValues[0];
            cookieContainer.Add(uri, new Cookie(cookieName, cookieValue));

            var parameters = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("VenContactPerson", name),
                new KeyValuePair<string,string>("VenTelNo", telno),
                new KeyValuePair<string,string>("VenFaxNo", faxno),
                new KeyValuePair<string,string>("VenEmail", email)

            });

            try
            {
                var response = await httpClient.PostAsync(uri, parameters);
                Console.WriteLine("Response code: " + response.StatusCode);
                responseStatus = response.StatusCode.ToString();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return responseStatus;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return responseStatus;
        }

        public static async Task<String> registerNewAdmin(string name, string email, string role, string username, string password, string confPassword)
        {
            if (!String.IsNullOrEmpty(adminAuth.Username))
            {
                string result = "";
                var parameters = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string,string>("adminUsername", adminAuth.Username),
                    new KeyValuePair<string,string>("adminPassword", adminAuth.Password),
                    new KeyValuePair<string,string>("name", name),
                    new KeyValuePair<string,string>("email", email),
                    new KeyValuePair<string,string>("role", role),
                    new KeyValuePair<string,string>("username", username),
                    new KeyValuePair<string,string>("password", password),
                    new KeyValuePair<string,string>("confPassword", confPassword)
                });

                HttpClient httpClient = new HttpClient();
                try
                {
                    var response = await httpClient.PostAsync("https://sebannouncement.000webhostapp.com/process_appAddAdmin.php", parameters);

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
            } else
            {
                return "Admin not logged in";
            }
            
        }
    }
}
