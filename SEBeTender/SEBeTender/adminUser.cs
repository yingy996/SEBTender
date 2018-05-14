using System;
using System.Collections.Generic;
using System.Text;

namespace SEBeTender
{
    public class adminUser
    {
        private string role = "";
        public string administratorName { get; set; }
        public string administratorEmail { get; set; }
        public string Role {
            get {
                if(role == "admin")
                {
                    return "Administrator";
                } else
                {
                    return "Editor";
                }
            }
            set { role = value; } }
        public string Username { get; set; }
    }
}
