using Avalonia.Data.Converters;
using Avalonia.Styling;
using System;
using System.Globalization;

namespace NetX.AppCore.Converters
{
    public class ThemeVariantToDisplayCoverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var variant = "浅色";
            if (value is not ThemeVariant themeVariant)
                return variant;
            variant = themeVariant == ThemeVariant.Dark ? "浅色" : "深色";
            return variant;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not string variant)
                return ThemeVariant.Light;
            return variant switch
            {
                "浅色" => ThemeVariant.Light,
                "深色" => ThemeVariant.Dark,
                _ => ThemeVariant.Light
            };
        }
    }
}
