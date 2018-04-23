using System;
using System.Collections.Generic;
using System.Text;

namespace SEBeTender
{
    public static class userSession
    {
        public static string userLoginCookie = "";
        public static string username = "";
        private static Cart cart = new Cart();

        public static Cart UserCart {
            get { return cart; }
            set { cart = value; }
        }
    }
}
