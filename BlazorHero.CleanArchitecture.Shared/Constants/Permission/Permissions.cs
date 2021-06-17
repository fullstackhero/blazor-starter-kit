using System.ComponentModel;

namespace BlazorHero.CleanArchitecture.Shared.Constants.Permission
{
    public static class Permissions
    {
        public static class Products
        {
            [Description("View Products")]
            public const string View = "Permissions.Products.View";
            [Description("Create a new Product")]
            public const string Create = "Permissions.Products.Create";
            [Description("Edit existing Product")]
            public const string Edit = "Permissions.Products.Edit";
            [Description("Delete any Product")]
            public const string Delete = "Permissions.Products.Delete";
        }

        public static class Brands
        {
            [Description("View Brands")]
            public const string View = "Permissions.Brands.View";
            [Description("Create a new Brand")]
            public const string Create = "Permissions.Brands.Create";
            [Description("Edit existing Brand")]
            public const string Edit = "Permissions.Brands.Edit";
            [Description("Delete any Brand")]
            public const string Delete = "Permissions.Brands.Delete";
        }

        public static class Documents
        {
            [Description("View Documents")]
            public const string View = "Permissions.Documents.View";
            [Description("Create a new Document")]
            public const string Create = "Permissions.Documents.Create";
            [Description("Edit existing Document")]
            public const string Edit = "Permissions.Documents.Edit";
            [Description("Delete any Document")]
            public const string Delete = "Permissions.Documents.Delete";
        }

        public static class Users
        {
            [Description("View Users")]
            public const string View = "Permissions."+nameof(Users)+".View";
            [Description("Create a new User")]
            public const string Create = "Permissions." + nameof(Users) + ".Create";
            [Description("Edit existing User")]
            public const string Edit = "Permissions." + nameof(Users) + ".Edit";
            [Description("Delete any User")]
            public const string Delete = "Permissions." + nameof(Users) + ".Delete";
        }

        public static class Roles
        {
            [Description("View Roles")]
            public const string View = "Permissions." + nameof(Roles) + ".View";
            [Description("Create a new Role")]
            public const string Create = "Permissions." + nameof(Roles) + ".Create";
            [Description("Edit existing Role")]
            public const string Edit = "Permissions." + nameof(Roles) + ".Edit";
            [Description("Delete any Role")]
            public const string Delete = "Permissions." + nameof(Roles) + ".Delete";
        }

        public static class RoleClaims
        {
            [Description("View RoleClaims")]
            public const string View = "Permissions." + nameof(RoleClaims) + ".View";
            [Description("Create a new RoleClaim")]
            public const string Create = "Permissions." + nameof(RoleClaims) + ".Create";
            [Description("Edit existing RoleClaim")]
            public const string Edit = "Permissions." + nameof(RoleClaims) + ".Edit";
            [Description("Delete any RoleClaim")]
            public const string Delete = "Permissions." + nameof(RoleClaims) + ".Delete";
        }

        public static class Communication
        {
            [Description("Chat with other user")]
            public const string Chat = "Permissions." + nameof(Communication) + ".Chat";
        }

        public static class Preferences
        {
            [Description("Change Language")]
            public const string ChangeLanguage = "Permissions." + nameof(Preferences) + ".ChangeLanguage";

            //TODO - add permissions
        }

        public static class Dashboards
        {
            [Description("View Dashboards")]
            public const string View = "Permissions." + nameof(Dashboards) + ".View";
        }

        public static class Hangfire
        {
            [Description("View Hangfire")]
            public const string View = "Permissions." + nameof(Hangfire) + ".View";
        }

        public static class AuditTrails
        {
            [Description("View AuditTrails")]
            public const string View = "Permissions." + nameof(AuditTrails) + ".View";
        }
    }
}