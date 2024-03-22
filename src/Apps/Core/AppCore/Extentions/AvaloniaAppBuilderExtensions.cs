using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Fonts;
using NetX.AppCore.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace NetX.AppCore.Extentions
{
    public static class AvaloniaAppBuilderExtensions
    {
        /// <summary>
        /// 使用阿里巴巴字体
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configDelegate"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 使用日志
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static AppBuilder UseLog(this AppBuilder builder)
        {
            return builder.LogToTrace();
        }
    }
}
