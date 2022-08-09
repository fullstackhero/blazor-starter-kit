using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;

namespace BlazorHero.CleanArchitecture.Server.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            //TODO implement authorization logic

            //var httpContext = context.GetHttpContext();

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            //return httpContext.User.Identity.IsAuthenticated;
            //return httpContext.User.IsInRole(Permissions.Hangfire.View);

            // example of authorization using token from navlink
            var httpContext = context.GetHttpContext();

            string jwtToken;
            bool readFromCookie = false;

            bool readFromQuery = httpContext.Request.Query.TryGetValue("token", out var jwtTokenFromQuery);

            if (readFromQuery)
            {
                jwtToken = jwtTokenFromQuery.ToString();
            }
            else
            {
                readFromCookie = httpContext.Request.Cookies.TryGetValue("token", out jwtToken);
            }

            if (!readFromCookie && !readFromQuery) return false;

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            if (token is null) return false;

            var expirationTime = token.ValidTo;

            if (DateTime.Now > expirationTime) return false;

            if (readFromQuery)
            {
                CookieOptions options = new CookieOptions
                {
                    Expires = expirationTime
                };
                httpContext.Response.Cookies.Append("token", jwtToken, options);
            }

            var hangfireViewPermission =
                token.Claims.Any(w => w.Value.Equals(Permissions.Hangfire.View));

            return hangfireViewPermission;

            // return true;
        }
    }
}