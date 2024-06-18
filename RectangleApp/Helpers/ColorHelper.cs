using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace RectangleApp.Helpers
{
    public static class ColorHelper
    {
        public static IEnumerable<String> GetColors()
        {
            return typeof(Colors)
                .GetProperties()
                .Where(prop =>
                    typeof(System.Windows.Media.Color).IsAssignableFrom(prop.PropertyType))
                .Select(prop => prop.Name);
        }

        public static string GetColorName(string colorHex)
        {
            Color color = ColorTranslator.FromHtml(colorHex);
            return color.Name;
        }
    }
}
