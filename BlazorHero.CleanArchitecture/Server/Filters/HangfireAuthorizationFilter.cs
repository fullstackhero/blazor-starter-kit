using Hangfire.Dashboard;

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

            return true;
        }
    }
}