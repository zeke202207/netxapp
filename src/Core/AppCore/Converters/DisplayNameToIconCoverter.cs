using Avalonia.Data.Converters;
using Material.Icons;
using SukiUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextMateSharp.Themes;

namespace NetX.AppCore.Converters
{
    public static class DisplayNameToIconConverters
    {
        public static readonly DisplayNameToIconCoverter IsThemeColorChecked = new(MaterialIconKind.RadioButtonChecked, MaterialIconKind.RadioButtonUnchecked);
    }

    public class DisplayNameToIconCoverter(MaterialIconKind trueIcon, MaterialIconKind falseIcon) : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (null == value) 
                return null;
            var currentThemeColor = SukiTheme.GetInstance();
            if(currentThemeColor.ThemeColor.ToString().ToLower() == value.ToString()?.ToLower())
                return trueIcon;
            return falseIcon;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
