using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server
{
    public static class StringExtensions
    {
        public static string WithWildcards(this string term)
        {
            return "%" + term + "%";
        }
    }
}