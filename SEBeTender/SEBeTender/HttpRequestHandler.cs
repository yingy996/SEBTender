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

        //get originating source from web database
        public static async Task<string> searchGetOriginatingSource(string url)
        {
            string responseStatus = "";
            string result = "";
            var parameters = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("retrieveOriginatingSource", "retrieveOriginatingSource")
            });

            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync(url, parameters);
                responseStatus = response.StatusCode.ToString();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                    
                }
            }
            catch
            {
                return "Error: " + responseStatus + ". Please try again";
            }
            return result;
        }
               
        //get tender search result from web database
        public static async Task<string> searchTendersFromDatabase(string url, string tenderReference, string tenderTitle, string originatingSource, string closingDateFrom, string  closingDateTo)
        {
            /*if (tenderReference == "")
            {
                Console.WriteLine("TENDER REFERENCE" + tenderReference);
            }
            Console.WriteLine("TenderTitle: "+tenderTitle);
            Console.WriteLine("Originatingsource: "+originatingSource);
            Console.WriteLine("ClosingDateFrom: " +closingDateFrom);
            Console.WriteLine("ClosingDateTo: " + closingDateTo);*/
            string responseStatus = "";
            string result = "";
            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("searchReference", tenderReference),
                new KeyValuePair<string,string>("searchTitle", tenderTitle),
                new KeyValuePair<string,string>("searchOriginatingSource", originatingSource),
                new KeyValuePair<string,string>("searchClosingDateFrom", closingDateFrom),
                new KeyValuePair<string, string>("searchClosingDateTo", closingDateTo)
            });

            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync(url, parameters);
                responseStatus = response.StatusCode.ToString();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    result = response.Content.ReadAsStringAsync().Result;

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
            //string cookieResult = "";
            string responseStatus = "";
            string result = "";
            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("password", password)
            });

            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync("https://pockettender.000webhostapp.com/process_appUserLogin.php", parameters);
                
                responseStatus = response.StatusCode.ToString();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //cookieResult = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
                    userSession.userLoginCookie = "success";
                    result = await response.Content.ReadAsStringAsync();
                    userSession.username = username;
                    //for future automated login (user just need to login for once and will be kept logged in afterward)
                    Settings.Username = username;
                    Settings.Password = password;
                    Settings.Role = "user";
                } else
                {
                    //if login request failed, return error message
                    return "Error: " + responseStatus + ". Please try again.";
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
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
                var response = await httpClient.PostAsync("https://pockettender.000webhostapp.com/process_appAdminLogin.php", parameters);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    result = await response.Content.ReadAsStringAsync();

                    //for future automated login (user just need to login for once and will be kept logged in afterward)
                    Settings.Username = username;
                    Settings.Password = password;
                    Settings.Role = "admin";
                    Console.WriteLine("Username in setting: " + Settings.Username);
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
                var response = await httpClient.GetAsync("https://pockettender.000webhostapp.com/web/logout.php");
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

                var response = await client.GetAsync("https://pockettender.000webhostapp.com/web/getAnnouncementMobile.php");

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
                var response = await httpClient.PostAsync("https://pockettender.000webhostapp.com/web/checkAccountExists.php", parameters);
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
                var response = await httpClient.PostAsync("https://pockettender.000webhostapp.com/web/process_appPostAnnouncement.php", parameters);

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
                var response = await httpClient.PostAsync("https://pockettender.000webhostapp.com/web/process_appEditAnnouncement.php", parameters);

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
                var response = await httpClient.PostAsync("https://pockettender.000webhostapp.com/web/deleteAnnouncement.php", parameters);


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
                var response = await httpClient.PostAsync("https://pockettender.000webhostapp.com/process_getUserBookmark.php", parameters);

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
                //tenderClosingDate = tenderClosingDate.Replace("Closing date: ", "");
                if (tender.TenderSource == "0")
                {
                    tenderClosingDate = tenderClosingDate.Replace(" at ", " ");
                }                
            }

            //default add tender bookmark action
            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("tenderReferenceNumber", tender.Reference),
                new KeyValuePair<string,string>("tenderTitle", tender.Title),
                new KeyValuePair<string,string>("originatingSource", tender.OriginatingStation),
                new KeyValuePair<string,string>("closingDate", tenderClosingDate)
            });

            if (action == "delete")
            {
                parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("tenderReferenceNumber", tender.Reference),
                new KeyValuePair<string,string>("tenderTitle", tender.Reference),
                new KeyValuePair<string,string>("isDelete", "1"),
                });
            }
            
            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync("https://pockettender.000webhostapp.com/process_manageTenderBookmark.php", parameters);

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

        public static async Task<string> PostGetBookmarkDetails(string tenderRefNo, string tenderTitle)
        {
            HttpClient client = new HttpClient();
            string result = "";
            /*var parameters = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("SchReferno", tenderRefNo),
                new KeyValuePair<string,string>("SchTendertitle", ""),
                new KeyValuePair<string,string>("SchStation", ""),
                new KeyValuePair<string,string>("SchFromClosedate", ""),
                new KeyValuePair<string,string>("SchToClosedate", ""),
                new KeyValuePair<string,string>("SchFromEbidClosedate", ""),
                new KeyValuePair<string,string>("SchToEbidClosedate", "")
            });

            var uri = new Uri(string.Format("http://www2.sesco.com.my/etender/notice/notice.jsp", string.Empty));*/
            var parameters = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("tenderReferenceNo", tenderRefNo),
                new KeyValuePair<string,string>("tenderTitle", tenderTitle)
            });

            if (tenderRefNo == "" || tenderRefNo == null)
            {
                parameters = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("tenderReferenceNo", "None"),
                    new KeyValuePair<string,string>("tenderTitle", tenderTitle)
                });
            }     

            var uri = new Uri(string.Format("https://pockettender.000webhostapp.com/process_appGetBookmarkItem.php", string.Empty));
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

        public static async Task<string> PostManageSearchBookmark(string searchID, string tenderReference, string tenderTitle, string originatingSource, string closingDateFrom, string closingDateTo, string username, string identifier, string action)
        {
            string result = "";
            
            //default add tender bookmark action
            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("searchID", searchID),
                new KeyValuePair<string,string>("tenderReference", tenderReference),
                new KeyValuePair<string,string>("tenderTitle", tenderTitle),
                new KeyValuePair<string,string>("originatingSource", originatingSource),
                new KeyValuePair<string,string>("closingDateFrom", closingDateFrom),
                new KeyValuePair<string,string>("closingDateTo", closingDateTo),
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("identifier", identifier)
                });

            if (action == "delete")
            {
                parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("searchID", searchID),
                new KeyValuePair<string,string>("tenderReference", tenderReference),
                new KeyValuePair<string,string>("tenderTitle", tenderTitle),
                new KeyValuePair<string,string>("originatingStation", originatingSource),
                new KeyValuePair<string,string>("closingDateFrom", closingDateFrom),
                new KeyValuePair<string,string>("closingDateTo", closingDateTo),
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("identifier", identifier),
                new KeyValuePair<string,string>("isDelete", "1")
                });
            }

            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync("https://pockettender.000webhostapp.com/process_manageCustomSearch.php", parameters);

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
                var response = await httpClient.PostAsync("https://pockettender.000webhostapp.com/process_getuserCustomSearches.php", parameters);

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
                    var response = await httpClient.PostAsync("https://pockettender.000webhostapp.com/web/process_appAddAdmin.php", parameters);

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

                    var response = await client.PostAsync("https://pockettender.000webhostapp.com/web/process_manageUsers.php", parameters);

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

                    var response = await client.PostAsync("https://pockettender.000webhostapp.com/web/process_deleteUser.php", parameters);

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
                    var response = await httpClient.PostAsync("https://pockettender.000webhostapp.com/web/process_appEditUser.php", parameters);

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
                    var response = await httpClient.PostAsync("https://pockettender.000webhostapp.com/web/process_appChangePassword.php", parameters);

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

                    var response = await client.PostAsync("https://pockettender.000webhostapp.com/web/process_appGetUser.php", parameters);

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
                var response = await httpClient.PostAsync("https://pockettender.000webhostapp.com/web/process_appCreatePoll.php", parameters);

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

                var response = await client.PostAsync("https://pockettender.000webhostapp.com/web/process_appGetPoll.php", parameters);

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

                var response = await client.PostAsync("https://pockettender.000webhostapp.com/web/process_appGetPoll.php", parameters);

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
                var response = await httpClient.PostAsync("https://pockettender.000webhostapp.com/web/process_appEditPoll.php", parameters);

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
                var response = await httpClient.PostAsync("https://pockettender.000webhostapp.com/web/process_appDeleteOption.php", parameters);

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

        public static async Task<string> PostClosePoll(string pollID, string username, string password)
        {
            string result = "";
            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("pollID", pollID),
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("password", password)
            });
            HttpClient httpClient = new HttpClient();
            try
            {
                //Send HTTP request to close poll
                var response = await httpClient.PostAsync("https://pockettender.000webhostapp.com/web/process_appClosePoll.php", parameters);

                result = await response.Content.ReadAsStringAsync();
                
                Console.WriteLine(result);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
        }

        public static async Task<String> PostRegisterNewUser(string name, string email, string username, string password, string confPassword)
        {
            string result = "";
            var parameters = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string,string>("name", name),
                    new KeyValuePair<string,string>("email", email),
                    new KeyValuePair<string,string>("username", username),
                    new KeyValuePair<string,string>("password", password),
                    new KeyValuePair<string,string>("confPassword", confPassword)
                });

            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync("https://pockettender.000webhostapp.com/process_appRegisterUser.php", parameters);

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

        public static async Task<String> PostSubmitPollAnswer(string pollID, string optionID, string userID, string answerInText)
        {
            string result = "";
            var parameters = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string,string>("pollID", pollID),
                    new KeyValuePair<string,string>("optionID", optionID),
                    new KeyValuePair<string,string>("userID", userID),
                    new KeyValuePair<string,string>("answerInText", answerInText)
                });

            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync("https://pockettender.000webhostapp.com/web/process_appInsertPollAnswer.php", parameters);

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
        public static async Task<string> PostGetSurveys()
        {
            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("infoToObtain", "surveys")
            });

            try
            {
                HttpClient client = new HttpClient();

                var response = await client.PostAsync("https://pockettender.000webhostapp.com/web/process_appGetSurvey.php", parameters);

                string result = response.Content.ReadAsStringAsync().Result;

                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Fetch surveys error: " + ex);
                return null;
            }
        }
        
        //get list of survey questions
        public static async Task<string> PostGetSurveyQuestions(string surveyID)
        {
            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("infoToObtain", "questions"),
                new KeyValuePair<string,string>("surveyID", surveyID)
            });

            try
            {

                HttpClient client = new HttpClient();

                var response = await client.PostAsync("https://pockettender.000webhostapp.com/web/process_appGetSurvey.php", parameters);

                string result = response.Content.ReadAsStringAsync().Result;

                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Fetch survey questions error: " + ex);
                return null;
            }
        }

        //get list of survey question dropdown/radiobox/checkbox answers
        public static async Task<string> PostGetSurveyQuestionAnswers(string questionID)
        {
            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("infoToObtain", "answers"),
                new KeyValuePair<string,string>("questionID", questionID)
            });

            try
            {

                HttpClient client = new HttpClient();

                var response = await client.PostAsync("https://pockettender.000webhostapp.com/web/process_appGetSurvey.php", parameters);

                string result = response.Content.ReadAsStringAsync().Result;

                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Fetch survey dropdown/checkbox/radiobox error: " + ex);
                return null;
            }
        }

        //Submit survey Response
        public static async Task<string> PostSurveyAnswers(string jsonanswers, string username)
        {
            string result = "";
            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("surveyJson", jsonanswers)
            });
            
            try
            {

                HttpClient client = new HttpClient();

                var response = await client.PostAsync("https://pockettender.000webhostapp.com/web/process_appSubmitSurvey.php", parameters);
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }
                
                
                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Submit user survey response error: " + ex);
                return null;
            }

        }

        public static async Task<string> PostRetrieveOrigSources()
        {
            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("dataToRetrieve", "originating source")
            });

            try
            {
                HttpClient client = new HttpClient();

                var response = await client.PostAsync("https://pockettender.000webhostapp.com/process_appRetrieveData.php", parameters);

                string result = response.Content.ReadAsStringAsync().Result;

                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Retrieve originating source error: " + ex);
                return null;
            }
        }
    }
}