using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Server
{
    public static class Settings
    {
        public static string RootUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["rootUrl"];
            }
        }

        public static string ToAbsolute(string relativeUrl)
        {
            return RootUrl + relativeUrl;
        }
    }
}