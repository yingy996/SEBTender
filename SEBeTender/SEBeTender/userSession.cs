using System;
using System.Collections.Generic;
using System.Text;

namespace SEBeTender
{
    public static class userSession
    {
        public static string userLoginCookie = "";
        private static Cart cart = new Cart();

        public static Cart UserCart {
            get { return cart; }
            set { cart = value; }
        }
    }
}
