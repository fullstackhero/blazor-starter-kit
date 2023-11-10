using MudBlazor;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Settings
{
    public class BlazorHeroTheme
    {
        private static Typography DefaultTypography = new Typography()
        {
            Default = new Default()
            {
                FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                FontSize = ".875rem",
                FontWeight = 400,
                LineHeight = 1.43,
                LetterSpacing = ".01071em"
            },
            H1 = new H1()
            {
                FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                FontSize = "6rem",
                FontWeight = 300,
                LineHeight = 1.167,
                LetterSpacing = "-.01562em"
            },
            H2 = new H2()
            {
                FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                FontSize = "3.75rem",
                FontWeight = 300,
                LineHeight = 1.2,
                LetterSpacing = "-.00833em"
            },
            H3 = new H3()
            {
                FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                FontSize = "3rem",
                FontWeight = 900,
                LineHeight = 1.167,
                LetterSpacing = "0"
            },
            H4 = new H4()
            {
                FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                FontSize = "2.125rem",
                FontWeight = 900,
                LineHeight = 1.235,
                LetterSpacing = ".00735em"
            },
            H5 = new H5()
            {
                FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                FontSize = "1.5rem",
                FontWeight = 900,
                LineHeight = 1.334,
                LetterSpacing = "0"
            },
            H6 = new H6()
            {
                FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                FontSize = "1.22rem",
                FontWeight = 900,
                LineHeight = 1.6,
                LetterSpacing = ".0075em"
            },
            Button = new Button()
            {
                FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                FontSize = ".875rem",
                FontWeight = 500,
                LineHeight = 1.75,
                LetterSpacing = ".02857em"
            },
            Body1 = new Body1()
            {
                FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                FontSize = "1rem",
                FontWeight = 400,
                LineHeight = 1.5,
                LetterSpacing = ".00938em"
            },
            Body2 = new Body2()
            {
                FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                FontSize = ".875rem",
                FontWeight = 400,
                LineHeight = 1.43,
                LetterSpacing = ".01071em"
            },
            Caption = new Caption()
            {
                FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                FontSize = ".75rem",
                FontWeight = 400,
                LineHeight = 1.66,
                LetterSpacing = ".03333em"
            },
            Subtitle1 = new Subtitle1()
            {
                FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                FontSize = ".875rem",
                FontWeight = 100,
                LineHeight = 1.57,
                LetterSpacing = ".00714em"
            },
            Subtitle2 = new Subtitle2()
            {
                FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                FontSize = ".870rem",
                FontWeight = 500,
                LineHeight = 1.57,
                LetterSpacing = ".00714em"
            }
        };

        private static LayoutProperties DefaultLayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "10px"
        };

        public static MudTheme DefaultTheme = new MudTheme()
        {
            Palette = new Palette()
            {
                Primary = "#0691d8",
                Secondary = "#f19429",
                Tertiary = "#df0a78",
                AppbarBackground = "#1863b3",
                AppbarText = "rgba(0,0,0, 0.7)",
                Background = "#F9F9F9",
                DrawerBackground = "#FFF",
                DrawerText = "rgba(0,0,0, 0.7)",
                Success = "#007E33",
                TextPrimary = "#262526",
                SuccessDarken = "#262526"
            },
            Typography = DefaultTypography,
            LayoutProperties = DefaultLayoutProperties
        };

        public static MudTheme DarkTheme = new MudTheme()
        {
            Palette = new Palette()
            {
                Primary = "#ff9800",
                Secondary = "#0074d7",
                Tertiary = "#f39a9a",
                Success = "#007E33",
                Black = "#27272f",
                Background = "#0f0f0f",
                BackgroundGrey = "#27272f",
                Surface = "#181818",
                DrawerBackground = "#0f0f0f",
                DrawerText = "#f1f1f1",
                AppbarBackground = "#0f0f0f",
                AppbarText = "#f1f1f1",
                TextPrimary = "#fff",
                TextSecondary = "rgba(255,255,255, 0.50)",
                ActionDefault = "#FFF",
                ActionDisabled = "rgba(255,255,255, 0.26)",
                ActionDisabledBackground = "rgba(255,255,255, 0.12)",
                DrawerIcon = "#f1f1f1",
                SuccessDarken = "#1bd566",
                TextDisabled = "#9b9b9b",
                TableLines = "#e0e0e01c",
                Divider = "#e0e0e026"
            },
            Typography = DefaultTypography,
            LayoutProperties = DefaultLayoutProperties
        };
    }
}