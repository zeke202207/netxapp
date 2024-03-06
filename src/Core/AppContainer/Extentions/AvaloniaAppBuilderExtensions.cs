﻿using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Fonts;
using NetX.AppContainer.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace NetX.AppContainer.Extentions
{
    public static class AvaloniaAppBuilderExtensions
    {
        public static AppBuilder UseAlibabaFont([DisallowNull] this AppBuilder builder, Action<FontSettings>? configDelegate = default)
        {
            var setting = new FontSettings();
            configDelegate?.Invoke(setting);
            return builder.With(new FontManagerOptions
            {
                DefaultFamilyName = setting.DefaultFontFamily,
                FontFallbacks = new[]
                {
                  new FontFallback{ FontFamily = new FontFamily(setting.DefaultFontFamily) }
                }
            }).ConfigureFonts(manager => manager.AddFontCollection(new EmbeddedFontCollection(setting.Key, setting.Source)));
        }
    }
}
