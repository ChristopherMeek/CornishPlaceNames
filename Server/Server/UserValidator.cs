using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Authentication.Basic;
using Nancy.Security;

namespace Server
{
    public class UserValidator : IUserValidator
    {
        public IUserIdentity Validate(string username, string password)
        {
            if (username == "cpnAdmin" && password == "pulls bouncy cuff feels")
            {
                return new Admin() { UserName = "cpnAdmin" };
            }

            return null;
        }
    }

    public class Admin : IUserIdentity
    {
        public string UserName { get; set; }

        public IEnumerable<string> Claims { get; set; }
    }
}