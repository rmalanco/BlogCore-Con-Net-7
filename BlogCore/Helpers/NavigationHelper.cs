using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogCore.Helpers
{
    public static class NavigationHelper
    {
        public static bool IsNavActive(string controller, string action, ViewContext viewContext)
        {
            var currentController = viewContext.RouteData.Values["Controller"]?.ToString();
            var currentAction = viewContext.RouteData.Values["Action"]?.ToString();

            return string.Equals(currentController, controller, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(currentAction, action, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsActivePage(string area, string page, ViewContext viewContext)
        {
            var currentArea = viewContext.RouteData.Values["Area"]?.ToString();
            var currentPage = viewContext.RouteData.Values["Page"]?.ToString();

            return string.Equals(currentArea, area, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(currentPage, page, StringComparison.OrdinalIgnoreCase);
        }
    }
}