using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Projet_Session_Entreprise.Themes
{
    public static class ThemeManager
    {
        private static bool _isDark = true;

        public static void ToggleTheme()
        {
            string path = _isDark ? "Themes/LightTheme.xaml" : "Themes/DarkTheme.xaml";
            _isDark = !_isDark;

            var dic = new ResourceDictionary { Source = new Uri(path, UriKind.Relative) };
            Application.Current.Resources.MergedDictionaries[0] = dic;
        }
    }
}
