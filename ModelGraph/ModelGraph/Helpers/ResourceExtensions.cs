using System;
using System.Runtime.InteropServices;

using Windows.ApplicationModel.Resources;

namespace ModelGraph.Helpers
{
    internal static class ResourceExtensions
    {
        private static ResourceLoader _resLoader = new ResourceLoader();
        private static ResourceLoader _coreLoader = ResourceLoader.GetForCurrentView("CoreResources");

        public static string GetLocalized(this string resourceKey)
        {
            return _resLoader.GetString(resourceKey);
        }
        public static Func<string, string> CoreLocalizer()
        {
            return (s) => _coreLoader.GetString(s);
        }
    }
}
