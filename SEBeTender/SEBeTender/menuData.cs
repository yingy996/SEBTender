﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SEBeTender
{
	public class menuData : List<menuItem>
	{
		public menuData ()
		{
            this.Add(new menuItem()
            {
                Title = "Announcement",
                TargetType = typeof(announcementPage)
            });

            this.Add(new menuItem() {
                Title = "Tender Document",
                TargetType = typeof(tenderPage)
            });

            this.Add(new menuItem()
            {
                Title = "Search",
                TargetType = typeof(searchTenderPage)
            });
            
            this.Add(new menuItem()
            {
                Title = "Login",
                TargetType = typeof(loginPage)
            });

            this.Add(new menuItem()
            {
                Title = "Registration",
                TargetType = typeof(accountRegistrationPage)
            });
        }

        public menuData(bool isUserLoggedIn)
        {
            if (isUserLoggedIn)
            {
                this.Add(new menuItem()
                {
                    Title = "Announcement",
                    TargetType = typeof(announcementPage)
                });

                this.Add(new menuItem()
                {
                    Title = "Poll",
                    TargetType = typeof(pollPage)
                });

                this.Add(new menuItem()
                {
                    Title = "Survey",
                    TargetType = typeof(surveyList)
                });

                this.Add(new menuItem()
                {
                    Title = "Tender Document",
                    TargetType = typeof(tenderPage)
                });

                /*this.Add(new menuItem()
                {
                    Title = "Available Tender Document",
                    TargetType = typeof(tenderEligiblePage)
                });*/

                this.Add(new menuItem()
                {
                    Title = "Search",
                    TargetType = typeof(searchTenderPage)
                });

                this.Add(new menuItem()
                {
                    Title = "Tender Bookmark",
                    TargetType = typeof(tenderBookmarkPage)
                });

                this.Add(new menuItem()
                {
                    Title = "Favourite Searches",
                    TargetType = typeof(customSearchPage)
                });

                this.Add(new menuItem()
                {
                    Title = "Calendar",
                    TargetType = typeof(userCalendar)
                });

                /*this.Add(new menuItem()
                {
                    Title = "Purchased Tender Document",
                    TargetType = typeof(purchasedTendersPage)
                });

                /*this.Add(new menuItem()
                {
                    Title = "User Info",
                    TargetType = typeof(userInfoPage)
                });*/

                this.Add(new menuItem()
                {
                    Title = "Logout",
                    TargetType = typeof(logoutPage)
                });
            } else
            {
                //Admin is logged in
                this.Add(new menuItem()
                {
                    Title = "View Announcement",
                    TargetType = typeof(announcementPage)
                });

                this.Add(new menuItem()
                {
                    Title = "Add Announcement",
                    TargetType = typeof(adminPostAnnouncement)
                });

                this.Add(new menuItem()
                {
                    Title = "Poll",
                    TargetType = typeof(pollPage)
                });

                if (userSession.adminRole == "admin")
                {
                    this.Add(new menuItem()
                    {
                        Title = "Manage Admin Users",
                        TargetType = typeof(manageUserPage)
                    });

                    this.Add(new menuItem()
                    {
                        Title = "Register New Admin",
                        TargetType = typeof(registerAdminPage)
                    });
                }

                this.Add(new menuItem()
                {
                    Title = "Edit My Profile",
                    TargetType = typeof(editUserPage)
                });
                
                this.Add(new menuItem()
                {
                    Title = "Change Password",
                    TargetType = typeof(changePasswordPage)
                });

                this.Add(new menuItem()
                {
                    Title = "Logout",
                    TargetType = typeof(adminLogoutPage)
                });
            }
        }

    }
}