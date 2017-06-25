using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWebApplication.Common
{
    public class Const
    {
        //セッション変数用Const
        public static class Session
        {
            public static string SESSION_ID = "SessionId";
            public static string ASP_ID = "AspId";
        }
        //ViewData用Const
        public static class ViewData
        {
            public static string PRODUCT_DROPDOWN_LIST = "ProductDropdownList";
        }

        public static class Redirect
        {
            public static string ORDER_END = "~/Order/End";
        }
    }
}