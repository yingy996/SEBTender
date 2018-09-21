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
                var response = await httpClient.PostAsync("https://sebannouncement.000webhostapp.com/process_appLogin.php", parameters);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Login success");
                } else
                {
                    Console.WriteLine("Status code: " + response.IsSuccessStatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Admin login error: " + ex);
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

        public static async Task<string> getManageAdminUserList()
        {
            if (!String.IsNullOrEmpty(adminAuth.Username))
            {
                try
                {
                    string result = "";
                    var parameters = new FormUrlEncodedContent(new[] {
                        new KeyValuePair<string,string>("username", adminAuth.Username),
                        new KeyValuePair<string,string>("password", adminAuth.Password)
                    });
                    HttpClient client = new HttpClient();

                    var response = await client.PostAsync("https://sebannouncement.000webhostapp.com/process_manageUsers.php", parameters);

                    if (response.IsSuccessStatusCode)
                    {
                        result = response.Content.ReadAsStringAsync().Result;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fetch announcement error: " + ex);
                    return null;
                }
            }
            else
            {
                return "Admin not logged in";
            }          
        }

        public static async Task<string> deleteAdminUser(adminUser user)
        {
            if (!String.IsNullOrEmpty(adminAuth.Username))
            {
                try
                {
                    string result = "";
                    var parameters = new FormUrlEncodedContent(new[] {
                        new KeyValuePair<string,string>("adminUsername", adminAuth.Username),
                        new KeyValuePair<string,string>("adminPassword", adminAuth.Password),
                        new KeyValuePair<string,string>("username", user.Username)
                    });
                    HttpClient client = new HttpClient();

                    var response = await client.PostAsync("https://sebannouncement.000webhostapp.com/process_deleteUser.php", parameters);

                    if (response.IsSuccessStatusCode)
                    {
                        result = response.Content.ReadAsStringAsync().Result;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Delete user error: " + ex);
                    return null;
                }
            }
            else
            {
                return "Admin not logged in";
            }
        }

        public static async Task<String> editAdmin(string name, string email, string role, string username)
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
                });

                HttpClient httpClient = new HttpClient();
                try
                {
                    var response = await httpClient.PostAsync("https://sebannouncement.000webhostapp.com/process_appEditUser.php", parameters);

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
            else
            {
                return "Admin not logged in";
            }
        }

        public static async Task<String> adminChangePassword(string oldPassword, string newPassword, string confPassword)
        {
            if (!String.IsNullOrEmpty(adminAuth.Username))
            {
                string result = "";
                var parameters = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string,string>("adminUsername", adminAuth.Username),
                    new KeyValuePair<string,string>("adminPassword", adminAuth.Password),
                    new KeyValuePair<string,string>("oldPassword", oldPassword),
                    new KeyValuePair<string,string>("newPassword", newPassword),
                    new KeyValuePair<string,string>("confPassword", confPassword)
                });

                HttpClient httpClient = new HttpClient();
                try
                {
                    var response = await httpClient.PostAsync("https://sebannouncement.000webhostapp.com/process_appChangePassword.php", parameters);

                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                Console.WriteLine("Result is: " + result);
                return result;
            }
            else
            {
                return "Admin not logged in";
            }
        }

        public static async Task<string> getCurrentUserDetails()
        {
            if (!String.IsNullOrEmpty(adminAuth.Username))
            {
                try
                {
                    string result = "";
                    var parameters = new FormUrlEncodedContent(new[] {
                        new KeyValuePair<string,string>("username", adminAuth.Username),
                        new KeyValuePair<string,string>("password", adminAuth.Password)
                    });
                    HttpClient client = new HttpClient();

                    var response = await client.PostAsync("https://sebannouncement.000webhostapp.com/process_appGetUser.php", parameters);

                    if (response.IsSuccessStatusCode)
                    {
                        result = response.Content.ReadAsStringAsync().Result;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fetch announcement error: " + ex);
                    return null;
                }
            }
            else
            {
                return "Admin not logged in";
            }
        }

        public static async Task<string> ChangePasswordRequest(string url, string oldpass, string newpass, string renewpass)
        {
            string result = "";

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
                new KeyValuePair<string,string>("OldPassword", oldpass),
                new KeyValuePair<string,string>("NewPassword", newpass),
                new KeyValuePair<string,string>("RetypePassword", renewpass)

            });

            try
            {
                var response = await httpClient.PostAsync(uri, parameters);
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

        public static async Task<string> PostAddPoll(string username, string password, string question, string option_number, string[] options)
        {
            string result = "";
            var formData = new List<KeyValuePair<string, string>>();
            formData.Add(new KeyValuePair<string, string>("username", username));
            formData.Add(new KeyValuePair<string, string>("password", password));
            formData.Add(new KeyValuePair<string, string>("question", question));
            formData.Add(new KeyValuePair<string, string>("option_number", option_number));

            int count = 1;
            if (options.Count() > 0)
            {
                Console.WriteLine("I have options in options");
                Console.WriteLine(options[0]);
            } else
            {
                Console.WriteLine("NO options in options");
            }
            foreach(string option in options)
            {
                formData.Add(new KeyValuePair<string, string>("option" + count.ToString(), option));
                Console.WriteLine("GGGOption: " + option);
                count++;
            }
            /*var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("password", password),
                new KeyValuePair<string,string>("question", title),
                new KeyValuePair<string,string>("option_number", title),
                new KeyValuePair<string,string>("title", title),
                new KeyValuePair<string,string>("content", content)
            });*/
            var parameters = new FormUrlEncodedContent(formData);


            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync("https://sebannouncement.000webhostapp.com/process_appCreatePoll.php", parameters);

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

        public static async Task<string> PostGetPollQuestion()
        {
            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("infoToObtain", "question")
            });

            try
            {

                HttpClient client = new HttpClient();

                var response = await client.PostAsync("https://sebannouncement.000webhostapp.com/process_appGetPoll.php", parameters);

                string result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("GET poll question: " + result);
                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Fetch announcement error: " + ex);
                return null;
            }
        }

        public static async Task<string> PostGetPollOptions(string pollID)
        {
            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("infoToObtain", "option"),
                new KeyValuePair<string,string>("pollID", pollID)
            });

            try
            {

                HttpClient client = new HttpClient();

                var response = await client.PostAsync("https://sebannouncement.000webhostapp.com/process_appGetPoll.php", parameters);

                string result = response.Content.ReadAsStringAsync().Result;

                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Fetch announcement error: " + ex);
                return null;
            }
        }

        public static async Task<string> PostUpdatePoll(string username, string password, string question, string option_number, string[] options, Poll poll)
        {
            string result = "";

            var formData = new List<KeyValuePair<string, string>>();
            formData.Add(new KeyValuePair<string, string>("username", username));
            formData.Add(new KeyValuePair<string, string>("password", password));
            formData.Add(new KeyValuePair<string, string>("pollID", poll.pollID));
            formData.Add(new KeyValuePair<string, string>("question", question));
            formData.Add(new KeyValuePair<string, string>("option_number", option_number));

            int count = 1;
            foreach (string option in options)
            {
                formData.Add(new KeyValuePair<string, string>("option" + count.ToString(), option));
                count++;
            }

            count = 1;
            foreach (pollOption option in poll.pollOptions)
            {
                formData.Add(new KeyValuePair<string, string>("optionID" + count.ToString(), option.optionID));
                count++;
            }

            var parameters = new FormUrlEncodedContent(formData);


            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync("https://sebannouncement.000webhostapp.com/process_appEditPoll.php", parameters);

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

        public static async Task<string> PostDeleteOption(string username, string password, string optionID)
        {
            string result = "";

            var formData = new List<KeyValuePair<string, string>>();
            formData.Add(new KeyValuePair<string, string>("username", username));
            formData.Add(new KeyValuePair<string, string>("password", password));
            formData.Add(new KeyValuePair<string, string>("optionID", optionID));

            var parameters = new FormUrlEncodedContent(formData);

            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync("https://sebannouncement.000webhostapp.com/process_appDeleteOption.php", parameters);

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

        //get list of surveys
        public static async Task<string> PostGetSurveyQuestions()
        {
            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("infoToObtain", "surveys")
            });

            try
            {

                HttpClient client = new HttpClient();

                var response = await client.PostAsync("https://sebannouncement.000webhostapp.com/process_appGetSurvey.php", parameters);

                string result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("GET survey titles: " + result);
                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Fetch surveys error: " + ex);
                return null;
            }
        }
    }
}
