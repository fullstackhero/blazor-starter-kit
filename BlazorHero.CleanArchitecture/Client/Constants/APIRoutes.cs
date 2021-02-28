using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Constants
{
    public static class APIRoutes
    {
        public static string Login = "api/identity/login";
        public static string Register = "api/identity/register";
        public static string ChangePassword = "api/identity/changepassword";
        public static string UpdateProfile = "api/identity/updateprofile";
    }
}
